using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using Education.BLL.Services.ForumServices.Interfaces;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ForumServices
{
    public class ForumService : IForumService
    {
        private IUOWFactory DataFactory;
        private IForumDTOHelper ForumDTOHelper;
        private IGroupRules GroupRules;
        private IGetUserDTO GetUserService;

        public ForumService(IGetUserDTO getUserDTO, IUOWFactory dataFactory, IForumDTOHelper forumDTOHelper, IGroupRules groupRules)
        {
            GetUserService = getUserDTO;
            DataFactory = dataFactory;
            ForumDTOHelper = forumDTOHelper;
            GroupRules = groupRules;
        }

        public ForumDTO Get(UserDTO userDTO)
        {
            using (var Data = DataFactory.Get()) {
                var user = GetUserService.Get(userDTO, Data);
                var groups = new List<GroupPreviewDTO>();
                foreach(var Group in Data.GroupRepository.Get())
                    groups.Add(ForumDTOHelper.GetDTO(user, Group));
                return new ForumDTO { Groups = groups, CanCreateGroup = GroupRules.CanCreate(user) };
            }
        }
    }
}
