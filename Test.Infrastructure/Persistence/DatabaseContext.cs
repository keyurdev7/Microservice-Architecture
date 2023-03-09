using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Model;

namespace Test.Infrastructure.Persistence
{
   

    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        { 
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { 
        }

        public DbSet<Blog> Blog { get; set; }
        public DbSet<User> User { get; set; }
    }
}
