// ----------------------------------------------------------------------------
// <copyright file="RequestedAttribute.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using Microsoft.IdentityModel.Tokens.Saml2;

    public class RequestedAttribute : Saml2Attribute
    {
        public RequestedAttribute(string name)
            : base(name) {
        }

        public bool? IsRequired { get; set; }
    }
}