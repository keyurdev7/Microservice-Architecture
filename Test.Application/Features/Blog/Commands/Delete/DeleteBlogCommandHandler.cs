using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Features.Blog.Commands.Upsert;
using Test.Application.Message;
using Test.Application.Persistence.Blog;
using Test.Application.Wrappers;

namespace Test.Application.Features.Blog.Commands.Delete
{
    public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, Response<string>>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public DeleteBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            await _blogRepository.DeleteBlogById(request);
            return new Response<string>(Messages.Delete_Blog_Success);
        }
    }
}
