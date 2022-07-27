// ----------------------------------------------------------------------------
// <copyright file="ReferenceList.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Xml {
    using System;
    using System.Collections.ObjectModel;

    public class ReferenceList {
        public Collection<Uri> DataReferences { get; } = new Collection<Uri>();

        public Collection<Uri> KeyReferences { get; } = new Collection<Uri>();
    }
}
