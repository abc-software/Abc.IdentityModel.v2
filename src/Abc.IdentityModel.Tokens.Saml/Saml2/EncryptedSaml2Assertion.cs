// ----------------------------------------------------------------------------
// <copyright file="EncryptedSaml2Assertion.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Tokens.Saml2 {
    using Abc.IdentityModel.Xml;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml2;

    public class EncryptedSaml2Assertion : Saml2Assertion {
        public EncryptedSaml2Assertion(Saml2NameIdentifier issuer)
            : base(issuer) {
        }

        internal EncryptedSaml2Assertion(Saml2Assertion assertion)
            : base(assertion?.Issuer) {
            this.Id = assertion.Id;
            this.IssueInstant = assertion.IssueInstant;

            this.Signature = assertion.Signature;
            this.SigningCredentials = assertion.SigningCredentials;
            this.InclusiveNamespacesPrefixList = assertion.InclusiveNamespacesPrefixList;
            this.Advice = assertion.Advice;
            this.Conditions = assertion.Conditions;
            this.Subject = assertion.Subject;
            
            foreach (var item in assertion.Statements) {
                this.Statements.Add(item);
            }
        }

        public EncryptingCredentials EncryptingCredentials { get; set; }

        public EncryptedData EncryptedData { get; set; }
    }
}
