using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Abc.IdentityModel.Security {
    internal static class CryptoUtil {
        private const string SHAString = "SHA";
        private const string SHA1String = "SHA1";
        private const string SHA224String = "SHA244";
        private const string SHA256String = "SHA256";
        private const string SHA384String = "SHA384";
        private const string SHA512String = "SHA512";

        public static string MapAlgorithmToOidName(string algorithm) {
            if (algorithm == null) {
                return null;
            }

            switch (algorithm) {
                case SecurityAlgorithms.RsaSha1Signature:
                case SecurityAlgorithms.DsaSha1Signature:
                case SecurityAlgorithms.Sha1Digest:
                    return SHA1String;
                case SecurityAlgorithms.RsaSha244Signature:
                case SecurityAlgorithms.Sha244Digest:
                    return SHA224String;
                case SecurityAlgorithms.DsaSha256Signature:
                case SecurityAlgorithms.RsaSha256Signature:
#if NET471 || NET472
                case SecurityAlgorithms.RsaMgf1Sha256Signature:
#endif
                case SecurityAlgorithms.Sha256Digest:
                    return SHA256String;
                case SecurityAlgorithms.RsaSha384Signature:
                case SecurityAlgorithms.Sha384Digest:
                    return SHA384String;
                case SecurityAlgorithms.RsaSha512Signature:
                case SecurityAlgorithms.Sha512Digest:
                    return SHA512String;
                default:
                    throw new CryptographicException("UnsupportedAlgorithmForCryptoOperation");
            }
        }

        public static HashAlgorithm GetHashAlgorithm(string algorithm) {
            if (string.IsNullOrEmpty(algorithm)) {
                throw new ArgumentNullException(nameof(algorithm));
            }

            object algorithmFromConfig = CryptoConfig.CreateFromName(algorithm);
            if (algorithmFromConfig == null) {
                var fipsCompilance = CryptoConfig.AllowOnlyFipsAlgorithms;
                HashAlgorithm hashAlgorithm;
                switch (algorithm) {
                    case SHAString:
                    case SHA1String:
                    case SecurityAlgorithms.RsaSha1Signature:
                    case SecurityAlgorithms.DsaSha1Signature:
                    case SecurityAlgorithms.Sha1Digest:
                        hashAlgorithm = new SHA1CryptoServiceProvider();
                        break;
                    case SHA256String:
                    case SecurityAlgorithms.RsaSha256Signature:
                    case SecurityAlgorithms.DsaSha256Signature:
                    case SecurityAlgorithms.Sha256Digest:
                        hashAlgorithm = fipsCompilance ? new SHA256CryptoServiceProvider() : (HashAlgorithm)new SHA256Managed();
                        break;
                    case SHA384String:
                    case SecurityAlgorithms.RsaSha384Signature:
                    case SecurityAlgorithms.Sha384Digest:
                        hashAlgorithm = fipsCompilance ? new SHA384CryptoServiceProvider() : (HashAlgorithm)new SHA384Managed();
                        break;
                    case SHA512String:
                    case SecurityAlgorithms.RsaSha512Signature:
                    case SecurityAlgorithms.Sha512Digest:
                        hashAlgorithm = fipsCompilance ? new SHA512CryptoServiceProvider() : (HashAlgorithm)new SHA512Managed();
                        break;
                    default:
                        throw new CryptographicException("UnsupportedAlgorithmForCryptoOperation");
                }

                return hashAlgorithm;
            }
            else {
                var description = algorithmFromConfig as SignatureDescription;
                if (description != null) {
                    return description.CreateDigest();
                }

                var hashAlgorithm = algorithmFromConfig as HashAlgorithm;
                if (hashAlgorithm != null) {
                    return hashAlgorithm;
                }
            }

            throw new CryptographicException("UnsupportedAlgorithmForCryptoOperation");
        }

        public static AsymmetricSignatureFormatter GetSignatureFormatter(AsymmetricAlgorithm key, string algorithm) {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrEmpty(algorithm)) {
                throw new ArgumentNullException(nameof(algorithm));
            }

            object algorithmFromConfig = CryptoConfig.CreateFromName(algorithm);
            if (algorithmFromConfig == null) {
                switch (algorithm) {
                    case SecurityAlgorithms.RsaSha1Signature:
                    case SecurityAlgorithms.RsaSha256Signature:
                    case SecurityAlgorithms.RsaSha384Signature:
                    case SecurityAlgorithms.RsaSha512Signature:
                        var formatterRsa = new RSAPKCS1SignatureFormatter(key);
                        formatterRsa.SetHashAlgorithm(MapAlgorithmToOidName(algorithm));
                        return formatterRsa;
                    case SecurityAlgorithms.DsaSha1Signature:
                    case SecurityAlgorithms.DsaSha256Signature:
                        var formatterDsa = new DSASignatureFormatter(key);
                        formatterDsa.SetHashAlgorithm(MapAlgorithmToOidName(algorithm));
                        return formatterDsa;
                    default:
                        throw new CryptographicException("UnsupportedAlgorithmForCryptoOperation");
                }
            }
            else {
                var description = algorithmFromConfig as SignatureDescription;
                if (description != null) {
                    return description.CreateFormatter(key);
                }

                var formatter = algorithmFromConfig as AsymmetricSignatureFormatter;
                if (formatter != null) {
                    try {
                        formatter.SetKey(key);
                    }
                    catch (InvalidCastException exception) {
                        throw new NotSupportedException("AlgorithmAndKeyMisMatch", exception);
                    }

                    return formatter;
                }
            }

            throw new CryptographicException("UnsupportedAlgorithmForCryptoOperation");
        }

        public static AsymmetricSignatureDeformatter GetSignatureDeformatter(AsymmetricAlgorithm key, string algorithm) {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrEmpty(algorithm)) {
                throw new ArgumentNullException(nameof(algorithm));
            }

            object algorithmFromConfig = CryptoConfig.CreateFromName(algorithm);
            if (algorithmFromConfig == null) {
                switch (algorithm) {
                    case SecurityAlgorithms.RsaSha1Signature:
                    case SecurityAlgorithms.RsaSha256Signature:
                    case SecurityAlgorithms.RsaSha384Signature:
                    case SecurityAlgorithms.RsaSha512Signature:
                        var deformatterRsa = new RSAPKCS1SignatureDeformatter(key);
                        deformatterRsa.SetHashAlgorithm(MapAlgorithmToOidName(algorithm));
                        return deformatterRsa;
                    case SecurityAlgorithms.DsaSha1Signature:
                    case SecurityAlgorithms.DsaSha256Signature:
                        var deformatterDsa = new DSASignatureDeformatter(key);
                        deformatterDsa.SetHashAlgorithm(MapAlgorithmToOidName(algorithm));
                        return deformatterDsa;
                    default:
                        throw new CryptographicException("UnsupportedAlgorithmForCryptoOperation");
                }
            }
            else {
                var description = algorithmFromConfig as SignatureDescription;
                if (description != null) {
                    return description.CreateDeformatter(key);
                }

                var deformatter = algorithmFromConfig as AsymmetricSignatureDeformatter;
                if (deformatter != null) {
                    try {
                        deformatter.SetKey(key);
                    }
                    catch (InvalidCastException exception) {
                        throw new NotSupportedException("AlgorithmAndKeyMisMatch", exception);
                    }

                    return deformatter;
                }
            }

            throw new CryptographicException("UnsupportedAlgorithmForCryptoOperation");
        }
    }
}
