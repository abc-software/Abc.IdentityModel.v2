
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddWsFederation(options =>
    {
        options.Wtrealm = Configuration["wsfed:realm"];
        options.MetadataAddress = Configuration["wsfed:metadata"];
        // enable decryption
        options.TokenValidationParameters.TokenDecryptionKey = new X509Certificate2(Convert.FromBase64String(Configuration["wsfed:cert"]));
        options.SecurityTokenHandlers = new Collection<ISecurityTokenValidator>() {
                   new Abc.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler(),
                   new Abc.IdentityModel.Tokens.Saml.SamlSecurityTokenHandler(),
                   new JwtSecurityTokenHandler()
               };
    });
}
```