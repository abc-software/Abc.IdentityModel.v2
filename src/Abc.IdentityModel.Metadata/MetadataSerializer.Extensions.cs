// ----------------------------------------------------------------------------
// <copyright file="MetadataSerializer.Extensions.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Provides support for metadata serialization.
    /// </summary>
    public partial class MetadataSerializer {
        /// <summary>Extensible point to write any attributes.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="source">The source element of type <typeparamref name="T" />.</param>
        /// <typeparam name="T">The type that represents the element whose attribute is being written.</typeparam>
        protected virtual void WriteAnyAttributes<T>(XmlWriter writer, T source) {
        }

        /// <summary>Extensible point to write any elements.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="source">The source element of type <typeparamref name="T" />.</param>
        /// <typeparam name="T">The type that represents the element that is being written.</typeparam>
        protected virtual void WriteAnyElements<T>(XmlWriter writer, T source) {
        }

        /// <summary>Write extensions elements.</summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="source">The source element of type <typeparamref name="T" />.</param>
        /// <typeparam name="T">The type that represents the element that is being written.</typeparam>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        protected virtual void WriteExtensionsElement<T>(XmlWriter writer, T source) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (source is EntityDescriptor entityDescriptor &&
                (entityDescriptor.DigestMethods.Count > 0 || entityDescriptor.EntityAttributes.Count > 0 || entityDescriptor.DigestMethods.Count > 0)) {
                // <Extensions>
                writer.WriteStartElement(this.Prefix, MetadataConstants.ElementNames.Extensions, MetadataConstants.Namespaces.Metadata);

                this.WriteEntityDescriptorAttributes(writer, entityDescriptor);
                this.WriteEntityDescriptorDigestMethods(writer, entityDescriptor);
                this.WriteEntityDescriptorSigningMethods(writer, entityDescriptor);

                this.WriteAnyElements(writer, source);

                // </Extensions>
                writer.WriteEndElement();
            }

            if (source is IdpSsoDescriptor identityProviderSingleSignOnDescriptor
                && identityProviderSingleSignOnDescriptor.SourceId != null) {
                // <Extensions>
                writer.WriteStartElement(MetadataConstants.ElementNames.Extensions, MetadataConstants.Namespaces.Metadata);

                writer.WriteStartElement(MetadataConstants.Prefixes.Saml1, MetadataConstants.ElementNames.SourceID, MetadataConstants.Namespaces.Saml1);
                writer.WriteRaw(ByteArrayToHexString(identityProviderSingleSignOnDescriptor.SourceId));
                writer.WriteEndElement();

                this.WriteAnyElements(writer, source);

                // </Extensions>
                writer.WriteEndElement();
            }

            string ByteArrayToHexString(byte[] bytes) {
                var result = new StringBuilder(bytes.Length * 2);
                const string HexAlphabet = "0123456789abcdef";

                foreach (byte b in bytes) {
                    result.Append(HexAlphabet[(int)(b >> 4)]);
                    result.Append(HexAlphabet[(int)(b & 0xF)]);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Writes the signing methods.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="descriptor">The descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="descriptor" /> is <c>null</c>.</exception>
        protected void WriteEntityDescriptorSigningMethods(XmlWriter writer, EntityDescriptor descriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (descriptor is null) {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if (descriptor.SigningMethods.Count > 0) {
                foreach (var method in descriptor.SigningMethods) {
                    // <alg:SigningMethod>
                    writer.WriteStartElement(MetadataConstants.Prefixes.Alg, MetadataConstants.ElementNames.SigningMethod, MetadataConstants.Namespaces.Alg);

                    // @Algorithm - required
                    Debug.Assert(method.Algorithm != null, "Must be not null");
                    Debug.Assert(method.Algorithm.IsAbsoluteUri, "Must be absolute Uri.");
                    writer.WriteAttributeString(MetadataConstants.AttributeNames.Algorithm, method.Algorithm.AbsoluteUri);

                    // @MinKeySize - optional
                    if (method.MinKeySize.HasValue) {
                        writer.WriteAttributeString(MetadataConstants.AttributeNames.MinKeySize, XmlConvert.ToString(method.MinKeySize.Value));
                    }

                    // @MaxKeySize - optional
                    if (method.MaxKeySize.HasValue) {
                        writer.WriteAttributeString(MetadataConstants.AttributeNames.MaxKeySize, XmlConvert.ToString(method.MaxKeySize.Value));
                    }

                    // <any>
                    this.WriteAnyElements(writer, method);

                    // </alg:SigningMethod>
                    writer.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Writes the digest methods.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="descriptor">The descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="descriptor" /> is <c>null</c>.</exception>
        protected void WriteEntityDescriptorDigestMethods(XmlWriter writer, EntityDescriptor descriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (descriptor is null) {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if (descriptor.DigestMethods.Count > 0) {
                foreach (var method in descriptor.DigestMethods) {
                    // <alg:DigestMethod>
                    writer.WriteStartElement(MetadataConstants.Prefixes.Alg, MetadataConstants.ElementNames.DigestMethod, MetadataConstants.Namespaces.Alg);

                    // @Algorithm - required
                    Debug.Assert(method.Algorithm != null, "Must be not null.");
                    Debug.Assert(method.Algorithm.IsAbsoluteUri, "Must be absolute Uri.");
                    writer.WriteAttributeString(MetadataConstants.AttributeNames.Algorithm, method.Algorithm.AbsoluteUri);

                    // <any>
                    this.WriteAnyElements(writer, method);

                    // </alg:DigestMethod>
                    writer.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Writes the digest methods.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="descriptor">The entity descriptor.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="descriptor" /> is <c>null</c>.</exception>
        protected void WriteEntityDescriptorAttributes(XmlWriter writer, EntityDescriptor descriptor) {
            if (writer is null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (descriptor is null) {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if (descriptor.EntityAttributes.Count > 0) {
                // <mdattr:EntityAttributes>
                writer.WriteStartElement(MetadataConstants.Prefixes.Attributes, MetadataConstants.ElementNames.EntityAttributes, MetadataConstants.Namespaces.Attributes);

                foreach (var attribute in descriptor.EntityAttributes) {
                    this.WriteAttribute(writer, attribute);
                }

                // </mdattr:EntityAttributes>
                writer.WriteEndElement();
            }
        }
    }
}