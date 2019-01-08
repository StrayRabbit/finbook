namespace User.Identity.Services
{
    public class AuthCodeService:IAuthCodeService
    {
        public bool Validate(string phone, string authCode)
        {
            return true;
        }
    }
}