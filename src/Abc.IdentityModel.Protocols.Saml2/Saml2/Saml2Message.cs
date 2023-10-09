// ----------------------------------------------------------------------------
// <copyright file="Saml2Message.cs" company="ABC Software Ltd">
//    Copyright © 2010-2019 ABC Software Ltd. All rights reserved.
//
//    This library is free software; you can redistribute it and/or.
//    modify it under the terms of the GNU Lesser General Public
//    License  as published by the Free Software Foundation, either
//    version 3 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with the library. If not, see http://www.gnu.org/licenses/.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols.Saml2 {
    using System;
#if WIF35
    using System.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml2;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens.Saml2;
#else
    using System.IdentityModel.Tokens;
#endif

    /// <summary>
    /// The <c>Saml2Message</c> class contains information common to all SAML messages.
    /// </summary>
    /// <remarks>See the common parts of samlp:RequestAbstractType and samlp:StatusResponseType defined in
    /// [SamlCore, 3.2.1] and [SamlCore, 3.2.2] for more details.</remarks>
    internal abstract class Saml2Message {
        private Saml2Id id = new Saml2Id();
        private readonly string version = "2.0";
        private DateTime issueInstant = DateTime.UtcNow;
        private Uri destination;
        private Uri consent = Saml2Constants.ConsentIdentifiers.Unspecified;

        private Saml2NameIdentifier issuer;
        private SigningCredentials signingCredentials;
        ////private SecurityToken signingToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2Message"/> class.
        /// </summary>
        protected Saml2Message() {
        }

        /// <summary>
        /// Gets or sets the identifier of the message. 
        /// </summary>
        /// <value>The identifier of the message.</value>
        public Saml2Id Id {
            get { 
                return this.id;
            }

            set {
                this.id = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Gets the version of this message.
        /// </summary>
        /// <details>
        /// As outlined in [SamlCore, 4], the implementer has the option of 
        /// choosing to process messages which have a supported major version 
        /// even if the minor version is unsupported. At this time, this code
        /// intends to support only exactly version "2.0". However, the 
        /// version string will be maintained in the data structures in case
        /// of such a time where a revised minor version may be supported. It
        /// is expected that a major version revision would result in a new 
        /// processing model with new data structures.
        /// </details>
        /// <value>The version of this message.</value>
        public string Version {
            get {
                return this.version;
            }
        }

        /// <summary>
        /// Gets or sets the time instant of issue of the message.
        /// </summary>
        /// <value>The time instant of issue of the message.</value>
        public DateTime IssueInstant {
            get {
                return this.issueInstant;
            }

            set {
                if (value.Kind == DateTimeKind.Unspecified) {
                    throw new ArgumentException("Must be specified kind.", nameof(value));
                }
                
                this.issueInstant = value.ToUniversalTime();
            }
        }

        /// <summary>
        /// Gets or sets a URI indicating the address to which this message has 
        /// been sent.
        /// </summary>
        /// <value>A URI indicating the address to which this message has 
        /// been sent.</value>
        public Uri Destination {
            get { 
                return this.destination; 
            }

            set {
                this.destination = value; 
            }
        }

        /// <summary>
        /// Gets or sets whether or not (and under what conditions) consent has been 
        /// obtained from a principal in the sending of this message.
        /// </summary>
        /// <value>Whether or not (and under what conditions) consent has been 
        /// obtained from a principal in the sending of this message.</value>
        public Uri Consent {
            get {
                return this.consent;
            }

            set {
                if (value == null) {
                    this.consent = Saml2Constants.ConsentIdentifiers.Unspecified;
                }
                else {
                    this.consent = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the entity that generated the message.
        /// </summary>
        /// <value>The identifier of the entity that generated the message.</value>
        public Saml2NameIdentifier Issuer {
            get {
                return this.issuer;
            }

            set {
                this.issuer = value;
            }
        }

        /// <summary>
        /// Gets or sets the credentials used to sign this message when it is serialized.
        /// </summary>
        /// <value>The credentials used to sign this message when it is serialized.</value>
        public SigningCredentials SigningCredentials {
            get {
                return this.signingCredentials;
            }

            set {
                this.signingCredentials = value;
            }
        }

        /*
        public SecurityToken SigningToken {
            get {
                return this.signingToken;
            }

            set {
                this.signingToken = value;
            }
        }
        */
    }
}
