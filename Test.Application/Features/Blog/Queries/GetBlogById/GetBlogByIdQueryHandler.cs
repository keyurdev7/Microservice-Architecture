using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Features.Blog.Queries.GetBlogList;
using Test.Application.Persistence.Blog;
using Test.Application.Wrappers;

namespace Test.Application.Features.Blog.Queries.GetBlogById
{
    internal class GetBlogByIdQueryHandler : IRequestHandler<GetBlogByIdQuery, Response<BlogList>>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public GetBlogByIdQueryHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<Response<BlogList>> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
        {
            var query = await _blogRepository.GetBlogById(request);
            return new Response<BlogList>(_mapper.Map<BlogList>(query));
        }
    }
}
