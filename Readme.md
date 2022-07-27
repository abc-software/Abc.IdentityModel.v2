# Azure Active Directory IdentityModel Extensions for .NET

## Decryption
Enable decryption of encrypted SAML1.1 and SAML2.0 tokens

```cs
services
.AddAuthentication()
.AddWsFederation(options =>
    options.TokenValidationParameters.TokenDecryptionKey = new X509SecurityKey(serviceCertificate);
    options.SecurityTokenHandlers = new Collection<ISecurityTokenValidator>() {
                new Abc.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler(),
                new Abc.IdentityModel.Tokens.Saml.SamlSecurityTokenHandler(),
            }
);
```