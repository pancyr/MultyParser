using System.Configuration;

namespace MultyParser.Configuration
{
    public class KeywordPatternConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
        }
    }
}
