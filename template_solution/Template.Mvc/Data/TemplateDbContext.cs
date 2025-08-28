using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Template.Mvc.Models;
using System.Collections.Generic;

namespace Template.Mvc.Data
{
    public class TemplateDbContext : IdentityDbContext
    {
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
        {
        }


        //public DbSet<PadelSquare> PadelSquares { get; set; }
        public DbSet<TemplateUser> TemplateUsers { get; set; }
        //public DbSet<Reservation> Reservations { get; set; }
    }
}
