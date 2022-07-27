// ----------------------------------------------------------------------------
// <copyright file="EncryptedSecurityToken.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Tokens.Saml {
    using Microsoft.IdentityModel.Tokens;
    using System;

    internal class EncryptedSecurityToken : SecurityToken {
        public EncryptedSecurityToken(SecurityToken token, EncryptingCredentials encryptingCredentials) {
            this.Token = token ?? throw new ArgumentNullException(nameof(token));
            this.EncryptingCredentials = encryptingCredentials ?? throw new ArgumentNullException(nameof(encryptingCredentials));
        }

        public SecurityToken Token { get; }

        public EncryptingCredentials EncryptingCredentials { get; }

        public override string Id => this.Token.Id;

        public override string Issuer => this.Token.Issuer;

        public override SecurityKey SecurityKey => this.Token.SecurityKey;

        public override SecurityKey SigningKey { get => this.Token.SigningKey; set => throw new NotSupportedException(); }

        public override DateTime ValidFrom => this.Token.ValidFrom;

        public override DateTime ValidTo => this.Token.ValidTo;
    }
}
