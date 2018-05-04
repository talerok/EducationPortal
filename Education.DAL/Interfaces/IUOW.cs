using System;
using System.Collections.Generic;
using System.Text;
using Education.DAL.Entities;
using Education.DAL.Entities.Pages;

namespace Education.DAL.Interfaces
{
    public interface IUOW : IDisposable
    {
        IRepos<User> UserRepository { get; }
        IRepos<UserInfo> UserInfoRepository { get; }
        IRepos<Contact> ContactRepository { get; }
        IRepos<Key> AuthKeyRepository { get; }
        IRepos<Ban> BanRepository { get; }
        IRepos<UserClaim> UserClaimRepository { get; }
        //------------Forums
        IRepos<Message> MessageRepository { get; }
        IRepos<Theme> ThemeRepository { get; }
        IRepos<Section> SectionRepository { get; }
        IRepos<Group> GroupRepository { get; }
        IRepos<UserGroup> UserGroupRepository { get; }
        //------------Pages
        IRepos<Note> NoteRepository { get; }
        IRepos<Page> PageRepository { get; }
        //------------------
        void SaveChanges();
    }
}
