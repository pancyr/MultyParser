using System;
using System.Collections.Generic;
using System.Text;

namespace MultyParser.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SiteUrlAttribute : System.Attribute
    {
        public SiteUrlAttribute(string url)
        {
            this._url = url;
        }

        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TableColumnAttribute : System.Attribute
    {
        public TableColumnAttribute(int iNum, string title)
        {
            this._num = iNum;
            this._title = title;
        }

        private int _num;
        public int Num
        {
            get
            {
                return _num;
            }
        }
        
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateSetAttribute : System.Attribute
    {
        public string Name { get; set; }

        public TemplateSetAttribute()
        { }

        public TemplateSetAttribute(string name)
        {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class MultyStructurePriceAttribute : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SecondaryPriceAttribute : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class NextPriceClassAttribute : System.Attribute
    {
        private string _className;
        public NextPriceClassAttribute(string className)
        {
            this.ClassName = className;
        }

        public string ClassName
        {
            get
            {
                return _className;
            }
            set
            {
                _className = value;
            }
        }
    }
}
