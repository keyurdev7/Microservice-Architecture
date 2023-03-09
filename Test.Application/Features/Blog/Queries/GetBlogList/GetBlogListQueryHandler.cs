using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Persistence.Blog;
using Test.Application.Wrappers;

namespace Test.Application.Features.Blog.Queries.GetBlogList
{
    public class GetBlogListQueryHandler : IRequestHandler<GetBlogListQuery, Response<BlogVm>>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public GetBlogListQueryHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<Response<BlogVm>> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
        {
            var query = await _blogRepository.GetBlogs(request);
            return new Response<BlogVm>(_mapper.Map<BlogVm>(query));
        }
    }
}
