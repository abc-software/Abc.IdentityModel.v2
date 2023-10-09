// ----------------------------------------------------------------------------
// <copyright file="FindClaims.cs" company="ABC Software Ltd">
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
#if WIF35
namespace Abc.IdentityModel.Extensions {
    using System;
    using System.Collections.Generic;
    using Microsoft.IdentityModel.Claims;

    /// <summary>
    /// Extension methods for IClaimsPrincipal.
    /// </summary>
    public static partial class IClaimsPrincipalExtensions {
        #region Methods

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="predicate">A search predicate.</param>
        /// <returns>
        /// A list of claims that match the search criteria.
        /// </returns>
        /// <remarks>
        /// For compatibility with .NET45.
        /// </remarks>
        public static IEnumerable<Claim> FindAll(this IClaimsPrincipal principal, Predicate<Claim> predicate) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (IClaimsIdentity identity in principal.Identities) {
                foreach (Claim claim in identity.FindClaims(predicate)) {
                    yield return claim;
                }
            }
        }

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="predicate">A search predicate.</param>
        /// <returns>
        /// A list of claims that match the search criteria.
        /// </returns>
        public static IEnumerable<Claim> FindClaims(this IClaimsPrincipal principal, Predicate<Claim> predicate) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (IClaimsIdentity identity in principal.Identities) {
                foreach (Claim claim in identity.FindClaims(predicate)) {
                    yield return claim;
                }
            }
        }

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns>
        /// A list of claims that match the search criteria.
        /// </returns>
        public static IEnumerable<Claim> FindClaims(this IClaimsPrincipal principal, string claimType) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            return principal.FindClaims(
                c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="issuer">The issuer.</param>
        /// <returns>
        /// A list of claims that match the search criteria.
        /// </returns>
        public static IEnumerable<Claim> FindClaims(this IClaimsPrincipal principal, string claimType, string issuer) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (string.IsNullOrEmpty(issuer)) {
                throw new ArgumentNullException(nameof(issuer));
            }

            return principal.FindClaims(
                c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A list of claims that match the search criteria.
        /// </returns>
        public static IEnumerable<Claim> FindClaims(
            this IClaimsPrincipal principal, string claimType, string issuer, string value) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (string.IsNullOrEmpty(issuer)) {
                throw new ArgumentNullException(nameof(issuer));
            }

            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentNullException(nameof(value));
            }

            return principal.FindClaims(
                c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(value, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="claim">The claim.</param>
        /// <returns>
        /// A list of claims that match the search criteria.
        /// </returns>
        public static IEnumerable<Claim> FindClaims(this IClaimsPrincipal principal, Claim claim) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (claim == null) {
                throw new ArgumentNullException(nameof(claim));
            }

            return principal.FindClaims(
                c =>
                c.ClaimType.Equals(claim.ClaimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(claim.Value, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(claim.Issuer, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
#endif