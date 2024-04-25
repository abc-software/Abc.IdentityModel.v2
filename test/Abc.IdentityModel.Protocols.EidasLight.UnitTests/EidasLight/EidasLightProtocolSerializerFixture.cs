using Abc.IdentityModel.EidasLight.UnitTests;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace Abc.IdentityModel.Protocols.EidasLight.UnitTests {
    [Category("eIDASLight")]
    public class EidasLightProtocolSerializerFixture {

        public EidasLightProtocolSerializerFixture() {
            var dir = Path.GetDirectoryName(new Uri(typeof(EidasLightProtocolSerializerFixture).Assembly.Location).LocalPath);
            Environment.CurrentDirectory = dir;
        }

        [Fact]
        public void ReadEidasAuthorizationRequest() {
            XmlReaderSettings settings = new XmlReaderSettings();

            XmlTextReader textReader = new XmlTextReader(@"..\..\..\_Data\EidasLightRequest.xml");
            XmlReader reader = XmlReader.Create(textReader, settings);

            var serializer = new EidasLightProtocolSerializer();
            EidasLightMessage message = serializer.ReadMessage(reader);

            message.Should().NotBeNull();
            message.Should().BeOfType<EidasLightRequest>();

            message.Id.Should().Be("id");
            //Assert.Equal(new Uri("urn:oasis:names:tc:SAML:2.0:consent:unspecified"), message.RelayState);
            message.Issuer.Should().Be("pythonSpecificConnectorCA");

            var eidasLightMessage = message as EidasLightRequest;

            Assert.Single(eidasLightMessage.LevelsOfAssurance);
            Assert.Equal(new Uri("http://eidas.europa.eu/LoA/low"), eidasLightMessage.LevelsOfAssurance.First().Value);
            Assert.Equal(new Uri("urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified"), eidasLightMessage.NameIdFormat);
            Assert.Equal("DEMO-SP-CA", eidasLightMessage.ProviderName);
            //eidasLightMessage.RequesterId
            //eidasLightMessage.SpCountryCode
            Assert.Equal(EidasSpType.Public, eidasLightMessage.SpType);
            Assert.Equal(20, eidasLightMessage.RequestedAttributes.Count);
            var list = eidasLightMessage.RequestedAttributes.ToList();

            {
                var c = list[0];
                Assert.Equal("http://eidas.europa.eu/attributes/legalperson/D-2012-17-EUIdentifier", c.Definition);
                Assert.Equal(0, c.Values.Count);
            }

            {
                var c = list[19];
                Assert.Equal("http://eidas.europa.eu/attributes/naturalperson/AdditionalAttribute", c.Definition);
                Assert.Equal(0, c.Values.Count);
            }

        }

        [Fact]
        public void ReadEidasAuthorizationRequestFull() {
            XmlReaderSettings settings = new XmlReaderSettings();

            XmlTextReader textReader = new XmlTextReader(@"..\..\..\_Data\EidasLightRequestFull.xml");
            XmlReader reader = XmlReader.Create(textReader, settings);

            var serializer = new EidasLightProtocolSerializer();
            EidasLightMessage message = serializer.ReadMessage(reader);

            message.Should().NotBeNull();
            message.Should().BeOfType<EidasLightRequest>();

            message.Id.Should().Be("id1");
            message.RelayState.Should().Be("relayState1");
            message.Issuer.Should().Be("issuer1");

            var eidasLightMessage = message as EidasLightRequest;

            Assert.Equal(3, eidasLightMessage.LevelsOfAssurance.Count);
            Assert.Equal(new Uri("http://eidas.europa.eu/LoA/low"), eidasLightMessage.LevelsOfAssurance.First().Value);
            Assert.Equal(LevelOfAssuranceType.Notified, eidasLightMessage.LevelsOfAssurance.First().Type);
            Assert.Equal(new Uri("http://eidas.europa.eu/LoA/high"), eidasLightMessage.LevelsOfAssurance.Last().Value);
            Assert.Equal(LevelOfAssuranceType.NonNotified, eidasLightMessage.LevelsOfAssurance.Last().Type);

            Assert.Equal(new Uri("urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"), eidasLightMessage.NameIdFormat);
            Assert.Equal("providerName1", eidasLightMessage.ProviderName);
            Assert.Equal(EidasSpType.Public, eidasLightMessage.SpType);
            Assert.Equal("spCountryCode1", eidasLightMessage.SpCountryCode);
            Assert.Equal("http://uri1", eidasLightMessage.RequesterId);
            Assert.Equal(3, eidasLightMessage.RequestedAttributes.Count);
            var list = eidasLightMessage.RequestedAttributes.ToList();

            {
                var c = list[0];
                Assert.Equal("definition1", c.Definition);
                Assert.Equal(3, c.Values.Count);
                Assert.Equal("value1", c.Values.First());
                Assert.Equal("value3", c.Values.Last());
            }

            {
                var c = list[2];
                Assert.Equal("definition3", c.Definition);
                Assert.Equal(3, c.Values.Count);
                Assert.Equal("value7", c.Values.First());
                Assert.Equal("value9", c.Values.Last());
            }
        }

        [Fact]
        public void ReadEidasAuthorizationRequestMin() {
            XmlReaderSettings settings = new XmlReaderSettings();

            XmlTextReader textReader = new XmlTextReader(@"..\..\..\_Data\EidasLightRequestMin.xml");
            XmlReader reader = XmlReader.Create(textReader, settings);

            var serializer = new EidasLightProtocolSerializer();
            EidasLightMessage message = serializer.ReadMessage(reader);

            message.Should().NotBeNull();
            message.Should().BeOfType<EidasLightRequest>();

            message.Id.Should().Be("id1");
            message.RelayState.Should().BeNull();
            message.Issuer.Should().Be("issuer1");

            var eidasLightMessage = message as EidasLightRequest;

            Assert.Equal(1, eidasLightMessage.LevelsOfAssurance.Count);
            Assert.Equal(new Uri("http://uri1"), eidasLightMessage.LevelsOfAssurance.First().Value);
            Assert.Equal(LevelOfAssuranceType.Notified, eidasLightMessage.LevelsOfAssurance.First().Type);

            Assert.Equal(null, eidasLightMessage.NameIdFormat);
            Assert.Equal(null, eidasLightMessage.ProviderName);
            Assert.Equal(null, eidasLightMessage.SpType);
            Assert.Equal(null, eidasLightMessage.SpCountryCode);
            Assert.Equal(null, eidasLightMessage.RequesterId);
            Assert.Equal(1, eidasLightMessage.RequestedAttributes.Count);
            var list = eidasLightMessage.RequestedAttributes.ToList();

            {
                var c = list[0];
                Assert.Equal("definition1", c.Definition);
                Assert.Equal(0, c.Values.Count);
            }
        }

        [Fact]
        public void ReadEidasAuthorizationRequestEmpty() {
            XmlReaderSettings settings = new XmlReaderSettings();

            XmlTextReader textReader = new XmlTextReader(@"..\..\..\_Data\EidasLightRequestEmpty.xml");
            XmlReader reader = XmlReader.Create(textReader, settings);

            var serializer = new EidasLightProtocolSerializer();
            Assert.Throws<EidasSerializationException>(() => serializer.ReadMessage(reader));
        }

        [Fact]
        public void ReadEidasAuthorizationResponseFull() {
            XmlReaderSettings settings = new XmlReaderSettings();

            XmlTextReader textReader = new XmlTextReader(@"..\..\..\_Data\EidasLightResponseFull.xml");
            XmlReader reader = XmlReader.Create(textReader, settings);

            var serializer = new EidasLightProtocolSerializer();
            EidasLightMessage message = serializer.ReadMessage(reader);

            message.Should().NotBeNull();
            message.Should().BeOfType<EidasLightResponse>();

            Assert.Equal("id1", message.Id);
            Assert.Equal("relayState1", message.RelayState);
            Assert.Equal("issuer1", message.Issuer);

            var eidasLightMessage = message as EidasLightResponse;
            Assert.Equal("inResponseToId1", eidasLightMessage.InResponseToId);
            Assert.Equal(new Uri("urn:oasis:names:tc:SAML:2.0:consent:unspecified"), eidasLightMessage.Consent);
            Assert.Equal("ipAddress1", eidasLightMessage.IpAddress);
            Assert.Equal("subject1", eidasLightMessage.Subject);
            Assert.Equal(new Uri("urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"), eidasLightMessage.SubjectNameIdFormat);
            Assert.Equal(new Uri("http://eidas.europa.eu/LoA/low"), eidasLightMessage.LevelOfAssurance.Value);

            eidasLightMessage.Status.Should().NotBeNull();
            Assert.Equal(true, eidasLightMessage.Status.Failure);
            Assert.Equal(Tuple.Create(new Uri("urn:oasis:names:tc:SAML:2.0:status:Success"), new Uri("urn:oasis:names:tc:SAML:2.0:status:AuthnFailed")), eidasLightMessage.Status.StatusCode);
            Assert.Equal("statusMessage1", eidasLightMessage.Status.StatusMessage);

            Assert.Equal(3, eidasLightMessage.Attributes.Count);
            var list = eidasLightMessage.Attributes.ToList();

            {
                var c = list[0];
                Assert.Equal("definition1", c.Definition);
                Assert.Equal(3, c.Values.Count);
                Assert.Equal("value1", c.Values.First());
                Assert.Equal("value3", c.Values.Last());
            }

            {
                var c = list[2];
                Assert.Equal("definition3", c.Definition);
                Assert.Equal(3, c.Values.Count);
                Assert.Equal("value7", c.Values.First());
                Assert.Equal("value9", c.Values.Last());
            }
        }

        [Fact]
        public void ReadEidasAuthorizationResponseMin() {
            XmlReaderSettings settings = new XmlReaderSettings();

            XmlTextReader textReader = new XmlTextReader(@"..\..\..\_Data\EidasLightResponseMin.xml");
            XmlReader reader = XmlReader.Create(textReader, settings);

            var serializer = new EidasLightProtocolSerializer();
            EidasLightMessage message = serializer.ReadMessage(reader);

            message.Should().NotBeNull();
            message.Should().BeOfType<EidasLightResponse>();

            Assert.Equal("id1", message.Id);
            Assert.Equal(null, message.RelayState);
            Assert.Equal("issuer1", message.Issuer);

            var eidasLightMessage = message as EidasLightResponse;
            Assert.Equal("inResponseToId1", eidasLightMessage.InResponseToId);
            Assert.Equal(null, eidasLightMessage.Consent);
            Assert.Equal(null, eidasLightMessage.IpAddress);
            Assert.Equal(null, eidasLightMessage.Subject);
            Assert.Equal(null, eidasLightMessage.SubjectNameIdFormat);
            Assert.Equal(null, eidasLightMessage.LevelOfAssurance);

            Assert.NotNull(eidasLightMessage.Status);
            Assert.Equal(null, eidasLightMessage.Status.Failure);
            Assert.Equal(null, eidasLightMessage.Status.StatusCode);
            Assert.Equal(null, eidasLightMessage.Status.StatusMessage);

            Assert.Equal(0, eidasLightMessage.Attributes.Count);
        }

        [Fact]
        public void WriteStorkAuthRequest() {
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder)) {
                var request = new EidasLightRequest {
                    CitizenCountryCode = "LV",
                    Id = "id",
                    Issuer = "issuer",
                    NameIdFormat = new Uri("urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified"),
                    SpType = EidasSpType.Public,
                    ProviderName = "VPM",
                    RelayState = "context",
                    RequesterId = "req:request",
                    SpCountryCode = "CA",
                };

                request.LevelsOfAssurance.Add(new LevelOfAssurance(new Uri("http://eidas.europa.eu/LoA/low")));

                request.RequestedAttributes.Add(new AttributeDefinition("http://www.stork.gov.eu/1.0/dateOfBirth"));
                request.RequestedAttributes.Add(new AttributeDefinition("http://www.stork.gov.eu/1.0/eIdentitfer"));

                var serializer = new EidasLightProtocolSerializer();
                serializer.WriteMessage(writer, request);
            }

            string xml = builder.ToString();
            ValidateMessage(xml);
        }

        [Fact]
        public void WriteStorkAuthReqsponse() {
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder)) {
                var request = new EidasLightResponse {
                    Id = "id1",
                    Issuer = "issuer",
                    RelayState = "context",
                    LevelOfAssurance = new LevelOfAssurance(new Uri("http://eidas.europa.eu/LoA/low"), LevelOfAssuranceType.Notified),
                    InResponseToId = "id2",
                };

                request.Attributes.Add(new AttributeDefinition("http://www.stork.gov.eu/1.0/dateOfBirth", "2020-01-01"));
                request.Attributes.Add(new AttributeDefinition("http://www.stork.gov.eu/1.0/eIdentitfer", "LV/EU/1324567856"));

                var serializer = new EidasLightProtocolSerializer();
                serializer.WriteMessage(writer, request);
            }

            string xml = builder.ToString();
            ValidateMessage(xml);
        }


        /**/
        private XmlReaderSettings readersettings;
        private void ValidateMessage(string xml) {
            if (readersettings == null) {
                readersettings = new XmlReaderSettings();
                readersettings.IgnoreWhitespace = true;
                readersettings.IgnoreComments = true;
                readersettings.ValidationType = ValidationType.Schema;
                readersettings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings
                                                 | System.Xml.Schema.XmlSchemaValidationFlags.AllowXmlAttributes
                                                 | System.Xml.Schema.XmlSchemaValidationFlags.ProcessIdentityConstraints;
                readersettings.ValidationEventHandler +=
                    delegate (object sender, System.Xml.Schema.ValidationEventArgs vargs) {
                        Assert.Fail($"Schema problem: Line {vargs.Exception.LineNumber}: {vargs.Severity}: {vargs.Message}");
                    };

                readersettings.DtdProcessing = DtdProcessing.Ignore;

                XmlReaderSettings dtdSettings = new XmlReaderSettings();
                dtdSettings.DtdProcessing = DtdProcessing.Ignore;
                dtdSettings.XmlResolver = new TestXmlResolver();

                readersettings.Schemas.XmlResolver = new TestXmlResolver();
                readersettings.Schemas.Add(null, @"..\..\..\_Data\LightRequest.xsd");
                readersettings.Schemas.Add(null, @"..\..\..\_Data\LightResponse.xsd");
                //readersettings.Schemas.Add("http://www.w3.org/2000/09/xmldsig#", XmlReader.Create(@"..\..\..\_Data\xmldsig-core-schema.xsd", dtdSettings));
                //readersettings.Schemas.Add("http://www.w3.org/2001/04/xmlenc#", XmlReader.Create(@"..\..\..\_Data\xenc-schema.xsd", dtdSettings));
                //readersettings.Schemas.Add(null, @"..\..\..\_Data\saml-schema-protocol-2.0.xsd");
                //readersettings.Schemas.Add(null, @"..\..\..\_Data\saml_eidas_extension.xsd");
                //readersettings.Schemas.Add(null, @"..\..\..\_Data\saml_eidas_legal_person.xsd");
                //readersettings.Schemas.Add(null, @"..\..\..\_Data\saml_eidas_natural_person.xsd");
                //readersettings.Schemas.Add(null, @"..\..\..\_Data\saml_eidas_representative_legal_person.xsd");
                //readersettings.Schemas.Add(null, @"..\..\..\_Data\saml_eidas_representative_natural_person.xsd");
            }

            using (XmlReader vr = XmlReader.Create(new System.IO.StringReader(xml), readersettings)) {
                while (vr.Read()) ;
            }
        }

    }
}