using APITemplate.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace APITemplate.Model.DatabaseContext
{
    public class APITemplateContext : DbContext
    {
        public APITemplateContext(DbContextOptions<APITemplateContext> options)
          : base(options)
        { }

        public DbSet<TestModel> TestModel { get; set; }
    }
}
