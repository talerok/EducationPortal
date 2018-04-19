using Microsoft.EntityFrameworkCore;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Proxies;
using System;

namespace Education.DAL.Repositories
{
    public class EFUOW : IUOW
    {
        private EFContext dbContext;
        //-------------------------------
        public EFUOW(string connSrting)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFContext>();
            var options = optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(connSrting)
            .Options;
            dbContext = new EFContext(options);
            InitAll();
        }

        private IRepos<T> InitRepos<T>() where T : class
        {
            return new EFRepos<T>(dbContext);
        }

        void InitAll()
        {
            UserInfoRepository = InitRepos<UserInfo>();
            UserRepository = InitRepos <User>();
            ContactRepository = InitRepos <Contact>();
            AuthKeyRepository = InitRepos <Key>();
            BanRepository = InitRepos <Ban>();
            UserClaimRepository = InitRepos <UserClaim>();
            //Forum
            MessageRepository = InitRepos <Message>();
            ThemeRepository = InitRepos <Theme>();
            SectionRepository = InitRepos <Section>();
            GroupRepository = InitRepos <Group>();
            UserGroupRepository = InitRepos <UserGroup>();
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        #region Repos

        public IRepos<UserInfo> UserInfoRepository { get; private set; }

        public IRepos<User> UserRepository { get; private set; }

        public IRepos<Contact> ContactRepository { get; private set; }

        public IRepos<Key> AuthKeyRepository { get; private set; }

        public IRepos<Ban> BanRepository { get; private set; }

        public IRepos<UserClaim> UserClaimRepository { get; private set; }

        public IRepos<Message> MessageRepository { get; private set; }

        public IRepos<Theme> ThemeRepository { get; private set; }

        public IRepos<Section> SectionRepository { get; private set; }

        public IRepos<Group> GroupRepository { get; private set; }

        public IRepos<UserGroup> UserGroupRepository { get; private set; }
        #endregion
    }
}
