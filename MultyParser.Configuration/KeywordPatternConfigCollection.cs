using System.Configuration;

namespace MultyParser.Configuration
{
    [ConfigurationCollection(typeof(KeywordPatternConfigElement), AddItemName = "keywordPattern")]
    public class KeywordPatternConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new KeywordPatternConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((KeywordPatternConfigElement)element).Value;
        }
    }
}