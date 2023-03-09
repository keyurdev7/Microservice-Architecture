using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Features.Blog.Queries.GetBlogList;
using Test.Application.Wrappers;

namespace Test.Application.Features.Blog.Queries.GetBlogById
{
    public class GetBlogByIdQuery : IRequest<Response<BlogList>>
    {
        public GetBlogByIdQuery() { }
        public long BlogId { get; set; }

    }
}
