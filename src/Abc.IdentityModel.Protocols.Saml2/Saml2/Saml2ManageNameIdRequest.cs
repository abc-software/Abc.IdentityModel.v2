// ----------------------------------------------------------------------------
// <copyright file="Saml2ManageNameIdRequest.cs" company="ABC Software Ltd">
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
    using Microsoft.IdentityModel.Tokens.Saml2;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens.Saml2;
#else
    using System.IdentityModel.Tokens;
#endif
#if NET40 || NET35
    using Diagnostic;
#else
    using Abc.Diagnostics;
#endif

    /// <summary>
    /// The <c>Saml2ManageNameIdRequest</c> class requests a partner to change the SAML name identifier used to identify a subject.
    /// </summary>
    /// <remarks>See the samlp:ManageNameIDRequest element defined in [SamlCore, 3.6.1] for more details.</remarks>
    internal class Saml2ManageNameIdRequest : Saml2Request {
        private Saml2NameIdentifier identifier;
        private Saml2NewNameIdentifier newIdentifier;
        private bool terminate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ManageNameIdRequest"/> class.
        /// </summary>
        /// <param name="identifier">The principal whose name identifier is to be managed.</param>
        public Saml2ManageNameIdRequest(Saml2NameIdentifier identifier) {
            this.identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        /// <summary>
        /// Gets or sets the name identifier and associated descriptive data (in plaintext or encrypted form) that specify the
        /// principal as currently recognized by the identity and service providers prior to this request.
        /// </summary>
        /// <value>The name identifier and associated descriptive data (in plaintext or encrypted form) that specify the
        /// principal.</value>
        public Saml2NameIdentifier Identifier {
            get {
                return this.identifier;
            }

            set {
                this.identifier = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Gets or sets the new identifier value (in plaintext or encrypted form) to be used when communicating with the
        /// requesting provider concerning this principal.
        /// </summary>
        /// <value>The new identifier value (in plaintext or encrypted form) to be used when communicating with the
        /// requesting provider concerning this principal.</value>
        public Saml2NewNameIdentifier NewIdentifier {
            get {
                return this.newIdentifier;
            }

            set {
                if (value != null && this.Terminate) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperInvalidOperation("ID4360");
                }

                this.newIdentifier = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the use of the old identifier must be terminated.
        /// </summary>
        /// <value>Whether the use of the old identifier must be terminated.</value>
        public bool Terminate {
            get {
                return this.terminate;
            }

            set {
                if (value && this.newIdentifier != null) {
                    throw DiagnosticTools.ExceptionUtil.ThrowHelperInvalidOperation("ID4360");
                }

                this.terminate = value;
            }
        }
    }
}
