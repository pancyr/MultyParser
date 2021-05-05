using System;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MultyParser.Core
{
    public abstract class ParserBase
    {
        public DoWorkEventArgs WorkerArgs { get; set; }

        protected ResultBookCreaterBase _resultBookCreater;
        public ResultBookCreaterBase ResultBookCreater 
        { 
            get
            {
                return _resultBookCreater;
            }
        }

        protected abstract string GetCodeOfTovarGroup();
        protected abstract Dictionary<string, int> GetSpecifications();

        #region Реализация события для индикатора обработки

        public delegate void SetProgressValueHandler(int NewValue);
        public event SetProgressValueHandler SetProgressValue;

        protected void OnSetProgressValue(int currentPos)
        {
            if (SetProgressValue != null)
                SetProgressValue(currentPos);
        }

        #endregion

    }
}