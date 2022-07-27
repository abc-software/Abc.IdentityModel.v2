// ----------------------------------------------------------------------------
// <copyright file="LogMessages.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Xml {
    /// <summary>
    /// Log messages and codes
    /// </summary>
    /// <remarks>
    /// Range: 51000 - 51999
    /// </remarks>
    internal static class LogMessages {
#pragma warning disable 1591
        // EncryptionSerializing reading
        internal const string IDX51106 = "IDX51106: Unable to read for EncryptedData. Element: '{0}' as missing Attribute: '{1}'.";
        internal const string IDX51107 = "IDX51107: When reading '{0}', '{1}' was not a Absolute Uri, was: '{2}'.";
#pragma warning restore 1591
    }
}
