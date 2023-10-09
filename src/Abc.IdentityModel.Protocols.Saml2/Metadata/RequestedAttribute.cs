namespace Abc.IdentityModel.Protocols.Metadata {
    using System;
    using System.Collections.Generic;
#if WIF35
    using Microsoft.IdentityModel.Tokens.Saml2;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens.Saml2;
#else
    using System.IdentityModel.Tokens;
#endif

    /// <summary>
    /// Represents the RequestedAttribute element specified in [SAML2Meta].
    /// </summary>
    public class RequestedAttribute : Saml2Attribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestedAttribute"/> class with the specified name and format.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="nameFormat">The name format of the attribute.</param>
        public RequestedAttribute(string name, Uri nameFormat)
            : base(name) {
            this.NameFormat = nameFormat ?? throw new ArgumentNullException(nameof(nameFormat));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestedAttribute"/> class with the specified name, format and value.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="nameFormat">The name format of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        public RequestedAttribute(string name, Uri nameFormat, string value)
            : base(name, value) {
            this.NameFormat = nameFormat ?? throw new ArgumentNullException(nameof(nameFormat));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestedAttribute"/> class with the specified name, format and values.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="nameFormat">The name format of the attribute.</param>
        /// <param name="values">The values of the attribute.</param>
        public RequestedAttribute(string name, Uri nameFormat, IEnumerable<string> values)
            : base(name, values) {
            this.NameFormat = nameFormat ?? throw new ArgumentNullException(nameof(nameFormat));
        }

        /// <summary>
        /// Gets or sets the is required.
        /// </summary>
        /// <value>
        /// The is required.
        /// </value>
        public bool? IsRequired { get; set; }
    }
}
