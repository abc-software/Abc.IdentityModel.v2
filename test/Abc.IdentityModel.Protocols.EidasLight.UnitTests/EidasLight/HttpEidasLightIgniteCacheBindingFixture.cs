namespace Abc.IdentityModel.EidasLight.UnitTests.Protocols.EidasLight {
    using Abc.IdentityModel.EidasLight.Ignite;
    using Abc.IdentityModel.Protocols.EidasLight;
    using Apache.Ignite.Core;
    using Apache.Ignite.Core.Client;
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;

    [TestFixture]
    [Category("eIDASLight")]
    public class HttpEidasLightIgniteCacheBindingFixture {

        public HttpEidasLightIgniteCacheBindingFixture() {
            var dir = Path.GetDirectoryName(new Uri(this.GetType().Assembly.Location).LocalPath);
            Environment.CurrentDirectory = dir;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        [Fact]
        public void ConnectToIgnite() {
            var ignateClientConfiguration = new IgniteClientConfiguration {
                Endpoints = new System.Collections.ObjectModel.Collection<string>() { "eidas2-2.abc:10800", "eidas2-1.abc:10800" },
                EnablePartitionAwareness =false,// true,
                /*
                SslStreamFactory = new Ssl​Stream​Factory() {
                    CertificatePassword = "123456",
                    CertificatePath = @"..\..\..\_Data\thinclient.pfx",
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls,
                    CheckCertificateRevocation = false,
                    SkipServerCertificateValidation = true,
                },
                */
                SslStreamFactory = new CertStoreSslStreamFactory() { 
                    CheckCertificateRevocation = false, 
                    SkipServerCertificateValidation = true,
                    FindValue = "11c3e2eedc7288b148262333cb29cf4052b1d4ba",
                },
                Logger = new TraceSourceLogger(),
                ReconnectDisabled = true,
            };

            IIgniteClient client = null;
            try {
                client = Apache.Ignite.Core.Ignition.StartClient(ignateClientConfiguration);
                var id = Guid.NewGuid().ToString();

                var cache = client.GetOrCreateCache<string, string>("testcache");
                cache.Put(id, "SomeData");
                var data = cache.Get(id);

                Assert.Equal("SomeData", data);
                Assert.IsTrue(cache.Remove(id));
                client.DestroyCache("testcache");
            }
            catch (Exception e) {
                throw e;
            }
            finally {
                if (client != null) {
                    client.Dispose();
                }
            }
            /**/
            Thread.Sleep(100);
            IIgniteClient client2 = null;
            try {
                client2 = Apache.Ignite.Core.Ignition.StartClient(ignateClientConfiguration);
                var id = Guid.NewGuid().ToString();

                var cache = client2.GetOrCreateCache<string, string>("testcache2");
                cache.Put(id, "SomeData");
                var data = cache.Get(id);

                Assert.Equal("SomeData", data);
                Assert.IsTrue(cache.Remove(id));
                client2.DestroyCache("testcache2");
            }
            catch (Exception e) {
                throw e;
            }
            finally {
                if (client2 != null) {
                    client2.Dispose();
                }
            }
            /**/
        }

        [Fact]
        public void ProcessRequest() {
            var configuration = new EidasProxyIgniteCacheConfiguration() {
                Endpoints = new System.Collections.ObjectModel.Collection<string>() { "eidas2-2.abc:10800", "eidas2-1.abc:10800" },
                IncomingSecret = "84e96b8953e759d43ebeef7db6c6a4bb",
                OutgoingSecret = "2848259ef13a24ab68cfaa0f4c856b9c",
                CertificateFindValue = "11c3e2eedc7288b148262333cb29cf4052b1d4ba",
            };

            var target = new HttpEidasLightIgniteCacheBinding(configuration);

            var request = new EidasLightRequest();
            request.LevelsOfAssurance.Add(new LevelOfAssurance(new Uri("http://eidas.europa.eu/LoA/low")));
            request.RequestedAttributes.Add(new AttributeDefinition("http://eidas.europa.eu/attributes/legalperson/D-2012-17-EUIdentifier"));
            var message = new HttpEidasLightMessage(new Uri("http://baseUri"), request);

            var payloadBinding = new HttpEidasLightPayloadMessageBinding();
            payloadBinding.ProcessOutgoingMessage(message);
            Assert.IsNotNull(message.Data);

            target.ProcessOutgoingMessage(message);
            Assert.IsNotNull(message.Token);
        }

        [Fact]
        public void ProcessResponse() {
            var configuration = new EidasProxyIgniteCacheConfiguration() {
                Endpoints = new System.Collections.ObjectModel.Collection<string>() { "eidas2-2.abc:10800", "eidas2-1.abc:10800" },
                IncomingSecret = "84e96b8953e759d43ebeef7db6c6a4bb",
                //ResponseSecret = "2848259ef13a24ab68cfaa0f4c856b9c",
                OutgoingSecret = "mySecretConnectorRequest",
                CertificateFindValue = "11c3e2eedc7288b148262333cb29cf4052b1d4ba",
            };

            var target = new HttpEidasLightIgniteCacheBinding(configuration);

            using (var client = Ignition.StartClient(new IgniteClientConfiguration {
                Endpoints = configuration.Endpoints,
                EnablePartitionAwareness = false, // NET library throw exception when calculate hash for key of the type string
                SslStreamFactory = new CertStoreSsl​Stream​Factory() {
                    FindValue = configuration.CertificateFindValue,
                    X509FindType = configuration.CertificateX509FindType,
                    StoreName = configuration.CertificateStoreName,
                    StoreLocation = configuration.CertificateStoreLocation,
                    CheckCertificateRevocation = false,
                    SkipServerCertificateValidation = true,
                }
            })) {
                var cache = client.GetOrCreateCache<string, string>(configuration.OutgoingCacheName);
                cache.Put("852a64c0-8ac1-445f-b0e1-992ada493033", File.ReadAllText(@"..\..\..\_Data\EidasLightRequest.xml"));
            }

            var message = new HttpEidasLightMessage(new Uri("http://baseUri"), Http.HttpDeliveryMethods.PostRequest);
            message.Token = "c3BlY2lmaWNDb21tdW5pY2F0aW9uRGVmaW5pdGlvbkNvbm5lY3RvclJlcXVlc3R8ODUyYTY0YzAtOGFjMS00NDVmLWIwZTEtOTkyYWRhNDkzMDMzfDIwMTctMTItMTEgMTQ6MTI6MDUgMTQ4fDdNOHArdVA4Q0tYdU1pMklxU2RhMXRnNDUyV2xSdmNPU3d1MGRjaXNTWUU9";
            Assert.IsNotNull(message.Token);

            target.ProcessIncomingMessage(message);
            Assert.IsNotNull(message.Data);

            var payloadBinding = new HttpEidasLightPayloadMessageBinding();
            payloadBinding.ProcessIncomingMessage(message);
            Assert.IsNotNull(message.Message);

        }
    }
}
