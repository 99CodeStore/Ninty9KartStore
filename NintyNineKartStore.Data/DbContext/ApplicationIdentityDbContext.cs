﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NintyNineKartStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Data
{
    public class ApplicationIdentityDbContext : IdentityDbContext
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}