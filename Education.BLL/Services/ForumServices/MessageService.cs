using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Forum.Edit;
using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using Education.BLL.Services.ForumServices.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Education.BLL.Services.ForumServices
{
    public class MessageService : IMessageService
    {

        private IMessageRules MessageRules;
        private IGetUserDTO GetUserService;
        private IUOWFactory DataFactory;
        private IForumDTOHelper forumDTOHelper;

        public MessageService(IMessageRules rules, IGetUserDTO getUserDTO, IUOWFactory dataFactory, IForumDTOHelper forumHelper)
        {
            GetUserService = getUserDTO;
            DataFactory = dataFactory;
            forumDTOHelper = forumHelper;
            MessageRules = rules;
        }

        private void EditMessage(Message message, MessageEditDTO messageDTO, User user, bool edit = true)
        {
            message.Text = messageDTO.Text;
            if (edit)
            {
                message.LastEditor = user;
                message.LastEditTime = DateTime.Now;
            }
            else
            {
                message.Owner = user;
                message.Time = DateTime.Now;
            }
        }

        private bool IsFirstMessage(Message message)
        {
            if (message.Theme == null) return false;
            if (message?.Theme?.Messages?.OrderBy(X => X.Id).FirstOrDefault() == message) return true;
            return false;
        }

        private (AccessCode Code, Message Message, User User) CheckMessage(UserDTO userDTO, int MessageId, Func<User, Message, bool> checkFunc, IUOW Data)
        {
            var user = GetUserService.Get(userDTO, Data);
            var message = Data.MessageRepository.Get().FirstOrDefault(x => x.Id == MessageId);
            if (message == null) return (AccessCode.NotFound, null, user);
            if (checkFunc(user, message)) return (AccessCode.Succsess, message, user);
            else return (AccessCode.NoPremision, null, user);
        }

        public CreateResultDTO Create(MessageEditDTO DTO, UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                var theme = Data.ThemeRepository.Get().FirstOrDefault(x => x.Id == DTO.ThemeId);
                if (theme == null) return CreateResultDTO.NotFound;
                if (MessageRules.CanCreate(user, theme))
                {
                    var message = new Message();
                    EditMessage(message, DTO, user, false);
                    message.Theme = theme;
                    Data.MessageRepository.Add(message);
                    Data.SaveChanges();
                    return new CreateResultDTO(message.Id, AccessCode.Succsess);
                }
                else return CreateResultDTO.NoPremision;
            }
        }

        public AccessCode Delete(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckMessage(userDTO, id, MessageRules.CanDelete, Data);
                if (check.Code == AccessCode.Succsess)
                {
                    if (IsFirstMessage(check.Message)) return AccessCode.NoPremision;
                    Data.MessageRepository.Delete(check.Message);
                    Data.SaveChanges();
                }
                return check.Code;
            }
        }

        public (AccessCode, MessageDTO) Read(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckMessage(userDTO, id, MessageRules.CanRead, Data);
                if (check.Code == AccessCode.Succsess)
                    return (check.Code, forumDTOHelper.GetDTO(check.Message, check.User));
                else return (check.Code, null);
            }
        }

        public AccessCode Update(MessageEditDTO DTO, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckMessage(userDTO, DTO.Id, MessageRules.CanRead, Data);
                if (check.Code == AccessCode.Succsess)
                {
                    if (IsFirstMessage(check.Message)) return AccessCode.NoPremision;
                    EditMessage(check.Message, DTO, check.User);
                    Data.MessageRepository.Edited(check.Message);
                    Data.SaveChanges();
                }
                return check.Code;
            }
        }

        public bool CanCreate(int ThemeId, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                var theme = Data.ThemeRepository.Get().FirstOrDefault(x => x.Id == ThemeId);
                if (theme == null) return false;
                return MessageRules.CanCreate(user, theme);
            }
        }


        public MessagePreviewDTO Get(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                var message = Data.MessageRepository.Get().FirstOrDefault(x => x.Id == id);
                if (message == null || !MessageRules.CanRead(user, message)) return null;
                return forumDTOHelper.GetDTO(message);
            }
        }
    }
}
