namespace Abc.IdentityModel.Protocols.EidasLight.UnitTests {
    using Abc.IdentityModel.Protocols.EidasLight;
    using System;

    public class BinaryLightTokenFixture {
        [Fact]
        public void Sign() {
            var token = new BinaryLightToken("specificCommunicationDefinitionConnectorRequest", "852a64c0-8ac1-445f-b0e1-992ada493033", new DateTime(2017, 12, 11, 14, 12, 05, 148));
            var value = token.Sign("mySecretConnectorRequest");

            value.Should().Be("c3BlY2lmaWNDb21tdW5pY2F0aW9uRGVmaW5pdGlvbkNvbm5lY3RvclJlcXVlc3R8ODUyYTY0YzAtOGFjMS00NDVmLWIwZTEtOTkyYWRhNDkzMDMzfDIwMTctMTItMTEgMTQ6MTI6MDUgMTQ4fDdNOHArdVA4Q0tYdU1pMklxU2RhMXRnNDUyV2xSdmNPU3d1MGRjaXNTWUU9");
            token.Digest.Should().Be("7M8p+uP8CKXuMi2IqSda1tg452WlRvcOSwu0dcisSYE=");
        }

        [Fact]
        public void Parse() {
            // Bad b64
            Assert.Throws<FormatException>(() => BinaryLightToken.Parse("c3BlY2lmaWNDb21tdW5pY2F0aW9uRGVmaW5pdGlvbkNvbm5lY3RvclJlcXVlc3R8ODUyYTY0YzAtOGFjMS00NDVmLWIwZTEtOTkyYWRhNDkzMDMzfDIwMTctMTItMTEgMTQ6MTI6MDUgMTQ4fDdNOHArdVA4Q0tYdU1pMklxU2RhMXRnNDUyV2xSdmNPU3d1MGRjaXNTWUU"));

            // Bad format
            Assert.Throws<FormatException>(() => BinaryLightToken.Parse("c3BlY2lmaWNDb21tdW5pY2F0aW9uRGVmaW5pdGlvbkNvbm5lY3RvclJlcXVlc3R8ODUyYTY0YzAtOGFjMS00NDVmLWIwZTEtOTkyYWRhNDkzMDMzfDIwMTctMTItMTEgMTQ6MTI6MDUgMTQ4"));
        }


        [Fact]
        public void Validate() {
            {
                var token = BinaryLightToken.Parse("c3BlY2lmaWNDb21tdW5pY2F0aW9uRGVmaW5pdGlvbkNvbm5lY3RvclJlcXVlc3R8ODUyYTY0YzAtOGFjMS00NDVmLWIwZTEtOTkyYWRhNDkzMDMzfDIwMTctMTItMTEgMTQ6MTI6MDUgMTQ4fDdNOHArdVA4Q0tYdU1pMklxU2RhMXRnNDUyV2xSdmNPU3d1MGRjaXNTWUU9");
                var result = token.Validate("mySecretConnectorRequest");
                result.Should().BeTrue();
            }
            {
                var token = BinaryLightToken.Parse("c3BlY2lmaWNDb21tdW5pY2F0aW9uRGVmaW5pdGlvbkNvbm5lY3RvclJlcXVlc3R8ODUyYTY0YzAtOGFjMS00NDVmLWIwZTEtOTkyYWRhNDkzMDMzfDIwMTctMTItMTEgMTQ6MTI6MDUgMTQ4fDdNOHArdVA4Q0tYdU1pMklxU2RhMXRnNDUyV2xSdmNPU3d1MGRjaXNTWUU9");
                var result = token.Validate("badSecet");
                result.Should().BeFalse();
            }
        }

    }
}
