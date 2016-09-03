using SaleManagement.Core.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core.Services
{
   public class SaleManagementSerivce: IDisposable
    {
        protected DbContext m_DbContext;

        public void Dispose()
        {
            if (m_DbContext != null)
            {
                m_DbContext.Dispose();
            }
        }

        public DbContext DbContext
        {
            get
            {
                if (m_DbContext == null)
                {
                    m_DbContext = new SaleManagementDbContext();
                }

                return m_DbContext;
            }
            set
            {
                m_DbContext = value;
            }
        }
    }
}
