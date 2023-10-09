using Abc.IdentityModel.Protocols.Saml2;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Xml;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Abc.IdentityModel {
    internal class SamlUtil {
		internal static SecurityKey ResolveSigningKey(Saml2Message message, TokenValidationParameters validationParameters) {
			if (message == null) {
				return null;
			}

			var issuer = message.Issuer?.Value;
			if (!string.IsNullOrEmpty(message.Issuer?.Value)) {
				if (validationParameters.IssuerSigningKey != null && string.Equals(validationParameters.IssuerSigningKey.KeyId, issuer, (validationParameters.IssuerSigningKey is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) {
					return validationParameters.IssuerSigningKey;
				}

				if (validationParameters.IssuerSigningKeys != null) {
					foreach (SecurityKey issuerSigningKey in validationParameters.IssuerSigningKeys) {
						if (issuerSigningKey != null && string.Equals(issuerSigningKey.KeyId, issuer, (issuerSigningKey is X509SecurityKey) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) {
							return issuerSigningKey;
						}
					}
				}
			}

			return null;
		}

		internal static SigningCredentials ResolveSigningCredentials(Signature signature, TokenValidationParameters validationParameters) {
			if (signature == null || signature.KeyInfo == null) {
				throw new InvalidOperationException("No set signature KeyInfo.");
			}

			var key = ResolveSigningKey(signature.KeyInfo, validationParameters);
			if (key != null) { 
				return new SigningCredentials(key, signature.SignedInfo.SignatureMethod, signature.SignedInfo.References[0].DigestMethod);
			}

			return null;
        }

		internal static SecurityKey ResolveSigningKey(KeyInfo tokenKeyInfo, TokenValidationParameters validationParameters) {
			if (tokenKeyInfo == null) {
				return null;
			}

			if (validationParameters.IssuerSigningKey != null && MatchesKey(tokenKeyInfo, validationParameters.IssuerSigningKey)) {
				return validationParameters.IssuerSigningKey;
			}
			if (validationParameters.IssuerSigningKeys != null) {
				foreach (SecurityKey issuerSigningKey in validationParameters.IssuerSigningKeys) {
					if (MatchesKey(tokenKeyInfo, issuerSigningKey)) {
						return issuerSigningKey;
					}
				}
			}
			return null;
		}

		private static bool MatchesKey(KeyInfo tokenKeyInfo, SecurityKey key) {
			if (key == null) {
				return false;
			}

			switch (key) {
				case X509SecurityKey x509SecurityKey:
					return Matches(tokenKeyInfo, x509SecurityKey);
				case RsaSecurityKey rsaSecurityKey:
					return Matches(tokenKeyInfo, rsaSecurityKey);
				default:
					return false;
			}
		}

        private static bool Matches(KeyInfo tokenKeyInfo, RsaSecurityKey key) {
			if (key == null) {
				return false;
			}

			if (!key.Parameters.Equals(default(RSAParameters))) {
				if (tokenKeyInfo.RSAKeyValue.Exponent.Equals(Convert.ToBase64String(key.Parameters.Exponent), StringComparison.InvariantCulture)) {
					return tokenKeyInfo.RSAKeyValue.Modulus.Equals(Convert.ToBase64String(key.Parameters.Modulus), StringComparison.InvariantCulture);
				}

				return false;
			}

			if (key.Rsa != null) {
				var rSAParameters = key.Rsa.ExportParameters(includePrivateParameters: false);
				if (tokenKeyInfo.RSAKeyValue.Exponent.Equals(Convert.ToBase64String(rSAParameters.Exponent), StringComparison.InvariantCulture)) {
					return tokenKeyInfo.RSAKeyValue.Modulus.Equals(Convert.ToBase64String(rSAParameters.Modulus), StringComparison.InvariantCulture);
				}

				return false;
			}

			return false;
		}

        private static bool Matches(KeyInfo tokenKeyInfo, X509SecurityKey key) {
			if (key == null) {
				return false;
			}

			foreach (var x509Datum in tokenKeyInfo.X509Data) {
                foreach (string certificate in x509Datum.Certificates) {
					using (var x509Certificate = new X509Certificate2(Convert.FromBase64String(certificate))) {
						if (x509Certificate.Equals(key.Certificate)) {
							return true;
						}
					}
				}
			}

			return false;
		}
    }
}
