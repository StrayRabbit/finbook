using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace User.Identity.Authentication
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            if (!int.TryParse(subjectId, out int intUserId))
            {
                throw new ArgumentNullException(nameof(context.Subject));
            }

            context.IssuedClaims = context.Subject.Claims.ToList();
            

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            context.IsActive = int.TryParse(subjectId, out int intUserId);
            return Task.CompletedTask;
        }
    }
}