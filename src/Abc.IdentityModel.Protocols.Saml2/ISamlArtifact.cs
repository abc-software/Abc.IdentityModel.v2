// ----------------------------------------------------------------------------
// <copyright file="ISamlArtifact.cs" company="ABC Software Ltd">
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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The Saml artifact.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Saml", Justification = "SAML")]
    public interface ISamlArtifact {
        /// <summary>
        /// Gets the artifact type code.
        /// </summary>
        /// <value>The artifact type code.</value>
        short TypeCode { get; }

        /// <summary>
        /// Gets or sets the assertion handle.
        /// </summary>
        /// <value>The assertion handle.</value>
        byte[] AssertionHandle { get; set; }

        /// <summary>
        /// Loads the artifact from BASE64 string.
        /// </summary>
        /// <param name="base64Artifact">The artifact BASE64 string.</param>
        void LoadArtifactFromString(string base64Artifact);

        /// <summary>
        /// Loads the artifact from byte array.
        /// </summary>
        /// <param name="binaryArtifact">The artifact byte array.</param>
        void LoadArtifactFromByteArray(byte[] binaryArtifact);

        /// <summary>
        /// Verifies the artifact.
        /// </summary>
        /// <param name="identificationUrl">The identification URL.</param>
        /// <returns><c>true</c> if artifact valid, otherwise <c>false</c>.</returns>
        bool VerifyArtifact(string identificationUrl);

        /// <summary>Returns a byte array that contains the value of this instance.</summary>
        /// <returns>A byte array.</returns>
        byte[] ToByteArray();
    }
}
