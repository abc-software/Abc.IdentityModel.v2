using Abc.IdentityModel.Tokens.UnitTests;
using Microsoft.IdentityModel.Tokens.Saml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Abc.IdentityModel.Tokens.Saml.UnitTests {
    internal class ReferenceSaml {
        public static SamlAssertion Assertion {
            get {
                //var reference = new Reference {
                //    CanonicalizingTransfrom = new ExclusiveCanonicalizationTransform(),
                //    DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256",
                //    DigestValue = "JaDhvSguu/XZ8jZmh7KmhbOr4deZB4/iL1adETm9oPc=",
                //    Prefix = "",
                //    TokenStream = Default.TokenStream,
                //    Uri = "#091a00cc-4361-4303-9f1a-d4be45b2b84c"
                //};

                //reference.Transforms.Add(new EnvelopedSignatureTransform());
                //var signature = new Signature {
                //    KeyInfo = new KeyInfo(KeyingMaterial.AADSigningCert),
                //    SignatureValue = "NRV7REVbDRflg616G6gYg0fAGTEw8BhtyPzqaU+kPQI35S1vpgt12VlQ57PkY7Rs0Jucx9npno+bQVMKN2DNhhnzs9qoNY2V3TcdJCcwaMexinHoFXHA0+J6+vR3RWTXhX+iAnfudtKThqbh/mECRLrjyTdy6L+qNkP7sALCWrSVwJVRmzkTOUF8zG4AKY9dQziec94Zv4S7G3cFgj/i7ok2DfBi7AEMCu1lh3dsQAMDeCvt7binhIH2D2ad3iCfYyifDGJ2ncn9hIyxrEiBdS8hZzWijcLs6+HQhVaz9yhZL9u/ZxSRaisXClMdqrLFjUghJ82sVfgQdp7SF165+Q==",
                //    SignedInfo = new SignedInfo(reference) {
                //        Prefix = ""
                //    }
                //};

                return new SamlAssertion(Default.SamlAssertionID, Default.Issuer, Default.IssueInstant, Conditions, null, new Collection<SamlStatement> { AttributeStatement }) {
                    //Signature = signature
                };
            }
        }

        public static SamlConditions Conditions {
            get {
                var audiences = new Uri(Default.Audience);
                var conditions = new Collection<SamlCondition> { new SamlAudienceRestrictionCondition(audiences) };
                return new SamlConditions(Default.NotBefore, Default.NotOnOrAfter, conditions);
            }
        }

        public static SamlSubject Subject {
            get {
                var subject = new SamlSubject();
                subject.ConfirmationMethods.Add(Default.SamlConfirmationMethod);
                return subject;
            }
        }

        public static SamlAttributeStatement AttributeStatement {
            get => GetAttributeStatement(Subject, Default.DefaultClaims);
        }

        public static SamlSecurityToken SecurityToken {
            get => new SamlSecurityToken(Assertion);
        }

        public static SamlAttributeStatement GetAttributeStatement(SamlSubject subject, IEnumerable<Claim> claims) {
            string defaultNamespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims";
            Collection<SamlAttribute> attributes = new Collection<SamlAttribute>();
            foreach (var claim in claims) {
                string type = claim.Type;
                string name = type;
                if (type.Contains("/")) {
                    int lastSlashIndex = type.LastIndexOf('/');
                    name = type.Substring(lastSlashIndex + 1);
                }

                type = defaultNamespace;

                string value = claim.Value;
                SamlAttribute attribute = new SamlAttribute(type, name, claim.Value);
                attributes.Add(attribute);
            }

            return new SamlAttributeStatement(subject, attributes);
        }
    }
}