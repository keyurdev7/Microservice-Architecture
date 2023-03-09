using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Features.Blog.Queries.GetBlogList;
using Test.Application.Features.User.Command.UserLogin;
using Test.Domain.Model;

namespace Test.Application.Persistence.User
{
    public interface IUserRespository
    {
        Task<UsersViewModel> UserLogin(UserLoginCommand query);
    }
}
