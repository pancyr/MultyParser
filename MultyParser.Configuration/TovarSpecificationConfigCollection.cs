using System.Configuration;

namespace MultyParser.Configuration
{
    [ConfigurationCollection(typeof(TovarSpecificationConfigElement), AddItemName = "attribute")]
    public class TovarSpecificationConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TovarSpecificationConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TovarSpecificationConfigElement)element).ID;
        }
    }
}
