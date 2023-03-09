using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Persistence.Blog;
using Test.Application.Wrappers;
using Test.Application.Message;

namespace Test.Application.Features.Blog.Commands.Upsert
{
    public class UpsertBlogCommandHandler : IRequestHandler<UpsertBlogCommand, Response<string>>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public UpsertBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(UpsertBlogCommand request, CancellationToken cancellationToken)
        {

            await _blogRepository.UpsertBlog(request);

            if (request.BlogId  == 0)
                return new Response<string>(Messages.Insert_Blog_Success);
            else
                return new Response<string>(Messages.Update_Blog_Success);
        }
    }
}
