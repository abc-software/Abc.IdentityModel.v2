// ----------------------------------------------------------------------------
// <copyright file="MetadataSerializer.Write.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

#pragma warning disable SA1101 // Prefix local calls with this

namespace Abc.IdentityModel.Metadata {
    using Microsoft.IdentityModel.Tokens.Saml2;
    using Microsoft.IdentityModel.Xml;
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Provides support for metadata serialization.
    /// </summary>
    public partial class MetadataSerializer {
        private DSigSerializer dsigSerializer = DSigSerializer.Default;
        private string prefix = MetadataConstants.Prefixes.Metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataSerializer"/> class.
        /// </summary>
        public MetadataSerializer() {
        }

        /// <summary>
        /// Gets or sets the <see cref="DSigSerializer"/> to use for reading / writing the <see cref="Xml.Signature"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">if value is null.</exception>
        /// <remarks>Passed to <see cref="EnvelopedSignatureReader"/> and <see cref="EnvelopedSignatureWriter"/>.</remarks>
        public DSigSerializer DSigSerializer {
            get => this.dsigSerializer;
            set => this.dsigSerializer = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the prefix to use when writing XML.
        /// </summary>
        /// <exception cref="ArgumentNullException">if value is null or empty.</exception>
        public string Prefix {
            get => this.prefix;
            set => this.prefix = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>Writes the metadata.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="metadata">The metadata descriptor base.
        /// An instance of the <see cref="EntityDescriptor" /> or <see cref="EntitiesDescriptor" />.
        /// </param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="metadata" /> is <c>null</c>.</exception>
        /// <exception cref="MetadataSerializationException"><paramref name="metadata" /> is not assignable from <see cref="EntityDescriptor" /> or <see cref="EntitiesDescriptor" />.</exception>
        public virtual void WriteMetadata(XmlWriter writer, DescriptorBase metadata) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (metadata is null) {
                throw new ArgumentNullException(nameof(metadata));
            }

            switch (metadata) {
                case EntitiesDescriptor entitiesDescriptor:
                    this.WriteEntitiesDescriptor(writer, entitiesDescriptor);
                    break;
                case EntityDescriptor entityDescriptor:
                    this.WriteEntityDescriptor(writer, entityDescriptor);
                    break;
                default:
                    throw new MetadataSerializationException("An error occurred while writing the metadata document.");
            }
        }

        /// <summary>Writes an endpoint.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="element">The XML qualified name element.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="endpoint" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="element" /> is <c>null</c>.</exception>
        protected virtual void WriteEndpointType(XmlWriter writer, EndpointType endpoint, XmlQualifiedName element) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (endpoint is null) {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            // <element.Name>
            writer.WriteStartElement(this.Prefix, element.Name, element.Namespace);

            // @Binding - required
            writer.WriteAttributeString(MetadataConstants.AttributeNames.Binding, endpoint.Binding.OriginalString);

            // @Location - required
            writer.WriteAttributeString(MetadataConstants.AttributeNames.Location, endpoint.Location.OriginalString);

            // @ResponseLocation - optional
            if (endpoint.ResponseLocation != null) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.ResponseLocation, endpoint.ResponseLocation.OriginalString);
            }

            // @anyAttribute
            WriteAnyAttributes(writer, endpoint);

            // <any>
            WriteAnyElements(writer, endpoint);

            // </element.Name>
            writer.WriteEndElement();
        }

        /// <summary>Writes an indexed endpoint.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="indexedEndpoint">The indexed endpoint.</param>
        /// <param name="element">The XML qualified element.</param>
        protected virtual void WriteIndexedEndpointType(XmlWriter writer, IndexedEndpointType indexedEndpoint, XmlQualifiedName element) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (indexedEndpoint is null) {
                throw new ArgumentNullException(nameof(indexedEndpoint));
            }

            if (element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            // <element.Name>
            writer.WriteStartElement(this.Prefix, element.Name, element.Namespace);

            // @Binding - required
            writer.WriteAttributeString(MetadataConstants.AttributeNames.Binding, indexedEndpoint.Binding.OriginalString);

            // @Location - required
            writer.WriteAttributeString(MetadataConstants.AttributeNames.Location, indexedEndpoint.Location.OriginalString);

            // @ResponseLocation - optional
            if (indexedEndpoint.ResponseLocation != null) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.ResponseLocation, indexedEndpoint.ResponseLocation.OriginalString);
            }

            // @index - required
            writer.WriteAttributeString(MetadataConstants.AttributeNames.Index, XmlConvert.ToString(indexedEndpoint.Index));

            // @isDefault - optional
            if (indexedEndpoint.IsDefault.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.IsDefault, XmlConvert.ToString(indexedEndpoint.IsDefault.Value));
            }

            // @anyAttribute
            WriteAnyAttributes(writer, indexedEndpoint);

            // <any>
            WriteExtensionsElement(writer, indexedEndpoint);

            // </element.Name>
            writer.WriteEndElement();
        }

        /// <summary>Writes a localized name.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="name">The localized name.</param>
        /// <param name="element">The XML qualified name.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="element" /> is <c>null</c>.</exception>
        protected virtual void WriteLocalizedName(XmlWriter writer, LocalizedName name, XmlQualifiedName element) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (name is null) {
                throw new ArgumentNullException(nameof(name));
            }

            if (element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            // <element.Name>
            writer.WriteStartElement(this.Prefix, element.Name, element.Namespace);

            // @xml:lang - required
            writer.WriteAttributeString(MetadataConstants.Prefixes.Xml, MetadataConstants.AttributeNames.Lang, MetadataConstants.Namespaces.Xml, name.Language.Name);

            // string content
            if (!string.IsNullOrEmpty(name.Name)) {
                writer.WriteString(name.Name);
            }

            // </element.Name>
            writer.WriteEndElement();
        }

        /// <summary>Writes a localized URI.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="uri">The localized URI.</param>
        /// <param name="element">The XML qualified name.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="uri" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="element" /> is <c>null</c>.</exception>
        protected virtual void WriteLocalizedUri(XmlWriter writer, LocalizedUri uri, XmlQualifiedName element) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (uri is null) {
                throw new ArgumentNullException(nameof(uri));
            }

            if (element is null) {
                throw new ArgumentNullException(nameof(element));
            }

            // <element.Name>
            writer.WriteStartElement(this.Prefix, element.Name, element.Namespace);

            // @xml:lang - required
            writer.WriteAttributeString(MetadataConstants.Prefixes.Xml, MetadataConstants.AttributeNames.Lang, MetadataConstants.Namespaces.Xml, uri.Language.Name);

            // string content
            if (uri.Uri != null) {
                writer.WriteString(uri.Uri.OriginalString);
            }

            // </element.Name>
            writer.WriteEndElement();
        }

        /// <summary>Writes an entities descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="entitiesDescriptor">The entities descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="entitiesDescriptor" /> is <c>null</c>.</exception>
        /// <exception cref="MetadataSerializationException">if <paramref name="entitiesDescriptor" /> does not contain any entity or entity group or same time contain entities and entity groups.</exception>
        protected virtual void WriteEntitiesDescriptor(XmlWriter writer, EntitiesDescriptor entitiesDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (entitiesDescriptor is null) {
                throw new ArgumentNullException(nameof(entitiesDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, entitiesDescriptor);

            // <EntitiesDescriptor>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.EntitiesDescriptor, MetadataConstants.Namespaces.Metadata);

            // @ID, @validUntil, @cacheDuration - optional
            this.WriteDescriptorBaseAttributes(writer, entitiesDescriptor);

            // @Name - optional
            if (!string.IsNullOrEmpty(entitiesDescriptor.Name)) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.Name, entitiesDescriptor.Name);
            }

            // @anyAttribute
            this.WriteAnyAttributes(writer, entitiesDescriptor);

            // <ds:Signature> 0-1
            if (entitiesDescriptor.SigningCredentials != null && writer is EnvelopedSignatureWriter envelopedSignatureWriter) {
                envelopedSignatureWriter.WriteSignature();
            }

            // <Extensions> 0-1
            this.WriteExtensionsElement(writer, entitiesDescriptor);

            // <EntitiyDescriptor> or <EntitiesDescriptor> 1-OO
            if ((entitiesDescriptor.Entities.Count == 0 && entitiesDescriptor.EntityGroups.Count == 0)
                || (entitiesDescriptor.Entities.Count > 0 && entitiesDescriptor.EntityGroups.Count > 0)) {
                throw new MetadataSerializationException("EntitiesDescriptor must contains at least one entity or a nested entity group");
            }

            foreach (var entityDescriptor in entitiesDescriptor.Entities) {
                if (!string.IsNullOrEmpty(entityDescriptor.FederationId) && !string.Equals(entityDescriptor.FederationId, entitiesDescriptor.Name, StringComparison.Ordinal)) {
                    throw new MetadataSerializationException("FederationID must be equal Name.");
                }

                this.WriteEntityDescriptor(writer, entityDescriptor);
            }

            foreach (var entityGroup in entitiesDescriptor.EntityGroups) {
                this.WriteEntitiesDescriptor(writer, entityGroup);
            }

            // </EntitiesDescriptor>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes an descriptor base attributes.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="metadataDescriptor">The metadata base descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="metadataDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteDescriptorBaseAttributes(XmlWriter writer, DescriptorBase metadataDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (metadataDescriptor is null) {
                throw new ArgumentNullException(nameof(metadataDescriptor));
            }

            // @validUntil - optional
            if (metadataDescriptor.ValidUntil.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.ValidUntil, XmlConvert.ToString(metadataDescriptor.ValidUntil.Value, XmlDateTimeSerializationMode.Utc));
            }

            // @cacheDuration - optional
            if (metadataDescriptor.CacheDuration.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.CacheDuration, XmlConvert.ToString(metadataDescriptor.CacheDuration.Value));
            }

            // @ID - optional
            if (metadataDescriptor.Id != null) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.Id, metadataDescriptor.Id.Value);
            }
        }

        /// <summary>Writes an entity descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="entityDescriptor">The entity descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="entityDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteEntityDescriptor(XmlWriter writer, EntityDescriptor entityDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (entityDescriptor is null) {
                throw new ArgumentNullException(nameof(entityDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, entityDescriptor);

            // <EntityDescriptor>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.EntityDescriptor, MetadataConstants.Namespaces.Metadata);

            // @entityID - required
            writer.WriteAttributeString(MetadataConstants.AttributeNames.EntityId, entityDescriptor.EntityId.Id);

            // @FederationID - optional
            if (!string.IsNullOrEmpty(entityDescriptor.FederationId)) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.FederationId, MetadataConstants.Namespaces.Federation, entityDescriptor.FederationId);
            }

            // @ID, @validUntil, @cacheDuration - optional
            this.WriteDescriptorBaseAttributes(writer, entityDescriptor);

            // @anyAttribute
            this.WriteAnyAttributes(writer, entityDescriptor);

            // <ds:Signature> 0-1
            if (entityDescriptor.SigningCredentials != null && writer is EnvelopedSignatureWriter envelopedSignatureWriter) {
                envelopedSignatureWriter.WriteSignature();
            }

            // <Extensions> 0-1
            this.WriteExtensionsElement(writer, entityDescriptor);

            // <RoleDescriptor>, <IDPSSODescriptor>, <SPSSODescriptor>, <AuthnAuthorityDescriptor>, <AttributeAuthorityDescriptor>, <PDPDescriptor> 0-OO
            // or <AffiliationDescriptor> 1
            if (entityDescriptor.RoleDescriptors.Count == 0 && entityDescriptor.AffiliationDescriptor == null) {
                throw new MetadataSerializationException("EntityDescriptor at least one RoleDescriptor or AffiliationDescriptor must be present");
            }

            foreach (var roleDescriptor in entityDescriptor.RoleDescriptors) {
                switch (roleDescriptor) {
                    case IdpSsoDescriptor ispSsoDescriptor:
                        WriteIdpSsoDescriptor(writer, ispSsoDescriptor);
                        break;
                    case SpSsoDescriptor spSsoDescriptor:
                        WriteSpSsoDescriptor(writer, spSsoDescriptor);
                        break;
                    case AuthnAuthorityDescriptor authnAuthorityDescriptor:
                        WriteAuthnAuthorityDescriptor(writer, authnAuthorityDescriptor);
                        break;
                    case PdpDescriptor pdpDescriptor:
                        WritePdpDescriptor(writer, pdpDescriptor);
                        break;
                    case AttributeAuthorityDescriptor attributeAuthorityDescriptor:
                        WriteAttributeAuthorityDescriptor(writer, attributeAuthorityDescriptor);
                        break;
                    case ApplicationServiceDescriptor appServiceDesciptor:
                        WriteApplicationServiceDescriptor(writer, appServiceDesciptor);
                        break;
                    case SecurityTokenServiceDescriptor securityTokenServiceDescriptor:
                        WriteSecurityTokenServiceDescriptor(writer, securityTokenServiceDescriptor);
                        break;
                    case PseudonymServiceDescriptor pseudonymServiceDescriptor:
                        WritePseudonymServiceDescriptor(writer, pseudonymServiceDescriptor);
                        break;
                    case AttributeServiceDescriptor attributeServiceDescriptor:
                        WriteAttributeServiceDescriptor(writer, attributeServiceDescriptor);
                        break;
                }
            }

            if (entityDescriptor.AffiliationDescriptor != null) {
                WriteAffiliationDescriptor(writer, entityDescriptor.AffiliationDescriptor);
            }

            // <Organization> 0-1
            if (entityDescriptor.Organization != null) {
                this.WriteOrganization(writer, entityDescriptor.Organization);
            }

            // <ContactPerson> 0-OO
            foreach (var contact in entityDescriptor.ContactPersons) {
                this.WriteContactPerson(writer, contact);
            }

            // </EntityDescriptor>
            writer.WriteEndElement();
        }

        /// <summary>Writes an organization.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="organization">The organization.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="organization" /> is <c>null</c>.</exception>
        protected virtual void WriteOrganization(XmlWriter writer, Organization organization) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (organization is null) {
                throw new ArgumentNullException(nameof(organization));
            }

            // <Organization>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.Organization, MetadataConstants.Namespaces.Metadata);

            // @anyAttribute
            WriteAnyAttributes(writer, organization);

            // <Extensions>
            WriteExtensionsElement(writer, organization);

            // <OrganizationName> 1-OO
            if (organization.Names.Count == 0) {
                throw new MetadataSerializationException($"Must be set at least one {MetadataConstants.ElementNames.OrganizationName}");
            }

            foreach (var name in organization.Names) {
                WriteLocalizedName(writer, name, new XmlQualifiedName(MetadataConstants.ElementNames.OrganizationName, MetadataConstants.Namespaces.Metadata));
            }

            // <OrganizationDisplayName> 1-OO
            if (organization.DisplayNames.Count == 0) {
                throw new MetadataSerializationException($"Must be set at least one {MetadataConstants.ElementNames.OrganizationDisplayName}");
            }

            foreach (var displayName in organization.DisplayNames) {
                WriteLocalizedName(writer, displayName, new XmlQualifiedName(MetadataConstants.ElementNames.OrganizationDisplayName, MetadataConstants.Namespaces.Metadata));
            }

            // <OrganizationURL> 1-OO
            if (organization.Urls.Count == 0) {
                throw new MetadataSerializationException($"Must be set at least one {MetadataConstants.ElementNames.OrganizationURL}");
            }

            foreach (var url in organization.Urls) {
                WriteLocalizedUri(writer, url, new XmlQualifiedName(MetadataConstants.ElementNames.OrganizationURL, MetadataConstants.Namespaces.Metadata));
            }

            // </Organization>
            writer.WriteEndElement();
        }

        /// <summary>Writes a contact person.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="contactPerson">The contact person.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="contactPerson" /> is <c>null</c>.</exception>
        protected virtual void WriteContactPerson(XmlWriter writer, ContactPerson contactPerson) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (contactPerson is null) {
                throw new ArgumentNullException(nameof(contactPerson));
            }

            // <ContactPerson>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.ContactPerson, MetadataConstants.Namespaces.Metadata);

            // @contactType - required
            writer.WriteAttributeString(MetadataConstants.AttributeNames.ContactType, contactPerson.ContactType.ToString().ToLowerInvariant());

            // @anyAttribute
            WriteAnyAttributes(writer, contactPerson);

            // <Extensions>
            WriteExtensionsElement(writer, contactPerson);

            // <Company> 0-1
            if (!string.IsNullOrEmpty(contactPerson.Company)) {
                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.Company, MetadataConstants.Namespaces.Metadata, contactPerson.Company);
            }

            // <GivenName> 0-1
            if (!string.IsNullOrEmpty(contactPerson.GivenName)) {
                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.GivenName, MetadataConstants.Namespaces.Metadata, contactPerson.GivenName);
            }

            // <SurName> 0-1
            if (!string.IsNullOrEmpty(contactPerson.SurName)) {
                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.SurName, MetadataConstants.Namespaces.Metadata, contactPerson.SurName);
            }

            // <EmailAddress> 0-OO
            foreach (var emailAddress in contactPerson.EmailAddresses) {
                if (emailAddress.Scheme != "mailto") {
                    throw new MetadataSerializationException("URI must represent e-mail address.");
                }

                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.EmailAddress, MetadataConstants.Namespaces.Metadata, emailAddress.OriginalString);
            }

            // <TelephoneNumber> 0-OO
            foreach (var telephoneNumber in contactPerson.TelephoneNumbers) {
                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.TelephoneNumber, MetadataConstants.Namespaces.Metadata, telephoneNumber);
            }

            // </ContactPerson>
            writer.WriteEndElement();
        }

        /// <summary>Writes role descriptor attributes.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="roleDescriptor">The role descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="roleDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteRoleDescriptorAttributes(XmlWriter writer, RoleDescriptor roleDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (roleDescriptor is null) {
                throw new ArgumentNullException(nameof(roleDescriptor));
            }

            // @ID, @validUntil, @cacheDuration
            this.WriteDescriptorBaseAttributes(writer, roleDescriptor);

            // @errorURL - optional
            if (roleDescriptor.ErrorUrl != null) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.ErrorURL, roleDescriptor.ErrorUrl.OriginalString);
            }

            // @protocolSupportEnumeration - required
            if (roleDescriptor.ProtocolsSupported.Count == 0) {
                throw new MetadataSerializationException("protocolSupportEnumeration must be at least one value.");
            }

            var stringBuilder = new StringBuilder();
            foreach (Uri item in roleDescriptor.ProtocolsSupported) {
                stringBuilder.Append(item.OriginalString);
                stringBuilder.Append(" ");
            }

            stringBuilder.Length--; // remove last space

            writer.WriteAttributeString(MetadataConstants.AttributeNames.ProtocolSupportEnumeration, stringBuilder.ToString());

            // @anyAttribute
            WriteAnyAttributes(writer, roleDescriptor);
        }

        /// <summary>Writes the role descriptor element.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="roleDescriptor">The role descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="roleDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteRoleDescriptorElements(XmlWriter writer, RoleDescriptor roleDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (roleDescriptor is null) {
                throw new ArgumentNullException(nameof(roleDescriptor));
            }

            // <ds:Signature> 0-1
            if (roleDescriptor.SigningCredentials != null && writer is EnvelopedSignatureWriter envelopedSignatureWriter) {
                envelopedSignatureWriter.WriteSignature();
            }

            // <Extensions> 0-1
            WriteExtensionsElement(writer, roleDescriptor);

            // <KeyDescriptor> 0-OO
            foreach (var key in roleDescriptor.KeyDescriptors) {
                WriteKeyDescriptor(writer, key);
            }

            // <Organization> 0-1
            if (roleDescriptor.Organization != null) {
                WriteOrganization(writer, roleDescriptor.Organization);
            }

            // <ContactPerson> 0-OO
            foreach (var contact in roleDescriptor.ContactPersons) {
                WriteContactPerson(writer, contact);
            }
        }

        /// <summary>Writes a key descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="keyDescriptor">The key descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="keyDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteKeyDescriptor(XmlWriter writer, KeyDescriptor keyDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (keyDescriptor is null) {
                throw new ArgumentNullException(nameof(keyDescriptor));
            }

            // <KeyDescriptor>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.KeyDescriptor, MetadataConstants.Namespaces.Metadata);

            // @use - optional
            if (keyDescriptor.Use.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.Use, keyDescriptor.Use.Value.ToString().ToLowerInvariant());
            }

            // <ds:KeyInfo> 1
            DSigSerializer.WriteKeyInfo(writer, keyDescriptor.KeyInfo);

            // <EncryptionMethod> 0-OO
            foreach (var method in keyDescriptor.EncryptionMethods) {
                writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.EncryptionMethod, MetadataConstants.Namespaces.Metadata);

                Debug.Assert(method.Algorithm != null, "Must be not null");
                Debug.Assert(method.Algorithm.IsAbsoluteUri, "Must be absolute Uri.");
                writer.WriteAttributeString(MetadataConstants.AttributeNames.Algorithm, method.Algorithm.AbsoluteUri);
                writer.WriteEndElement();
            }

            // </KeyDescriptor>
            writer.WriteEndElement();
        }

        /// <summary>Writes the SSO descriptor attributes.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="ssoDescriptor">The SSO descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="ssoDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteSsoDescriptorAttributes(XmlWriter writer, SsoDescriptor ssoDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (ssoDescriptor is null) {
                throw new ArgumentNullException(nameof(ssoDescriptor));
            }

            WriteRoleDescriptorAttributes(writer, ssoDescriptor);
        }

        /// <summary>Writes the SSO descriptor element.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="ssoDescriptor">The SSO descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="ssoDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteSsoDescriptorElements(XmlWriter writer, SsoDescriptor ssoDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (ssoDescriptor is null) {
                throw new ArgumentNullException(nameof(ssoDescriptor));
            }

            WriteRoleDescriptorElements(writer, ssoDescriptor);

            // <ArtifactResolutionService> 0-1
            foreach (var value in ssoDescriptor.ArtifactResolutionServices) {
                if (value.ResponseLocation != null) {
                    throw new MetadataSerializationException("The ResponseLocation attribute must be omitted.");
                }

                WriteIndexedEndpointType(writer, value, new XmlQualifiedName(MetadataConstants.ElementNames.ArtifactResolutionService, MetadataConstants.Namespaces.Metadata));
            }

            // <SingleLogoutService> 0-1
            foreach (var singleLogoutService in ssoDescriptor.SingleLogoutServices) {
                WriteEndpointType(writer, singleLogoutService, new XmlQualifiedName(MetadataConstants.ElementNames.SingleLogoutService, MetadataConstants.Namespaces.Metadata));
            }

            // <ManageNameIDService> 0-1
            foreach (var singleLogoutService in ssoDescriptor.ManageNameIDServices) {
                WriteEndpointType(writer, singleLogoutService, new XmlQualifiedName(MetadataConstants.ElementNames.ManageNameIDService, MetadataConstants.Namespaces.Metadata));
            }

            // <NameIDFormat> 0-1
            foreach (Uri nameIdentifierFormat in ssoDescriptor.NameIdFormats) {
                // TODO: absoluteURI
                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.NameIDFormat, MetadataConstants.Namespaces.Metadata, nameIdentifierFormat.AbsoluteUri);
            }
        }

        /// <summary>Writes an IDPSSO descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="idpSsoDescriptor">The IDPSSO descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="ssoDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteIdpSsoDescriptor(XmlWriter writer, IdpSsoDescriptor idpSsoDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (idpSsoDescriptor is null) {
                throw new ArgumentNullException(nameof(idpSsoDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, idpSsoDescriptor);

            // <IDPSSODescriptor>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.IdpSsoDescriptor, MetadataConstants.Namespaces.Metadata);

            // @WantAuthnRequestsSigned - optional
            if (idpSsoDescriptor.WantAuthnRequestsSigned.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.WantAuthnRequestsSigned, XmlConvert.ToString(idpSsoDescriptor.WantAuthnRequestsSigned.Value));
            }

            WriteSsoDescriptorAttributes(writer, idpSsoDescriptor);
            WriteSsoDescriptorElements(writer, idpSsoDescriptor);

            // <SingleSignOnService> 1-OO
            if (idpSsoDescriptor.SingleSignOnServices.Count == 0) {
                throw new MetadataSerializationException("SingleSignOnService must be defined at least one endpoint");
            }

            foreach (var singleSignOnService in idpSsoDescriptor.SingleSignOnServices) {
                if (singleSignOnService.ResponseLocation != null) {
                    throw new MetadataSerializationException("The ResponseLocation attribute must be omitted.");
                }

                WriteEndpointType(writer, singleSignOnService, new XmlQualifiedName(MetadataConstants.ElementNames.SingleSignOnService, MetadataConstants.Namespaces.Metadata));
            }

            // <NameIDMappingService> 0-OO
            foreach (var service in idpSsoDescriptor.NameIdMappingServices) {
                if (service.ResponseLocation != null) {
                    throw new MetadataSerializationException("The ResponseLocation attribute must be omitted.");
                }

                WriteEndpointType(writer, service, new XmlQualifiedName(MetadataConstants.ElementNames.NameIDMappingService, MetadataConstants.Namespaces.Metadata));
            }

            // <AssertionIDRequestService> 0-OO
            foreach (var service in idpSsoDescriptor.AssertionIdRequestServices) {
                WriteEndpointType(writer, service, new XmlQualifiedName(MetadataConstants.ElementNames.AssertionIDRequestService, MetadataConstants.Namespaces.Metadata));
            }

            // <AttributeProfile> 0-OO
            foreach (var attributeProfile in idpSsoDescriptor.AttributeProfiles) {
                // TODO: absoluteURI
                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.AttributeProfile, MetadataConstants.Namespaces.Metadata, attributeProfile.AbsoluteUri);
            }

            // <saml:Attribute> 0-OO
            foreach (var attribute in idpSsoDescriptor.Attributes) {
                WriteAttribute(writer, attribute);
            }

            // </IDPSSODescriptor>
            writer.WriteEndElement();
        }

        /// <summary>Writes an SPSSO descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="spSsoDescriptor">The SPSSO descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="spSsoDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteSpSsoDescriptor(XmlWriter writer, SpSsoDescriptor spSsoDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (spSsoDescriptor is null) {
                throw new ArgumentNullException(nameof(spSsoDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, spSsoDescriptor);

            // <SPSSODescriptor>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.SpSsoDescriptor, MetadataConstants.Namespaces.Metadata);

            // @AuthnRequestsSigned - optional
            if (spSsoDescriptor.AuthnRequestsSigned.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.AuthnRequestsSigned, XmlConvert.ToString(spSsoDescriptor.AuthnRequestsSigned.Value));
            }

            // @WantAssertionsSigned - optional
            if (spSsoDescriptor.WantAssertionsSigned.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.WantAssertionsSigned, XmlConvert.ToString(spSsoDescriptor.WantAssertionsSigned.Value));
            }

            WriteSsoDescriptorAttributes(writer, spSsoDescriptor);
            WriteSsoDescriptorElements(writer, spSsoDescriptor);

            // <AssertionConsumerService> 1-OO
            if (spSsoDescriptor.AssertionConsumerServices.Count == 0) {
                throw new MetadataSerializationException("AssertionConsumerService must be at least one");
            }

            foreach (var service in spSsoDescriptor.AssertionConsumerServices) {
                // TODO: at least one such endpoint, by definition
                WriteIndexedEndpointType(writer, service, new XmlQualifiedName(MetadataConstants.ElementNames.AssertionConsumerService, MetadataConstants.Namespaces.Metadata));
            }

            // <AttributeConsumingService> 0-OO
            foreach (var attributeConsumingService in spSsoDescriptor.AttributeConsumingServices) {
                WriteAttributeConsumingService(writer, attributeConsumingService);
            }

            // </SPSSODescriptor>
            writer.WriteEndElement();
        }

        /// <summary>Writes an attribute consuming service.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="attributeConsumingService">The attribute consuming service.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="attributeConsumingService" /> is <c>null</c>.</exception>
        protected virtual void WriteAttributeConsumingService(XmlWriter writer, AttributeConsumingService attributeConsumingService) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (attributeConsumingService is null) {
                throw new ArgumentNullException(nameof(attributeConsumingService));
            }

            // <AttributeConsumingService>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.AttributeConsumingService, MetadataConstants.Namespaces.Metadata);

            // @index - required
            writer.WriteAttributeString(MetadataConstants.AttributeNames.Index, XmlConvert.ToString(attributeConsumingService.Index));

            // @isDefault - optional
            if (attributeConsumingService.IsDefault.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.IsDefault, XmlConvert.ToString(attributeConsumingService.IsDefault.Value));
            }

            // <ServiceName> 1-OO
            foreach (var serviceName in attributeConsumingService.ServiceNames) {
                WriteLocalizedName(writer, serviceName, new XmlQualifiedName(MetadataConstants.ElementNames.ServiceName, MetadataConstants.Namespaces.Metadata));
            }

            // <ServiceDescription> 0-OO
            foreach (var serviceDescription in attributeConsumingService.ServiceDescriptions) {
                WriteLocalizedName(writer, serviceDescription, new XmlQualifiedName(MetadataConstants.ElementNames.ServiceDescription, MetadataConstants.Namespaces.Metadata));
            }

            // <RequestedAttribute> 0-OO
            foreach (var requestedAttribute in attributeConsumingService.RequestedAttributes) {
                WriteRequestedAttribute(writer, requestedAttribute);
            }

            // </AttributeConsumingService>
            writer.WriteEndElement();
        }

        /// <summary>Writes an requested attribute.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="requestedAttribute">The requested attribute.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="requestedAttribute" /> is <c>null</c>.</exception>
        protected virtual void WriteRequestedAttribute(XmlWriter writer, RequestedAttribute requestedAttribute) {
            // <RequestedAttribute>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.RequestedAttribute, MetadataConstants.Namespaces.Metadata);

            // @isRequired - optional
            if (requestedAttribute.IsRequired.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.IsRequired, XmlConvert.ToString(requestedAttribute.IsRequired.Value));
            }

            // @Name
            writer.WriteAttributeString("Name", requestedAttribute.Name);

            // @NameFormat - optional
            if (requestedAttribute.NameFormat != null) {
                Debug.Assert(requestedAttribute.NameFormat != null, "Must be not null");
                Debug.Assert(requestedAttribute.NameFormat.IsAbsoluteUri, "Must be absolute Uri.");
                writer.WriteAttributeString("NameFormat", requestedAttribute.NameFormat.AbsoluteUri);
            }

            // @FriendlyName - optional
            if (requestedAttribute.FriendlyName != null) {
                writer.WriteAttributeString("FriendlyName", requestedAttribute.FriendlyName);
            }

            // <AttributeValue>
            foreach (string value in requestedAttribute.Values) {
                writer.WriteStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
                if (value == null) {
                    // @xsi:nil=true
                    writer.WriteAttributeString("nil", MetadataConstants.Namespaces.XmlSchema, XmlConvert.ToString(true));
                }
                else if (value.Length > 0) {
                    writer.WriteString(value);
                }

                // </AttributeValue>
                writer.WriteEndElement();
            }

            // </RequestedAttribute>
            writer.WriteEndElement();
        }

        /// <summary>Writes an authentication authority descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="authnAuthorityDescriptor">The authentication authority descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="authnAuthorityDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteAuthnAuthorityDescriptor(XmlWriter writer, AuthnAuthorityDescriptor authnAuthorityDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (authnAuthorityDescriptor is null) {
                throw new ArgumentNullException(nameof(authnAuthorityDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, authnAuthorityDescriptor);

            // <AuthnAuthorityDescriptor>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.AuthnAuthorityDescriptor, MetadataConstants.Namespaces.Metadata);

            // <AuthnQueryService> 1-OO
            if (authnAuthorityDescriptor.AuthnQueryServices.Count == 0) {
                throw new MetadataSerializationException("AuthnQueryServices must be at least one endpoint.");
            }

            foreach (var service in authnAuthorityDescriptor.AuthnQueryServices) {
                WriteEndpointType(writer, service, new XmlQualifiedName(MetadataConstants.ElementNames.AuthnQueryService, MetadataConstants.Namespaces.Metadata));
            }

            // <AssertionIDRequestService> 0-OO
            foreach (var service in authnAuthorityDescriptor.AssertionIdRequestServices) {
                WriteEndpointType(writer, service, new XmlQualifiedName(MetadataConstants.ElementNames.AssertionIDRequestService, MetadataConstants.Namespaces.Metadata));
            }

            // <NameIDFormat> 0-OO
            foreach (var format in authnAuthorityDescriptor.NameIdFormats) {
                // TODO: absoluteURI
                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.NameIDFormat, MetadataConstants.Namespaces.Metadata, format.AbsoluteUri);
            }

            // </AuthnAuthorityDescriptor>
            writer.WriteEndElement();
        }

        /// <summary>Writes an PDP descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="pdpDescriptor">The PDP descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="pdpDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WritePdpDescriptor(XmlWriter writer, PdpDescriptor pdpDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (pdpDescriptor is null) {
                throw new ArgumentNullException(nameof(pdpDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, pdpDescriptor);

            // <PDPDescriptor>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.PdpDescriptor, MetadataConstants.Namespaces.Metadata);

            // <AuthzService> 1-OO
            if (pdpDescriptor.AuthzServices.Count == 0) {
                throw new MetadataSerializationException("AuthzServices must be at least one endpoint.");
            }

            foreach (var service in pdpDescriptor.AuthzServices) {
                WriteEndpointType(writer, service, new XmlQualifiedName(MetadataConstants.ElementNames.AuthzService, MetadataConstants.Namespaces.Metadata));
            }

            // <AssertionIDRequestService> 0-OO
            foreach (var service in pdpDescriptor.AssertionIdRequestServices) {
                WriteEndpointType(writer, service, new XmlQualifiedName(MetadataConstants.ElementNames.AssertionIDRequestService, MetadataConstants.Namespaces.Metadata));
            }

            // <NameIDFormat> 0-OO
            foreach (var format in pdpDescriptor.NameIdFormats) {
                // TODO: absoluteURI
                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.NameIDFormat, MetadataConstants.Namespaces.Metadata, format.AbsoluteUri);
            }

            // </PDPDescriptor>
            writer.WriteEndElement();
        }

        /// <summary>Writes an attribute authority descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="attributeAuthorityDescriptor">The attribute authority descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="attributeAuthorityDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteAttributeAuthorityDescriptor(XmlWriter writer, AttributeAuthorityDescriptor attributeAuthorityDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (attributeAuthorityDescriptor is null) {
                throw new ArgumentNullException(nameof(attributeAuthorityDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, attributeAuthorityDescriptor);

            // <AttributeAuthorityDescriptor>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.AttributeAuthorityDescriptor, MetadataConstants.Namespaces.Metadata);

            // <AttributeService> 1-OO
            if (attributeAuthorityDescriptor.AttributeServices.Count == 0) {
                throw new MetadataSerializationException("AttributeServices must be defined at least one endpoint");
            }

            foreach (var service in attributeAuthorityDescriptor.AttributeServices) {
                WriteEndpointType(writer, service, new XmlQualifiedName(MetadataConstants.ElementNames.AttributeService, MetadataConstants.Namespaces.Metadata));
            }

            // <AssertionIDRequestService> 0-OO
            foreach (var service in attributeAuthorityDescriptor.AssertionIdRequestServices) {
                WriteEndpointType(writer, service, new XmlQualifiedName(MetadataConstants.ElementNames.AssertionIDRequestService, MetadataConstants.Namespaces.Metadata));
            }

            // <NameIDFormat> 0-OO
            foreach (var format in attributeAuthorityDescriptor.NameIdFormats) {
                // TODO: absoluteURI
                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.NameIDFormat, MetadataConstants.Namespaces.Metadata, format.AbsoluteUri);
            }

            // <AttributeProfile> 0-OO
            foreach (var attributeProfile in attributeAuthorityDescriptor.AttributeProfiles) {
                writer.WriteElementString(this.Prefix, MetadataConstants.ElementNames.AttributeProfile, MetadataConstants.Namespaces.Metadata, attributeProfile.OriginalString);
            }

            // <saml:Attribute> 0-OO
            foreach (var attribute in attributeAuthorityDescriptor.Attributes) {
                WriteAttribute(writer, attribute);
            }

            // </AttributeAuthorityDescriptor>
            writer.WriteEndElement();
        }

        /// <summary>Writes an affiliation descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="affiliationDescriptor">The affiliation descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="affiliationDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteAffiliationDescriptor(XmlWriter writer, AffiliationDescriptor affiliationDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (affiliationDescriptor is null) {
                throw new ArgumentNullException(nameof(affiliationDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, affiliationDescriptor);

            // <AffiliationDescriptor>
            writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.AffiliationDescriptor, MetadataConstants.Namespaces.Metadata);

            // @affiliationOwnerID - required
            writer.WriteAttributeString(MetadataConstants.AttributeNames.AffiliationOwnerId, affiliationDescriptor.AffiliationOwnerId.Id);

            // @validUntil, @cacheDuration, @ID
            this.WriteDescriptorBaseAttributes(writer, affiliationDescriptor);

            // @anyAttribute
            this.WriteAnyAttributes(writer, affiliationDescriptor);

            // <ds:Signature> 0-1
            if (affiliationDescriptor.SigningCredentials != null && writer is EnvelopedSignatureWriter envelopedSignatureWriter) {
                envelopedSignatureWriter.WriteSignature();
            }

            // <Extensions> 0-1
            this.WriteExtensionsElement(writer, affiliationDescriptor);

            // <AffiliateMember> 0-OO
            foreach (var member in affiliationDescriptor.AffiliateMembers) {
                writer.WriteElementString(this.Prefix, MetadataConstants.Namespaces.Metadata, member.Id);
            }

            // <KeyDescriptor> 0-OO
            foreach (var keyDescriptor in affiliationDescriptor.KeyDescriptors) {
                this.WriteKeyDescriptor(writer, keyDescriptor);
            }

            // </AffiliationDescriptor>
            writer.WriteEndElement();
        }

        /// <summary>Writes the web service descriptor attributes.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="webServiceDescriptor">The web service descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="webServiceDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteWebServiceDescriptorAttributes(XmlWriter writer, WebServiceDescriptor webServiceDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (webServiceDescriptor is null) {
                throw new ArgumentNullException(nameof(webServiceDescriptor));
            }

            WriteRoleDescriptorAttributes(writer, webServiceDescriptor);

            // @ServiceDisplayName - optional
            if (!string.IsNullOrEmpty(webServiceDescriptor.ServiceDisplayName)) {
                writer.WriteAttributeString("ServiceDisplayName", webServiceDescriptor.ServiceDisplayName);
            }

            // @ServiceDescription - optional
            if (!string.IsNullOrEmpty(webServiceDescriptor.ServiceDescription)) {
                writer.WriteAttributeString("ServiceDescription", webServiceDescriptor.ServiceDescription);
            }
        }

        /// <summary>Writes a web service descriptor element.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="webServiceDescriptor">The web service descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="webServiceDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteWebServiceDescriptorElements(XmlWriter writer, WebServiceDescriptor webServiceDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (webServiceDescriptor is null) {
                throw new ArgumentNullException(nameof(webServiceDescriptor));
            }

            WriteRoleDescriptorElements(writer, webServiceDescriptor);

            // <fed:LogicalServiceNamesOffered> 0-1

            // <fed:TokenTypesOffered> 0-1
            if (webServiceDescriptor.TokenTypesOffered.Count > 0) {
                writer.WriteStartElement(MetadataConstants.ElementNames.TokenTypesOffered, MetadataConstants.Namespaces.Federation);
                foreach (var item in webServiceDescriptor.TokenTypesOffered) {
                    // <fed:TokenType>
                    writer.WriteStartElement(MetadataConstants.ElementNames.TokenType, MetadataConstants.Namespaces.Federation);

                    // @Uri - required
                    writer.WriteAttributeString(MetadataConstants.AttributeNames.Uri, item.AbsoluteUri); // TODO: AbsoluteUri

                    // </fed:TokenType>
                    writer.WriteEndElement();
                }

                // </fed:TokenTypesOffered>
                writer.WriteEndElement();
            }

            // <fed:ClaimDialectsOffered> 0-1
            if (webServiceDescriptor.ClaimDialectsOffered.Count > 0) {
                writer.WriteStartElement(MetadataConstants.ElementNames.ClaimDialectsOffered, MetadataConstants.Namespaces.Federation);
                foreach (var item in webServiceDescriptor.ClaimDialectsOffered) {
                    // <fed:ClaimDialect>
                    writer.WriteStartElement(MetadataConstants.ElementNames.ClaimDialect, MetadataConstants.Namespaces.Federation);

                    // @Uri - required
                    writer.WriteAttributeString("Uri", item.AbsoluteUri); // TODO: AbsoluteUri

                    // </fed:ClaimDialect>
                    writer.WriteEndElement();
                }

                // </fed:ClaimTypesOffered>
                writer.WriteEndElement();
            }

            // <fed:ClaimTypesOffered> 0-1
            if (webServiceDescriptor.ClaimTypesOffered.Count > 0) {
                writer.WriteStartElement(MetadataConstants.ElementNames.ClaimTypesOffered, MetadataConstants.Namespaces.Federation);
                foreach (var claim in webServiceDescriptor.ClaimTypesOffered) {
                    WriteDisplayClaim(writer, claim);
                }

                // </fed:ClaimTypesOffered>
                writer.WriteEndElement();
            }

            // <fed:ClaimTypesRequested> 0-1
            if (webServiceDescriptor.ClaimTypesRequested.Count > 0) {
                writer.WriteStartElement(MetadataConstants.ElementNames.ClaimTypesRequested, MetadataConstants.Namespaces.Federation);
                foreach (var claim in webServiceDescriptor.ClaimTypesRequested) {
                    WriteDisplayClaim(writer, claim);
                }

                // </fed:ClaimTypesRequested>
                writer.WriteEndElement();
            }

            // <fed:AutomaticPseudonyms> 0-1
            if (webServiceDescriptor.AutomaticPseudonyms.HasValue) {
                writer.WriteElementString(MetadataConstants.ElementNames.AutomaticPseudonyms, MetadataConstants.Namespaces.Federation, XmlConvert.ToString(webServiceDescriptor.AutomaticPseudonyms.Value));
            }

            // <fed:TargetScopes> 0-1
            if (webServiceDescriptor.TargetScopes.Count > 0) {
                writer.WriteStartElement(MetadataConstants.ElementNames.TargetScopes, MetadataConstants.Namespaces.Federation);
                foreach (var targetScope in webServiceDescriptor.TargetScopes) {
                    WriteEndpointReference(writer, targetScope);
                }

                // </fed:TargetScopes>
                writer.WriteEndElement();
            }
        }

        /// <summary>Writes an application service descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="appServiceDesciptor">The application service descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="appServiceDesciptor" /> is <c>null</c>.</exception>
        protected virtual void WriteApplicationServiceDescriptor(XmlWriter writer, ApplicationServiceDescriptor appServiceDesciptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (appServiceDesciptor is null) {
                throw new ArgumentNullException(nameof(appServiceDesciptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, appServiceDesciptor);

            // <RoleDescriptor>
            writer.WriteStartElement(MetadataConstants.ElementNames.RoleDescriptor, MetadataConstants.Namespaces.Metadata);

            // @xsi:type="fed:ApplicationServiceType"
            writer.WriteAttributeString(MetadataConstants.Prefixes.XmlSchema, MetadataConstants.AttributeNames.Type, MetadataConstants.Namespaces.XmlSchema, "fed:ApplicationServiceType");
            writer.WriteAttributeString(MetadataConstants.Prefixes.Xmlns, MetadataConstants.Prefixes.Federation, null, MetadataConstants.Namespaces.Federation);

            WriteWebServiceDescriptorAttributes(writer, appServiceDesciptor);
            WriteWebServiceDescriptorElements(writer, appServiceDesciptor);

            foreach (var endpoint in appServiceDesciptor.ApplicationServiceEndpoints) {
                // <ApplicationServiceEndpoint>
                writer.WriteStartElement(MetadataConstants.ElementNames.ApplicationServiceEndpoint, MetadataConstants.Namespaces.Federation);

                WriteEndpointReference(writer, endpoint);

                // </ApplicationServiceEndpoint>
                writer.WriteEndElement();
            }

            foreach (var endpoint in appServiceDesciptor.PassiveRequestorEndpoints) {
                // <PassiveRequestorEndpoint>
                writer.WriteStartElement(MetadataConstants.ElementNames.PassiveRequestorEndpoint, MetadataConstants.Namespaces.Federation);

                WriteEndpointReference(writer, endpoint);

                // </PassiveRequestorEndpoint>
                writer.WriteEndElement();
            }

            // </RoleDescriptor>
            writer.WriteEndElement();
        }

        /// <summary>Writes a security token service descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="securityTokenServiceDescriptor">The security token service descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="securityTokenServiceDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteSecurityTokenServiceDescriptor(XmlWriter writer, SecurityTokenServiceDescriptor securityTokenServiceDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (securityTokenServiceDescriptor is null) {
                throw new ArgumentNullException(nameof(securityTokenServiceDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, securityTokenServiceDescriptor);

            // <RoleDescriptor>
            writer.WriteStartElement(MetadataConstants.ElementNames.RoleDescriptor, MetadataConstants.Namespaces.Metadata);

            // @xsi:type="fed:SecurityTokenServiceType"
            writer.WriteAttributeString(MetadataConstants.Prefixes.XmlSchema, MetadataConstants.AttributeNames.Type, MetadataConstants.Namespaces.XmlSchema, "fed:SecurityTokenServiceType");
            writer.WriteAttributeString(MetadataConstants.Prefixes.Xmlns, MetadataConstants.Prefixes.Federation, null, MetadataConstants.Namespaces.Federation);

            WriteWebServiceDescriptorAttributes(writer, securityTokenServiceDescriptor);
            WriteWebServiceDescriptorElements(writer, securityTokenServiceDescriptor);

            if (securityTokenServiceDescriptor.SecurityTokenServiceEndpoints.Count == 0) {
                throw new MetadataSerializationException("SecurityTokenServiceEndpoints must be define at least one endpoint");
            }

            foreach (var endpoint in securityTokenServiceDescriptor.SecurityTokenServiceEndpoints) {
                writer.WriteStartElement(MetadataConstants.ElementNames.SecurityTokenServiceEndpoint, MetadataConstants.Namespaces.Federation);
                WriteEndpointReference(writer, endpoint);
                writer.WriteEndElement();
            }

            foreach (var endpoint in securityTokenServiceDescriptor.PassiveRequestorEndpoints) {
                writer.WriteStartElement(MetadataConstants.ElementNames.PassiveRequestorEndpoint, MetadataConstants.Namespaces.Federation);
                WriteEndpointReference(writer, endpoint);
                writer.WriteEndElement();
            }

            // </RoleDescriptor>
            writer.WriteEndElement();
        }

        /// <summary>Writes a security attribute service descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="attributeServiceDescriptor">The attribute service descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="securityTokenServiceDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WriteAttributeServiceDescriptor(XmlWriter writer, AttributeServiceDescriptor attributeServiceDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (attributeServiceDescriptor is null) {
                throw new ArgumentNullException(nameof(attributeServiceDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, attributeServiceDescriptor);

            throw new NotImplementedException();
        }

        /// <summary>Writes a security pseudonym service descriptor.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="pseudonymServiceDescriptor">The pseudonym service descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="securityTokenServiceDescriptor" /> is <c>null</c>.</exception>
        protected virtual void WritePseudonymServiceDescriptor(XmlWriter writer, PseudonymServiceDescriptor pseudonymServiceDescriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (pseudonymServiceDescriptor is null) {
                throw new ArgumentNullException(nameof(pseudonymServiceDescriptor));
            }

            writer = CreateEnvelopedSignatureWriter(writer, pseudonymServiceDescriptor);

            throw new NotImplementedException();
        }

        /// <summary>Writes a endpoint reference.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="endpoint">The endpoint reference.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="endpoint" /> is <c>null</c>.</exception>
        protected virtual void WriteEndpointReference(XmlWriter writer, EndpointReference endpoint) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (endpoint is null) {
                throw new ArgumentNullException(nameof(endpoint));
            }

            // <wsa:EndpointReference>
            writer.WriteStartElement(MetadataConstants.Prefixes.WsAddressing10, MetadataConstants.ElementNames.EndpointReference, MetadataConstants.Namespaces.WsAddressing10);

            // <wsa:Address>
            Debug.Assert(endpoint.Uri != null, "Must be not null");
            Debug.Assert(endpoint.Uri.IsAbsoluteUri, "Must be absolute Uri.");
            writer.WriteElementString(MetadataConstants.Prefixes.WsAddressing10, MetadataConstants.ElementNames.Address, MetadataConstants.Namespaces.WsAddressing10, endpoint.Uri.AbsoluteUri);

            // <wsa:ReferenceProperties> | <wsa:ReferenceParameters> | <wsa:PortType> | <wsa:ServiceName> | <wsp:Policy>
            foreach (var detail in endpoint.Details) {
                detail.WriteTo(writer);
            }

            // </wsa:EndpointReference>
            writer.WriteEndElement();
        }

        /// <summary>Writes a display claim.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="claim">The display claim to write.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="claim" /> is <c>null</c>.</exception>
        protected virtual void WriteDisplayClaim(XmlWriter writer, ClaimType claim) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (claim is null) {
                throw new ArgumentNullException(nameof(claim));
            }

            // <auth:ClaimType>
            writer.WriteStartElement(MetadataConstants.Prefixes.Authorization, MetadataConstants.ElementNames.ClaimType, MetadataConstants.Namespaces.Authorization);

            // @Uri - required
            Debug.Assert(claim.Uri != null, "Must be not null");
            Debug.Assert(claim.Uri.IsAbsoluteUri, "Must be absolute Uri.");
            writer.WriteAttributeString(MetadataConstants.AttributeNames.Uri, claim.Uri.AbsoluteUri);

            // @Optional - optional
            if (claim.IsOptional.HasValue) {
                writer.WriteAttributeString(MetadataConstants.AttributeNames.Optional, XmlConvert.ToString(claim.IsOptional.Value));
            }

            // <auth:DisplayName> 0-1
            if (!string.IsNullOrEmpty(claim.DisplayName)) {
                writer.WriteElementString(MetadataConstants.Prefixes.Authorization, MetadataConstants.ElementNames.DisplayName, MetadataConstants.Namespaces.Authorization, claim.DisplayName);
            }

            // <auth:Description> 0-1
            if (!string.IsNullOrEmpty(claim.Description)) {
                writer.WriteElementString(MetadataConstants.Prefixes.Authorization, MetadataConstants.ElementNames.Description, MetadataConstants.Namespaces.Authorization, claim.Description);
            }

            // <auth:DisplayValue> 0-1
            if (!string.IsNullOrEmpty(claim.DisplayValue)) {
                writer.WriteElementString(MetadataConstants.Prefixes.Authorization, MetadataConstants.ElementNames.DisplayValue, MetadataConstants.Namespaces.Authorization, claim.DisplayValue);
            }

            // <auth:Value> | <auth:StructuredValue> | <auth:EncryptedValue> | <auth:ConstrainedValue>
            if (!string.IsNullOrEmpty(claim.Value)) {
                writer.WriteElementString(MetadataConstants.Prefixes.Authorization, MetadataConstants.ElementNames.Value, MetadataConstants.Namespaces.Authorization, claim.Value);
            }

            // </auth:ClaimType>
            writer.WriteEndElement();
        }

        /// <summary>Writes the &lt;saml:Attribute&gt; element.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="saml2Attribute">The Saml2 attribute.</param>
        protected virtual void WriteAttribute(XmlWriter writer, Saml2Attribute saml2Attribute) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (saml2Attribute is null) {
                throw new ArgumentNullException(nameof(saml2Attribute));
            }

            // <Attribute>
            writer.WriteStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");

            // @Name
            writer.WriteAttributeString("Name", saml2Attribute.Name);

            // @NameFormat
            if (saml2Attribute.NameFormat != null) {
                Debug.Assert(saml2Attribute.NameFormat != null, "Must be not null");
                Debug.Assert(saml2Attribute.NameFormat.IsAbsoluteUri, "Must be absolute Uri.");
                writer.WriteAttributeString("NameFormat", saml2Attribute.NameFormat.AbsoluteUri);
            }

            // @FriendlyName
            if (saml2Attribute.FriendlyName != null) {
                writer.WriteAttributeString("FriendlyName", saml2Attribute.FriendlyName);
            }

            // <AttributeValue>
            foreach (string value in saml2Attribute.Values) {
                writer.WriteStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
                if (value == null) {
                    // @xsi:nil=true
                    writer.WriteAttributeString("nil", MetadataConstants.Namespaces.XmlSchema, XmlConvert.ToString(true));
                }
                else if (value.Length > 0) {
                    writer.WriteString(value);
                }

                // </AttributeValue>
                writer.WriteEndElement();
            }

            // </Attribute>
            writer.WriteEndElement();
        }

        private XmlWriter CreateEnvelopedSignatureWriter(XmlWriter writer, DescriptorBase descriptor) {
            if (descriptor.SigningCredentials != null) {
                if (descriptor.Id == null) {
                    descriptor.Id = new Saml2Id();
                }

                return new EnvelopedSignatureWriter(writer, descriptor.SigningCredentials, descriptor.Id.Value) { DSigSerializer = DSigSerializer };
            }

            return writer;
        }
    }
}

#pragma warning restore SA1101 // Prefix local calls with this