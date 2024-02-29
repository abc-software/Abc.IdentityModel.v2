namespace Abc.IdentityModel.EidasLight.Ignite {
    using System.Collections.ObjectModel;
    using System.Security.Cryptography.X509Certificates;
    
    public interface IIgniteCacheNames {
        string IncomingSecret { get; set; }
        string OutgoingSecret { get; set; }
        string IncomingCacheName { get; set; }
        string OutgoingCacheName { get; set; }
    }

    public class EidasProxyIgniteCacheConfiguration : IgniteCacheConfiguration, IIgniteCacheNames {
        public EidasProxyIgniteCacheConfiguration() {
        }

        public EidasProxyIgniteCacheConfiguration(IgniteCacheConfiguration other) {
            other.ApplyTo(this);
        }

        public string IncomingCacheName { get; set; } = "nodeSpecificProxyserviceRequestCache";
        public string OutgoingCacheName { get; set; } = "specificNodeProxyserviceResponseCache";
        public string IncomingSecret { get; set; }
        public string OutgoingSecret { get; set; }
    }

    public class EidasConnectorIgniteCacheConfiguration : IgniteCacheConfiguration, IIgniteCacheNames {
        public EidasConnectorIgniteCacheConfiguration() {
        }

        public EidasConnectorIgniteCacheConfiguration(IgniteCacheConfiguration other) {
            other.ApplyTo(this);
        }

        public string IncomingCacheName { get; set; } = "nodeSpecificConnectorResponseCache";
        public string OutgoingCacheName { get; set; } = "specificNodeConnectorRequestCache";
        public string IncomingSecret { get; set; }
        public string OutgoingSecret { get; set; }
    }

    public partial class IgniteCacheConfiguration {
        public Collection<string> Endpoints { get; internal set; } = new Collection<string>();
        public X509Certificate2 Certificate { get; set; }

        public string CertificateFindValue { get; set; }
        public StoreLocation CertificateStoreLocation { get; set; } = StoreLocation.LocalMachine;
        public StoreName CertificateStoreName { get; set; } = StoreName.My;
        public X509FindType CertificateX509FindType { get; set; } = X509FindType.FindByThumbprint;

        public string CertificatePath { get; set; }
        public string CertificatePassword { get; set; }

        internal void ApplyTo(IgniteCacheConfiguration other) {
            other.Endpoints = this.Endpoints;
            other.Certificate = this.Certificate;

            other.CertificateFindValue = this.CertificateFindValue;
            other.CertificateStoreLocation = this.CertificateStoreLocation;
            other.CertificateStoreName = this.CertificateStoreName;
            other.CertificateX509FindType = this.CertificateX509FindType;

            other.CertificatePath = this.CertificatePath;
            other.CertificatePassword = this.CertificatePassword;
        }
    }
}

