using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Wrappers;

namespace Test.Application.Features.Blog.Commands.Upsert
{
    public class UpsertBlog
    {
        public string BlogTitle { get; set; }
        public string BlogDescription { get; set; }
        public DateTime publishedDate { get; set; }
        public long CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class UpsertBlogCommand : IRequest<Response<string>>
    {
        public UpsertBlogCommand() { }
        
        public long BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogDescription { get; set; }
        public DateTime publishedDate { get; set; }
        public long CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }   
    }
}
