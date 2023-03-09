using Test.Application.Features.Blog.Commands.Delete;
using Test.Application.Features.Blog.Commands.Upsert;
using Test.Application.Features.Blog.Queries.GetBlogById;
using Test.Application.Features.Blog.Queries.GetBlogList;

namespace Test.Application.Persistence.Blog
{
    public interface IBlogRepository
    {
        Task UpsertBlog(UpsertBlogCommand query);
        Task<BlogVm> GetBlogs(GetBlogListQuery query);
        Task<BlogList> GetBlogById(GetBlogByIdQuery query);
        Task DeleteBlogById(DeleteBlogCommand query);
    }
}
