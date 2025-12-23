using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModalLayer.Entities;

namespace DataAcessLayer.Data
{
   public class FundooDBContext :DbContext
    {
        public FundooDBContext( DbContextOptions<FundooDBContext> context) :base(context) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Notes> Notes { get; set; } 
    }
}
