// ----------------------------------------------------------------------------
// <copyright file="Saml2ManageNameIdResponse.cs" company="ABC Software Ltd">
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

    /// <summary>
    /// The <c>ManageNameIdResponse</c> class contains the result of a ManageNameIdRequest operation.
    /// </summary>
    /// <remarks>See the samlp:ManageNameIDResponse element defined in [SamlCore, 3.6.2] for more details.</remarks>
    internal class Saml2ManageNameIdResponse : Saml2StatusResponse {
        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2ManageNameIdResponse" /> class.
        /// </summary>
        /// <param name="status">The SAML status code associated with this response.</param>
        public Saml2ManageNameIdResponse(Saml2Status status)
            : base(status) {
        }
    }
}
