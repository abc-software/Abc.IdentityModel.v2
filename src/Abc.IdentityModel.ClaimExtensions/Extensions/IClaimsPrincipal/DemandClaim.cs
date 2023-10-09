/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */
#if WIF35
namespace Abc.IdentityModel.Extensions {
    using System;
    using System.Linq;
    using System.Security;
    using Microsoft.IdentityModel.Claims;

    /// <summary>
    /// Extension methods for IClaimsPrincipal.
    /// </summary>
    public static partial class IClaimsPrincipalExtensions {
        #region Methods

        /// <summary>
        /// Demands a specific claim.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <param name="predicate">
        /// The search predicate.
        /// </param>
        public static void DemandClaim(this IClaimsPrincipal principal, Predicate<Claim> predicate) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (principal.FindClaims(predicate).Any()) {
                return;
            }

            throw new SecurityException("Demand for Claim failed.");
        }

        /// <summary>
        /// Demands a specific claim.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <param name="claimType">
        /// Type of the claim.
        /// </param>
        public static void DemandClaim(this IClaimsPrincipal principal, string claimType) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            try {
                principal.DemandClaim(
                    claim =>
                    claim.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase));
            }
            catch (SecurityException) {
                throw new SecurityException($"Demand for Claim {claimType} failed.");
            }
        }

        /// <summary>
        /// Demands a specific claim.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <param name="claimType">
        /// Type of the claim.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void DemandClaim(this IClaimsPrincipal principal, string claimType, string value) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentNullException(nameof(value));
            }

            try {
                principal.DemandClaim(
                    claim =>
                    claim.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                    claim.Value.Equals(value, StringComparison.OrdinalIgnoreCase));
            }
            catch (SecurityException) {
                throw new SecurityException($"Demand for Claim {claimType} failed.");
            }
        }

        /// <summary>
        /// Demands a specific claim.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <param name="claimType">
        /// Type of the claim.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="issuer">
        /// The issuer.
        /// </param>
        public static void DemandClaim(this IClaimsPrincipal principal, string claimType, string value, string issuer) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
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

            try {
                principal.DemandClaim(
                    claim =>
                    claim.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                    claim.Value.Equals(value, StringComparison.OrdinalIgnoreCase) &&
                    claim.Issuer.Equals(issuer, StringComparison.OrdinalIgnoreCase));
            }
            catch (SecurityException) {
                throw new SecurityException($"Demand for Claim {claimType} failed.");
            }
        }

        #endregion
    }
}
#endif