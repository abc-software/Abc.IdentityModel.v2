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
    using System.Linq;
#if WIF35
    using Microsoft.IdentityModel.Claims;
#else
    using System.Security.Claims;
    using IClaimsIdentity = System.Security.Claims.ClaimsIdentity;
#endif

    /// <summary>
    /// Extension methods for IClaimsIdentity.
    /// </summary>
    public static partial class IClaimsIdentityExtensions {
        /// <summary>
        /// Retrieves the issuer name of an IClaimsIdentity.
        /// The algorithm checks the name claim first, and if no name is found, the first claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns>The issuer name.</returns>
        public static string GetIssuerName(this IClaimsIdentity identity) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            Debug.Assert(identity.Claims != null);

            var claim = identity.Claims.FirstOrDefault(c => c.Issuer != ClaimsIdentity.DefaultIssuer);
            if (claim != null && claim.Issuer != null) {
                return claim.Issuer;
            }

            return ClaimsIdentity.DefaultIssuer;
        }

#if WIF35
        /// <summary>
        /// Adds a single claim to this claims identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claim">The claim.</param>
        public static void AddClaim(this IClaimsIdentity identity, Claim claim) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (claim == null) {
                throw new ArgumentNullException(nameof(claim));
            }

            Debug.Assert(identity.Claims != null);

            identity.Claims.Add(claim);
        }
#endif
    }
}