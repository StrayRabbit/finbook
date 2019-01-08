using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Project.API.Dtos;

namespace Project.API.Controllers
{
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity
        {
            get
            {
                var identity = new UserIdentity();
                //TBD
                identity.UserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sub").Value ?? "");
                identity.Name = User.Claims.FirstOrDefault(c => c.Type == "name").Value ?? "";
                identity.Company = User.Claims.FirstOrDefault(c => c.Type == "company").Value ?? "";
                identity.Avatar = User.Claims.FirstOrDefault(c => c.Type == "avatar").Value ?? "";
                identity.Title = User.Claims.FirstOrDefault(c => c.Type == "title").Value ?? "";

                return identity;
            }
        }
    }
}
