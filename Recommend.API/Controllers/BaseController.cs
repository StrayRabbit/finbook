using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Recommend.API.Dtos;

namespace Recommend.API.Controllers
{
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity
        {
            get
            {
                var identity = new UserIdentity
                {
                    //TBD
                    UserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sub").Value ?? ""),
                    Name = User.Claims.FirstOrDefault(c => c.Type == "name").Value ?? "",
                    Company = User.Claims.FirstOrDefault(c => c.Type == "company").Value ?? "",
                    Avatar = User.Claims.FirstOrDefault(c => c.Type == "avatar").Value ?? "",
                    Title = User.Claims.FirstOrDefault(c => c.Type == "title").Value ?? ""
                };

                return identity;
            }
        }
    }
}
