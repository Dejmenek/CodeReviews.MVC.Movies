using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movies.Dejmenek.Models;

namespace Movies.Dejmenek.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext (DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movies.Dejmenek.Models.Movie> Movie { get; set; } = default!;
    }
}
