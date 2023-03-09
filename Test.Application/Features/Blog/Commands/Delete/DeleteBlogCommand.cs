using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Wrappers;

namespace Test.Application.Features.Blog.Commands.Delete
{
    public class DeleteBlogCommand: IRequest<Response<string>>
    {
        public DeleteBlogCommand() { }

        public long BlogId { get; set; }
    }
}
