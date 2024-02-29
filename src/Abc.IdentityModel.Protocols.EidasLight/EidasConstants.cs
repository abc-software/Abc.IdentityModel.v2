// ----------------------------------------------------------------------------
// <copyright file="EidasConstants.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System;
    using System.Diagnostics.CodeAnalysis;

#pragma warning disable 1591

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Constants is not commented.")]
    [ExcludeFromCodeCoverage]
    public class EidasConstants {
        /// <summary>
        /// eIDAS SAML Message Format [eIDAS SAML, 1,1]
        /// </summary>
        public sealed class Namespaces {
            /// <summary>
            /// The eIDAS SAML2 extension.
            /// </summary>
            public const string Assertion = "http://eidas.europa.eu/saml-extensions";

            /// <summary>
            /// The eIDAS natural extension.
            /// </summary>
            public const string EidasNatural = Eidas + "/naturalperson";

            /// <summary>
            /// The eIDAS legal extension.
            /// </summary>
            public const string EidasLegal = Eidas + "/legalperson";

            internal const string Eidas = "http://eidas.europa.eu/attributes";

            private Namespaces() {
            }
        }

        /// <summary>
        /// eIDAS SAML Message Format [eIDAS SAML, 1,1]
        /// </summary>
        public sealed class Prefixes {
            /// <summary>
            /// The eIDAS SAML2 extension prefix.
            /// </summary>
            public const string Assertion = "eidas";

            /// <summary>
            /// The eIDAS natural extension prefix.
            /// </summary>
            public const string EidasNatural = "eidas-natural";

            /// <summary>
            /// The eIDAS legal extension prefix.
            /// </summary>
            public const string EidasLegal = "eidas-legal";

            private Prefixes() {
            }
        }

        /// <summary>
        /// Natural and legal person representative [eIDAS SAML, 3.2]
        /// </summary>
        public sealed class AuthenticationContextClasses {
            public const string LevelOfAssuranceLowString = "http://eidas.europa.eu/LoA/low";
            public const string LevelOfAssurancesubSubstantialString = "http://eidas.europa.eu/LoA/substantial";
            public const string LevelOfAssuranceHighString = "http://eidas.europa.eu/LoA/high";

            public static readonly Uri LevelOfAssuranceLow = new Uri(LevelOfAssuranceLowString);
            public static readonly Uri LevelOfAssurancesubSubstantial = new Uri(LevelOfAssurancesubSubstantialString);
            public static readonly Uri LevelOfAssuranceHigh = new Uri(LevelOfAssuranceHighString);

            private AuthenticationContextClasses() {
            }
        }

        /// <summary>
        /// Natural and legal person representative [eIDAS-Attr-Profile, 2.8]
        /// </summary>
        public sealed class ClaimTypes {
            private const string Representative = "/representative";

            // Natural Person Attribute Definitions
            public const string PersonIdentifier = Namespaces.EidasNatural + "/PersonIdentifier";
            public const string FamilyName = Namespaces.EidasNatural + "/CurrentFamilyName";
            public const string FirstName = Namespaces.EidasNatural + "/CurrentGivenName";
            public const string DateOfBirth = Namespaces.EidasNatural + "/DateOfBirth";
            public const string BirthName = Namespaces.EidasNatural + "/BirthName";
            public const string PlaceOfBirth = Namespaces.EidasNatural + "/PlaceOfBirth";
            public const string CurrentAddress = Namespaces.EidasNatural + "/CurrentAddress";
            public const string Gender = Namespaces.EidasNatural + "/Gender";

            // Legal Person Attribute Definitions
            public const string LegalPersonIdentifier = Namespaces.EidasLegal + "/LegalPersonIdentifier";
            public const string LegalName = Namespaces.EidasLegal + "/LegalName";
            public const string LegalAddress = Namespaces.EidasLegal + "/LegalPersonAddress";
            public const string VATRegistrationNumber = Namespaces.EidasLegal + "/VATRegistrationNumber";
            public const string TaxReference = Namespaces.EidasLegal + "/TaxReference";
            public const string EUIdentifier = Namespaces.EidasLegal + "/D-2012-17-EUIdentifier";
            public const string LEI = Namespaces.EidasLegal + "/LEI";
            public const string EORI = Namespaces.EidasLegal + "/EORI";
            public const string SEED = Namespaces.EidasLegal + "/SEED";
            public const string SIC = Namespaces.EidasLegal + "/SIC";

            // Representative Natural Person Attribute Definitions
            public const string RepresentativePersonIdentifier = Namespaces.EidasNatural + Representative + "/PersonIdentifier";
            public const string RepresentativeFamilyName = Namespaces.EidasNatural + Representative + "/CurrentFamilyName";
            public const string RepresentativeFirstName = Namespaces.EidasNatural + Representative + "/CurrentGivenName";
            public const string RepresentativeDateOfBirth = Namespaces.EidasNatural + Representative + "/DateOfBirth";
            public const string RepresentativeBirthName = Namespaces.EidasNatural + Representative + "/BirthName";
            public const string RepresentativePlaceOfBirth = Namespaces.EidasNatural + Representative + "/PlaceOfBirth";
            public const string RepresentativeCurrentAddress = Namespaces.EidasNatural + Representative + "/CurrentAddress";
            public const string RepresentativeGender = Namespaces.EidasNatural + Representative + "/Gender";

            // Representative Legal Person Attribute Definitions
            public const string RepresentativeLegalPersonIdentifier = Namespaces.EidasLegal + Representative + "/LegalPersonIdentifier";
            public const string RepresentativeLegalName = Namespaces.EidasLegal + Representative + "/LegalName";
            public const string RepresentativeLegalAddress = Namespaces.EidasLegal + Representative + "/LegalAddress";
            public const string RepresentativeVATRegistrationNumber = Namespaces.EidasLegal + Representative + "/VATRegistrationNumber";
            public const string RepresentativeTaxReference = Namespaces.EidasLegal + Representative + "/TaxReference";
            public const string RepresentativeEUIdentifier = Namespaces.EidasLegal + Representative + "/D-2012-17-EUIdentifier";
            public const string RepresentativeLEI = Namespaces.EidasLegal + Representative + "/LEI";
            public const string RepresentativeEORI = Namespaces.EidasLegal + Representative + "/EORI";
            public const string RepresentativeSEED = Namespaces.EidasLegal + Representative + "/SEED";
            public const string RepresentativeSIC = Namespaces.EidasLegal + Representative + "/SIC";

            private ClaimTypes() {
            }
        }

        public sealed class ClaimValueTypes {
            // Natural Person Attribute Types
            public const string PersonIdentifierType = Namespaces.EidasNatural + "#PersonIdentifierType";
            public const string FamilyNameType = Namespaces.EidasNatural + "#CurrentFamilyNameType";
            public const string FirstNameType = Namespaces.EidasNatural + "#CurrentGivenNameType";
            public const string DateOfBirthType = Namespaces.EidasNatural + "#DateOfBirthType";
            public const string BirthNameType = Namespaces.EidasNatural + "#BirthNameType";
            public const string PlaceOfBirthType = Namespaces.EidasNatural + "#PlaceOfBirthType";
            public const string CurrentAddressType = Namespaces.EidasNatural + "#CurrentAddressType";
            public const string GenderType = Namespaces.EidasNatural + "#GenderType";

            // Legal Person Attribute Types
            public const string LegalPersonIdentifierType = Namespaces.EidasLegal + "#LegalPersonIdentifierType";
            public const string LegalNameType = Namespaces.EidasLegal + "#LegalNameType";
            public const string LegalAddressType = Namespaces.EidasLegal + "#LegalPersonAddressType";
            public const string VATRegistrationNumberType = Namespaces.EidasLegal + "#VATRegistrationNumberType";
            public const string TaxReferenceType = Namespaces.EidasLegal + "#TaxReferenceType";
            public const string EUIdentifierType = Namespaces.EidasLegal + "#D-2012-17-EUIdentifierType";
            public const string LEIType = Namespaces.EidasLegal + "#LEIType";
            public const string EORIType = Namespaces.EidasLegal + "#EORIType";
            public const string SEEDType = Namespaces.EidasLegal + "#SEEDType";
            public const string SICType = Namespaces.EidasLegal + "#SICType";

            private ClaimValueTypes() {
            }
        }

        internal sealed class AttributeNames {
            public const string Name = "Name";
            public const string NameFormat = "NameFormat";
            public const string FriendlyName = "FriendlyName";
            public const string IsRequired = "isRequired";
            public const string LatinScript = "LatinScript";

            private AttributeNames() {
            }
        }

        internal sealed class ElementNames {
            public const string SPType = "SPType";
            public const string RequestedAttributes = "RequestedAttributes";
            public const string RequestedAttribute = "RequestedAttribute";
            public const string AttributeValue = "AttributeValue";
            public const string PoBox = "PoBox";
            public const string LocatorDesignator = "LocatorDesignator";
            public const string LocatorName = "LocatorName";
            public const string CvAddressArea = "CvaddressArea";
            public const string Thoroughfare = "Thoroughfare";
            public const string PostName = "PostName";
            public const string AdminUnitFirstline = "AdminunitFirstline";
            public const string AdminUnitSecondline = "AdminunitSecondline";
            public const string PostCode = "PostCode";

            private ElementNames() {
            }
        }

        internal sealed class XmlTypes {
            public const string PersonIdentifierType = "PersonIdentifierType";
            public const string CurrentFamilyNameType = "CurrentFamilyNameType";
            public const string CurrentGivenNameType = "CurrentGivenNameType";
            public const string DateOfBirthType = "DateOfBirthType";
            public const string BirthNameType = "BirthNameType";
            public const string PlaceOfBirthType = "PlaceOfBirthType";
            public const string CurrentAddressType = "CurrentAddressType";
            public const string GenderType = "GenderType";

            public const string LegalPersonIdentifierType = "LegalPersonIdentifierType";
            public const string LegalNameType = "LegalNameType";
            public const string ATRegistrationNumberType = "ATRegistrationNumberType";
            public const string TaxReferenceType = "TaxReferenceType";
            public const string EUIdentifierType = "D-2012-17-EUIdentifierType";
            public const string LEIType = "LEIType";
            public const string EORIType = "EORIType";
            public const string SEEDType = "SEEDType";
            public const string SICType = "SICType";

            public const string RequestedAttributeType = "RequestedAttributeType";

            private XmlTypes() {
            }
        }
    }
#pragma warning restore 1591
}