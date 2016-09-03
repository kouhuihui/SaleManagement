using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dickson.Logging.EnterpriseLibrary
{
    public static class EnterpriseLibraryLoggerFactoryExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory)
        {
            factory.AddProvider(new EnterpriseLibraryLoggerProvider());
            return factory;
        }
    }
}
