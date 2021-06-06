using System.Configuration;

namespace MultyParser.Configuration
{
    [ConfigurationCollection(typeof(TemplateConfigElement), AddItemName = "template")]
    public class TemplateConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TemplateConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TemplateConfigElement)element).Code;
        }
    }
}