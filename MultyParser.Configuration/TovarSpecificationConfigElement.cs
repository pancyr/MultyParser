using System.Configuration;

namespace MultyParser.Configuration
{
    public class TovarSpecificationConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public int ID
        {
            get { return (int)this["id"]; }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("filter", IsRequired = false)]
        public bool filter
        {
            get { return (bool)this["filter"]; }
        }
    }
}
