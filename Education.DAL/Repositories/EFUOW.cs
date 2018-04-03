using Microsoft.EntityFrameworkCore;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using Ninject;
using Microsoft.EntityFrameworkCore.Proxies;

namespace Education.DAL.Repositories
{
    public class EFUOW : IUOW
    {
        private EFContext dbContext;
        //-------------------------------
        //-------------------------------
        private IKernel kernel = new StandardKernel();

        private void Binding()
        {
            kernel.Bind(typeof(IRepos<>)).To(typeof(EFRepos<>)).WithConstructorArgument("context", dbContext);
        }

        public EFUOW()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFContext>();
            var options = optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EDC;Trusted_Connection=True;")
            .Options;
            dbContext = new EFContext(options);
            Binding();
            Init();
        }

        void Init()
        {
            UserInfoRepository = kernel.Get<IRepos<UserInfo>>();
            UserRepository = kernel.Get<IRepos<User>>();
            ContactRepository = kernel.Get<IRepos<Contact>>();
            AuthKeyRepository = kernel.Get<IRepos<Key>>();
            BanRepository = kernel.Get<IRepos<Ban>>();
            UserClaimRepository = kernel.Get<IRepos<UserClaim>>();
        }

        #region Repos

        public IRepos<UserInfo> UserInfoRepository { get; private set; }

        public IRepos<User> UserRepository { get; private set; }

        public IRepos<Contact> ContactRepository { get; private set; }

        public IRepos<Key> AuthKeyRepository { get; private set; }

        public IRepos<Ban> BanRepository { get; private set; }

        public IRepos<UserClaim> UserClaimRepository { get; private set; }
        #endregion
    }
}
