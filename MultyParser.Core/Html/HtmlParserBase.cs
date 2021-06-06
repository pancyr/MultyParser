using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

using Leaf.xNet;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Dom;

namespace MultyParser.Core.Html
{
    public abstract class HtmlParserBase : ParserBase
    {
        public abstract string GetSiteName();

        protected abstract string GetBaseUrl();                                     // базовый адрес сайта
        protected abstract string GetSelectorForList();                             // селектор списка ссылок на товар
        protected abstract void ProcessingEntityInLoop(IHtmlDocument docDetails);   // обработка сущности в цикле


        protected virtual string GetSelectorForNextButton() => null;        // селектор кнопки пагинации
        protected virtual void BeforeEntityLoop() { }
        protected virtual void AfterEntityLoop() { }

        public virtual bool DoParsingOfIncomingHtml(string link, DoWorkEventArgs args)
        {
            /* Получаем все ссылки пагинации, чтобы подсчитать ко-во страниц */
            List<string> pages = new List<string>();
            GetListOfPages(link, pages);
            this.BeforeEntityLoop();

            foreach (string p in pages)
            {
                this.CurrentPageNum++;
                IHtmlDocument docList = GetWebDocument(p);
                var listItems = docList.QuerySelectorAll(GetSelectorForList()); // список всех товарных позиций на странице каталога
                this.TotalRows = listItems.Count();               // это для отображения в диалоге хода процесса
                foreach (var item in listItems)
                {
                    if (args.Cancel) break;
                    this.OnSetProgressValue(listItems.Index(item) + 1);     // ставим прогресс-бар на актуальную позицию
                    
                    string docUrl = item.GetAttribute("href");
                    if (docUrl.StartsWith("/"))
                        docUrl = GetBaseUrl() + docUrl;

                    try
                    {
                        // теперь переходим по ссылке из каталога на страницу и читаем данные о товаре
                        IHtmlDocument docDetails = GetWebDocument(docUrl);
                        this.ProcessingEntityInLoop(docDetails);
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(exp.Message);
                        continue;
                    }
                }
            }

            this.AfterEntityLoop();
            ResultBookCreater.FinalProcessing();
            return true;
        }

        public virtual string GetDepartmentByUrl(string url)
        {
            if (url.EndsWith("/"))
                url = url.Substring(0, url.Length - 1);
            Regex reg = new Regex(@"[^/]*", RegexOptions.RightToLeft);
            return reg.Match(url).ToString();
        }

        public virtual int CountPages(string link)
        {
            List<string> pages = new List<string>();
            GetListOfPages(link, pages);
            return pages.Count;
        }

        public virtual bool GetListOfPages(string link, List<string> pages)
        {
            pages.Add(link);
            IHtmlDocument doc = GetWebDocument(link);
            string nextSelector = GetSelectorForNextButton();

            if (nextSelector != null)
            {
                IElement next = doc.QuerySelector(nextSelector);

                if (next != null)
                {
                    string n_link = next.GetAttribute("href");
                    if (n_link.StartsWith("/"))
                        n_link = GetBaseUrl() + n_link;
                    if (link != n_link)
                        GetListOfPages(n_link, pages);
                }
            }

            return true;
        }

        protected string GetFullUrl(string address)
        {
            int q_index = address.IndexOf("?");
            if (q_index > -1)
                address = address.Substring(0, q_index);
            if (address.StartsWith(@"//"))
                address = "https:" + address;
            else if (address.StartsWith(@"/"))
                address = GetBaseUrl() + address;
            return address;
        }

        protected IHtmlDocument GetWebDocument(string link)
        {
            HttpRequest request = new HttpRequest();
            string response = request.Get(link).ToString();

            HtmlParser parser = new HtmlParser();
            IHtmlDocument result = parser.ParseDocument(response);
            return result;
        }
    }
}
