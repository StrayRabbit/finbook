using System;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Models;
using Contact.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    [Route("api/contacts")]
    public class ContactController : BaseController
    {
        private readonly IContactApplyRequestRepository _contactApplyRequestRepository;
        private readonly IUserService _userService;
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactApplyRequestRepository contactApplyRequestRepository, IUserService userService, IContactRepository contactRepository)
        {
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _userService = userService;
            _contactRepository = contactRepository;
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return Ok(await _contactRepository.GetContactsAsync(UserIdentity.UserId, cancellationToken));
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> Get(int userId, CancellationToken cancellationToken)
        {
            return Ok(await _contactRepository.GetContactsAsync(userId, cancellationToken));
        }

        [HttpPut]
        [Route("tag")]
        public async Task<IActionResult> TagContact([FromBody]ViewModels.TagContactInputViewModel viewModel, CancellationToken cancellationToken)
        {
            var result = await _contactRepository.TagContactAsync(UserIdentity.UserId, viewModel.ContactId, viewModel.Tags, cancellationToken);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-requests")]
        public async Task<IActionResult> GetApplyRequests()
        {
            CancellationToken cancellationToken = new CancellationToken();
            var result = await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId, cancellationToken);
            return Json(result);
        }

        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-requests/{userId}")]
        public async Task<IActionResult> AddApplyRequest(int userId, CancellationToken cancellationToken)
        {
            var userBaseInfo = await _userService.GetBaseUserInfoAsync(userId, cancellationToken);

            if (userBaseInfo == null)
            {
                throw new Exception("用户参数错误");
            }

            var result = await _contactApplyRequestRepository.AddRequestAsync(new ContactApplyRequest()
            {
                UserId = userId,
                ApplierId = UserIdentity.UserId,
                Name = userBaseInfo.Name,
                Company = userBaseInfo.Company,
                Title = userBaseInfo.Title,
                CreateTime = DateTime.Now,
                Avatar = userBaseInfo.Avatar
            }, new CancellationToken());

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// 通过好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("apply-requests/{applierId}")]
        public async Task<IActionResult> ApprovalRequest(int applierId, CancellationToken cancellationToken)
        {
            var result = await _contactApplyRequestRepository.ApprovalAsync(UserIdentity.UserId, applierId, cancellationToken);

            if (!result)
            {
                return BadRequest();
            }

            var applier = await _userService.GetBaseUserInfoAsync(applierId, cancellationToken);
            var user = await _userService.GetBaseUserInfoAsync(UserIdentity.UserId, cancellationToken);

            await _contactRepository.AddContact(UserIdentity.UserId, applier, cancellationToken);
            await _contactRepository.AddContact(applierId, user, cancellationToken);

            return Ok();
        }
    }
}
