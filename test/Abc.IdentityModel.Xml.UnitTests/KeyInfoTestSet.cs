using Microsoft.IdentityModel.Xml;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Abc.IdentityModel.Xml.UnitTests {

    public class KeyInfoTestSet : XmlTestSet {
        public KeyInfo KeyInfo { get; set; }

        public static KeyInfoTestSet KeyInfoFullyPopulated {
            get {
                var data = new X509Data(new X509Certificate2(Convert.FromBase64String("MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD"))) {
                    IssuerSerial = new IssuerSerial("CN=TAMURA Kent, OU=TRL, O=IBM, L=Yamato-shi, ST=Kanagawa, C=JP", "12345678"),
                    SKI = "31d97bd7",
                    SubjectName = "X509SubjectName"
                };
                var keyInfo = new KeyInfo {
                    RetrievalMethodUri = "http://RetrievalMethod",
                    RSAKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                    KeyName = "KeyName"
                };
                keyInfo.X509Data.Add(data);
                return new KeyInfoTestSet {
                    KeyInfo = keyInfo,
                    TestId = nameof(KeyInfoFullyPopulated),
                    Xml = @"<KeyInfo xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                                <KeyName>KeyName</KeyName>
                                <KeyValue>
                                    <RSAKeyValue>
                                        <Modulus>rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==</Modulus>
                                        <Exponent>AQAB</Exponent>
                                    </RSAKeyValue>
                                </KeyValue>
                                <RetrievalMethod URI=""http://RetrievalMethod""/>
                                <X509Data>
                                    <X509IssuerSerial>
                                        <X509IssuerName>CN=TAMURA Kent, OU=TRL, O=IBM, L=Yamato-shi, ST=Kanagawa, C=JP</X509IssuerName>
                                        <X509SerialNumber>12345678</X509SerialNumber>
                                    </X509IssuerSerial>
                                    <X509SKI>31d97bd7</X509SKI>
                                    <X509SubjectName>X509SubjectName</X509SubjectName>
                                    <X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate>
                                </X509Data>
                            </KeyInfo>"
                };
            }
        }

        public static KeyInfoTestSet KeyInfoEncryptedKey {
            get {
                var data = new X509Data(new X509Certificate2(Convert.FromBase64String("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")));
                var keyInfo = new KeyInfo();
                keyInfo.X509Data.Add(data);

                var encryptedKey = new EncryptedKey() {
                    EncryptionMethod = new EncryptionMethod(new Uri("http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p")),
                    KeyInfo = keyInfo,
                    CipherData = new CipherData(Convert.FromBase64String("isxvpl7w1SCZS7BwGEJn3bkGBtRMCxiObPbcIMJV3CS9L67PNWowGEz8qRjg2zfXpuYsn440PkawjOLTuJCZHcgyjnNB1MwLCrumgIyDhVNM7jQFufdIv+uzmrdB36hLF1klA+fbKX/foa1nZx9wLDCbEuDSbXMTKSkhCphKXSlYpHSNth50h47v84YnGSyyorclF0wurcPPBuBPeD/DmQ/I0s4NrddCz9tJ0I1ty3txlfDW6co3ig4xEF6M+e38W+39SDcqoSHtXAU42vmCRw+K6fXncGoCXtPc6p6vi8AkgMYBRQPXQaH8IFgADlRMuY2c1hkap5wihgHSYq40K5ffIujvmNF9f4qbHlwAvZsbK/Xx1x6Ca+BqHXHdurOkSaISoOHy3Z2FIPmtSNvnscvQe8Ackwr9jYQKg2SHtrOi4biN6h414d5FirunkxuaOuLqJEnYgZRIW6v4RHxGvHoIoS7FNBwPo4mZiIpiHx10Ak54nEVnMQ/HySYo8rmt"))
                };

                return new KeyInfoTestSet {
                    KeyInfo = new EncryptedKeyKeyInfo(encryptedKey),
                    TestId = nameof(KeyInfoEncryptedKey),
                    Xml = @"<KeyInfo xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                                <xenc:EncryptedKey xmlns:xenc=""http://www.w3.org/2001/04/xmlenc#"">
                                    <xenc:EncryptionMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p"">
                                        <ds:DigestMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#sha1"" xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""/>
                                    </xenc:EncryptionMethod>
                                    <KeyInfo>
                                        <X509Data>
                                            <X509Certificate>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</X509Certificate>
                                        </X509Data>
                                    </KeyInfo>
                                    <xenc:CipherData>
                                        <xenc:CipherValue>isxvpl7w1SCZS7BwGEJn3bkGBtRMCxiObPbcIMJV3CS9L67PNWowGEz8qRjg2zfXpuYsn440PkawjOLTuJCZHcgyjnNB1MwLCrumgIyDhVNM7jQFufdIv+uzmrdB36hLF1klA+fbKX/foa1nZx9wLDCbEuDSbXMTKSkhCphKXSlYpHSNth50h47v84YnGSyyorclF0wurcPPBuBPeD/DmQ/I0s4NrddCz9tJ0I1ty3txlfDW6co3ig4xEF6M+e38W+39SDcqoSHtXAU42vmCRw+K6fXncGoCXtPc6p6vi8AkgMYBRQPXQaH8IFgADlRMuY2c1hkap5wihgHSYq40K5ffIujvmNF9f4qbHlwAvZsbK/Xx1x6Ca+BqHXHdurOkSaISoOHy3Z2FIPmtSNvnscvQe8Ackwr9jYQKg2SHtrOi4biN6h414d5FirunkxuaOuLqJEnYgZRIW6v4RHxGvHoIoS7FNBwPo4mZiIpiHx10Ak54nEVnMQ/HySYo8rmt</xenc:CipherValue>
                                    </xenc:CipherData>
                                </xenc:EncryptedKey>
                            </KeyInfo>"
                };
            }
        }
    }
}
