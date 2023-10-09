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
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.IdentityModel.Claims;

    /// <summary>
    /// Extension methods for IClaimsIdentity.
    /// </summary>
    public static partial class IClaimsIdentityExtensions {
        #region Methods

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="predicate">The search predicate.</param>
        /// <returns>
        /// List of claims that match the search criteria.
        /// </returns>
        public static IEnumerable<Claim> FindClaims(this IClaimsIdentity identity, Predicate<Claim> predicate) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            Debug.Assert(identity.Claims != null);

            return from claim in identity.Claims
                   where predicate(claim)
                   select claim;
        }

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns>
        /// List of claims that match the search criteria.
        /// </returns>
        public static IEnumerable<Claim> FindClaims(this IClaimsIdentity identity, string claimType) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            Debug.Assert(identity.Claims != null);

            return identity.FindClaims(
                c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="issuer">The issuer.</param>
        /// <returns>
        /// List of claims that match the search criteria
        /// </returns>
        public static IEnumerable<Claim> FindClaims(this IClaimsIdentity identity, string claimType, string issuer) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (string.IsNullOrEmpty(issuer)) {
                throw new ArgumentNullException(nameof(issuer));
            }

            Debug.Assert(identity.Claims != null);

            return identity.FindClaims(
                c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// List of claims that match the search criteria
        /// </returns>
        public static IEnumerable<Claim> FindClaims(
            this IClaimsIdentity identity, string claimType, string issuer, string value) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
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

            Debug.Assert(identity.Claims != null);

            return identity.FindClaims(
                c =>
                c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(value, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Finds all instances of the specified claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claim">Search claim.</param>
        /// <returns>
        /// List of claims that match the search criteria.
        /// </returns>
        public static IEnumerable<Claim> FindClaims(this IClaimsIdentity identity, Claim claim) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (claim == null) {
                throw new ArgumentNullException(nameof(claim));
            }

            Debug.Assert(identity.Claims != null);

            return identity.FindClaims(
                c =>
                c.ClaimType.Equals(claim.ClaimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(claim.Value, StringComparison.OrdinalIgnoreCase) &&
                c.Issuer.Equals(claim.Issuer, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
#endif