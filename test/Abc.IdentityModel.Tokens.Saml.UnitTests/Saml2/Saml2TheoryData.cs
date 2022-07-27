using Abc.IdentityModel.Tokens.Saml.UnitTests;
using Microsoft.IdentityModel.Tokens.Saml2;
using System.Collections.Generic;

namespace Abc.IdentityModel.Tokens.Saml2.UnitTests {
    public class Saml2TheoryData : TokenTheoryData {
        public Saml2TheoryData() {
        }

        public Saml2TheoryData(TokenTheoryData tokenTheoryData)
            : base(tokenTheoryData) {
        }

        public string Xml { get; set; }

        public Saml2Action Action { get; set; }

        public Saml2Advice Advice { get; set; }

        public Saml2Assertion Assertion { get; set; }

        public Saml2Attribute Attribute { get; set; }

        public List<Saml2Attribute> Attributes { get; set; }

        public Saml2AttributeStatement AttributeStatement { get; set; }

        public Saml2AudienceRestriction AudienceRestriction { get; set; }

        public Saml2AuthenticationStatement AuthenticationStatement { get; set; }

        public Saml2AuthorizationDecisionStatement AuthorizationDecision { get; set; }

        public Saml2Conditions Conditions { get; set; }

        public List<Saml2Attribute> ConsolidatedAttributes { get; set; }

        public Saml2Evidence Evidence { get; set; }

        public Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler Handler { get; set; } = new Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler();

        public string InclusiveNamespacesPrefixList { get; set; }

        public Saml2Serializer Saml2Serializer { get; set; } = new Saml2Serializer();

        public Saml2Subject Subject { get; set; }

        public Saml2ProxyRestriction ProxyRestriction { get; set; }
    }
}
