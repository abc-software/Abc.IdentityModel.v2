using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Abc.IdentityModel.Tokens.UnitTests {
    internal static class ReferenceJwt {
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

        public static SecurityToken JwtToken => new JwtSecurityToken(AsymmetricJwt);
    }
}