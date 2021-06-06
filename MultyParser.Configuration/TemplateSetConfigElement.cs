using System.Configuration;

namespace MultyParser.Configuration
{
    public class TemplateSetConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("system", IsKey = true, IsRequired = true)]
        public string System
        {
            get { return (string)this["system"]; }
        }

        [ConfigurationProperty("dir", IsRequired = true)]
        public string Dir
        {
            get { return (string)this["dir"]; }
        }

        [ConfigurationProperty("templateList", IsDefaultCollection = true)]
        public TemplateConfigCollection TemplateList
        {
            get { return (TemplateConfigCollection)this["templateList"]; }
        }
    }
}