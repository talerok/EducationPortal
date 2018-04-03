using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Education.BLL.Services.UserServices.Interfaces;
using Education.BLL.Services.UserServices.Init;
using Education.BLL.Services.UserServices.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Education.DAL.Interfaces;
using Education.DAL.Repositories;
using Education.BLL.Services.UserServices.KeyGenerators;
using Education.BLL.Services.UserServices.Messagers;
using Education.BLL.Services.UserServices.Messengers;
using Education.BLL.Services.UserServices.Confirm;
using Education.BLL.Services.UserServices.Profile;
using Education.BLL.Services.UserServices.Restore;
using System.Threading.Tasks;

namespace Education
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
       
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //----------------------------------------------------
            IMessenger messenger_sms = new SmsMessenger();
            IMessenger messenger_email = new EmailMessenger();
            IPassHasher passHasher = new SHA256Hasher();
            IKeyGenerator smallKeyGenerator = new SmallKeyGenerator();
            IKeyGenerator bigKeyGenerator = new BigKeyGenerator();
            //----------------------------------------------------
            services.AddTransient<IUOW, EFUOW>();
            services.AddTransient<IClaimService, ClaimService>();
            services.AddSingleton<IInitDBService, InitDBService>();
            //-----------------------------------------------------
            services.AddTransient<IUserService, UserAuthService>(
                serviceProvider =>
                {
                    var uow = new EFUOW();
                    return new UserAuthService(
                        uow, 
                        new AuthKeyService(
                                uow,
                                smallKeyGenerator,
                                messenger_sms
                            ),
                        new AuthKeyService(
                                uow,
                                smallKeyGenerator,
                                messenger_email
                            ),
                        passHasher,
                        new RegValidator(uow),
                        new ClaimService(uow),
                        bigKeyGenerator
                        );
                }
            );

            services.AddTransient<IProfileService, ProfileService>(
               serviceProvider =>
               {
                   var uow = new EFUOW();
                   var regValidator = new RegValidator(uow);
                   IConfirmService emailCS = new ConfirmService(
                       new ConfirmKeyService(uow, bigKeyGenerator, messenger_email),
                       uow
                       );
                   IConfirmService phoneCS = new ConfirmService(
                       new ConfirmKeyService(uow, smallKeyGenerator, messenger_sms),
                       uow
                       );
                   IClaimService claimService = new ClaimService(uow);
                   return new ProfileService(
                       uow,
                       regValidator,
                       emailCS,
                       phoneCS,
                       passHasher,
                       claimService
                      );
               }
           );

            services.AddTransient<IRestorePasswordService, RestorePasswordService>(
               serviceProvider =>
               {
                   var uow = new EFUOW();
                   var emaiCKS = new ConfirmKeyService(uow, bigKeyGenerator, messenger_email);
                   var phoneCKS = new ConfirmKeyService(uow, smallKeyGenerator, messenger_sms);
                   return new RestorePasswordService(uow, emaiCKS, phoneCKS, new RegValidator(uow), passHasher);
               }
            );

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                     options.LogoutPath = new Microsoft.AspNetCore.Http.PathString("/Account/Logout");
                     options.Events.OnValidatePrincipal = PrincipalValidator.ValidateAsync;
                 });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

public static class PrincipalValidator
{
    public static Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        Task task = new Task(() =>
        {
            if (context == null) throw new System.ArgumentNullException(nameof(context));
            var authService = context.HttpContext.RequestServices.GetRequiredService<IClaimService>();
            var user = authService.GetUser(context.Principal.Claims);
            if (user == null) context.RejectPrincipal();
        });
        task.Start();
        return task;
    }
}