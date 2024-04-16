using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using UserManagement.context;
using UserManagement.CustomException;
using UserManagement.Dto;
using UserManagement.Entity;
using UserManagement.Enums;
using UserManagement.Interface;
using UserManagement.Service.Interface;

namespace UserManagement.Service
{
    public class UserManagementServiceImpl : IUserManagementService
    {
        private readonly UserManagementDapperContext context;
        private readonly IAuthService authService;
       
        public UserManagementServiceImpl(UserManagementDapperContext context, IAuthService authService)
        {
            this.authService = authService;
            this.context = context;
           }
        private UserEntity mapToUser(UserRequestDto userRequestDto, Enums.UserRole role)
        {
            return new UserEntity()
            {
                FirstName = userRequestDto.FirstName,
                LastName = userRequestDto.LastName,
                Email = userRequestDto.Email,
                Password = EncryptPassword(userRequestDto.Password),
                Role=role

            };
        }
        private String EncryptPassword(String password)
        {
            byte[] b=Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(b);
        }
        private String DecryptPassword(String password)
        {
            

            byte[] b = Convert.FromBase64String(password);
            
          
            return Encoding.UTF8.GetString(b);
        }
        public void CreateUser(UserRequestDto user, Enums.UserRole role)
        {
           IDbConnection con=context.GetConnection();
            try
            {
               int nora= con.Execute("InsertUsers", mapToUser(user,role), commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                throw new DuplicateEntryException("Email Id Already Present");
            }
        }
        public UserEntity GetUserByEmail(String email)
        {
            IDbConnection conn=context.GetConnection();
           var v= conn.Query<UserEntity>("Select * from Users where Email= @Mail", new { Mail = email }).FirstOrDefault();
            if (v != null)
            {
                v.Password=DecryptPassword(v.Password);
            }
            return v;
        }
        public string Login(string userEmail, string password)
        { 
            UserEntity e = GetUserByEmail(userEmail);
            return  e == null ? throw new UserNotFoundException("UserNotFoundByEmailId") : e.Password.Equals(password)?authService.GenerateJwtToken(e):throw new PasswordMissMatchException("InvalidPassword");
        }

        public UserEntity[] GetAllUser()
        {
            IDbConnection con= context.GetConnection();
             var v=con.Query<UserEntity>("Select * from Users").ToArray();
            foreach(UserEntity e in v)
            {
                e.Password =DecryptPassword(e.Password);
            }
            return v;
        }

        public UserEntity GetUserById(int id)
        {
            IDbConnection conn = context.GetConnection();
            var v = conn.Query<UserEntity>("Select * from Users where UserId= @UserId", new { UserId = id }).FirstOrDefault();
            if (v != null)
            {
                v.Password = DecryptPassword(v.Password);
            }
            return v;
        }
    }
}
