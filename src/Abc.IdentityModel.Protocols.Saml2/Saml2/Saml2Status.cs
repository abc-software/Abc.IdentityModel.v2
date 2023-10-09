// ----------------------------------------------------------------------------
// <copyright file="Saml2Status.cs" company="ABC Software Ltd">
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
    using System.Collections.ObjectModel;
    using System.Xml;

    /// <summary>
    /// The <c>Status</c> class represents the status of processing the associated request.
    /// </summary>
    /// <remarks>See the samlp:Status element defined in [SamlCore, 3.2.2.1] for more details.</remarks>
    internal class Saml2Status {
        private Saml2StatusCode statusCode;
        private string statusMessage;
        private Collection<XmlElement> statusDetail = new Collection<XmlElement>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2Status"/> class.
        /// </summary>
        /// <param name="statusCode">The top-level status code.</param>
        public Saml2Status(XmlQualifiedName statusCode)
            : this(new Saml2StatusCode(statusCode)) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2Status"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public Saml2Status(Saml2StatusCode statusCode) {
            if (statusCode == null) {
                throw new ArgumentNullException(nameof(statusCode));
            }

            if (!Saml2Constants.StatusCodes.TopLevelCodes.Contains(statusCode.Value)) {
                throw new ArgumentOutOfRangeException(nameof(statusCode));
            }

            this.statusCode = statusCode;
        }

        /// <summary>
        /// Gets or sets a set of nested status codes.
        /// </summary>
        /// <details>
        /// The first status code must be one of the codes from StatusCodes.TopLevelCodes.
        /// </details>
        /// <value>A set of nested status codes.</value>
        public Saml2StatusCode StatusCode {
            get {
                return this.statusCode;
            }

            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!Saml2Constants.StatusCodes.TopLevelCodes.Contains(value.Value)) {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.statusCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the message associated with the status.
        /// </summary>
        /// <value>The message associated with the status.</value>
        public string StatusMessage {
            get {
                return this.statusMessage;
            }

            set {
                this.statusMessage = !string.IsNullOrEmpty(value) ? value : null;
            }
        }

        /// <summary>
        /// Gets custom XML elements associated with the status.
        /// </summary>
        /// <value>The custom XML elements associated with the status.</value>
        public Collection<XmlElement> StatusDetail {
            get {
                return this.statusDetail;
            }
        }
    }
}
