// ----------------------------------------------------------------------------
// <copyright file="AbcSaml11SecurityTokenHandler.cs" company="ABC software">
//    Copyright © ABC SOFTWARE. All rights reserved.
//    The source code or its parts to use, reproduce, transfer, copy or
//    keep in an electronic form only from written agreement ABC SOFTWARE.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Tokens.Saml{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using SamlConstants = Microsoft.IdentityModel.Tokens.Saml.SamlConstants;
    using Microsoft.IdentityModel.Tokens.Saml;
    using Microsoft.IdentityModel.Xml;
    using static Microsoft.IdentityModel.Logging.LogHelper;

    public class AbcSaml11Serilizer : SamlSerializer {
        internal static Exception LogReadException(string format, Exception inner, params object[] args) {
            return LogExceptionMessage(new SamlSecurityTokenReadException(FormatInvariant(format, args), inner));
        }

        /// <inheritdoc/>
        protected override void WriteStatement(XmlWriter writer, SamlStatement statement) {
            var sapSubjectStatement = statement as SamlSapSubjectStatement;
            if (sapSubjectStatement != null) {
                this.WriteSapSubjectStatement(writer, sapSubjectStatement);
            }
            else {
                base.WriteStatement(writer, statement);
            }
        }

        /// <summary>
        /// Writes the &lt;saml:SubjectStatement&gt; element. 
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> with which to write the data.</param>
        /// <param name="statement">The statement to write.</param>
        protected virtual void WriteSapSubjectStatement(XmlWriter writer, SamlSapSubjectStatement statement) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if (statement == null) {
                throw new ArgumentNullException(nameof(statement));
            }

            writer.WriteStartElement(SamlConstants.Prefixes.Assertion, SamlConstants.ElementNames.SubjectStatement, SamlConstants.Namespaces.Assertion);
            writer.WriteAttributeString("xsi", "type", XmlSchema.InstanceNamespace, SamlConstants.Prefixes.AssertionSubject + ":" + SamlConstants.XmlTypes.SubjectStatementType);
            writer.WriteAttributeString("xmlns", SamlConstants.Prefixes.AssertionSubject, null, SamlConstants.Namespaces.AssertionSubject);

            this.WriteSubject(writer, statement.Subject);
            writer.WriteEndElement();
        }

        /// <inheritdoc/>
        protected override SamlStatement ReadStatement(XmlDictionaryReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.IsStartElement(SamlConstants.ElementNames.SubjectStatement, SamlConstants.Namespaces.Assertion)) {
                return this.ReadSapSubjectStatement(reader);
            }

            return base.ReadStatement(reader);
        }

        /// <summary>
        /// Reads the &lt;saml:SubjectStatement&gt; element or a &lt;saml:Statement&gt; element that specifies an xsi:type of samlsap:SubjectStatementType.
        /// </summary>
        /// <param name="reader">An <see cref="XmlDictionaryReader" /> positioned at the element to read.</param>
        /// <returns>
        /// A <see cref="SamlSapSubjectStatement" /> that represents the element that is read.
        /// </returns>
        protected virtual SamlSapSubjectStatement ReadSapSubjectStatement(XmlDictionaryReader reader) {
            if (reader == null) {
                throw new ArgumentNullException(nameof(reader));
            }

            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.ElementNames.SubjectStatement, SamlConstants.Namespaces.Assertion);

            try {
                // @xsi:type
                XmlUtil.ValidateXsiType(reader, SamlConstants.XmlTypes.SubjectStatementType, SamlConstants.Namespaces.AssertionSubject, true);


                reader.ReadStartElement();

                //if (!reader.IsStartElement(SamlConstants.ElementNames.Subject, SamlConstants.Namespaces.Assertion)) {
                //    throw DiagnosticTools.ExceptionUtil.ThrowHelperXml(reader, SR.ID2021Format(SamlConstants.ElementNames.Subject));
                //}

                XmlUtil.CheckReaderOnEntry(reader, SamlConstants.ElementNames.Subject, SamlConstants.Namespaces.Assertion);

                var statement = new SamlSapSubjectStatement() { Subject = this.ReadSubject(reader) };

                reader.MoveToContent();
                reader.ReadEndElement();

                return statement;
            }
            catch (Exception ex) {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException("LogMessages.IDX11112", ex, SamlConstants.ElementNames.SubjectStatement, ex);
            }
        }
    }
}
