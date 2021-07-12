using System.Text.RegularExpressions;

namespace MultyParser.Core
{
    public class MpTransliter : Transliteration
    {
        public override string Front(string text, TransliterationType type = TransliterationType.ISO)
        {
            return ProperReplace(base.Front(text, type));
        }

        public override string Back(string text, TransliterationType type = TransliterationType.ISO)
        {
            return ProperReplace(base.Back(text, type));
        }

        private string ProperReplace(string text)
        {
            string result = text.Replace(" ", "-");
            result = Regex.Replace(result, "[()\"]", "");
            return result;
        }
    }
}
