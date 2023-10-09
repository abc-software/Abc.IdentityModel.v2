using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Abc.IdentityModel.Tokens.Jwt.UnitTests {
    internal class ReferenceJwt {

        public static string JwtToken_Valid =
            "<wsse:BinarySecurityToken ValueType=\"urn:ietf:params:oauth:token-type:jwt\" EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">ZXlKaGJHY2lPaUpTVXpJMU5pSXNJbXRwWkNJNklrWXdNakZGT1VJMk16VTVOalF4T1VRMk5rUkZPVE13TlRCRU5UazVORVpCUmpjd09EVkVSRGdpTENKMGVYQWlPaUpLVjFRaWZRLmV5SnVZVzFsYVdRaU9pSkNiMklpTENKbGJXRnBiQ0k2SWtKdllrQmpiMjUwYjNOdkxtTnZiU0lzSW1kcGRtVnVYMjVoYldVaU9pSkNiMklpTENKdVltWWlPakUyTkRFd016RXlNREFzSW1WNGNDSTZNVFkwTVRBek5EZ3dNQ3dpYVdGMElqb3hOamsyT0RRNU5qa3hMQ0pwYzNNaU9pSjFjbTQ2YVhOemRXVnlJaXdpWVhWa0lqb2lkWEp1T21GMVpHbGxibU5sSW4wLg==</wsse:BinarySecurityToken>";

        public static string JwtTokenAlt_Valid =
            "<wsse:BinarySecurityToken ValueType=\"JWT\" EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">ZXlKaGJHY2lPaUpTVXpJMU5pSXNJbXRwWkNJNklrWXdNakZGT1VJMk16VTVOalF4T1VRMk5rUkZPVE13TlRCRU5UazVORVpCUmpjd09EVkVSRGdpTENKMGVYQWlPaUpLVjFRaWZRLmV5SnVZVzFsYVdRaU9pSkNiMklpTENKbGJXRnBiQ0k2SWtKdllrQmpiMjUwYjNOdkxtTnZiU0lzSW1kcGRtVnVYMjVoYldVaU9pSkNiMklpTENKdVltWWlPakUyTkRFd016RXlNREFzSW1WNGNDSTZNVFkwTVRBek5EZ3dNQ3dpYVdGMElqb3hOamsyT0RRNU5qa3hMQ0pwYzNNaU9pSjFjbTQ2YVhOemRXVnlJaXdpWVhWa0lqb2lkWEp1T21GMVpHbGxibU5sSW4wLg==</wsse:BinarySecurityToken>";

        public static string JwtToken_Invalid =
            "<wsse:BinarySecurityToken ValueType=\"JWT\" EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">ZXlKaGJHY2lPaUpTVXpJMU5pSXNJbXRwWkNJNklrWXdNakZGT1VJMk16VTVOalF4T1VRMk5rUkZPVE13TlRCRU5UazVORVpCUmpjd09EVkVSRGdpTENKMGVYQWlPaUpLVjFRaWZRLmV5SnVZVzFsYVdRaU9pSkNiMklpTENKbGJXRnBiQ0k2SWtKdllrQmpiMjUwYjNOdkxtTnZiU0lzSW1kcGRtVnVYMjVoYldVaU9pSkNiMklpTENKdVltWWlPakUyTkRFd016RXlNREFzSW1WNGNDSTZNVFkwTVRBek5EZ3dNQ3dpYVdGMElqb3hOamsyT0RRNU5qa3hMQ0pwYzNNaU9pSjFjbTQ2YVhOemRXVnlJaXdpWVhWa0lqb2lkWEp1T21GMVpHbGxibU5sSW4wLg==</wsse:BinarySecurityToken>";

        public static string Saml2Token_Valid =
            @"<Assertion ID = ""_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2"" IssueInstant=""2017-03-20T15:52:31.957Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""><Issuer>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</Issuer><Signature xmlns=""http://www.w3.org/2000/09/xmldsig#""><SignedInfo><CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /><SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" /><Reference URI=""#_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2""><Transforms><Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" /><Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /></Transforms><DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" /><DigestValue>Ytfkc60mLe1Zgu7TBQpMv8nJ1SVxT0ZjsFHaFqSB2VI=</DigestValue></Reference></SignedInfo><SignatureValue>NRV7REVbDRflg616G6gYg0fAGTEw8BhtyPzqaU+kPQI35S1vpgt12VlQ57PkY7Rs0Jucx9npno+bQVMKN2DNhhnzs9qoNY2V3TcdJCcwaMexinHoFXHA0+J6+vR3RWTXhX+iAnfudtKThqbh/mECRLrjyTdy6L+qNkP7sALCWrSVwJVRmzkTOUF8zG4AKY9dQziec94Zv4S7G3cFgj/i7ok2DfBi7AEMCu1lh3dsQAMDeCvt7binhIH2D2ad3iCfYyifDGJ2ncn9hIyxrEiBdS8hZzWijcLs6+HQhVaz9yhZL9u/ZxSRaisXClMdqrLFjUghJ82sVfgQdp7SF165+Q==</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate></X509Data></KeyInfo></Signature><Subject><NameID Format=""urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"">RrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s</NameID><SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"" /></Subject><Conditions NotBefore=""2017-03-20T15:47:31.957Z"" NotOnOrAfter=""2017-03-20T16:47:31.957Z""><AudienceRestriction><Audience>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=""http://schemas.microsoft.com/identity/claims/tenantid""><AttributeValue>add29489-7269-41f4-8841-b63c95564420</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/objectidentifier""><AttributeValue>d1ad9ce7-b322-4221-ab74-1e1011e1bbcb</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name""><AttributeValue>User1@Cyrano.onmicrosoft.com</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname""><AttributeValue>User</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/displayname""><AttributeValue>User1</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/identityprovider""><AttributeValue>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</AttributeValue></Attribute></AttributeStatement><AuthnStatement AuthnInstant=""2017-03-20T15:52:31.551Z""><AuthnContext><AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef></AuthnContext></AuthnStatement></Assertion>";

        public static SamlAssertion SamlAssertion {
            get {
                var subject = new SamlSubject();
                subject.ConfirmationMethods.Add(Default.SamlConfirmationMethod);

                string defaultNamespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims";
                Collection<SamlAttribute> attributes = new Collection<SamlAttribute>();
                foreach (var claim in Default.DefaultClaims) {
                    string type = claim.Type;
                    string name = type;
                    if (type.Contains("/")) {
                        int lastSlashIndex = type.LastIndexOf('/');
                        name = type.Substring(lastSlashIndex + 1);
                    }

                    type = defaultNamespace;

                    SamlAttribute attribute = new SamlAttribute(type, name, claim.Value);
                    attributes.Add(attribute);
                }

                return new SamlAssertion(Default.SamlAssertionID, Default.Issuer, DateTime.Parse(Default.IssueInstantString), null, null, new Collection<SamlStatement> { new SamlAttributeStatement(subject, attributes) });
            }
        }

        public static SamlSecurityToken SamlSecurityToken => new SamlSecurityToken(SamlAssertion);

        public static string AsymmetricJwt {
            get {
                var key = Default.X509SecurityKeySelfSigned2048_SHA256;
                var tokenDescriptor = new SecurityTokenDescriptor {
                    Audience = Default.Audience,
                    NotBefore = Default.NotBefore,
                    Expires = Default.Expires,
                    Issuer = Default.Issuer,
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                    Subject = new ClaimsIdentity(Default.DefaultClaims),
                };

                return new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescriptor);
            }
        }

        public static SecurityToken JwtSecurityToken => new JwtSecurityToken(AsymmetricJwt);
    }
}
