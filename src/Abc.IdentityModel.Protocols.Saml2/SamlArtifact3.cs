// ----------------------------------------------------------------------------
// <copyright file="SamlArtifact3.cs" company="ABC Software Ltd">
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
    using System.Diagnostics.CodeAnalysis;
    
    /// <summary>
    /// Saml atrtifact type 3.
    /// </summary>
    /// <remarks>
    /// Liberty Artifact Profile
    /// 3.2.2.2. Artifact Format
    /// ========================================================
    /// TypeCode := 0x0003
    /// RemainingArtifact := IdentityProviderSuccinctID AssertionHandle
    /// IdentityProviderSuccinctID:= 20-byte_sequence
    /// AssertionHandle := 20-byte_sequence
    /// ========================================================
    /// </remarks> 
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed.")]
    public sealed class SamlArtifact3 : SamlArtifact {
        private const short ArtifactTypeCode = 0x03;
        private const int SourceIdLenght = 20;
        private const int ArtifactLenght = TypeCodeLenght + AssertionHandleLenght + SourceIdLenght;

        private byte[] sourceId;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SamlArtifact3"/> class.
        /// </summary>
        public SamlArtifact3()
            : base(ArtifactTypeCode) {
            this.sourceId = new byte[SourceIdLenght];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamlArtifact3"/> class.
        /// </summary>
        /// <param name="identificationUrl">The identification URL.</param>
        public SamlArtifact3(string identificationUrl)
            : base(ArtifactTypeCode) {
            if (string.IsNullOrEmpty(identificationUrl)) {
                throw new ArgumentNullException(nameof(identificationUrl));
            }

            this.sourceId = CreateSourceId(identificationUrl);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamlArtifact3"/> class.
        /// </summary>
        /// <param name="sourceId">The source id.</param>
        /// <param name="assertionHandler">The assertion handler.</param>
        public SamlArtifact3(byte[] sourceId, byte[] assertionHandler)
            : base(ArtifactTypeCode) {
            this.sourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            this.AssertionHandle = assertionHandler ?? throw new ArgumentNullException(nameof(assertionHandler));

            if (sourceId.Length != SourceIdLenght) {
                throw new ArgumentOutOfRangeException(nameof(sourceId), "The source id length is invalid.");
            }
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the identity provider succinct id.
        /// </summary>
        /// <value>The identity provider succinct id.</value>
        public byte[] SourceId {
            get {
                return this.sourceId;
            }

            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Length != SourceIdLenght) {
                    throw new ArgumentOutOfRangeException(nameof(value), "The source id length is invalid.");
                }

                this.sourceId = value;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="samlArtifact1">The saml artifact1.</param>
        /// <param name="samlArtifact2">The saml artifact2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SamlArtifact3 samlArtifact1, SamlArtifact3 samlArtifact2) {
            return SamlArtifact3.Equals(samlArtifact1, samlArtifact2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="samlArtifact1">The saml artifact1.</param>
        /// <param name="samlArtifact2">The saml artifact2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SamlArtifact3 samlArtifact1, SamlArtifact3 samlArtifact2) {
            return !SamlArtifact3.Equals(samlArtifact1, samlArtifact2);
        }

        /// <summary>
        /// Equalses the specified saml artifact1.
        /// </summary>
        /// <param name="samlArtifact1">The saml artifact1.</param>
        /// <param name="samlArtifact2">The saml artifact2.</param>
        /// <returns><c>true</c> if the value of <paramref name="samlArtifact1"/> is the same as the value of <paramref name="samlArtifact2"/>; otherwise, <c>false</c>.</returns>
        public static bool Equals(SamlArtifact3 samlArtifact1, SamlArtifact3 samlArtifact2) {
            if (samlArtifact1 as object == samlArtifact2 as object) {
                return true;
            }

            if (samlArtifact1 as object == null || samlArtifact2 as object == null) {
                return false;
            }

            return samlArtifact1.TypeCode == samlArtifact2.TypeCode &&
                ArraysEqual(samlArtifact1.sourceId, samlArtifact2.sourceId) &&
                ArraysEqual(samlArtifact1.AssertionHandle, samlArtifact2.AssertionHandle);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="Abc.IdentityModel.Protocols.SamlArtifact3"/>.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SamlArtifact3(string artifact) {
            if (artifact == null) {
                return null;
            }

            var samlArtifact = new SamlArtifact3();
            samlArtifact.LoadArtifactFromString(artifact);
            return samlArtifact;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SamlArtifact3"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="samlArtifact">The saml artifact.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(SamlArtifact3 samlArtifact) {
            if (samlArtifact == null) {
                return null;
            }

            return samlArtifact.ToString();
        }

        /// <summary>
        /// Loads the artifact from byte array.
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="sourceArray"/> is <c>null</c>.</exception>
        public override void LoadArtifactFromByteArray(byte[] sourceArray) {
            if (sourceArray == null) {
                throw new ArgumentNullException(nameof(sourceArray));
            }

            if (sourceArray.Length != ArtifactLenght) {
                throw new ArgumentException($"SAML artifact parameter has wrong length {sourceArray.Length}, expected {ArtifactLenght}.", nameof(sourceArray));
            }

            short typeCode = (short)((sourceArray[0] * 0xff) + sourceArray[1]);
            if (typeCode != ArtifactTypeCode) {
                throw new ArgumentException($"Wrong SAML artifact type code {typeCode}, expected {ArtifactTypeCode}.", nameof(sourceArray));
            }

            byte[] sourceId = new byte[SourceIdLenght];
            Array.Copy(sourceArray, TypeCodeLenght, sourceId, 0, SourceIdLenght);
            this.SourceId = sourceId; 

            byte[] assertionHandle = new byte[AssertionHandleLenght];
            Array.Copy(sourceArray, TypeCodeLenght + SourceIdLenght, assertionHandle, 0, AssertionHandleLenght);
            this.AssertionHandle = assertionHandle; 
        }

        /// <summary>
        /// Verifies the artifact.
        /// </summary>
        /// <param name="identificationUrl">The identification URL.</param>
        /// <returns>
        /// <c>true</c> if artifact valid, otherwise <c>false</c>.
        /// </returns>
        public override bool VerifyArtifact(string identificationUrl) {
            if (string.IsNullOrEmpty(identificationUrl)) {
                throw new ArgumentNullException(nameof(identificationUrl));
            }

            return SamlArtifact.ArraysEqual(this.SourceId, SamlArtifact.CreateSourceId(identificationUrl)); 
        }

        /// <summary>
        /// Returns a byte array that contains the value of this instance.
        /// </summary>
        /// <returns>
        /// A byte array.
        /// </returns>
        public override byte[] ToByteArray() {
            byte[] destinationArray = new byte[ArtifactLenght];

            destinationArray[0] = (byte)(this.TypeCode / 0xff);
            destinationArray[1] = (byte)(this.TypeCode % 0xff);
            this.sourceId.CopyTo(destinationArray, TypeCodeLenght);
            this.AssertionHandle.CopyTo(destinationArray, TypeCodeLenght + SourceIdLenght);

            return destinationArray;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return Convert.ToBase64String(this.ToByteArray());
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(SamlArtifact3)) {
                return false;
            }

            return SamlArtifact3.Equals(this, (SamlArtifact3)obj);  
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return this.TypeCode ^
                this.sourceId.ToString().GetHashCode() ^ 
                this.AssertionHandle.ToString().GetHashCode();      
        }
        #endregion
    }
}