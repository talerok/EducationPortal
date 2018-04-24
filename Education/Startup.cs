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
using Education.BLL.Logic.Interfaces;
using Education.BLL.Logic.Rules;
using Education.BLL.Logic;
using Education.BLL.Services.ForumServices.Interfaces;
using Education.BLL.Services.ForumServices;

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
            string connString = @"Server=(localdb)\mssqllocaldb;Database=EDC;Trusted_Connection=True;";
            //----------------------------------------------------
            IMessenger messenger_sms = new SmsMessenger();
            IMessenger messenger_email = new EmailMessenger();
            IPassHasher passHasher = new SHA256Hasher();
            IKeyGenerator smallKeyGenerator = new SmallKeyGenerator();
            IKeyGenerator bigKeyGenerator = new BigKeyGenerator();
            IRegValidator regValidator = new RegValidator();
            IUOWFactory UOWFactory = new EFUOWFactory(connString);
            //----------------------------------------------------
            IClaimService claimService = new ClaimService(UOWFactory);
            //----------------------------------------------------
            services.AddSingleton<IUOWFactory, IUOWFactory>(
                serviceProvider =>
                {
                    return UOWFactory;
                }
            );

            services.AddSingleton<IClaimService, ClaimService>();
            //-----------------------------------------------------

            services.AddSingleton<IUserService, UserAuthService>(
                serviceProvider =>
                {
                    return new UserAuthService(
                        UOWFactory, 
                        new AuthKeyService(smallKeyGenerator, messenger_sms),
                        new AuthKeyService(smallKeyGenerator,messenger_email),
                        passHasher,
                        regValidator,
                        claimService,
                        bigKeyGenerator
                        );
                }
            );

            services.AddSingleton<IProfileService, ProfileService>(
               serviceProvider =>
               {
                   IConfirmService emailCS = new ConfirmService(
                       new ConfirmKeyService(bigKeyGenerator, messenger_email)
                       );
                   IConfirmService phoneCS = new ConfirmService(
                       new ConfirmKeyService(smallKeyGenerator, messenger_sms)
                       );
                   return new ProfileService(
                       UOWFactory,
                       regValidator,
                       emailCS,
                       phoneCS,
                       passHasher,
                       claimService
                      );
               }
           );

            services.AddSingleton<IRestorePasswordService, RestorePasswordService>(
               serviceProvider =>
               {
                   var emaiCKS = new ConfirmKeyService(bigKeyGenerator, messenger_email);
                   var phoneCKS = new ConfirmKeyService(smallKeyGenerator, messenger_sms);
                   return new RestorePasswordService(UOWFactory, emaiCKS, phoneCKS, new RegValidator(), passHasher);
               }
            );
            //---------Forum Services--------------------------------
            IGroupRules groupRules = new GroupRules();
            ISectionRules sectionRules = new SectionRules(groupRules);
            IThemeRules themeRules = new ThemeRules(sectionRules);
            IMessageRules messageRules = new MessageRules(themeRules, sectionRules);
            IForumDTOHelper forumDTOHelper = new ForumDTOHelper(messageRules, themeRules, sectionRules, groupRules);
            IGetUserDTO getUserDTO = new GetUserDTO();

            services.AddSingleton<IGroupService,GroupService>(
              serviceProvider =>
              {
                  return new GroupService(groupRules, getUserDTO, UOWFactory, forumDTOHelper);
              }
            );

            services.AddSingleton<ISectionService, SectionService>(
              serviceProvider =>
              {
                  return new SectionService(sectionRules, getUserDTO, UOWFactory, forumDTOHelper);
              }
            );

            services.AddSingleton<IThemeService, ThemeService>(
              serviceProvider =>
              {
                  return new ThemeService(themeRules, getUserDTO, UOWFactory, forumDTOHelper);
              }
            );

            services.AddSingleton<IMessageService, MessageService>(
              serviceProvider =>
              {
                  return new MessageService(messageRules, getUserDTO, UOWFactory, forumDTOHelper);
              }
            );

            //-------------------------------------------------------
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