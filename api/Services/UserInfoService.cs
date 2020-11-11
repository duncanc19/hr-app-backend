using CoreJwtExample.IServices;

namespace CoreJwtExample.Services
{
    public class UserInfoService : IUserInfoService
    {
        private List<UserInfo> _users = new List<UserInfo> {
            new UserInfo{ UserInfoId=Guid.NewGuid(), FullName = "Joanna Fawl", Username = "jfawl", Password = "test"};
        };
        private readonly AppSettings _appSettings;

        public UserInfoService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public UserInfo Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x =>
            {
                return x.Username == username && x.Password == password;
            });

            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor

            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.UserInfoId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        public IEnumerable<UserInfo> GetAll()
        {
            return _users;
        }
    }
}