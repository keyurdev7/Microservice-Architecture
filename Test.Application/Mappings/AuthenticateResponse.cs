using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Features.User.Command.UserLogin;
using Test.Domain.Model;

namespace Test.Application.Mappings
{
    public class AuthenticateResponse
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccessToken { get; set; }

        public AuthenticateResponse(UsersViewModel user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserId = user.UserId;
        }

        
    }
}
