// ----------------------------------------------------------------------------
// <copyright file="SamlArtifact.cs" company="ABC Software Ltd">
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

namespace Abc.IdentityModel.Protocols {
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Saml artifact base class.
    /// </summary>
    /// <remarks>
    /// Bindings and Profiles for the OASIS Security Assertion Markup Language (SAML) V1.1
    /// </remarks> 
    public abstract class SamlArtifact : ISamlArtifact {
        #region Fields
        /// <summary>
        /// AssertionHandle lenght in bytes.
        /// </summary>
        protected const int AssertionHandleLenght = 20;

        /// <summary>
        /// Artifact type lenght in bytes.
        /// </summary>
        protected const int TypeCodeLenght = sizeof(short);

        private readonly short typeCode;
        private byte[] assertionHandle;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SamlArtifact"/> class.
        /// </summary>
        /// <param name="typeCode">The artifact type code.</param>
        protected SamlArtifact(short typeCode) {
            if (typeCode <= 0) {
                throw new ArgumentOutOfRangeException(nameof(typeCode));
            }

            this.typeCode = typeCode;
            this.assertionHandle = CreateAssertionHandle();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the assertion handle.
        /// </summary>
        /// <value>The assertion handle.</value>
        public byte[] AssertionHandle {
            get {
                return this.assertionHandle;
            }

            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Length != 20) {
                    throw new ArgumentException("Assertion handle must be 20 bytes lenght.", nameof(value));
                }

                this.assertionHandle = value;
            }
        }

        /// <summary>
        /// Gets the artifact type code.
        /// </summary>
        /// <value>The artifact type code.</value>
        public short TypeCode {
            get { return this.typeCode; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compare two arrays.
        /// </summary>
        /// <param name="ba1">The first array.</param>
        /// <param name="ba2">The second array.</param>
        /// <returns><c>true</c> if the value of <paramref name="ba1"/> is the same as the value of <paramref name="ba2"/>; otherwise, <c>false</c>.</returns>
        public static bool ArraysEqual(byte[] ba1, byte[] ba2) {
            if (ba1 == null) {
                throw new ArgumentNullException(nameof(ba1));
            }

            if (ba2 == null) {
                throw new ArgumentNullException(nameof(ba2));
            }

            if (ba1.Length != ba2.Length) {
                return false;
            }

            for (int i = 0; i < ba1.Length; i++) {
                if (ba1[i] != ba2[i]) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates the assertion handle.
        /// </summary>
        /// <returns>The assertion handle</returns>
        public static byte[] CreateAssertionHandle() {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] assertionHandle = new byte[AssertionHandleLenght];
            rng.GetBytes(assertionHandle);
            return assertionHandle;
        }

        /// <summary>
        /// Creates the source id.
        /// </summary>
        /// <param name="identificationUrl">The identification URL.</param>
        /// <returns>The source Id.</returns>
        public static byte[] CreateSourceId(string identificationUrl) {
            if (string.IsNullOrEmpty(identificationUrl)) {
                throw new ArgumentNullException(nameof(identificationUrl));
            }

            byte[] bytes = Encoding.UTF8.GetBytes(identificationUrl);
            return SHA1.Create().ComputeHash(bytes);
        }

        /// <summary>
        /// Gets the saml artifact by type code.
        /// </summary>
        /// <param name="typeCode">The type code.</param>
        /// <returns>The saml artifact.</returns>
        public static ISamlArtifact GetSamlArtifactByTypeCode(short typeCode) {
            if (typeCode <= 0) {
                throw new ArgumentOutOfRangeException(nameof(typeCode));
            }

            if (typeCode == 0x01) {
                return new SamlArtifact1(); 
            }
            else if (typeCode == 0x02) {
                return new SamlArtifact2(); 
            }
            else if (typeCode == 0x03) {
                return new SamlArtifact3();
            }
            else if (typeCode == 0x04) {
                return new SamlArtifact4();
            }

            throw new NotSupportedException($"Artifact with type code {typeCode} do not supported."); 
        }

        /// <summary>
        /// Loads the saml artifact from BASE64 string.
        /// </summary>
        /// <param name="base64Artifact">The artifact BASE64 string.</param>
        /// <returns>The saml artifact.</returns>
        /// <exception cref="T:System.ArgumentNullException">if <paramref name="base64Artifact"/> is null.</exception>
        /// <exception cref="T:System.FormatException">if <paramref name="base64Artifact"/> is not BASE64 string.</exception>
        public static ISamlArtifact LoadSamlArtifactFromString(string base64Artifact) {
            if (string.IsNullOrEmpty(base64Artifact)) {
                throw new ArgumentNullException(nameof(base64Artifact));
            }

            byte[] sourceArray = Convert.FromBase64String(base64Artifact);
            if (sourceArray.Length < 2) {
                throw new InvalidOperationException("Decoded artifact must be at least 2 bytes lenght.");
            }

            short typeCode = (short)((sourceArray[0] * 0xff) + sourceArray[1]);

            ISamlArtifact samlArtifact = SamlArtifact.GetSamlArtifactByTypeCode(typeCode);
            samlArtifact.LoadArtifactFromByteArray(sourceArray);
            return samlArtifact;
        }

        /// <summary>
        /// Loads the saml artifact from byte array.
        /// </summary>
        /// <param name="binaryArtifact">The artifact byte arrray.</param>
        /// <returns>The saml artifact.</returns>
        /// <exception cref="T:System.ArgumentNullException">if <paramref name="binaryArtifact"/> is null.</exception>
        public static ISamlArtifact LoadSamlArtifactFromByteArray(byte[] binaryArtifact) {
            if (binaryArtifact == null) {
                throw new ArgumentNullException(nameof(binaryArtifact));
            }

            if (binaryArtifact.Length < 2) {
                throw new ArgumentException("The artifact length is invalid.", nameof(binaryArtifact));
            }

            short typeCode = (short)((binaryArtifact[0] * 0xff) + binaryArtifact[1]);

            ISamlArtifact samlArtifact = SamlArtifact.GetSamlArtifactByTypeCode(typeCode);
            samlArtifact.LoadArtifactFromByteArray(binaryArtifact);
            return samlArtifact;
        }

        /// <summary>
        /// Loads the artifact from BASE64 string.
        /// </summary>
        /// <param name="base64Artifact">The artifact BASE64 string.</param>
        /// <exception cref="T:System.ArgumentNullException">if <paramref name="base64Artifact"/> is null.</exception>
        /// <exception cref="T:System.FormatException">if <paramref name="base64Artifact"/> is not BASE64 string.</exception>
        public virtual void LoadArtifactFromString(string base64Artifact) {
            if (base64Artifact == null) {
                throw new ArgumentNullException(nameof(base64Artifact));
            }

            this.LoadArtifactFromByteArray(Convert.FromBase64String(base64Artifact)); 
        }

        /// <summary>
        /// Loads the artifact from byte array.
        /// </summary>
        /// <param name="binaryArtifact">The artifact byte array.</param>
        public abstract void LoadArtifactFromByteArray(byte[] binaryArtifact);

        /// <summary>
        /// Verifies the artifact.
        /// </summary>
        /// <param name="identificationUrl">The identification URL.</param>
        /// <returns><c>true</c> if artifact valid, otherwise <c>false</c>.</returns>
        public abstract bool VerifyArtifact(string identificationUrl);

        /// <summary>
        /// Returns a byte array that contains the value of this instance.
        /// </summary>
        /// <returns>
        /// A byte array.
        /// </returns>
        public abstract byte[] ToByteArray();
        #endregion
    }
}