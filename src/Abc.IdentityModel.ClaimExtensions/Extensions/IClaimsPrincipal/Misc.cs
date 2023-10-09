// ----------------------------------------------------------------------------
// <copyright file="Misc.cs" company="ABC Software Ltd">
//    Copyright © 2010-2019 ABC Software Ltd. All rights reserved.
//
//    This library is free software; you can redistribute it and/or.
//    modify it under the terms of the GNU Lesser General Public
//    License  as published by the Free Software Foundation, either
//    version 3 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with the library. If not, see http://www.gnu.org/licenses/.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Extensions {
    using System;
    using System.Diagnostics;
#if WIF35
    using Microsoft.IdentityModel.Claims;
#else
    using System.Security.Claims;
    using IClaimsPrincipal = System.Security.Claims.ClaimsPrincipal;
#endif

    /// <summary>
    /// Extension methods for IClaimsPrincipal.
    /// </summary>
    public static partial class IClaimsPrincipalExtensions {
#if WIF35
        /// <summary>
        /// Retrieves the first identity of an IClaimsPrincipal.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The first IClaimsIdentity.</returns>
        public static IClaimsIdentity First(this IClaimsPrincipal principal) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            Debug.Assert(principal.Identities.Count > 0);

            return principal.Identities[0];
        }
#endif

        /// <summary>
        /// Retrieves the issuer name of an IClaimsIdentity.
        /// The algorithm checks the name claim first, and if no name is found, the first claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The issuer name.</returns>
        public static string GetIssuerName(this IClaimsPrincipal principal) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            foreach (var identity in principal.Identities) {
                var isserName = identity.GetIssuerName();
                if (isserName != ClaimsIdentity.DefaultIssuer) {
                    return isserName;
                }
            }

            return ClaimsIdentity.DefaultIssuer;
        }
    }
}
