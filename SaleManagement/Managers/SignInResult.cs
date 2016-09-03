using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Managers
{
    public enum SignInResult
    {
        Failure,
        Success,
        LockedOut,
        Disabled,
        Unverified,
        Rejected
    }
}
