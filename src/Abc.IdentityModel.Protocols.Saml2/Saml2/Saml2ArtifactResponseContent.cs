// ----------------------------------------------------------------------------
// <copyright file="Saml2ArtifactResponseContent.cs" company="ABC Software Ltd">
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

    internal class Saml2ArtifactResponseContent {
        private readonly Saml2Message samlMessage;
        private readonly XmlElement responseXml;

        public Saml2ArtifactResponseContent(Saml2Message samlMessage) {
            this.samlMessage = samlMessage ?? throw new ArgumentNullException(nameof(samlMessage));
        }

        public Saml2ArtifactResponseContent(XmlElement responseXml) {
            this.responseXml = responseXml ?? throw new ArgumentNullException(nameof(responseXml));
        }

        public Saml2Message SamlMessage {
            get { return this.samlMessage;  }
        }

        public XmlElement ResponseXml {
            get { return this.responseXml; }
        }
    }
}
