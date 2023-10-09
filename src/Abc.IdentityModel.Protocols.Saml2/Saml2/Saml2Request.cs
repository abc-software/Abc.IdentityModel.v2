// ----------------------------------------------------------------------------
// <copyright file="Saml2Request.cs" company="ABC Software Ltd">
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
    /// <summary>
    /// The <c>Saml2Request</c> class contains information common to all SAML requests.
    /// </summary>
    /// <remarks>See the samlp:RequestAbstractType defined in [SamlCore, 3.2.1] for more details.</remarks>
    internal abstract class Saml2Request : Saml2Message {
        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2Request"/> class.
        /// </summary>
        protected Saml2Request()
            : base() {
        }
    }
}
