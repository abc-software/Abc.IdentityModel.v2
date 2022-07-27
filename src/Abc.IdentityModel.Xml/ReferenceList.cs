namespace Abc.IdentityModel.Xml {
    using System;
    using System.Collections.ObjectModel;

    public class ReferenceList {
        public Collection<Uri> DataReferences { get; } = new Collection<Uri>();

        public Collection<Uri> KeyReferences { get; } = new Collection<Uri>();
    }
}
