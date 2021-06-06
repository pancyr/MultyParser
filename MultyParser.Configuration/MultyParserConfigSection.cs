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

        [ConfigurationProperty("TemplateSets", IsDefaultCollection = true)]
        public TemplateSetConfigCollection TemplateSets
        {
            get { return (TemplateSetConfigCollection)this["TemplateSets"]; }
        }

        [ConfigurationProperty("KeywordPatternList", IsDefaultCollection = true)]
        public KeywordPatternConfigCollection KeywordPatterns
        {
            get { return (KeywordPatternConfigCollection)this["KeywordPatternList"]; }
        }

        private static MultyParserConfigSection _settings;
        public static MultyParserConfigSection Settings
        {
            get
            {
                if (_settings == null)
                    _settings = (MultyParserConfigSection)ConfigurationManager.GetSection("MultyParserConfigSection");
                return _settings;
            }
        }

        public static void ResetSettings()
        {
            _settings = null;
        }
    }
}
