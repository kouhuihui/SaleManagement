using SaleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SaleManagement.Managers
{
    public class SerialNumberManager : BaseManager
    {
        public SerialNumberManager(SaleUser user) : base(user)
        {

        }

        public async Task<string> NextSNAsync(string serialName)
        {
            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.Serializable };
            string code;
            using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOption,
                TransactionScopeAsyncFlowOption.Enabled))
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@companyId", User.CompanyId) ,
                    new SqlParameter("@name", serialName)
                };
                var serialNumber = await ((DbContext)DbContext).Database.SqlQuery<SerialNumber>("Select TOP 1 * FROM dbo.SerialNumbers WITH(updlock) where CompanyId = @companyId and Name=@name", parameters).FirstOrDefaultAsync();
                if (serialNumber == null)
                {
                    serialNumber = new SerialNumber()
                    {
                        SN = 1,
                        CompanyId = User.CompanyId,
                        Name = serialName
                    };
                }
                else
                {
                    serialNumber.SN = serialNumber.SN + 1;
                    serialNumber.Updated = DateTime.Now;
                }

                DbContext.Set<SerialNumber>().AddOrUpdate(serialNumber);
                DbContext.SaveChanges();

                code = serialNumber.SN.ToString("000000000");
                scope.Complete();
            }

            return code;
        }
    }
}
