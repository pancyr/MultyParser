using System.Configuration;

namespace MultyParser.Configuration
{
    public class TovarGroupConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("code", IsKey = true, IsRequired = true)]
        public string Code
        {
            get { return (string)this["code"]; }
        }

        [ConfigurationProperty("displayName", IsRequired = true)]
        public string DisplayName
        {
            get { return (string)this["displayName"]; }
        }

        [ConfigurationProperty("attributeList", IsDefaultCollection = true)]
        public TovarSpecificationConfigCollection AttributeList
        {
            get { return (TovarSpecificationConfigCollection)this["attributeList"]; }
        }
    }
}
