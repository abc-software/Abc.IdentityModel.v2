// ----------------------------------------------------------------------------
// <copyright file="BinaryLightToken.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.EidasLight {
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    public class BinaryLightToken {
        private const string TimestampFormat = "yyyy-MM-dd HH:mm:ss fff";

        public BinaryLightToken(string issuerName, string id, DateTime timestamp) {
            this.IssuerName = issuerName;
            this.Id = id;
            this.Timestamp = timestamp;
        }

        public string IssuerName { get; private set;  }
        public string Id { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Digest { get; private set; }

        public string Sign(string secret) {
            var str = $"{this.Id}|{this.IssuerName}|{this.Timestamp:yyyy-MM-dd HH:mm:ss fff}|{secret}";
            this.Digest = ComputeSha256Hash(str);

            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.IssuerName}|{this.Id}|{this.Timestamp:yyyy-MM-dd HH:mm:ss fff}|{this.Digest}"));
        }

        public static BinaryLightToken Parse(string s) {
            var token = Encoding.UTF8.GetString(Convert.FromBase64String(s));
            var tokenParts = token.Split(new char[] { '|' }, 4);

            if (tokenParts.Length != 4) {
                throw new FormatException("Invalid token parts count.");
            }

            var timestamp = DateTime.ParseExact(tokenParts[2], TimestampFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            return new BinaryLightToken(tokenParts[0], tokenParts[1], timestamp) {
                Digest = tokenParts[3],
            };
        }

        public bool Validate(string secret) {
            var str = $"{this.Id}|{this.IssuerName}|{this.Timestamp:yyyy-MM-dd HH:mm:ss fff}|{secret}";

            return string.Equals(this.Digest, ComputeSha256Hash(str), StringComparison.OrdinalIgnoreCase);
        }

        private static string ComputeSha256Hash(string rawData) {
            using (var sha256Hash = SHA256.Create()) {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}

