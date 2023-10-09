// ----------------------------------------------------------------------------
// <copyright file="ISaml2TokenToSerializerAdaptor.cs" company="ABC Software Ltd">
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
#if WIF35
    using System.IdentityModel.Selectors;
    using Microsoft.IdentityModel.Tokens.Saml2;
#elif AZUREAD
    using Microsoft.IdentityModel.Tokens.Saml2;
#else
    using System.IdentityModel.Selectors;
    using System.IdentityModel.Tokens;
#endif

    /// <summary>
    /// The <c>ISaml2TokenToSerializerAdaptor</c> interface is used by <c>Saml2ProtocolSerializer</c> to delegate serialization and deserialization calls for elements 
    /// that <c>Saml2SecurityTokenHandler</c> can read and write.
    /// </summary>
    public interface ISaml2TokenToSerializerAdaptor {
#if !AZUREAD
        /// <summary>
        /// Initializes a instance of the <see cref="ISaml2TokenToSerializerAdaptor" /> interface.
        /// </summary>
        /// <param name="signatureTokenResolver">The resolver to use when resolving security tokens used in XML signatures. Use null to indicate signatures should not be validated.</param>
        /// <param name="encryptionTokenResolver">The resolver to use when resolving security tokens used in XML encryption.</param>
        /// <param name="keyInfoSerializer">The serializer to use when serializing ds:KeyInfo elements.</param>
        /// <returns>The instance of the <see cref="ISaml2TokenToSerializerAdaptor" /> interface.</returns>
        ISaml2TokenToSerializerAdaptor IntitializeAdaptor(SecurityTokenResolver signatureTokenResolver, SecurityTokenResolver encryptionTokenResolver, SecurityTokenSerializer keyInfoSerializer);
#endif

        /// <summary>
        /// Reads the &lt;saml:Action&gt; element using the specified XML reader.
        /// </summary>
        /// <param name="reader">A <see cref="T:System.Xml.XmlReader"/> to read the &lt;saml:Action&gt; XML element.</param>
        /// <returns>The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Action"/>.</returns>
        Saml2Action ReadActionFromReader(XmlReader reader);

        /// <summary>Reads the &lt;saml:Assertion&gt; element using the specified XML reader.</summary>
        /// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> to read the &lt;saml:Assertion&gt; XML element.</param>
        /// <returns>The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Assertion"/>.</returns>
        Saml2Assertion ReadAssertionFromReader(XmlReader reader);

        /// <summary>Reads the &lt;saml:Attribute&gt; element using the specified XML reader.</summary>
        /// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> to read the &lt;saml:Attribute&gt; XML element.</param>
        /// <returns>The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Attribute"/>.</returns>
        Saml2Attribute ReadAttributeFromReader(XmlReader reader);

        /// <summary>Reads the &lt;saml:AttributeValue&gt; element using the specified XML reader.</summary>
        /// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> to read the &lt;saml:AttributeValue&gt; XML element.</param>
        /// <param name="attribute">The <see cref="Saml2Attribute"/>.</param>
        /// <returns>The attribute value as a string.</returns>
        string ReadAttributeValueFromReader(XmlReader reader, Saml2Attribute attribute);

        /// <summary>Reads the &lt;saml:Conditions&gt; element using the specified XML reader.</summary>
        /// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> to read the &lt;saml:Conditions&gt; XML element.</param>
        /// <returns>The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Conditions"/>.</returns>
        Saml2Conditions ReadConditionsFromReader(XmlReader reader);

        /// <summary>Reads the &lt;saml:EncryptedID&gt; element using the specified XML reader.</summary>
        /// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> to read the &lt;saml:EncryptedID&gt; XML element.</param>
        /// <returns>The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2NameIdentifier"/>.</returns>
        Saml2NameIdentifier ReadEncryptedIdFromReader(XmlReader reader);

        /// <summary>Reads the &lt;saml:Evidence&gt; element using the specified XML reader.</summary>
        /// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> to read the &lt;saml:Evidence&gt; XML element.</param>
        /// <returns>The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Evidence"/>.</returns>
        Saml2Evidence ReadEvidenceFromReader(XmlReader reader);

        /// <summary>Reads the &lt;saml:Issuer&gt; element using the specified XML reader.</summary>
        /// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> to read the &lt;saml:Issuer&gt; XML element.</param>
        /// <returns>The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2NameIdentifier"/>.</returns>
        Saml2NameIdentifier ReadIssuerFromReader(XmlReader reader);

        /// <summary>
        /// Reads the &lt;saml:NameID&gt; element using the specified XML reader.
        /// </summary>
        /// <param name="reader">A <see cref="T:System.Xml.XmlReader"/> to read the &lt;saml:NameID&gt; XML element.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <returns>
        /// The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2NameIdentifier"/>.
        /// </returns>
        Saml2NameIdentifier ReadNameIdFromReader(XmlReader reader, string parentElement);

        /// <summary>Reads the &lt;saml:Subject&gt; element using the specified XML reader.</summary>
        /// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> to read the &lt;saml:Subject&gt; XML element.</param>
        /// <returns>The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Subject"/>.</returns>
        Saml2Subject ReadSubjectFromReader(XmlReader reader);

        /// <summary>
        /// Writes the <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Action"/> into the specified XML writer as a &lt;saml:Action&gt; element.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Xml.XmlWriter"/> to write the &lt;saml:Action&gt; element.</param>
        /// <param name="action">The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Action"/>.</param>
        void WriteActionToWriter(XmlWriter writer, Saml2Action action);

        /// <summary>
        /// Writes the <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Assertion"/> into the specified XML writer as a &lt;saml:Assertion&gt; element.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Xml.XmlWriter"/> to write the &lt;saml:Assertion&gt; element.</param>
        /// <param name="assertion">The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Assertion"/>.</param>
        void WriteAssertionToWriter(XmlWriter writer, Saml2Assertion assertion);

        /// <summary>
        /// Writes the <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Attribute"/> into the specified XML writer as a &lt;saml:Attribute&gt; element.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Xml.XmlWriter"/> to write the &lt;saml:Attribute&gt; element.</param>
        /// <param name="attribute">The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Attribute"/>.</param>
        void WriteAttributeToWriter(XmlWriter writer, Saml2Attribute attribute);

        /// <summary>
        /// Writes the <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Conditions"/> into the specified XML writer as a &lt;saml:Conditions&gt; element.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Xml.XmlWriter"/> to write the &lt;saml:Conditions&gt; element.</param>
        /// <param name="conditions">The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Conditions"/>.</param>
        void WriteConditionsToWriter(XmlWriter writer, Saml2Conditions conditions);

        /// <summary>
        /// Writes the <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Evidence"/> into the specified XML writer as a &lt;saml:Evidence&gt; element.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Xml.XmlWriter"/> to write the &lt;saml:Evidence&gt; element.</param>
        /// <param name="evidence">The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Evidence"/>.</param>
        void WriteEvidenceToWriter(XmlWriter writer, Saml2Evidence evidence);

        /// <summary>
        /// Writes the <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2NameIdentifier"/> into the specified XML writer as a &lt;saml:NameID&gt; element.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Xml.XmlWriter"/> to write the &lt;saml:NameID&gt; element.</param>
        /// <param name="issuer">The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2NameIdentifier"/>.</param>
        void WriteIssuerToWriter(XmlWriter writer, Saml2NameIdentifier issuer);

        /// <summary>
        /// Writes the <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2NameIdentifier"/> into the specified XML writer as a &lt;saml:NameID&gt; element.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Xml.XmlWriter"/> to write the &lt;saml:NameID&gt; element.</param>
        /// <param name="nameId">The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2NameIdentifier"/>.</param>
        void WriteNameIdToWriter(XmlWriter writer, Saml2NameIdentifier nameId);

        /// <summary>
        /// Writes the <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Subject"/> into the specified XML writer as a &lt;saml:Subject&gt; element.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Xml.XmlWriter"/> to write the &lt;saml:Subject&gt; element.</param>
        /// <param name="subject">The <see cref="T:Micorosoft.IdentityModel.Tokens.Saml2.Saml2Subject"/>.</param>
        void WriteSubjectToWriter(XmlWriter writer, Saml2Subject subject);
    }
}
