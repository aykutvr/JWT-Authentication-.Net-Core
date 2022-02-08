using JWTAuthentication.Dto;

namespace JWTAuthentication.Services
{
    public interface IUserService
    {
        List<Dto.UserDto> Users();
        Dto.UserDto Get(int id);
        Dto.UserDto Get(string emailAddress);
        Dto.UserDto Get(string emailAddress, string password);
        string Login(Dto.UserDto user);
    }
    public class UserService : IUserService
    {
        public UserDto Get(int id)
            => Users().First(f=> f.Id == id);
        public UserDto Get(string emailAddress)
            => Users().First(f => f.EmailAddress == emailAddress);
        public UserDto Get(string emailAddress, string password)
            => Users().First(f => f.EmailAddress == emailAddress && f.Password == password);
        public string Login(UserDto user)
            => Builders.JWTokenBuilder.Build(user);
        public List<UserDto> Users()
        {
            return new List<UserDto>
            {
                new UserDto{ Id = 1 ,EmailAddress = "aykut.vuruskaner@gmail.com", UserName = "aykutvr",Password = "1234"},
                new UserDto{ Id = 2 ,EmailAddress = "aykut.vuruskaner@hotmail.com", UserName = "aykutvuruskaner", Password = "1234"}
            };
        }
    }
}
