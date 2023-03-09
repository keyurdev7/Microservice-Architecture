using Test.Application.Features.Blog.Queries.GetBlogList;
using Test.Application.Features.Blog.Commands.Upsert;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Test.Application.Features.Blog.Queries.GetBlogById;
using Test.Application.Features.Blog.Commands.Delete;
using Test.API.Middlewares;

namespace Test.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class BlogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("posts", Name = "BlogList")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BlogVm), (int)HttpStatusCode.OK)]
        [RateLimitDecorator(StrategyType = StrategyTypeEnum.IpAddress)]
        public async Task<ActionResult<BlogVm>> List([FromQuery] GetBlogListQuery query)
        {
            var blogList = await _mediator.Send(query);
            return Ok(blogList);
        }

        [HttpGet("posts/{Id}", Name = "BlogById")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BlogVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BlogVm>> GetBlogById(long Id)
        {
            GetBlogByIdQuery query = new GetBlogByIdQuery();
            query.BlogId = Id;
            var blogList = await _mediator.Send(query);
            return Ok(blogList);
        }

        [HttpPost("posts", Name = "AddBlog")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> Add(UpsertBlogCommand query)
        {
            var add = await _mediator.Send(query);
            return Ok(add);
        }

        [HttpPut("posts{Id}", Name = "UpdateBlog")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> Update(long Id, UpsertBlog query)
        {
            UpsertBlogCommand obj = new UpsertBlogCommand();
            obj.BlogId = Id;
            obj.BlogTitle = query.BlogTitle;
            obj.BlogDescription = query.BlogDescription;
            obj.publishedDate = query.publishedDate;
            obj.CreatedUserId = query.CreatedUserId;
            obj.CreatedDate = query.CreatedDate;

            var update = await _mediator.Send(obj);
            return Ok(update);
        }

        [HttpDelete("posts{Id}", Name = "DeleteBlog")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> Delete(long Id)
        {
            DeleteBlogCommand obj = new DeleteBlogCommand();
            obj.BlogId = Id;
            var update = await _mediator.Send(obj);
            return Ok(update);
        }
    }
}
