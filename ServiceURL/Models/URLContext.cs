using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceURL.Models
{
    public class URLContext:DbContext
    {
        public URLContext()
        {
        }

        public URLContext(DbContextOptions<URLContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Link> Link { get; set; }
        
    }
}
