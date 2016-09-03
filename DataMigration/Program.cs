using SaleManagement.Core.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            var databaseInitializer = new SaleManagementDatabaseInitializer();
            Database.SetInitializer(databaseInitializer);
            using (var dbContext = new SaleManagementDbContext())
            {
                dbContext.Database.Initialize(true);
            }
        }
    }
}
