using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Wrappers;
using Test.Domain.Model;

namespace Test.Application.Features.User.Command.UserLogin
{
    public class UserLoginCommand : IRequest<Response<UsersViewModel>>
    {
        public UserLoginCommand() { }

        public string username { get; set; }
        public string password { get; set; }
    }

    public class UsersViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string token { get; set; }
    }
}
