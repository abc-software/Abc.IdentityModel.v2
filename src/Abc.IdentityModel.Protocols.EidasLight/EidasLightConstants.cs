// ----------------------------------------------------------------------------
// <copyright file="EidasLightConstants.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Constants is not commented.")]
    [ExcludeFromCodeCoverage]
    public class EidasLightConstants {
        internal sealed class Namespaces {
            internal const string Request = "http://cef.eidas.eu/LightRequest";
            internal const string Response = "http://cef.eidas.eu/LightResponse";
        }

        internal sealed class Prefixes {
            internal const string Request = ""; // EIDAS does not support prefixes
            internal const string Response = ""; // EIDAS does not support prefixes
        }

        internal sealed class ElementNames {
            public const string SPType = "spType";
            public const string RequestedAttributes = "requestedAttributes";
            public const string LightResponse = "lightResponse";
            public const string LightRequest = "lightRequest";
            public const string Id = "id";
            public const string Issuer = "issuer";
            public const string RelayState = "relayState";
            public const string RequesterId = "requesterId";
            public const string SPCountryCode = "spCountryCode";
            public const string ProviderName = "providerName";
            public const string NameIdFormat = "nameIdFormat";
            public const string CitizenCountryCode = "citizenCountryCode";
            public const string InResponseToId = "inResponseToId";
            public const string Consent = "consent";
            public const string IpAddress = "ipAddress";
            public const string Subject = "subject";
            public const string SubjectNameIdFormat = "subjectNameIdFormat";
            public const string LevelOfAssurance = "levelOfAssurance";
            public const string Attributes = "attributes";
            public const string Status = "status";
            public const string Failure = "failure";
            public const string StatusCode = "statusCode";
            public const string SubStatusCode = "subStatusCode";
            public const string StatusMessage = "statusMessage";
            public const string Attribute = "attribute";
            public const string Definition = "definition";
            public const string Value = "value";

            private ElementNames() {
            }

        }

        internal sealed class AttributeNames {
            public const string Type = "type";

            private AttributeNames() {
            }
        }

        internal sealed class Parameters {
            public const string Token = "token";

            private Parameters() {
            }
        }

        //public class NameIdFormats {

        //}

        //public class Consents {

        //}

        //public class LevelsOfAssurance {

        //}
    }
}

