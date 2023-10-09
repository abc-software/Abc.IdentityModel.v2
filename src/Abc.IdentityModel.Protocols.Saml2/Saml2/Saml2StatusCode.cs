// ----------------------------------------------------------------------------
// <copyright file="Saml2StatusCode.cs" company="ABC Software Ltd">
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
    using System.Xml;

    /// <summary>
    /// The <c>StatusCode</c> class specifies a code or a set of nested codes representing the status of 
    /// the corresponding request.
    /// </summary>
    /// <remarks>See the samlp:StatusCode element defined in [SamlCore, 3.2.2.2] for more details.</remarks>
    internal class Saml2StatusCode {
        private Saml2StatusCode subStatus;
        private readonly XmlQualifiedName value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2StatusCode"/> class.
        /// </summary>
        /// <param name="value">The top-level status code value.</param>
        public Saml2StatusCode(XmlQualifiedName value)
            : this(value, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2StatusCode"/> class.
        /// </summary>
        /// <param name="value">The top-level status code value.</param>
        /// <param name="subStatus">The secondary status.</param>
        public Saml2StatusCode(XmlQualifiedName value, Saml2StatusCode subStatus) {
            this.value = value ?? throw new ArgumentNullException(nameof(value));
            this.subStatus = subStatus;
        }

        #region Public Static Properties
        /// <summary>
        /// Gets the success status code.
        /// </summary>
        public static Saml2StatusCode Success {
            get {
                return new Saml2StatusCode(Saml2Constants.StatusCodes.Success);
            }
        }

        /// <summary>
        /// Gets the requester status code.
        /// </summary>
        public static Saml2StatusCode Requester {
            get {
                return new Saml2StatusCode(Saml2Constants.StatusCodes.Requester);
            }
        }

        /// <summary>
        /// Gets the responder status code.
        /// </summary>
        public static Saml2StatusCode Responder {
            get {
                return new Saml2StatusCode(Saml2Constants.StatusCodes.Responder);
            }
        }

        /// <summary>
        /// Gets the request version too high status code.
        /// </summary>
        public static Saml2StatusCode RequestVersionTooHigh {
            get {
                return new Saml2StatusCode(Saml2Constants.StatusCodes.VersionMismatch, new Saml2StatusCode(Saml2Constants.StatusCodes.RequestVersionTooHigh));
            }
        }

        /// <summary>
        /// Gets the request version too low status code.
        /// </summary>
        public static Saml2StatusCode RequestVersionTooLow {
            get {
                return new Saml2StatusCode(Saml2Constants.StatusCodes.VersionMismatch, new Saml2StatusCode(Saml2Constants.StatusCodes.RequestVersionTooLow));
            }
        }

        /// <summary>
        /// Gets the request version deprecated status code.
        /// </summary>
        public static Saml2StatusCode RequestVersionDeprecated {
            get {
                return new Saml2StatusCode(Saml2Constants.StatusCodes.VersionMismatch, new Saml2StatusCode(Saml2Constants.StatusCodes.RequestVersionDeprecated));
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets the optional subordinate status code that provides more specific information
        /// on an error condition.
        /// </summary>
        /// <remarks>See [SamlCore, 3.2.2.2] for more details.</remarks>
        /// <value>The optional subordinate status code that provides more specific information
        /// on an error condition.</value>
        public Saml2StatusCode SubStatus {
            get { return this.subStatus; }
            set { this.subStatus = value; }
        }

        /// <summary>
        /// Gets the status code value.
        /// </summary>
        /// <remarks>See [SamlCore, 3.2.2.2] for more details.</remarks>
        /// <value>The status code value.</value>
        public XmlQualifiedName Value {
            get { return this.value; }
        }
    }
}
