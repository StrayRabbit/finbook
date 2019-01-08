using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.API.Data;
using User.API.Dtos;
using User.API.Models;

namespace User.API.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly UserContext _userContext;
        private readonly ILogger<UserController> _logger;
        private readonly ICapPublisher _capPublisher;

        public UserController(UserContext userContext, ILogger<UserController> logger, ICapPublisher capPublisher)
        {
            _userContext = userContext;
            _logger = logger;
            _capPublisher = capPublisher;
        }

        private void RaiseUserprofileChangedEvent(AppUser user)
        {
            if (_userContext.Entry(user).Property(nameof(user.Name)).IsModified ||
                _userContext.Entry(user).Property(nameof(user.Title)).IsModified ||
                _userContext.Entry(user).Property(nameof(user.Company)).IsModified ||
                _userContext.Entry(user).Property(nameof(user.Avatar)).IsModified
            )
            {
                _capPublisher.Publish("finbook_userapi_userprofilechanged", new UserProfileChangedEvent
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Avatar = user.Avatar,
                    Title = user.Title,
                });
            }
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);

            if (user == null)
            {
                _logger.LogError($"错误的用户上下文id{UserIdentity.UserId}");
                throw new Exceptions.UserOperationException($"错误的用户上下文id{UserIdentity.UserId}");
            }

            return Json(user);
        }

        [Route("")]
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]JsonPatchDocument<Models.AppUser> patch)
        {
            var user = await _userContext.Users
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);

            patch.ApplyTo(user);

            //foreach (var property in user.Properties)
            //{
            //    _userContext.Entry(property).State = EntityState.Detached;
            //}

            //var originProperties = await _userContext.UserProperties.AsNoTracking()
            //    .Where(u => u.AppUserId == UserIdentity.UserId).ToListAsync();
            //var allProperties = originProperties.Union(user.Properties).Distinct();

            //var removedProperties = originProperties.Except(user.Properties);
            //var newProperties = allProperties.Except(originProperties);

            //foreach (var property in removedProperties)
            //{
            //    _userContext.Remove(property);
            //}

            //foreach (var property in newProperties)
            //{
            //    _userContext.Add(property);
            //}

            using (var transaction = _userContext.Database.BeginTransaction())
            {
                //发布用户变更消息
                RaiseUserprofileChangedEvent(user);

                _userContext.Users.Update(user);
                _userContext.SaveChanges();

                transaction.Commit();
            }



            return Json(user);
        }

        [Route("check-or-create")]
        [HttpPost]
        public async Task<IActionResult> CheckOrCreate(string phone)
        {
            //TBD 做手机号的格式认证
            var user = await _userContext.Users.SingleOrDefaultAsync(u => u.Phone == phone);

            if (user == null)
            {
                user = new AppUser() { Phone = phone };
                _userContext.Add(user);
                await _userContext.SaveChangesAsync();
            }

            return Ok(new { UserId = user.Id, user.Name, user.Company, user.Title, user.Avatar });
        }

        /// <summary>
        /// 获取用户标签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("tags")]
        public async Task<IActionResult> GetUserTags()
        {
            return Ok(await _userContext.UserTags.Where(u => u.UserId == UserIdentity.UserId).ToListAsync());
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(string phone)
        {
            return Ok(await _userContext.Users.Include(u => u.Properties).SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId));
        }

        /// <summary>
        /// 更新用户标签数据
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("tags")]
        public async Task<IActionResult> UpdateUserTags([FromBody]List<string> tags)
        {
            var originTags = await _userContext.UserTags.Where(u => u.UserId == UserIdentity.UserId).ToListAsync();
            var newTags = tags.Except(originTags.Select(t => t.Tag));

            await _userContext.UserTags.AddRangeAsync(newTags.Select(t => new UserTag
            {
                CreateTime = DateTime.Now,
                UserId = UserIdentity.UserId,
                Tag = t,
            }));

            await _userContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// 获取指定userId的用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("baseinfo/{userId}")]
        public async Task<IActionResult> GetBaseUserInfo(int userId)
        {
            var user = await _userContext.Users.AsTracking()
                .SingleOrDefaultAsync(u => u.Id == userId);
            //（使用当前用户的id）获取当前用户，一般非用户界面的获取，而是其他代码的获取，不能获取到时 需要异常处理
            if (user == null)
            {
                _logger.LogError($"GetBaseUserInfo 没有获取到id{UserIdentity.UserId}的信息");
            }
            return Json(new
            {
                UserId = user.Id,
                user.Name,
                user.Company,
                user.Title,
                user.Avatar
            });
        }
    }
}