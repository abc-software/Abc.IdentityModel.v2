using Microsoft.IdentityModel.Tokens.Saml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Abc.IdentityModel.Tokens.Saml.UnitTests {
    public class SamlTheoryData : TokenTheoryData {
        public SamlTheoryData() {
        }

        public SamlTheoryData(TokenTheoryData tokenTheoryData)
            : base(tokenTheoryData) {
        }

        public SamlSecurityTokenHandler Handler { get; set; } = new SamlSecurityTokenHandler();

        public string InclusiveNamespacesPrefixList { get; set; }

        public SamlSerializer SamlSerializer { get; set; } = new SamlSerializer();

        public Claim Claim { get; set; }

        public SamlAttribute SamlAttribute { get; set; }
    }
}
