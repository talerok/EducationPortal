using Microsoft.EntityFrameworkCore;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Proxies;
using System;
using Education.DAL.Entities.Pages;

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
        }

        private IRepos<T> InitRepos<T>() where T : class
        {
            return new EFRepos<T>(dbContext);
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
        //-------------------
        private IRepos<UserInfo> UserInfoRepository_p;
        private IRepos<User> UserRepository_p;
        private IRepos<Contact> ContactRepository_p;
        private IRepos<Key> AuthKeyRepository_p;
        private IRepos<Ban> BanRepository_p;
        private IRepos<UserClaim> UserClaimRepository_p;
        private IRepos<Message> MessageRepository_p;
        private IRepos<Theme> ThemeRepository_p;
        private IRepos<Section> SectionRepository_p;
        private IRepos<Group> GroupRepository_p;
        private IRepos<UserGroup> UserGroupRepository_p;
        private IRepos<Note> NoteRepository_p;
        private IRepos<Page> PageRepository_p;

        public IRepos<UserInfo> UserInfoRepository
        {
            get
            {
                if (UserInfoRepository_p == null)
                    UserInfoRepository_p = InitRepos<UserInfo>();
                return UserInfoRepository_p;
            }
        }

        public IRepos<User> UserRepository
        {
            get
            {
                if (UserRepository_p == null)
                    UserRepository_p = InitRepos<User>();
                return UserRepository_p;
            }
        }

        public IRepos<Contact> ContactRepository
        {
            get
            {
                if (ContactRepository_p == null)
                    ContactRepository_p = InitRepos<Contact>();
                return ContactRepository_p;
            }
        }

        public IRepos<Key> AuthKeyRepository
        {
            get
            {
                if (AuthKeyRepository_p == null)
                    AuthKeyRepository_p = InitRepos<Key>();
                return AuthKeyRepository_p;
            }
        }

        public IRepos<Ban> BanRepository
        {
            get
            {
                if (BanRepository_p == null)
                    BanRepository_p = InitRepos<Ban>();
                return BanRepository_p;
            }
        }

        public IRepos<UserClaim> UserClaimRepository
        {
            get
            {
                if (UserClaimRepository_p == null)
                    UserClaimRepository_p = InitRepos<UserClaim>();
                return UserClaimRepository_p;
            }
        }

        public IRepos<Message> MessageRepository
        {
            get
            {
                if (MessageRepository_p == null)
                    MessageRepository_p = InitRepos<Message>();
                return MessageRepository_p;
            }
        }

        public IRepos<Theme> ThemeRepository
        {
            get
            {
                if (ThemeRepository_p == null)
                    ThemeRepository_p = InitRepos<Theme>();
                return ThemeRepository_p;
            }
        }

        public IRepos<Section> SectionRepository
        {
            get
            {
                if (SectionRepository_p == null)
                    SectionRepository_p = InitRepos<Section>();
                return SectionRepository_p;
            }
        }

        public IRepos<Group> GroupRepository
        {
            get
            {
                if (GroupRepository_p == null)
                    GroupRepository_p = InitRepos<Group>();
                return GroupRepository_p;
            }
        }

        public IRepos<UserGroup> UserGroupRepository
        {
            get
            {
                if (UserGroupRepository_p == null)
                    UserGroupRepository_p = InitRepos<UserGroup>();
                return UserGroupRepository_p;
            }
        }
        public IRepos<Note> NoteRepository
        {
            get
            {
                if (NoteRepository_p == null)
                    NoteRepository_p = InitRepos<Note>();
                return NoteRepository_p;
            }
        }
        public IRepos<Page> PageRepository
        {
            get
            {
                if (PageRepository_p == null)
                    PageRepository_p = InitRepos<Page>();
                return PageRepository_p;
            }
        }
        #endregion
    }
}
