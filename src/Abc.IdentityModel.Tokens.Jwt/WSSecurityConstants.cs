// ----------------------------------------------------------------------------
// <copyright file="WSSecurityConstants.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Tokens.Jwt {
    internal class WSSecurityConstants {
        /// <summary>
        /// The WS-security namespaces.
        /// </summary>
        public sealed class Namespaces {
            /// <summary>
            /// The WS-security namespace.
            /// </summary>
            public const string Wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

            /// <summary>
            /// The WS-security utility namespace.
            /// </summary>
            public const string Wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            private Namespaces() {
            }
        }

        /// <summary>
        /// The WS-security prefixes.
        /// </summary>
        public sealed class Prefixes {
            /// <summary>
            /// The WS-security namespace prefix.
            /// </summary>
            public const string Wsse = "wsse";

            /// <summary>
            /// The WS-security namespace prefix.
            /// </summary>
            public const string Wsu = "wsu";

            private Prefixes() {
            }
        }

        internal sealed class AttributeNames {
            public const string Id = "Id";
            public const string ValueType = "ValueType";
            public const string EncodingType = "EncodingType";

            private AttributeNames() {
            }
        }

        internal sealed class ElementNames {
            public const string BinarySecurityToken = "BinarySecurityToken";

            private ElementNames() {
            }
        }

        internal sealed class TokenTypes {
            public const string Jwt = "JWT";
            public const string JwtAlt = "urn:ietf:params:oauth:token-type:jwt";

            private TokenTypes() {
            }
        }

        internal sealed class EncodingTypes {
            public const string Base64Binary = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";

            private EncodingTypes() {
            }
        }
    }
}