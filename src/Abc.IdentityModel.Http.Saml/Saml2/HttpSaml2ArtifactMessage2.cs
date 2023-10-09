namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
    using Abc.IdentityModel.Http;

    public class HttpSaml2ArtifactMessage2 : HttpSaml2Message2 {
        private string artifact;

        public HttpSaml2ArtifactMessage2(Uri baseUrl, string artifact)
            : base(baseUrl, HttpDeliveryMethods.GetRequest) {
            if (string.IsNullOrEmpty(artifact)) {
                throw new ArgumentNullException(nameof(artifact));
            }

            this.artifact = artifact;
        }

        public HttpSaml2ArtifactMessage2(Uri baseUrl, ISamlArtifact samlArtifact)
            : base(baseUrl, HttpDeliveryMethods.GetRequest) {
            if (samlArtifact == null) { 
                throw new ArgumentNullException(nameof(samlArtifact)); 
            }

            this.artifact = samlArtifact.ToString();
        }

        internal HttpSaml2ArtifactMessage2(Uri baseUrl, HttpDeliveryMethods method)
            : base(baseUrl, method) {
        }

        [MessagePart(Saml2Constants.Parameters.SamlArtifact, IsRequired = true)]
        public string Artifact {
            get {
                return this.artifact;    
            }

            set {
                if (string.IsNullOrEmpty(value)) {
                    throw new ArgumentNullException(nameof(value));
                }

                this.artifact = value;
            }
        }
    }
}
