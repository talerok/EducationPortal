using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Forum.Edit;
using Education.BLL.DTO.User;
using Education.BLL.Services.ForumServices.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Education.Controllers.Base;
using Education.DAL.Entities;
using Education.Models;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class GroupController : DefaultController
    {
        private IGroupService GroupService;
        private IClaimService ClaimService;

        public GroupController(IGroupService groupService, IClaimService claimService)
        {
            GroupService = groupService;
            ClaimService = claimService;
        }

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        public IActionResult Index(int id)
        {
            var res = GroupService.Read(id, GetUser());
            if (res.Item1 == AccessCode.Succsess) return View(res.Item2);
            else return Redirect(res.Item1);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var res = GroupService.Delete(id, GetUser());
            return Redirect(res);
        }

        [HttpPost]
        public IActionResult Create(GroupRequest groupRequest)
        {
            var groupDTO = new GroupEditDTO
            {
                Name = groupRequest.Name,
                Open = groupRequest.Open,
                Logo = groupRequest.Logo
            };
            var res = GroupService.Create(groupDTO, GetUser());
            if (res.Code == AccessCode.Succsess) return RedirectToAction("Index", new { id = res.Id });
            else return Redirect(res.Code);
        }

        [HttpPost]
        public IActionResult Update(GroupRequest groupRequest)
        {
            var section = new GroupEditDTO
            {
                Name = groupRequest.Name,
                Open = groupRequest.Open,
                Id = groupRequest.Id,
                Logo = groupRequest.Logo
            };
            var res = GroupService.Update(section, GetUser());
            if (res == AccessCode.Succsess)
                return RedirectToAction("Index", new { id = groupRequest.Id });
            else return Redirect(res);
        }

        public IActionResult Users(int id)
        {
            var res = GroupService.GetUsers(id, GetUser());
            if (res.Code == AccessCode.Succsess)
                return View(new UsersOfGroup
                {
                    Data = res.Data,
                    GroupId = id
                });
            else return Redirect(res.Code);
        }

        public IActionResult ControlUser(ControlUser request)
        {
            AccessCode res;
            if (request.Status == Status.Delete)
                res = GroupService.RemoveUser(request.GroupId, request.UserId, GetUser());
            else if (request.Status == Status.Member)
                res = GroupService.ChangeUserRole(request.GroupId, request.UserId, UserGroupStatus.member, GetUser());
            else if (request.Status == Status.Owner)
                res = GroupService.ChangeUserRole(request.GroupId, request.UserId, UserGroupStatus.owner, GetUser());
            else if (request.Status == Status.Request)
                res = GroupService.ChangeUserRole(request.GroupId, request.UserId, UserGroupStatus.request, GetUser());
            else return Redirect(AccessCode.NotFound);
            if (res != AccessCode.Succsess) return Redirect(res);
            else return RedirectToAction("Users", new { id = request.GroupId });
        }

        public IActionResult Enter(int id)
        {
            var res = GroupService.Request(GetUser(), id);
            return Redirect(res);
        }

        public IActionResult Leave(int id)
        {
            var res = GroupService.Leave(GetUser(), id);
            return Redirect(res);
        }

        public IActionResult Control(int id = -1)
        {
            if (id == -1)
            {
                if(GroupService.CanCreate(GetUser()))
                    return View(null);
                else return Redirect(AccessCode.NoPremision);
            }
            var res = GroupService.Read(id, GetUser());
            if (res.Item1 == AccessCode.Succsess && res.Item2.Access.CanUpdate)
                return View(res.Item2);
            else return Redirect(res.Item1);
        }
    }
}