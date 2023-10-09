// ----------------------------------------------------------------------------
// <copyright file="ClaimExists.cs" company="ABC Software Ltd">
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
    using IClaimsPrincipal = System.Security.Claims.ClaimsPrincipal;
    using IClaimsIdentity = System.Security.Claims.ClaimsIdentity;
#endif

    /// <summary>
    /// Extension methods for IClaimsIdentity.
    /// </summary>
    /// <remarks>
    /// The idea of Dominick Baier.
    /// </remarks>
    public static partial class IClaimsIdentityExtensions {
        #region Methods

        /// <summary>
        /// Checks whether a given claim exists
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="predicate">The search predicate.</param>
        /// <returns><c>true</c> id claim exists, otherwise <c>false</c>.</returns>
        public static bool ClaimExists(this IClaimsIdentity identity, Predicate<Claim> predicate) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            Debug.Assert(identity.Claims != null);
#if WIF35
            Claim claim = identity.FindClaims(predicate).FirstOrDefault();
#else
            Claim claim = identity.FindFirst(predicate);
#endif
            return claim != null;
        }

        /// <summary>
        /// Checks whether a given claim exists
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns><c>true</c> id claim exists, otherwise <c>false</c>.</returns>
        public static bool ClaimExists(this IClaimsIdentity identity, string claimType) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            Debug.Assert(identity.Claims != null);

            return identity.ClaimExists(
                c => c != null &&
#if WIF35
                c.ClaimType
#else
                c.Type
#endif
                .Equals(claimType, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks whether a given claim exists
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> id claim exists, otherwise <c>false</c>.</returns>
        public static bool ClaimExists(this IClaimsIdentity identity, string claimType, string value) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentNullException(nameof(value));
            }

            Debug.Assert(identity.Claims != null);

            return identity.ClaimExists(
                c => c != null &&
#if WIF35
                c.ClaimType
#else
                c.Type
#endif
                .Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks whether a given claim exists
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="value">The value.</param>
        /// <param name="issuer">The issuer.</param>
        /// <returns><c>true</c> id claim exists, otherwise <c>false</c>.</returns>
        public static bool ClaimExists(this IClaimsIdentity identity, string claimType, string value, string issuer) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrEmpty(issuer)) {
                throw new ArgumentNullException(nameof(issuer));
            }

            Debug.Assert(identity.Claims != null);

            return identity.ClaimExists(
                c => c != null &&
#if WIF35
                c.ClaimType
#else
                c.Type
#endif
                .Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(value, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
