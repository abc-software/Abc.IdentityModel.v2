namespace Abc.IdentityModel.EidasLight.Ignite {
    using System;
    using System.IO;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;

    internal class CertStoreSslStreamFactory : CertificateSslStreamFactory {
        public StoreLocation StoreLocation { get; set; } = StoreLocation.LocalMachine;
        public StoreName StoreName { get; set; } = StoreName.My;
        public X509FindType X509FindType { get; set; } = X509FindType.FindByThumbprint;
        public string FindValue { get; set; }
        public bool ValidOnly { get; set; }

        public override SslStream Create(Stream stream, string targetHost) {
            if (stream is null) {
                throw new ArgumentNullException(nameof(stream));
            }

            var certs = new X509CertificateCollection(new X509Certificate[] { GetCertificate() });

            var sslStream = new SslStream(stream, false, ValidateServerCertificate, null);
            sslStream.AuthenticateAsClient(targetHost, certs, this.SslProtocols, this.CheckCertificateRevocation);
            return sslStream;
        }

        private X509Certificate2 GetCertificate() {
            X509Certificate2 certificate = null;
            X509Certificate2Collection certificates = null;

            // Open Certificate
            var store = new X509Store(this.StoreName, this.StoreLocation);
            store.Open(OpenFlags.ReadOnly);

            try {
                // Every time we call store.Certificates property, a new collection will be returned.
                certificates = store.Certificates;

                X509Certificate2Collection matches = certificates.Find(this.X509FindType, this.FindValue, this.ValidOnly);
                if (matches.Count != 1) {
                    if (matches.Count > 1) {
                        throw new InvalidOperationException($"There are multiple certificates in the certificate store that match the find value of '{this.FindValue}'");
                    }

                    throw new InvalidOperationException($"There are no certificates in the certificate store that match the find value of '{this.FindValue}'");
                }

                certificate = matches[0];
            }
            finally {
                if (certificates != null) {
                    for (int i = 0; i < certificates.Count; ++i) {
                        var current = certificates[i];
                        if (certificate != null && !object.ReferenceEquals(current, certificate)) {
                            current.Reset();
                        }
                    }
                }

                store.Close();
            }

            return certificate;
        }
    }
}