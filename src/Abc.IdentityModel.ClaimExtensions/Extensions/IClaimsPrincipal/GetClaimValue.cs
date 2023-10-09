// ----------------------------------------------------------------------------
// <copyright file="GetClaimValue.cs" company="ABC Software Ltd">
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
    /// Extension methods for IClaimsPrincipal
    /// </summary>
    /// <remarks>
    /// The idea of Dominick Baier.
    /// </remarks>
    public static partial class IClaimsPrincipalExtensions {
        #region Methods

        /// <summary>
        /// Retrieves the value of a claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns>The claim value.</returns>
        /// <exception cref="ClaimNotFoundException">If claim not found.</exception>
        public static string GetClaimValue(this IClaimsPrincipal principal, string claimType) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            Debug.Assert(principal.Identities != null);

            string value;
            foreach (var identity in principal.Identities) {
                if (identity.TryGetClaimValue(claimType, out value)) {
                    return value;
                }
            }

            throw new ClaimNotFoundException(claimType);
        }

        /// <summary>
        /// Retrieves the value of a claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="issuer">The issuer.</param>
        /// <returns>The claim value.</returns>
        /// <exception cref="ClaimNotFoundException">If claim not found.</exception>
        public static string GetClaimValue(this IClaimsPrincipal principal, string claimType, string issuer) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (string.IsNullOrEmpty(issuer)) {
                throw new ArgumentNullException(nameof(issuer));
            }

            Debug.Assert(principal.Identities != null);

            string value;
            foreach (var identity in principal.Identities) {
                if (identity.TryGetClaimValue(claimType, issuer, out value)) {
                    return value;
                }
            }

            throw new ClaimNotFoundException(claimType);
        }

        /// <summary>
        /// Retrieves the object of a claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns>The claim value.</returns>
        /// <exception cref="ClaimNotFoundException">If claim not found.</exception>
        public static object GetClaimObject(this IClaimsPrincipal principal, string claimType) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            Debug.Assert(principal.Identities != null);

            object value;
            foreach (var identity in principal.Identities) {
                if (identity.TryGetClaimObject(claimType, out value)) {
                    return value;
                }
            }

            throw new ClaimNotFoundException(claimType);
        }

        /// <summary>
        /// Tries to retrieve the value of a claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns><c>true</c> if found claim type, otherwise <c>false</c>.</returns>
        public static bool TryGetClaimValue(this IClaimsPrincipal principal, string claimType, out string claimValue) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            Debug.Assert(principal.Identities != null);

            claimValue = null;
#if WIF35
            Claim claim = principal.FindClaims(claimType).FirstOrDefault();
#else
            Claim claim = principal.FindFirst(claimType);
#endif

            if (claim != null) {
                claimValue = claim.Value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to retrieve the value of a claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns><c>true</c> if found claim type, otherwise <c>false</c>.</returns>
        public static bool TryGetClaimValue(
            this IClaimsPrincipal principal, string claimType, string issuer, out string claimValue) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (string.IsNullOrEmpty(issuer)) {
                throw new ArgumentNullException(nameof(issuer));
            }

            Debug.Assert(principal.Identities != null);

            claimValue = null;
#if WIF35
            Claim claim = principal.FindClaims(claimType, issuer).FirstOrDefault();
#else
            Claim claim = principal.FindFirst(c => c != null && c.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase)
                && c.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
#endif

            if (claim != null) {
                claimValue = claim.Value;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Tries to retrieve the object of a claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns>
        ///   <c>true</c> if found claim type, otherwise <c>false</c>.
        /// </returns>
        public static bool TryGetClaimObject(this IClaimsPrincipal principal, string claimType, out object claimValue) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            Debug.Assert(principal.Identities != null);

            claimValue = null;
#if WIF35
            Claim claim = principal.FindClaims(claimType).FirstOrDefault();
#else
            Claim claim = principal.FindFirst(claimType);
#endif

            if (claim != null) {
                claimValue = IClaimsIdentityExtensions.Convert(claim.Value, claim.ValueType);
            }

            return claimValue != null;
        }
        #endregion
    }
}