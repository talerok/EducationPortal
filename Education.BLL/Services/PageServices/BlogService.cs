using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Pages;
using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using Education.BLL.Services.PageServices.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Entities.Pages;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Education.BLL.Services.PageServices
{
    public class BlogService : IBlogService
    {
        private const int NotesPerPage = 10;
        private INoteRules NoteRules;
        private IUOWFactory DataFactory;
        private IGetUserDTO GetUserService;
        private IDTOHelper DTOHelper;

        public BlogService(INoteRules noteRules, IUOWFactory dataFactory, IGetUserDTO getUserService, IDTOHelper dtoHelper)
        {
            NoteRules = noteRules;
            DataFactory = dataFactory;
            GetUserService = getUserService;
            DTOHelper = dtoHelper;
        }

        private NoteDTO GetDTO(Note note, User user)
        {
            return new NoteDTO
            {
                Id = note.Id,
                Preview = note.Preview,
                Name = note.Name,
                Text = note.Text,
                Time = note.Time,
                Published = note.Published,
                Owner = DTOHelper.GetUser(note.Owner),
                Access = new DTO.AccessDTO
                {
                    CanDelete = NoteRules.CanDelete(user, note),
                    CanRead = NoteRules.CanRead(user, note),
                    CanUpdate = NoteRules.CanEdit(user, note)
                }
            };
        } 

        private void EditNote(Note note, NoteEditDTO noteEditDTO)
        {
            if (note == null) return;
            note.Name = noteEditDTO.Name;
            note.Preview = noteEditDTO.Preview;
            note.Published = noteEditDTO.Published;
            note.Text = noteEditDTO.Text;
            note.Time = DateTime.Now;
        }

        private IEnumerable<NoteDTO> Get(User user, IEnumerable<Note> notes)
        {
            var res = new List<NoteDTO>();
            foreach(var note in notes)
                if (NoteRules.CanRead(user, note))
                    res.Add(GetDTO(note, user));
            return res;
        }
        
        private IEnumerable<Note> GetNotes(int page, IUOW Data)
        {
            return Data.NoteRepository.Get().Skip((page - 1) * NotesPerPage)
            .Take(NotesPerPage);
        }

        public (AccessCode,NoteDTO) Get(int id, UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var note = Data.NoteRepository.Get().FirstOrDefault(x => x.Id == id);
                var user = GetUserService.Get(userDTO,Data);
                if (note == null) return (AccessCode.NotFound, null);
                if (!NoteRules.CanRead(user,note)) return (AccessCode.NoPremision, null);
                return (AccessCode.Succsess, GetDTO(note, user));
            }
        }

        public BlogDTO Get(UserDTO userDTO, int page)
        {
            using (var Data = DataFactory.Get())
            {
                if (page < 1) page = 1;
                var user = GetUserService.Get(userDTO, Data);
                var notes = GetNotes(page, Data);

                return new BlogDTO {
                    Page = page,
                    Pages = Data.NoteRepository.Get().Count() / NotesPerPage + 1,
                    Notes = Get(user,notes),
                    CanCreate = NoteRules.CanCreate(user)
                };
            }
        }  
        
        public AccessCode Update(NoteEditDTO noteEditDTO, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var note = Data.NoteRepository.Get().FirstOrDefault(x => x.Id == noteEditDTO.Id);
                var user = GetUserService.Get(userDTO, Data);
                if (note == null) return AccessCode.NotFound;
                if (!NoteRules.CanEdit(user, note)) return AccessCode.NoPremision;
                EditNote(note, noteEditDTO);
                Data.NoteRepository.Edited(note);
                Data.SaveChanges();
            }
            return AccessCode.Succsess;
        }

        public AccessCode Delete(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var note = Data.NoteRepository.Get().FirstOrDefault(x => x.Id == id);
                var user = GetUserService.Get(userDTO, Data);
                if (note == null) return AccessCode.NotFound;
                if (!NoteRules.CanEdit(user, note)) return AccessCode.NoPremision;
                Data.NoteRepository.Delete(note);
                Data.SaveChanges();
                return AccessCode.Succsess;
            }
        }

        public CreateResultDTO Create(NoteEditDTO noteEditDTO, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (!NoteRules.CanCreate(user)) return CreateResultDTO.NoPremision;
                var note = new Note { Owner = user };
                EditNote(note, noteEditDTO);
                Data.NoteRepository.Add(note);
                Data.SaveChanges();
                return new CreateResultDTO(note.Id, AccessCode.Succsess);
            }
        }

        public bool CanCreate(UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
                return NoteRules.CanCreate(GetUserService.Get(userDTO, Data));
        }

    }
}
