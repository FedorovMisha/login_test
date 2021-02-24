using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace AccountWeb.Infrastructure
{

    public class ClaimsTransformationLite : IClaimsTransformation
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public ClaimsTransformationLite(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var clone = principal.Clone();
            
            var newIdentity = (ClaimsIdentity)clone.Identity;
            
            var nameId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameId == null)
            {
                return principal;
            }
 
            // Get user from database
            var user = await _userManager.FindByIdAsync(nameId.Value);
            if (user == null)
            {
                return principal;
            }

            newIdentity?.AddClaim(new Claim(ClaimTypes.UserData, user.DateRegistration.ToString("h:mm:ss tt zz")));
            return clone;
        }
    }
}