using Microsoft.IdentityModel.Tokens.Saml2;
using System.Xml;

namespace Abc.IdentityModel.Protocols.Saml2 {
    public class Saml2TokenToSerializerAdaptor : Saml2Serializer, ISaml2TokenToSerializerAdaptor {

        #region Read
        public Saml2Assertion ReadAssertionFromReader(XmlReader reader) {
            return this.ReadAssertion(reader);
        }

        public Saml2Action ReadActionFromReader(XmlReader reader) {
            return this.ReadAction(XmlDictionaryReader.CreateDictionaryReader(reader));
        }

        public Saml2Attribute ReadAttributeFromReader(XmlReader reader) {
            return this.ReadAttribute(XmlDictionaryReader.CreateDictionaryReader(reader));
        }

        public string ReadAttributeValueFromReader(XmlReader reader, Saml2Attribute attribute) {
            return this.ReadAttributeValue(XmlDictionaryReader.CreateDictionaryReader(reader), attribute);
        }

        public Saml2Conditions ReadConditionsFromReader(XmlReader reader) {
            return this.ReadConditions(XmlDictionaryReader.CreateDictionaryReader(reader));
        }

        public Saml2NameIdentifier ReadEncryptedIdFromReader(XmlReader reader) {
            return this.ReadEncryptedId(XmlDictionaryReader.CreateDictionaryReader(reader));
        }

        public Saml2Evidence ReadEvidenceFromReader(XmlReader reader) {
            return this.ReadEvidence(XmlDictionaryReader.CreateDictionaryReader(reader));
        }

        public Saml2NameIdentifier ReadIssuerFromReader(XmlReader reader) {
            return this.ReadIssuer(XmlDictionaryReader.CreateDictionaryReader(reader));
        }

        public Saml2NameIdentifier ReadNameIdFromReader(XmlReader reader, string parentElement) {
            return this.ReadNameIdentifier(XmlDictionaryReader.CreateDictionaryReader(reader), parentElement);
        }

        public Saml2Subject ReadSubjectFromReader(XmlReader reader) {
            return this.ReadSubject(XmlDictionaryReader.CreateDictionaryReader(reader));
        }
        #endregion

        #region Write
        public void WriteAssertionToWriter(XmlWriter writer, Saml2Assertion assertion) {
            this.WriteAssertion(writer, assertion);
        }

        public void WriteActionToWriter(XmlWriter writer, Saml2Action action) {
            this.WriteAction(writer, action);
        }

        public void WriteAttributeToWriter(XmlWriter writer, Saml2Attribute attribute) {
            this.WriteAttribute(writer, attribute);
        }

        public void WriteConditionsToWriter(XmlWriter writer, Saml2Conditions conditions) {
            this.WriteConditions(writer, conditions);
        }

        public void WriteEvidenceToWriter(XmlWriter writer, Saml2Evidence evidence) {
            this.WriteEvidence(writer, evidence);
        }

        public void WriteIssuerToWriter(XmlWriter writer, Saml2NameIdentifier issuer) {
            this.WriteIssuer(writer, issuer);
        }

        public void WriteNameIdToWriter(XmlWriter writer, Saml2NameIdentifier nameId) {
            this.WriteNameId(writer, nameId);
        }

        public void WriteSubjectToWriter(XmlWriter writer, Saml2Subject subject) {
            this.WriteSubject(writer, subject);
        }
        #endregion
    }
}