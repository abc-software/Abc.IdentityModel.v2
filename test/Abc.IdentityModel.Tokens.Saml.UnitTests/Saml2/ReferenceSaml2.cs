using Abc.IdentityModel.Tokens.UnitTests;
using Microsoft.IdentityModel.Tokens.Saml2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace Abc.IdentityModel.Tokens.Saml2.UnitTests {
    internal class ReferenceSaml2 {
        public static Saml2Assertion Assertion {
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

                return new Saml2Assertion(new Saml2NameIdentifier(Default.Issuer)) {
                    Conditions = Conditions,
                     IssueInstant = Default.IssueInstant,
                     Subject = Subject,
                    //Signature = signature
                };
            }
        }

        public static Saml2Conditions Conditions {
            get {
                var audienceRestrictions = new Collection<Saml2AudienceRestriction> { new Saml2AudienceRestriction(Default.Audience) };
                return new Saml2Conditions(audienceRestrictions);
            }
        }

        public static Saml2Subject Subject {
            get {
                var subject = new Saml2Subject(new Saml2SubjectConfirmation(new Uri(Default.SamlConfirmationMethod)));
                return subject;
            }
        }

        public static Saml2SecurityToken SecurityToken {
            get => new Saml2SecurityToken(Assertion);
        }
    }
}