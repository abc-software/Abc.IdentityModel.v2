#if AZUREAD

namespace Abc.IdentityModel.Protocols {
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml2;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    public class SecurityTokenElement {
        private readonly XmlElement securityTokenXml;
        private readonly ICollection<SecurityTokenHandler> securityTokenHandlers;
        private SecurityToken securityToken;

        public SecurityTokenElement(SecurityToken securityToken) {
            this.securityToken = securityToken;
        }

        public SecurityTokenElement(XmlElement securityTokenXml, ICollection<SecurityTokenHandler> securityTokenHandlers) {
            this.securityTokenXml = securityTokenXml;
            this.securityTokenHandlers = securityTokenHandlers;
        }

        public XmlElement SecurityTokenXml => securityTokenXml;

        public SecurityToken GetSecurityToken() {
            if (securityToken == null) {
                securityToken = ReadSecurityToken(securityTokenXml, securityTokenHandlers);
            }

            return securityToken;
        }

        protected virtual SecurityToken ReadSecurityToken(XmlElement securityTokenXml, ICollection<SecurityTokenHandler> securityTokenHandlers) {
            using (var reader = new XmlNodeReader(securityTokenXml)) {
                reader.MoveToContent();

                foreach (var securityTokenHandler in securityTokenHandlers) {
                    if (!securityTokenHandler.CanReadToken(reader)) {
                        continue;
                    }

                    return securityTokenHandler.ReadToken(reader);
                }

                throw new InvalidOperationException("Cannot resolve SecurityToken.");
            }
        }
    }
}

#endif