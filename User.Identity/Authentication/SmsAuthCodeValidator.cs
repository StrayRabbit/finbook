using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using User.Identity.Services;

namespace User.Identity.Authentication
{
    public class SmsAuthCodeValidator : IExtensionGrantValidator
    {
        private readonly IUserService _userService;
        private readonly IAuthCodeService _authCodeService;

        public SmsAuthCodeValidator(IUserService userService, IAuthCodeService authCodeService)
        {
            _userService = userService;
            _authCodeService = authCodeService;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];

            var errorValidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
            {
                context.Result = errorValidationResult;
                return;
            }

            //检查验证码
            if (!_authCodeService.Validate(phone, code))
            {
                context.Result = errorValidationResult;
                return;
            }

            //完成用户注册
            var user = await _userService.CheckOrCreate(phone);
            if (user == null)
            {
                context.Result = errorValidationResult;
                return;
            }

            Claim[] claims = new Claim[]{
                    new Claim("name", user.Name??string.Empty),
                    new Claim("company", user.Company??string.Empty),
                    new Claim("title", user.Title??string.Empty),
                    new Claim("avatar", user.Avatar??string.Empty)
                };
            //获取用户信息 放入 Claim[], 也可以 在 ProfileService中的验证方法中 获取用户信息 放入 Claim[]
            //这里 已经获取了用户信息，所有直接就完成上述操作
            context.Result = new GrantValidationResult(user.UserId.ToString(), GrantType, claims);
        }

        public string GrantType => "sms_auth_code";
    }
}