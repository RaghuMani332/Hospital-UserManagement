using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace UserManagement.context
{
    public class UserManagementDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly String Connection;

        public UserManagementDapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Connection = _configuration.GetConnectionString("SqlConnection");
        }
        public IDbConnection GetConnection()
        {
            return new SqlConnection(Connection);
        }
    }
}
