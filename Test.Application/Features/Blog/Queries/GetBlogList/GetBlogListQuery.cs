using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Wrappers;

namespace Test.Application.Features.Blog.Queries.GetBlogList
{
    public partial class GetBlogListQuery : IRequest<Response<BlogVm>>
    {
        public GetBlogListQuery() { }
       
    }
}
