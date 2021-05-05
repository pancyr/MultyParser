using System.Configuration;

namespace MultyParser.Configuration
{
    public class MultyParserConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("TovarGroups", IsDefaultCollection = true)]
        public TovarGroupConfigCollection TovarGroups
        {
            get { return (TovarGroupConfigCollection)this["TovarGroups"]; }
        }
    }
}
