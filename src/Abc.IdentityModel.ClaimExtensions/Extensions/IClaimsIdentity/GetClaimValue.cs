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
    using System.Xml;
#if WIF35
    using Microsoft.IdentityModel.Claims;
#else
    using System.Security.Claims;
    using IClaimsPrincipal = System.Security.Claims.ClaimsPrincipal;
    using IClaimsIdentity = System.Security.Claims.ClaimsIdentity;
#endif

    /// <summary>
    /// Extension methods for IClaimsIdentity
    /// </summary>
    /// <remarks>
    /// The idea of Dominick Baier.
    /// </remarks> 
    public static partial class IClaimsIdentityExtensions {
        #region Methods

        /// <summary>
        /// Retrieves the value of a claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns>The claim value.</returns>
        /// <exception cref="ClaimNotFoundException">If claim not found.</exception>
        public static string GetClaimValue(this IClaimsIdentity identity, string claimType) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            Debug.Assert(identity.Claims != null);

            string value;
            if (identity.TryGetClaimValue(claimType, out value)) {
                return value;
            }

            throw new ClaimNotFoundException(claimType);
        }

        /// <summary>
        /// Retrieves the value of a claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="issuer">The issuer.</param>
        /// <returns>The claim value</returns>
        /// <exception cref="ClaimNotFoundException">If claim not found.</exception>
        public static string GetClaimValue(this IClaimsIdentity identity, string claimType, string issuer) {
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

            string value;
            if (identity.TryGetClaimValue(claimType, issuer, out value)) {
                return value;
            }

            throw new ClaimNotFoundException(claimType);
        }

        /// <summary>
        /// Tries to retrieve the value of a claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns><c>true</c> if found claim type, otherwise <c>false</c>.</returns>
        public static bool TryGetClaimValue(this IClaimsIdentity identity, string claimType, out string claimValue) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            Debug.Assert(identity.Claims != null);

            claimValue = null;
#if WIF35
            Claim claim = identity.FindClaims(claimType).FirstOrDefault();
#else
            Claim claim = identity.FindFirst(claimType);
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
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns><c>true</c> if found claim type, otherwise <c>false</c>.</returns>
        public static bool TryGetClaimValue(
            this IClaimsIdentity identity, string claimType, string issuer, out string claimValue) {
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

            claimValue = null;
#if WIF35
            Claim claim = identity.FindClaims(claimType, issuer).FirstOrDefault();
#else
            Claim claim = identity.FindFirst(c => c != null && c.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase)
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
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns><c>true</c> if found claim type, otherwise <c>false</c>.</returns>
        public static bool TryGetClaimObject(this IClaimsIdentity identity, string claimType, out object claimValue) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            Debug.Assert(identity.Claims != null);

            claimValue = null;
#if WIF35
            Claim claim = identity.FindClaims(claimType).FirstOrDefault();
#else
            Claim claim = identity.FindFirst(claimType);
#endif

            if (claim != null) {
                claimValue = IClaimsIdentityExtensions.Convert(claim.Value, claim.ValueType);
            }

            return claimValue != null;
        }

        #endregion

        internal static object Convert(string claimValue, string claimType) {
            object obj = null;
            try {
                switch (claimType) {
                    case ClaimValueTypes.Boolean:
                        obj = XmlConvert.ToBoolean(claimValue);
                        break;
                    case ClaimValueTypes.Integer:
                        obj = XmlConvert.ToDecimal(claimValue);
                        break;
#if !WIF35
                    case ClaimValueTypes.Integer32:
                        obj = XmlConvert.ToInt32(claimValue);
                        break;
                    case ClaimValueTypes.Integer64:
                        obj = XmlConvert.ToInt64(claimValue);
                        break;
                    case ClaimValueTypes.UInteger32:
                        obj = XmlConvert.ToUInt32(claimValue);
                        break;
                    case ClaimValueTypes.UInteger64:
                        obj = XmlConvert.ToUInt64(claimValue);
                        break;
#endif

                    case ClaimValueTypes.Date:
#if WIF35
                    case ClaimValueTypes.Datetime:
#else
                    case ClaimValueTypes.DateTime:
#endif
                        obj = XmlConvert.ToDateTime(claimValue, XmlDateTimeSerializationMode.RoundtripKind);
                        break;
                    case ClaimValueTypes.Double:
                        obj = XmlConvert.ToDouble(claimValue);
                        break;
                    case ClaimValueTypes.String:
                        obj = claimValue;
                        break;
                }
            }
            catch (FormatException) {
                // do noting
            }

            return obj;
        }
    }
}
