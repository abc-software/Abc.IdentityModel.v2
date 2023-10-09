// ----------------------------------------------------------------------------
// <copyright file="SecurityAlgorithms.cs" company="ABC Software Ltd">
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
      
namespace Abc.IdentityModel.Security {
    /// <summary>
    /// http://www.w3.org/TR/xmlsec-algorithms/
    /// </summary>
    internal sealed class SecurityAlgorithms {
        #region DSA
        
        /// <summary>Specifies a URI that points to the DSA cryptographic algorithm for digitally signing XML.</summary>
        public const string DsaSha1Signature = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";

        /// <summary>Specifies a URI that points to the DSA cryptographic algorithm for digitally signing XML.</summary>
        public const string DsaSha256Signature = "http://www.w3.org/2009/xmldsig11#dsa-sha256";

        #endregion
        
        #region RSA

        /// <summary>Specifies a URI that points to the RSA-MD5 cryptographic algorithm for digitally signing XML.</summary>
        public const string RsaMd5Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-md5";

        /// <summary>Specifies a URI that points to the RSA-SHA1 cryptographic algorithm for digitally signing XML.</summary>
        public const string RsaSha1Signature = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

        /// <summary>Specifies a URI that points to the RSA-SHA224 cryptographic algorithm for digitally signing XML.</summary>
        public const string RsaSha244Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha224";

        /// <summary>Specifies a URI that points to the RSA-SHA256 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>[RFC4051]</remarks>
        public const string RsaSha256Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

        /// <summary>Specifies a URI that points to the RSA-SHA384 cryptographic algorithm for digitally signing XML.</summary>
        public const string RsaSha384Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384";

        /// <summary>Specifies a URI that points to the RSA-SHA512 cryptographic algorithm for digitally signing XML.</summary>
        public const string RsaSha512Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";

        /// <summary>Specifies a URI that points to the RSA-RIPEMD160 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>[RFC3447] section 2.3.1</remarks>
        public const string RsaRipemd160Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-ripemd160";

        #endregion

        #region RSA MFG1
        /// <summary>Specifies a URI that points to the RSA-MGF1-SHA256 cryptographic algorithm for digitally signing XML.</summary>
        public const string RsaMgf1Sha256Signature = "http://www.w3.org/2007/05/xmldsig-more#sha256-rsa-MGF1";

        /// <summary>Specifies a URI that points to the RSA-MGF1-SHA384 cryptographic algorithm for digitally signing XML.</summary>
        public const string RsaMgf1Sha384Signature = "http://www.w3.org/2007/05/xmldsig-more#sha384-rsa-MGF1";

        /// <summary>Specifies a URI that points to the RSA-MGF1-SHA512 cryptographic algorithm for digitally signing XML.</summary>
        public const string RsaMgf1Sha512Signature = "http://www.w3.org/2007/05/xmldsig-more#sha512-rsa-MGF1";

        #endregion

        #region ECDSA
        /// <summary>Specifies a URI that points to the ECDSA-SHA1 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>The Elliptic Curve Digital Signature Algorithm (ECDSA) [FIPS-186-2]</remarks>
        public const string EcdsaSha1Signature = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha1";

        /// <summary>Specifies a URI that points to the ECDSA-SHA224 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>The Elliptic Curve Digital Signature Algorithm (ECDSA) [FIPS-186-2]</remarks>
        public const string EcdsaSha244Signature = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha224";

        /// <summary>Specifies a URI that points to the ECDSA-SHA256 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>The Elliptic Curve Digital Signature Algorithm (ECDSA) [FIPS-186-2]</remarks>
        public const string EcdsaSha256Signature = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256";

        /// <summary>Specifies a URI that points to the ECDSA-SHA384 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>The Elliptic Curve Digital Signature Algorithm (ECDSA) [FIPS-186-2]</remarks>
        public const string EcdsaSha384Signature = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha384";

        /// <summary>Specifies a URI that points to the ECDSA-SHA512 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>The Elliptic Curve Digital Signature Algorithm (ECDSA) [FIPS-186-2]</remarks>
        public const string EcdsaSha512Signature = "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512";
        #endregion

        #region ESIGN
        /// <summary>Specifies a URI that points to the ESIGN-SHA1 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>The ESIGN algorithm specified in [IEEE-P1363a]</remarks>
        public const string EsignSha1Signature = "http://www.w3.org/2001/04/xmldsig-more#esign-sha1";

        /// <summary>Specifies a URI that points to the ESIGN-SHA224 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>The ESIGN algorithm specified in [IEEE-P1363a]</remarks>
        public const string EsignSha244Signature = "http://www.w3.org/2001/04/xmldsig-more#esign-sha224";

        /// <summary>Specifies a URI that points to the ESIGN-SHA256 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>The ESIGN algorithm specified in [IEEE-P1363a]</remarks>
        public const string EsignSha256Signature = "http://www.w3.org/2001/04/xmldsig-more#esign-sha256";

        /// <summary>Specifies a URI that points to the ESIGN-SHA384 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>The ESIGN algorithm specified in [IEEE-P1363a]</remarks>
        public const string EsignSha384Signature = "http://www.w3.org/2001/04/xmldsig-more#esign-sha384";

        /// <summary>Specifies a URI that points to the ESIGN-SHA512 cryptographic algorithm for digitally signing XML.</summary>
        /// <remarks>The ESIGN algorithm specified in [IEEE-P1363a]</remarks>
        public const string EsignSha512Signature = "http://www.w3.org/2001/04/xmldsig-more#esign-sha512";
        #endregion

        #region HMAC

        /// <summary>Specifies a URI that points to the HMAC cryptographic algorithm for digitally signing XML.</summary>
        public const string HmacSha1Signature = "http://www.w3.org/2000/09/xmldsig#hmac-sha1";

        /// <summary>Specifies a URI that points to the 224-bit HMAC cryptographic algorithm for digitally signing XML.</summary>
        public const string HmacSha224Signature = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha224";

        /// <summary>Specifies a URI that points to the 256-bit HMAC cryptographic algorithm for digitally signing XML.</summary>
        public const string HmacSha256Signature = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";

        /// <summary>Specifies a URI that points to the 384-bit HMAC cryptographic algorithm for digitally signing XML.</summary>
        public const string HmacSha384Signature = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384";

        /// <summary>Specifies a URI that points to the 512-bit HMAC cryptographic algorithm for digitally signing XML.</summary>
        public const string HmacSha512Signature = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512";

        /// <summary>Specifies a URI that points to the RIPEMD160 HMAC cryptographic algorithm for digitally signing XML.</summary>
        public const string HmacRipemd160Signature = "http://www.w3.org/2001/04/xmldsig-more#hmac-ripemd160";

        #endregion

        #region Digests methods

        /// <summary>Specifies a URI that points to the MD-5 digest algorithm.</summary>
        public const string Md5Digest = "http://www.w3.org/2001/04/xmldsig-more#md5";

        /// <summary>Specifies a URI that points to the 160-bit SHA-1 digest algorithm.</summary>
        public const string Sha1Digest = "http://www.w3.org/2000/09/xmldsig#sha1";

        /// <summary>Specifies a URI that points to the 224-bit SHA-224 digest algorithm.</summary>
        public const string Sha244Digest = "http://www.w3.org/2001/04/xmldsig-more#sha224";

        /// <summary>Specifies a URI that points to the 256-bit SHA-256 digest algorithm.</summary>
        public const string Sha256Digest = "http://www.w3.org/2001/04/xmlenc#sha256";

        /// <summary>Specifies a URI that points to the 384-bit SHA-384 digest algorithm.</summary>
        public const string Sha384Digest = "http://www.w3.org/2001/04/xmldsig-more#sha384";

        /// <summary>Specifies a URI that points to the 512-bit SHA-512 digest algorithm.</summary>
        public const string Sha512Digest = "http://www.w3.org/2001/04/xmlenc#sha512";

        /// <summary>Specifies a URI that points to the RIPEMD-160 cryptographic digest algorithm.</summary>
        public const string Ripemd160Digest = "http://www.w3.org/2001/04/xmlenc#ripemd160";

        #endregion

        #region Symmetric Key Encryption Algorithms
        /// <summary>Specifies a URI that points to the DES cryptographic algorithm for encrypting XML.</summary>
        public const string DesEncryption = "http://www.w3.org/2001/04/xmlenc#des-cbc";

        /// <summary>Specifies a URI that points to the Triple DES cryptographic algorithm for encrypting XML.</summary>
        public const string TripleDesEncryption = "http://www.w3.org/2001/04/xmlenc#tripledes-cbc";

        /// <summary>Specifies a URI that points to the 128-bit AES cryptographic algorithm for encrypting XML.</summary>
        public const string Aes128Encryption = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";

        /// <summary>Specifies a URI that points to the 192-bit AES cryptographic algorithm for encrypting XML.</summary>
        public const string Aes192Encryption = "http://www.w3.org/2001/04/xmlenc#aes192-cbc";

        /// <summary>Specifies a URI that points to the 256-bit AES cryptographic algorithm for encrypting XML.</summary>
        public const string Aes256Encryption = "http://www.w3.org/2001/04/xmlenc#aes256-cbc";

        /// <summary>Specifies a URI that points to the 128-bit AES GCM cryptographic algorithm for encrypting XML.</summary>
        public const string Aes128GcmEncryption = "http://www.w3.org/2009/xmlenc11#aes128-gcm";

        /// <summary>Specifies a URI that points to the 192-bit AES GCM cryptographic algorithm for encrypting XML.</summary>
        public const string Aes192GcmEncryption = "http://www.w3.org/2009/xmlenc11#aes192-gcm";

        /// <summary>Specifies a URI that points to the 256-bit AES GCM cryptographic algorithm for encrypting XML.</summary>
        public const string Aes256GcmEncryption = "http://www.w3.org/2009/xmlenc11#aes256-gcm";

        /// <summary>Specifies a URI that points to the 128-bit AES CCM cryptographic algorithm for encrypting XML.</summary>
        /// <remarks>[Camellia, RFC3713]</remarks>
        public const string Camelia128Encryption = "http://www.w3.org/2001/04/xmldsig-more#camellia128-cbc";

        /// <summary>Specifies a URI that points to the 192-bit AES CCM cryptographic algorithm for encrypting XML.</summary>
        /// <remarks>[Camellia, RFC3713]</remarks>
        public const string Camelia192Encryption = "http://www.w3.org/2001/04/xmldsig-more#camellia192-cbc";

        /// <summary>Specifies a URI that points to the 256-bit AES CCM cryptographic algorithm for encrypting XML.</summary>
        /// <remarks>[Camellia, RFC3713]</remarks>
        public const string Camelia256Encryption = "http://www.w3.org/2001/04/xmldsig-more#camellia256-cbc";

        /// <remarks>[ISO/IEC-18033-2]</remarks>
        public const string PsecKemEncryption = "http://www.w3.org/2001/04/xmldsig-more#psec-kem";

        #endregion

        #region Key Transport Algorithms
        /// <summary>Specifies a URI that points to the RSAES-PKCS1-v1_5 cryptographic algorithm for encrypting and decrypting asymmetric keys (key wrap).</summary>
        public const string RsaV15KeyWrap = "http://www.w3.org/2001/04/xmlenc#rsa-1_5";

        /// <summary>Specifies a URI that points to the RSAES-OAEP (including MGF1 with SHA1 mask generation function) cryptographic algorithm for encrypting and decrypting asymmetric keys (key wrap).</summary>
        public const string RsaOaepMgf1Sha1KeyWrap = "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p";

        /// <summary>Specifies a URI that points to the RSAES-OAEP cryptographic algorithm for encrypting and decrypting asymmetric keys (key wrap).</summary>
        public const string RsaOaepKeyWrap = "http://www.w3.org/2009/xmlenc11#rsa-oaep";

        public const string Mgf1Sha1 = "http://www.w3.org/2009/xmlenc11#mgf1sha1";
        public const string Mgf1Sha224 = " http://www.w3.org/2009/xmlenc11#mgf1sha224";
        public const string Mgf1Sha256 = "http://www.w3.org/2009/xmlenc11#mgf1sha256";
        public const string Mgf1Sha384 = "http://www.w3.org/2009/xmlenc11#mgf1sha384";
        public const string Mgf1Sha512 = "http://www.w3.org/2009/xmlenc11#mgf1sha512";

        #endregion

        #region Symmetric Key Wrap Algorithm URIs

        /// <summary>Specifies a URI that points to the Triple DES cryptographic algorithm for encrypting and decrypting symmetric keys (key wrap).</summary>
        public const string TripleDesKeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-tripledes";

        /// <summary>Specifies a URI that points to the 128-bit AES cryptographic algorithm for encrypting and decrypting symmetric keys (key wrap).</summary>
        public const string Aes128KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes128";

        /// <summary>Specifies a URI that points to the 192-bit AES cryptographic algorithm for encrypting and decrypting symmetric keys (key wrap).</summary>
        public const string Aes192KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes192";

        /// <summary>Specifies a URI that points to the 256-bit AES cryptographic algorithm for encrypting and decrypting symmetric keys (key wrap).</summary>
        public const string Aes256KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes256";

        /// <summary>Specifies a URI that points to the Transport Layer Security (TLS) algorithm for encrypting and decrypting symmetric keys (key wrap).</summary>
        public const string TlsSspiKeyWrap = "http://schemas.xmlsoap.org/2005/02/trust/tlsnego#TLS_Wrap";

        /// <summary>Specifies a URI that points to the GSS-API cryptographic algorithm for encrypting and decrypting Kerberos ticket session keys (key wrap).</summary>
        public const string WindowsSspiKeyWrap = "http://schemas.xmlsoap.org/2005/02/trust/spnego#GSS_Wrap";

        /// <summary>Specifies a URI that points to the 128-bit Camellia cryptographic algorithm for encrypting and decrypting symmetric keys (key wrap).</summary>
        /// <remarks>[Camellia, RFC3713]</remarks>
        public const string Camelia128KeyWrap = "http://www.w3.org/2001/04/xmldsig-more#kw-camellia128";

        /// <summary>Specifies a URI that points to the 192-bit Camellia cryptographic algorithm for encrypting and decrypting symmetric keys (key wrap).</summary>
        /// <remarks>[Camellia, RFC3713]</remarks>
        public const string Camelia192KeyWrap = "http://www.w3.org/2001/04/xmldsig-more#kw-camellia192";

        /// <summary>Specifies a URI that points to the 256-bit Camellia cryptographic algorithm for encrypting and decrypting symmetric keys (key wrap).</summary>
        /// <remarks>[Camellia, RFC3713]</remarks>
        public const string Camelia256KeyWrap = "http://www.w3.org/2001/04/xmldsig-more#kw-camellia256";

        #endregion

        #region Canonicalization Algorithms

        /// <summary>Represents the Exclusive XML Without Comments Canonicalization algorithm.</summary>
        public const string ExclusiveC14nCanonicalization = "http://www.w3.org/2001/10/xml-exc-c14n#";

        /// <summary>Represents the Exclusive XML With Comments Canonicalization algorithm.</summary>
        public const string ExclusiveC14nWithCommentsCanonicalization = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";

        /// <summary>Represents the Minimal Canonicalization algorithm.</summary>
        /// <remarks>Proposed Standard [RFC3075] to Draft Standard[RFC3275]</remarks>
        public const string MinimalCanonicalization = "http://www.w3.org/2000/09/xmldsig#minimal";

        #endregion

        #region Transform Algorithms
        /// <summary>Represents the Security Token Reference-Transform (STR-Transform) algorithm.</summary>
        public const string StrTransform = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#STR-Transform";

        /// <summary>Represents the Security Token Reference-Transform (STR-Transform) algorithm.</summary>
        /// <remarks>[RFC3092]</remarks>
        public const string XPointerTransform = "http://www.w3.org/2001/04/xmldsig-more/xptr";

        #endregion

        #region Key Derivation

        /// <summary>Represents the P-SHA1 key generation algorithm.</summary>
        public const string Psha1KeyDerivation = "http://schemas.xmlsoap.org/ws/2005/02/sc/dk/p_sha1";

        /// <summary>Represents the December 2007 version of the P-SHA1 key generation algorithm.</summary>
        public const string Psha1KeyDerivationDec2005 = "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512/dk/p_sha1";

        /// <summary>Represents the ConcatKDF key generation algorithm.</summary>
        public const string ConcatKdfKeyDerivation = "http://www.w3.org/2009/xmlenc11#ConcatKDF";

        /// <summary>Represents the PBKDF2 key generation algorithm.</summary>
        public const string Pbkdf2KeyDerivation = "http://www.w3.org/2009/xmlenc11#pbkdf2";

        #endregion

        /*
        internal const int DefaultSymmetricKeyLength = 256;
        internal const string DefaultEncryptionAlgorithm = Aes256Encryption;
        internal const string DefaultAsymmetricKeyWrapAlgorithm = RsaOaepKeyWrap;
        internal const string DefaultAsymmetricSignatureAlgorithm = RsaSha256Signature;
        internal const string DefaultDigestAlgorithm = Sha256Digest;
         */

        private SecurityAlgorithms() {
        }
    }
}
