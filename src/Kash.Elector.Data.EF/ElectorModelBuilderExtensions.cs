using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Data
{
    public static class ElectorModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var election = new Election(1, "Generales 2019", new List<ElectoralList> { });

            modelBuilder.Entity<Election>().HasData(election);
            modelBuilder.Entity<District>().HasData(
                new District(election, 1, "Zaragoza", 7),
                new District(election, 2, "Huesca", 3),
                new District(election, 3, "Teruel", 3)
            );
        }
    }
}
