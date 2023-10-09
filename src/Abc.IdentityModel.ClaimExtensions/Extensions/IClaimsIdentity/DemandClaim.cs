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
    using System.Diagnostics;
    using System.Security;
    using Microsoft.IdentityModel.Claims;

    /// <summary>
    /// Extension methods for IClaimsIdentity.
    /// </summary>
    public static partial class IClaimsIdentityExtensions {
        #region Methods

        /// <summary>
        /// Demands a specific claim.
        /// </summary>
        /// <param name="identity">
        /// The principal.
        /// </param>
        /// <param name="predicate">
        /// The search predicate.
        /// </param>
        public static void DemandClaim(this IClaimsIdentity identity, Predicate<Claim> predicate) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            Debug.Assert(identity.Claims != null);

            if (!identity.ClaimExists(predicate)) {
                throw new SecurityException("Demand for Claim failed");
            }
        }

        /// <summary>
        /// Demands a specific claim.
        /// </summary>
        /// <param name="identity">
        /// The identity.
        /// </param>
        /// <param name="claimType">
        /// Type of the claim.
        /// </param>
        public static void DemandClaim(this IClaimsIdentity identity, string claimType) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            Debug.Assert(identity.Claims != null);

            try {
                identity.DemandClaim(
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
        /// <param name="identity">
        /// The identity.
        /// </param>
        /// <param name="claimType">
        /// Type of the claim.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void DemandClaim(this IClaimsIdentity identity, string claimType, string value) {
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

            try {
                identity.DemandClaim(
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
        /// <param name="identity">
        /// The identity.
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
        public static void DemandClaim(this IClaimsIdentity identity, string claimType, string value, string issuer) {
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

            try {
                identity.DemandClaim(
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