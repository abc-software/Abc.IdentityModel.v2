using Apache.Ignite.Core.Client;
using System;
using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Abc.IdentityModel.EidasLight.Ignite {
    public class CertificateSslStreamFactory : ISslStreamFactory {
        public bool SkipServerCertificateValidation { get; set; } = false;
        public bool CheckCertificateRevocation { get; set; } = true;
        public SslProtocols SslProtocols { get; set; } = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
        public X509Certificate2 Certificate { get; set; }

        public virtual SslStream Create(Stream stream, string targetHost) {
            if (stream is null) {
                throw new ArgumentNullException(nameof(stream));
            }

            var certs = new X509CertificateCollection(new X509Certificate[] { this.Certificate });

            var sslStream = new SslStream(stream, false, ValidateServerCertificate, null);
            sslStream.AuthenticateAsClient(targetHost, certs, this.SslProtocols, this.CheckCertificateRevocation);
            return sslStream;
        }

        protected bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            if (this.SkipServerCertificateValidation) {
                return true;
            }

            if (sslPolicyErrors == SslPolicyErrors.None) {
                return true;
            }

            return false;
        }
    }
}
