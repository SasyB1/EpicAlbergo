using EpicAlbergo.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using EpicAlbergo.Models.Dto;

namespace EpicAlbergo.Services
{
    public class UserService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _http;

        public UserService(IConfiguration config, IHttpContextAccessor http)
        {
            _config = config;
            _http = http;
        }

        public User GetUser(UserDto userDto)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    const string SELECT_USER_COMMAND = @"
                SELECT UserId, Username, Password
                FROM Users
                WHERE Username = @Username AND Password = @Password";

                    User user = null;
                    using (SqlCommand cmd = new SqlCommand(SELECT_USER_COMMAND, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", userDto.Username);
                        cmd.Parameters.AddWithValue("@Password", userDto.Password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    UserId = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Password = reader.GetString(2),
                                    Roles = new List<string>()
                                };
                            }
                        }
                    }

                    if (user != null)
                    {
                        const string SELECT_ROLES_COMMAND = @"
                    SELECT r.RoleType
                    FROM UsersRoles ur
                    JOIN Roles r ON ur.RoleId = r.RoleId
                    WHERE ur.UserId = @UserId";

                        using (SqlCommand cmd = new SqlCommand(SELECT_ROLES_COMMAND, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserId", user.UserId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    user.Roles.Add(reader.GetString(0));
                                }
                            }
                        }
                    }

                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore nel recupero dell'utente", ex);
            }
        }


        public async Task Login(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var authProperties = new AuthenticationProperties();

            await _http.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }

        public async Task Logout()
        {
            await _http.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
