using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Features.Blog.Commands.Delete;
using Test.Application.Features.Blog.Commands.Upsert;
using Test.Application.Features.Blog.Queries.GetBlogById;
using Test.Application.Features.Blog.Queries.GetBlogList;
using Test.Application.Persistence.Blog;
using Test.Domain.Model;
using Test.Infrastructure.Persistence;
using Test.Infrastructure.Repositories.Base;

namespace Test.Infrastructure.Repositories.Blog
{
    public class BlogRepository : BaseRepository, IBlogRepository
    {
        private readonly DatabaseContext _context;

        public BlogRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<BlogVm> GetBlogs(GetBlogListQuery query)
        {
            var lst = _context.Blog.Include(x => x.User).Where(x => x.IsDeleted == false)
             .Select(x => new BlogList()
             {
                 BlogId = x.BlogId,
                 BlogTitle = x.BlogTitle,
                 BlogDescription = x.BlogDescription,
                 publishedDate = x.publishedDate,
                 CreatedBy = x.User.FirstName + " " + x.User.LastName,
                 CreatedDate = x.CreatedDate
             }).ToList();

            BlogVm vm = new BlogVm();
            vm.Blogs = lst;
            return vm;
        }

        public async Task UpsertBlog(UpsertBlogCommand query)
        {
            if (query.BlogId == 0)
            {
                Test.Domain.Model.Blog blog = new Domain.Model.Blog();
                blog.BlogTitle = query.BlogTitle;
                blog.BlogDescription = query.BlogDescription;
                blog.publishedDate = query.publishedDate;
                blog.CreatedUserId = query.CreatedUserId;
                blog.CreatedDate = query.CreatedDate;
                _context.Blog.Add(blog);
            }
            else
            {
                var blog = _context.Blog.FirstOrDefault(m => m.BlogId == query.BlogId);
                blog.BlogTitle = query.BlogTitle;
                blog.BlogDescription = query.BlogDescription;
                blog.publishedDate = query.publishedDate;
                blog.CreatedUserId = query.CreatedUserId;
                blog.CreatedDate = query.CreatedDate;
            }

            _context.SaveChanges();
        }

        public async Task DeleteBlogById(DeleteBlogCommand query)
        {
            var blog = _context.Blog.FirstOrDefault(m => m.BlogId == query.BlogId);
            blog.IsDeleted = true;
            _context.SaveChanges();
        }

        public async Task<BlogList> GetBlogById(GetBlogByIdQuery query)
        {
            var result = _context.Blog.Include(x => x.User)
             .Select(x => new BlogList()
             {
                 BlogId = x.BlogId,
                 BlogTitle = x.BlogTitle,
                 BlogDescription = x.BlogDescription,
                 publishedDate = x.publishedDate,
                 CreatedBy = x.User.FirstName + " " + x.User.LastName,
                 CreatedDate = x.CreatedDate
             }).Where(x => x.BlogId == query.BlogId).FirstOrDefault();


            return result;
        }

    }
}
