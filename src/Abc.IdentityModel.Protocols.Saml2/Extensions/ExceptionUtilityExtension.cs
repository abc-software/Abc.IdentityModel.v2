// ----------------------------------------------------------------------------
// <copyright file="ExceptionUtilityExtension.cs" company="ABC Software Ltd">
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

namespace Abc.IdentityModel {
    using System;
    using System.Xml;
#if NET40 || NET35
    using ExceptionUtility = Diagnostic.ExceptionUtility;
#else
    using Abc.Diagnostics;
    using ExceptionUtility = Abc.Diagnostics.ExceptionUtility;
#endif

    /// <summary>
    /// Extended Diagnostic exception utility.
    /// </summary>
    internal static class ExceptionUtilityExtension {
        /// <summary>
        /// Throw System.InvalidOperationException with a specified error message.
        /// </summary>
        /// <param name="exceptionUtil">The exception utility.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <returns>
        /// The System.InvalidOperationException.
        /// </returns>
        public static Exception ThrowHelperInvalidOperation(this ExceptionUtility exceptionUtil, string message) {
            if (exceptionUtil == null) {
                throw new ArgumentNullException(nameof(exceptionUtil));
            }

            return exceptionUtil.ThrowHelperError(new InvalidOperationException(message)); 
        }

        /// <summary>
        /// Throw System.Xml.XmlException with a specified error message.
        /// </summary>
        /// <param name="exceptionUtil">The exception utility.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <returns>
        /// The System.Xml.XmlException exception.
        /// </returns>
        public static Exception ThrowHelperXml(this ExceptionUtility exceptionUtil, XmlReader reader, string message) {
            if (exceptionUtil == null) {
                throw new ArgumentNullException(nameof(exceptionUtil));
            }

            return exceptionUtil.ThrowHelperXml(reader, message, null);
        }

        /// <summary>
        /// Throw System.Xml.XmlException with a specified error message.
        /// </summary>
        /// <param name="exceptionUtil">The exception utility.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The System.Exception that threw System.Xml.XmlException.</param>
        /// <returns>
        /// The System.Xml.XmlException exception.
        /// </returns>
        public static Exception ThrowHelperXml(this ExceptionUtility exceptionUtil, XmlReader reader, string message, Exception innerException) {
            if (exceptionUtil == null) {
                throw new ArgumentNullException(nameof(exceptionUtil));
            }

            IXmlLineInfo info = reader as IXmlLineInfo;
            return exceptionUtil.ThrowHelperError(new XmlException(message, innerException, info != null ? info.LineNumber : 0, info != null ? info.LinePosition : 0)); 
        }
    }
}
