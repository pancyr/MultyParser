using System.Configuration;

namespace MultyParser.Configuration
{
    public class TemplateConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("code", IsRequired = true)]
        public string Code
        {
            get { return (string)this["code"]; }
        }

        [ConfigurationProperty("file", IsRequired = true)]
        public string File
        {
            get { return (string)this["file"]; }
        }
    }
}