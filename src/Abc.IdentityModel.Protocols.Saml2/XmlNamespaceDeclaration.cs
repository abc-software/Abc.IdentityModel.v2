// ----------------------------------------------------------------------------
// <copyright file="XmlNamespaceDeclaration.cs" company="ABC Software Ltd">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Xml namespace declaration.
    /// </summary>
    internal class XmlNamespaceDeclaration {
        private string ns;
        private string prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlNamespaceDeclaration"/> class.
        /// </summary>
        /// <param name="prefix">The namespace prefix.</param>
        /// <param name="ns">The namespace.</param>
        public XmlNamespaceDeclaration(string prefix, string ns) {
            this.prefix = prefix;
            this.ns = ns;
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace {
            get { return this.ns; }
        }

        /// <summary>
        /// Gets the namepspace prefix.
        /// </summary>
        /// <value>The namepspace prefix.</value>
        public string Prefix {
            get { return this.prefix; }
        }
    }
}
