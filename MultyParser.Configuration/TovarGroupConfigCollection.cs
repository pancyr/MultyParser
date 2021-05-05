using System.Configuration;

namespace MultyParser.Configuration
{
    [ConfigurationCollection(typeof(TovarGroupConfigElement), AddItemName = "TovarGroup")]
    public class TovarGroupConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TovarGroupConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TovarGroupConfigElement)element).Code;
        }
    }
}