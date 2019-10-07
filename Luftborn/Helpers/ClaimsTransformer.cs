using Microsoft.AspNetCore.Authentication;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Luftborn.Helpers
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // This will run every time Authenticate is called so its better to create a new Principal
            var transformed = new ClaimsPrincipal();
            transformed.AddIdentities(principal.Identities);
            transformed.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim("Transformed", Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture))
            }));
            return Task.FromResult(transformed);
        }
    }
}
