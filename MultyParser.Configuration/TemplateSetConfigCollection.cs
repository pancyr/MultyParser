using System.Configuration;

namespace MultyParser.Configuration
{
    [ConfigurationCollection(typeof(TemplateSetConfigElement), AddItemName = "TemplateSet")]
    public class TemplateSetConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TemplateSetConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TemplateSetConfigElement)element).System;
        }
    }
}
