using Test.Application.Features.Blog.Queries.GetBlogList;
using Test.Application.Features.Blog.Commands.Upsert;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Test.Application.Features.User.Command.UserLogin;
using Test.Domain.Model;

namespace Test.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("Login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(UsersViewModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UsersViewModel>> List([FromQuery] UserLoginCommand query)
        {
            var blogList = await _mediator.Send(query);
            return Ok(blogList);
        }
    }
}
