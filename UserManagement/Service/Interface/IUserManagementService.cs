using UserManagement.Dto;
using UserManagement.Entity;

namespace UserManagement.Service.Interface
{
    public interface IUserManagementService
    {
        void CreateUser(UserRequestDto user, Enums.UserRole role);
        UserEntity[] GetAllUser();
        String Login(string userEmail, string password);
        public UserEntity GetUserByEmail(String email);
        UserEntity GetUserById(int id);
    }
}
