using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dickson.Library.Security.Principal
{
    public static class BaseClaimTypes
    {
        public static readonly string Version = "urn:identity:claims:version";
        public static readonly string AvatarId = "urn:identity:claims:avatarid";
        public static readonly string SignedDate = "urn:identity:claims:signeddate";
        public static readonly string FullName = "urn:identity:claims:fullname";
    }
}
