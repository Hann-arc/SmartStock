using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartStockAI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
         public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? Guid.Parse(claim.Value) : Guid.Empty;
        }

        public static bool TryGetUserId(this ClaimsPrincipal principal, out Guid userId)
        {
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null && Guid.TryParse(claim.Value, out userId))
            {
                return true;
            }

            userId = Guid.Empty;
            return false;
        }
    }
}