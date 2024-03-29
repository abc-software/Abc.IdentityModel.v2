﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Abc.IdentityModel.Tokens.UnitTests {
    internal class Default {
        public static DateTime NotBefore => new DateTime(2022, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        public static DateTime NotOnOrAfter => NotBefore.AddHours(1);
        public static string SamlConfirmationMethod => "urn:oasis:names:tc:SAML:1.0:cm:bearer";
        public static string SamlAssertionID => "_id";
        public static string Issuer => "urn:issuer";
        public static DateTime IssueInstant => NotBefore;
        public static string Audience => "urn:audience";

        public static List<Claim> DefaultClaims => new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "Bob", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Email, "Bob@contoso.com", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.GivenName, "Bob", ClaimValueTypes.String, Issuer),
            };

        public static DateTime? Expires => NotOnOrAfter;

        public static SecureString SelfSigned2048_SHA256_Password = ConvertToSecureString("SelfSigned2048_SHA256");

        public static string SelfSigned2048_SHA256 = @"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";
        public static string SelfSigned2048_SHA256_Public = @"MIIDXjCCAkagAwIBAgIQY9+BvEWchJpAX/tDKzHwFDANBgkqhkiG9w0BAQsFADA1MTMwMQYDVQQDHioAUwBlAGwAZgBTAGkAZwBuAGUAZAAyADAANAA4AF8AUwBIAEEAMgA1ADYwHhcNMTQxMjI2MTUyNzM2WhcNMzkxMjMxMjM1OTU5WjA1MTMwMQYDVQQDHioAUwBlAGwAZgBTAGkAZwBuAGUAZAAyADAANAA4AF8AUwBIAEEAMgA1ADYwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDWE7VB3zRniE3CsYLy9sCLAdFB7AsGKMkZsJxiwKD1uv+OKshPN9Epm7ZJQWjm6YGeQYKGQUaNs5Z1NaapKLaT52jqcTLRbOC5g331GkXTPICkjDHsR+NPyd7J4O4Hl2ls8q1+mcYhHSJOoamWOGZqtpCfpqqOhHhG75Rn282kA90Ybc6xY+rTgBIYgSt+/l3/muI3XTU6wghifYwZfID1IngBEb+MD346QgpiJcWObL+WIXPGpLNmDjwJZ8IlXvgO5JPSz1wxCyb8EJHUp4hQUc778RtKB82UXbckhL3eW49v1jpuJoqeNm924vlMX3IYAwYDBF93K6F8yu2otpwvAgMBAAGjajBoMGYGA1UdAQRfMF2AEM2V/dQqCNOhP9VPwFcAubuhNzA1MTMwMQYDVQQDHioAUwBlAGwAZgBTAGkAZwBuAGUAZAAyADAANAA4AF8AUwBIAEEAMgA1ADaCEGPfgbxFnISaQF/7Qysx8BQwDQYJKoZIhvcNAQELBQADggEBAKSksE7/5TOc5ngnD54poNnaPWrw4kolFzqYdw1/s/evScT4tgFYR1FrmPB50KYoZ0c8FzDY7PK4SkB7x7xFbjPYZwcEzeHqZ+WsHO3UxI2nU94CUsBmNR09CMMIwt/1A1yfzSNTJE452YtycdLVJUC6NBR30Di5YOFWPwIEO5XE0J7Os1xuhZc6AEKy2STp0I3FL27gHu3R+3Xhqru6fQIOw52Pcp1axXsuE9cQ/HKyeNTvM02FZmjlx/Vy3lC5I/2xiUJrhqzczOMRRB8clpsAp2uNWfrzJ0aLCprQO62pN9L/51PWbSMsNfnfuUo4eZHv2noQ3mGzJPXyK43Omn0=";
        public static X509Certificate2 CertSelfSigned2048_SHA256 = new X509Certificate2(Convert.FromBase64String(SelfSigned2048_SHA256), SelfSigned2048_SHA256_Password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
        public static X509SecurityKey X509SecurityKeySelfSigned2048_SHA256 = new X509SecurityKey(CertSelfSigned2048_SHA256);
        public static X509Certificate2 CertSelfSigned2048_SHA256_Public = new X509Certificate2(Convert.FromBase64String(SelfSigned2048_SHA256_Public), SelfSigned2048_SHA256_Password);
        public static X509SecurityKey X509SecurityKeySelfSigned2048_SHA256_Public = new X509SecurityKey(CertSelfSigned2048_SHA256_Public);

        private static SecureString ConvertToSecureString(string password) {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var securePassword = new SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }
    }
}