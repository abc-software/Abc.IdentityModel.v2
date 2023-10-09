// ----------------------------------------------------------------------------
// <copyright file="SamlArtifact2.cs" company="ABC Software Ltd">
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
    using System.Text;

    /// <summary>
    /// Saml artifact type 2.
    /// </summary>
    /// <remarks>
    /// Bindings and Profiles for the OASIS Security Assertion Markup Language (SAML) V1.1
    /// 7 Alternative SAML Artifact Format
    /// ========================================================
    /// TypeCode            := 0x0002
    /// RemainingArtifact   := SourceID SourceLocation
    /// SourceID            := 20-byte_sequence
    /// AssertionHandle     := URI
    /// ========================================================
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed.")]
    public sealed class SamlArtifact2 : SamlArtifact {
        private const short ArtifactTypeCode = 0x02;
        private const int ArtifactMinimumLenght = TypeCodeLenght + AssertionHandleLenght;

        private string sourceLocation;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SamlArtifact2"/> class.
        /// </summary>
        public SamlArtifact2()
            : base(ArtifactTypeCode) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamlArtifact2"/> class.
        /// </summary>
        /// <param name="sourceLocation">The source location.</param>
        public SamlArtifact2(string sourceLocation)
            : base(ArtifactTypeCode) {
            if (string.IsNullOrEmpty(sourceLocation)) {
                throw new ArgumentNullException(nameof(sourceLocation));
            }

            this.sourceLocation = sourceLocation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamlArtifact2"/> class.
        /// </summary>
        /// <param name="sourceLocation">The source location.</param>
        /// <param name="assertionHandler">The assertion handler.</param>
        public SamlArtifact2(string sourceLocation, byte[] assertionHandler)
            : base(ArtifactTypeCode) {
            if (string.IsNullOrEmpty(sourceLocation)) {
                throw new ArgumentNullException(nameof(sourceLocation));
            }

            if (assertionHandler == null) {
                throw new ArgumentNullException(nameof(assertionHandler));
            }

            this.sourceLocation = sourceLocation;
            this.AssertionHandle = assertionHandler;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the source location.
        /// </summary>
        /// <value>The source location.</value>
        public string SourceLocation {
            get {
                return this.sourceLocation;
            }

            set {
                if (string.IsNullOrEmpty(value)) {
                    throw new ArgumentNullException(nameof(value));
                }

                this.sourceLocation = value;
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
        public static bool operator ==(SamlArtifact2 samlArtifact1, SamlArtifact2 samlArtifact2) {
            return SamlArtifact2.Equals(samlArtifact1, samlArtifact2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="samlArtifact1">The saml artifact1.</param>
        /// <param name="samlArtifact2">The saml artifact2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SamlArtifact2 samlArtifact1, SamlArtifact2 samlArtifact2) {
            return !SamlArtifact2.Equals(samlArtifact1, samlArtifact2);
        }

        /// <summary>
        /// Equalses the specified saml artifact1.
        /// </summary>
        /// <param name="samlArtifact1">The saml artifact1.</param>
        /// <param name="samlArtifact2">The saml artifact2.</param>
        /// <returns><c>true</c> if the value of <paramref name="samlArtifact1"/> is the same as the value of <paramref name="samlArtifact2"/>; otherwise, <c>false</c>.</returns>
        public static bool Equals(SamlArtifact2 samlArtifact1, SamlArtifact2 samlArtifact2) {
            if (samlArtifact1 as object == samlArtifact2 as object) {
                return true;
            }

            if (samlArtifact1 as object == null || samlArtifact2 as object == null) {
                return false;
            }

            return samlArtifact1.TypeCode == samlArtifact2.TypeCode &&
                ArraysEqual(samlArtifact1.AssertionHandle, samlArtifact2.AssertionHandle) &&
                string.Equals(samlArtifact1.sourceLocation, samlArtifact2.sourceLocation, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="SamlArtifact2"/>.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SamlArtifact2(string artifact) {
            if (artifact == null) {
                return null;
            }

            var samlArtifact = new SamlArtifact2();
            samlArtifact.LoadArtifactFromString(artifact);
            return samlArtifact;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SamlArtifact2"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="samlArtifact">The saml artifact.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(SamlArtifact2 samlArtifact) {
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

            if (sourceArray.Length < ArtifactMinimumLenght) {
                throw new ArgumentException($"SAML artifact parameter has wrong length {sourceArray.Length}, expected at least {ArtifactMinimumLenght}.", nameof(sourceArray));
            }

            short typeCode = (short)((sourceArray[0] * 0xff) + sourceArray[1]);
            if (typeCode != ArtifactTypeCode) {
                throw new ArgumentException($"Wrong SAML artifact type code {typeCode}, expected {ArtifactTypeCode}.", nameof(sourceArray));
            }

            byte[] assertionHandle = new byte[AssertionHandleLenght];
            Array.Copy(sourceArray, TypeCodeLenght, assertionHandle, 0, AssertionHandleLenght);
            this.AssertionHandle = assertionHandle; 

            this.SourceLocation = Encoding.UTF8.GetString(sourceArray, TypeCodeLenght + AssertionHandleLenght, sourceArray.Length - TypeCodeLenght - AssertionHandleLenght);
        }

        /// <summary>
        /// Verifies the artifact.
        /// </summary>
        /// <param name="identificationUrl">The identification URL.</param>
        /// <returns>
        /// <c>true</c> if artifact valid, otherwise <c>false</c>.
        /// </returns>
        public override bool VerifyArtifact(string identificationUrl) {
            return string.Equals(this.SourceLocation, identificationUrl, StringComparison.OrdinalIgnoreCase);  
        }

        /// <summary>
        /// Returns a byte array that contains the value of this instance.
        /// </summary>
        /// <returns>
        /// A byte array.
        /// </returns>
        public override byte[] ToByteArray() {
            int artifactLenght = ArtifactMinimumLenght;
            byte[] sourceArray = null;
            if (sourceLocation != null) {
                sourceArray = Encoding.UTF8.GetBytes(sourceLocation);
                artifactLenght += sourceArray.Length;
            }

            byte[] destinationArray = new byte[artifactLenght];

            destinationArray[0] = (byte)(this.TypeCode / 0xff);
            destinationArray[1] = (byte)(this.TypeCode % 0xff);
            this.AssertionHandle.CopyTo(destinationArray, TypeCodeLenght);

            if (sourceArray != null) {
                sourceArray.CopyTo(destinationArray, TypeCodeLenght + AssertionHandleLenght);
            }

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
            if (obj == null || obj.GetType() != typeof(SamlArtifact2)) {
                return false;
            }

            return SamlArtifact2.Equals(this, (SamlArtifact2)obj);  
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return this.TypeCode ^
                this.AssertionHandle.ToString().GetHashCode() ^
                sourceLocation.GetHashCode(); 
        }
        #endregion
    }
}