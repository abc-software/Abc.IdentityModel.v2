namespace Abc.IdentityModel.Protocols.EidasLight {
    /// <summary>
    /// Log messages and codes.
    /// </summary>
    internal static class LogMessages {
        internal const string ID8001 = "ID8001: The eIDAS protocol message cannot be read because it contains data that is not valid.";
        internal const string ID8002 = "ID8002: Unrecognized eIDAS message: '{0}'.";
        internal const string ID8003 = "ID8003: eIDAS message is not valid. The element cannot be empty('{0}', '{1}')";
        internal const string ID8004 = "ID8004: Unable to read eIDAS protocol message. Expecting XmlReader to be at element: ('{0}', '{1}'), found: ('{2}', '{3}').";
        internal const string ID8005 = "ID8005: eIDAS message contains an unrecognized element('{0}', '{1}').";
        internal const string ID8007 = "ID8007: eIDAS message is not valid. '{0}' is not a valid xsd:anyURI '{1}' value.";
        internal const string ID8008 = "ID8008: eIDAS message is not valid. '{0}' has invalid '{1}' value.";
        internal const string ID8009 = "ID8009: When reading 'lightRequest', no LevelsOfAssurance were found.";
        internal const string ID8010 = "ID8010: When reading 'lightRequest', no RequestedAttributes were found.";
        internal const string ID8011 = "ID8011: Exception thrown while reading '{0}'. Inner exception: '{1}'.";

        internal const string ID8109 = "ID8109: A 'lightRequest' must have at least one LevelsOfAssurance.";
        internal const string ID8110 = "ID8110: A 'lightRequest' must have at least one RequestedAttributes.";
    }
}