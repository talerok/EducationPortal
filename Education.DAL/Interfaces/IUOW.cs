using System;
using System.Collections.Generic;
using System.Text;
using Education.DAL.Entities;

namespace Education.DAL.Interfaces
{
    public interface IUOW
    {
        IRepos<User> UserRepository { get; }
        IRepos<UserInfo> UserInfoRepository { get; }
        IRepos<Contact> ContactRepository { get; }
        IRepos<Key> AuthKeyRepository { get; }
        IRepos<Ban> BanRepository { get; }
        IRepos<UserClaim> UserClaimRepository { get; }
    }
}
