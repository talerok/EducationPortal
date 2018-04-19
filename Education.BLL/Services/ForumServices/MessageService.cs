using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using Education.BLL.Services.ForumServices.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Education.BLL.Services.ForumServices
{
    class MessageService : IControlService<MessageDTO>
    {
        private IGetUserService getUserService;
        private IUOWFactory DataFactory;

        

        private (ControlResult Code, Theme Theme) GetPremision(User user, int themeId,  IUOW Data)
        {
            var theme = Data.ThemeRepository.Get().FirstOrDefault(x => x.Id == themeId);
            
            if (theme?.Section?.Group == null) return (ControlResult.notFound, null);
            if (user == null) return (ControlResult.noPremission, theme);

            var userStatus = theme.Section.Group.Users.FirstOrDefault(x => x.User == user);
            if (userStatus == null || userStatus.Status == UserGroupStatus.request)
                return (ControlResult.noPremission, theme);
            if (!theme.Open && user.Level < 1 && userStatus.Status != UserGroupStatus.owner)
                return (ControlResult.noPremission, theme);

            return (ControlResult.succsess, theme);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParrentId">Theme Id</param>
        /// <param name="Value">Message</param>
        /// <param name="userDTO">User who sent it</param>
        /// <returns>Result and id of message</returns>
        public (ControlResult Code, int Id) Create(int ParrentId, MessageDTO Value, UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var user = getUserService.Get(userDTO, Data);

                var res = GetPremision(user, ParrentId, Data);
                if (res.Code != ControlResult.succsess) return (res.Code, 0);

                res.Theme.Messages.Add(
                    new Message
                    {
                        Owner = user,
                        Text = Value.Text,
                        Time = DateTime.Now,
                        Theme = res.Theme
                    }
                );
                Data.ThemeRepository.Edited(res.Theme);
                Data.SaveChanges();
                return (res.Code, res.Theme.Messages.Count - 1);
            }
            
        }

        public ControlResult Delete(int Id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = getUserService.Get(userDTO, Data);
                var message = Data.MessageRepository.Get().FirstOrDefault(x => x.Id == Id);
                if (message == null || message.Theme == null) return ControlResult.notFound;
                var res = GetPremision(user, message.Theme.Id, Data);
                if (res.Code == ControlResult.succsess)
                {
                    Data.MessageRepository.Delete(message);
                    Data.SaveChanges();
                }
                return res.Code;
            }
         }

        private UserPublicInfoDTO GetPubliUserInfo(User user)
        {
            if (user == null) return null;
            return new UserPublicInfoDTO
            {
                AvatarPath = user.Info.Avatar,
                Name = user.Info.FullName
            };
        }

        private MessageDTO GetMessageDTO(Message message)
        {
            return new MessageDTO
            {
                Owner = GetPubliUserInfo(message.Owner),
                LastEditor = GetPubliUserInfo(message.LastEditor),
                Time = message.Time,
                LastEditTime = message.LastEditTime,
                Text = message.Text
            };
        }

        public (ControlResult Code, MessageDTO Value) Get(int Id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = getUserService.Get(userDTO, Data);
                var message = Data.MessageRepository.Get().FirstOrDefault(x => x.Id == Id);
                if (message == null) return (ControlResult.notFound, null);
                var res = GetPremision(user, Id, Data);
                MessageDTO messageDTO = null;
                if (res.Code == ControlResult.succsess)  messageDTO = GetMessageDTO(message);
                return (res.Code, messageDTO);
            }
        }

        public ControlResult Update(int Id, MessageDTO Value, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = getUserService.Get(userDTO, Data);
                var message = Data.MessageRepository.Get().FirstOrDefault(x => x.Id == Id);
                if (message == null || message.Theme == null) return ControlResult.notFound;

                var res = GetPremision(user, message.Theme.Id, Data);
                if (res.Code == ControlResult.succsess)
                {
                    message.LastEditor = user;
                    message.LastEditTime = DateTime.Now;
                    message.Text = Value.Text;
                    Data.MessageRepository.Edited(message);
                    Data.SaveChanges();
                }


                return res.Code;
            }
        }
    }
}
