using UserManagement.Entity;

namespace UserManagement.Interface
{
    public interface IAuthService
    {
        public string GenerateJwtToken(UserEntity user);
    }
}
