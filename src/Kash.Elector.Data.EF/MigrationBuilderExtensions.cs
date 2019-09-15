using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Data
{
    public static class MigrationBuilderExtensions
    {
        public static MigrationBuilder SeedInitialData(this MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT Elections ON 
                GO
                INSERT INTO Elections (Id, Name) VALUES (1, 'Generales 2019')
                GO
                SET IDENTITY_INSERT Elections OFF
                GO
                SET IDENTITY_INSERT Districts ON 
                GO
                INSERT INTO Districts (Id, ElectionId, Name, Seats) VALUES (1, 1, 'Zaragoza', 7)
                GO
                INSERT INTO Districts (Id, ElectionId, Name, Seats) VALUES (2, 1, 'Huesca', 3)
                GO
                INSERT INTO Districts (Id, ElectionId, Name, Seats) VALUES (3, 1, 'Teruel', 3)
                GO
                SET IDENTITY_INSERT Districts OFF
                GO");

            return migrationBuilder;
        }
    }
}
