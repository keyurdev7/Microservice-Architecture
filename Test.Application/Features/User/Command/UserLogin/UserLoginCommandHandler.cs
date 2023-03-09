using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Features.Blog.Commands.Upsert;
using Test.Application.Features.Blog.Queries.GetBlogList;
using Test.Application.Mappings;
using Test.Application.Message;
using Test.Application.Persistence.Blog;
using Test.Application.Persistence.User;
using Test.Application.Wrappers;
using Test.Domain.Model;

namespace Test.Application.Features.User.Command.UserLogin
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, Response<UsersViewModel>>
    {
        private readonly IUserRespository _userRespository;
        private readonly IMapper _mapper;

        public UserLoginCommandHandler(IUserRespository userRespository, IMapper mapper)
        {
            _userRespository = userRespository;
            _mapper = mapper;
        }

        public async Task<Response<UsersViewModel>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {

            var query = await _userRespository.UserLogin(request);
            return new Response<UsersViewModel>(_mapper.Map<UsersViewModel>(query));

            //new AuthenticateResponse(_mapper.Map<UsersViewModel>(query))
        }
    }
}
