// ----------------------------------------------------------------------------
// <copyright file="ClaimRulesExtension.cs" company="ABC Software Ltd">
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
#if WIF35
    using Microsoft.IdentityModel.Claims;
#else
    using System.Security.Claims;
    using IClaimsIdentity = System.Security.Claims.ClaimsIdentity;
    using IClaimsPrincipal = System.Security.Claims.ClaimsPrincipal;
#endif

    /// <summary>
    /// Claim Rules.
    /// </summary>
    public static class ClaimRulesExtension {
        /// <summary>
        /// Adds the claim to identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="value">The value of the claim.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="originalIssuer">The original issuer.</param>
        /// <param name="nameFormat">The attribute NameFormat or NameIdentifier format.</param>
        public static void AddRule(this IClaimsIdentity identity, string claimType, string value, string valueType = ClaimValueTypes.String, string issuer = null, string originalIssuer = null, string nameFormat = null) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (claimType == null) {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            var claim = new Claim(claimType, value, valueType, issuer, originalIssuer);
            identity.AddRule(claim, nameFormat);
        }

        /// <summary>
        /// Adds the claim to identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="claim">The incoming claim.</param>
        /// <param name="nameFormat">The attribute NameFormat or NameIdentifier format.</param>
        public static void AddRule(this IClaimsIdentity identity, Claim claim, string nameFormat = null) {
            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (claim == null) {
                throw new ArgumentNullException(nameof(claim));
            }

#if WIF35
            var claimType = claim.ClaimType;
#else
            var claimType = claim.Type;
#endif

            if (!string.IsNullOrEmpty(nameFormat)) {
                var propertyName = claimType == ClaimTypes.NameIdentifier ? ClaimProperties.SamlNameIdentifierFormat : ClaimProperties.SamlAttributeNameFormat;
                claim.Properties[propertyName] = nameFormat;
            }

            identity.AddClaim(claim);
        }

        /// <summary>
        /// Adds the claim to claim list.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="value">The value of the claim.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="originalIssuer">The original issuer.</param>
        /// <param name="nameFormat">The attribute NameFormat or NameIdentifier format.</param>
        public static void AddRule(this IList<Claim> claims, string claimType, string value, string valueType = ClaimValueTypes.String, string issuer = null, string originalIssuer = null, string nameFormat = null) {
            if (claims == null) {
                throw new ArgumentNullException(nameof(claims));
            }

            if (claimType == null) {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            var claim = new Claim(claimType, value, valueType, issuer, originalIssuer);
            claims.AddRule(claim, nameFormat);
        }

        /// <summary>
        /// Adds the claim.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="claim">The incoming claim.</param>
        /// <param name="nameFormat">The attribute NameFormat or NameIdentifier format.</param>
        public static void AddRule(this IList<Claim> claims, Claim claim, string nameFormat = null) {
            if (claims == null) {
                throw new ArgumentNullException(nameof(claims));
            }

            if (claim == null) {
                throw new ArgumentNullException(nameof(claim));
            }

#if WIF35
            var claimType = claim.ClaimType;
#else
            var claimType = claim.Type;
#endif

            if (!string.IsNullOrEmpty(nameFormat)) {
                var propertyName = claimType == ClaimTypes.NameIdentifier ? ClaimProperties.SamlNameIdentifierFormat : ClaimProperties.SamlAttributeNameFormat;
                claim.Properties[propertyName] = nameFormat;
            }

            claims.Add(claim);
        }

        /// <summary>
        /// Transforms the rule.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="identity">The identity.</param>
        /// <param name="predicate">The search predicate.</param>
        /// <param name="outgoingClaimType">Type of the outgoing claim.</param>
        /// <param name="outgoingClaimValueType">Type of the outgoing claim value.</param>
        /// <param name="outgoingClaimIssuer">The outgoing claim issuer.</param>
        /// <param name="outgoingClaimNameFormat">The outgoing claim name format.</param>
        /// <returns>
        /// The outgoing claim value if incoming claim found, <c>null</c> otherwise.
        /// </returns>
        public static string TransformRule(this IList<Claim> claims, IClaimsIdentity identity, Predicate<Claim> predicate, string outgoingClaimType = null, string outgoingClaimValueType = null, string outgoingClaimIssuer = null, string outgoingClaimNameFormat = null) {
            if (claims == null) {
                throw new ArgumentNullException(nameof(claims));
            }

            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            Debug.Assert(identity.Claims != null);

            string claimValue = null;
            foreach (var claim in identity.Claims.Where(p => predicate(p))) {
                claimValue = claim.Value;
                if (string.IsNullOrEmpty(outgoingClaimType)
                    && string.IsNullOrEmpty(outgoingClaimValueType)
                    && string.IsNullOrEmpty(outgoingClaimIssuer)
                    && string.IsNullOrEmpty(outgoingClaimNameFormat)) {
                    claims.AddRule(claim);
                }
                else {
                    claims.AddRule(outgoingClaimType ??
#if WIF35
                        claim.ClaimType,
#else
                        claim.Type,
#endif
                        claimValue,
                        outgoingClaimValueType ?? claim.ValueType,
                        outgoingClaimIssuer ?? claim.Issuer,
                        claim.OriginalIssuer,
                        outgoingClaimNameFormat);
                }
            }

            return claimValue;
        }

        /// <summary>
        /// Transforms the rule.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="identity">The identity.</param>
        /// <param name="incomingClaimType">Type of the incoming claim.</param>
        /// <param name="outgoingClaimType">Type of the outgoing claim.</param>
        /// <param name="outgoingClaimValueType">Type of the outgoing claim value.</param>
        /// <param name="outgoingClaimIssuer">The outgoing claim issuer.</param>
        /// <param name="outgoingClaimNameFormat">The outgoing claim name format.</param>
        /// <returns>
        /// The outgoing claim value if incoming claim found, <c>null</c> otherwise.
        /// </returns>
        public static string TransformRule(this IList<Claim> claims, IClaimsIdentity identity, string incomingClaimType, string outgoingClaimType = null, string outgoingClaimValueType = null, string outgoingClaimIssuer = null, string outgoingClaimNameFormat = null) {
            if (claims == null) {
                throw new ArgumentNullException(nameof(claims));
            }

            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            Debug.Assert(identity.Claims != null);

            if (string.IsNullOrEmpty(incomingClaimType)) {
                throw new ArgumentNullException(nameof(incomingClaimType));
            }

            return claims.TransformRule(
                identity,
#if WIF35
                c => c.ClaimType
#else
                c => c.Type
#endif
                    .Equals(incomingClaimType, StringComparison.OrdinalIgnoreCase),
                        outgoingClaimType, outgoingClaimValueType, outgoingClaimIssuer, outgoingClaimNameFormat);
        }

        /// <summary>
        /// Passes the through rule.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="identity">The identity.</param>
        /// <param name="predicate">The search predicate.</param>
        /// <returns>
        /// The outgoing claim value if incoming claim found, <c>null</c> otherwise.
        /// </returns>
        public static string PassThroughRule(this IList<Claim> claims, IClaimsIdentity identity, Predicate<Claim> predicate) {
            if (claims == null) {
                throw new ArgumentNullException(nameof(claims));
            }

            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            Debug.Assert(identity.Claims != null);

            return claims.TransformRule(identity, predicate, null, null, null);
        }

        /// <summary>
        /// Passes the through rule.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="identity">The identity.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns>
        /// The outgoing claim value if incoming claim found, <c>null</c> otherwise.
        /// </returns>
        public static string PassThroughRule(this IList<Claim> claims, IClaimsIdentity identity, string claimType) {
            if (claims == null) {
                throw new ArgumentNullException(nameof(claims));
            }

            if (identity == null) {
                throw new ArgumentNullException(nameof(identity));
            }

            Debug.Assert(identity.Claims != null);

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            return claims.PassThroughRule(
                identity,
#if WIF35
                c => c.ClaimType
#else
                c => c.Type
#endif
                    .Equals(claimType, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Transforms the rule.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="predicate">The search predicate.</param>
        /// <param name="outgoingClaimType">Type of the outgoing claim.</param>
        /// <param name="outgoingClaimValueType">Type of the outgoing claim value.</param>
        /// <param name="outgoingClaimIssuer">The outgoing claim issuer.</param>
        /// <param name="outgoingClaimNameFormat">The outgoing claim name format.</param>
        /// <returns>
        /// The outgoing claim value if incoming claim found, <c>null</c> otherwise.
        /// </returns>
        public static string TransformRule(this IList<Claim> claims, IClaimsPrincipal principal, Predicate<Claim> predicate, string outgoingClaimType = null, string outgoingClaimValueType = null, string outgoingClaimIssuer = null, string outgoingClaimNameFormat = null) {
            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            string claimValue = null;
            foreach (var claim in principal
#if WIF35
                .FindClaims(predicate)
#else
                .FindAll(predicate)
#endif
                ) {
                claimValue = claim.Value;
                if (string.IsNullOrEmpty(outgoingClaimType)
                    && string.IsNullOrEmpty(outgoingClaimValueType)
                    && string.IsNullOrEmpty(outgoingClaimIssuer)
                    && string.IsNullOrEmpty(outgoingClaimNameFormat)) {
                    claims.AddRule(claim);
                }
                else {
                    claims.AddRule(outgoingClaimType ??
#if WIF35
                        claim.ClaimType,
#else
                        claim.Type,
#endif
                        claimValue,
                        outgoingClaimValueType ?? claim.ValueType,
                        outgoingClaimIssuer ?? claim.Issuer,
                        claim.OriginalIssuer,
                        outgoingClaimNameFormat);
                }
            }

            return claimValue;
        }

        /// <summary>
        /// Transforms the rule.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="incomingClaimType">Type of the incoming claim.</param>
        /// <param name="outgoingClaimType">Type of the outgoing claim.</param>
        /// <param name="outgoingClaimValueType">Type of the outgoing claim value.</param>
        /// <param name="outgoingClaimIssuer">The outgoing claim issuer.</param>
        /// <param name="outgoingClaimNameFormat">The outgoing claim name format.</param>
        /// <returns>
        /// The outgoing claim value if incoming claim found, <c>null</c> otherwise.
        /// </returns>
        public static string TransformRule(this IList<Claim> claims, IClaimsPrincipal principal, string incomingClaimType, string outgoingClaimType = null, string outgoingClaimValueType = null, string outgoingClaimIssuer = null, string outgoingClaimNameFormat = null) {
            if (claims == null) {
                throw new ArgumentNullException(nameof(claims));
            }

            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(incomingClaimType)) {
                throw new ArgumentNullException(nameof(incomingClaimType));
            }

            return claims.TransformRule(
                principal,
#if WIF35
                c => c.ClaimType
#else
                c => c.Type
#endif
                    .Equals(incomingClaimType, StringComparison.OrdinalIgnoreCase),
                        outgoingClaimType, outgoingClaimValueType, outgoingClaimIssuer, outgoingClaimNameFormat);
        }

        /// <summary>
        /// Passes the through rule.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="predicate">The search predicate.</param>
        /// <returns>
        /// The outgoing claim value if incoming claim found, <c>null</c> otherwise.
        /// </returns>
        public static string PassThroughRule(this IList<Claim> claims, IClaimsPrincipal principal, Predicate<Claim> predicate) {
            if (claims == null) {
                throw new ArgumentNullException(nameof(claims));
            }

            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            return claims.TransformRule(principal, predicate, null, null, null);
        }

        /// <summary>
        /// Passes the through rule.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns>
        /// The outgoing claim value if incoming claim found, <c>null</c> otherwise.
        /// </returns>
        public static string PassThroughRule(this IList<Claim> claims, IClaimsPrincipal principal, string claimType) {
            if (claims == null) {
                throw new ArgumentNullException(nameof(claims));
            }

            if (principal == null) {
                throw new ArgumentNullException(nameof(principal));
            }

            if (string.IsNullOrEmpty(claimType)) {
                throw new ArgumentNullException(nameof(claimType));
            }

            return claims.PassThroughRule(
                principal,
#if WIF35
                c => c.ClaimType
#else
                c => c.Type
#endif
                    .Equals(claimType, StringComparison.OrdinalIgnoreCase));
        }
    }
}