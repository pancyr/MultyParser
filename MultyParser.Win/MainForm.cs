using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultyParser.Core;
using MultyParser.Core.Excel;
using MultyParser.Opencart;

using MultyParser.Configuration;
using System.Configuration;

namespace MultyParser.Win
{
    public partial class MainForm : Form
    {
        private ProgressDialog formDialog;

        public BackgroundWorker PriceProcess
        {
            get
            {
                return this.bgPriceWorker;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void FillListOfFiles()
        {
            string sResultDirName = String.Format("{0}\\Result", Application.StartupPath);
            DirectoryInfo dirInfo = new DirectoryInfo(@sResultDirName);

            listView1.Items.Clear();
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                ListViewItem item = new ListViewItem(file.Name, 1);
                ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "Прайслист"),
                     new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString()),
                    new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortTimeString())};

                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MultyParserConfigSection configSection = 
                (MultyParserConfigSection)ConfigurationManager.GetSection("MultyParserConfigSection");
            MultyParserApp.InitConfiguration(configSection);
            this.FillListOfFiles();
        }

        private void cmdLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.InitialDirectory = Application.StartupPath;
            openDialog.Filter = "Excel (*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls|Все файлы (*.*)|*.*";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                //formDialog = new ProgressDialog(this);
                //ParseSingleFile(openDialog.FileName);
                formDialog = new ProgressDialog(this);
                Object[] args = new Object[] { 1, openDialog.FileName };
                bgPriceWorker.RunWorkerAsync(args);
                formDialog.ResetParams();
                formDialog.ShowDialog();
            }
        }

        private void cmdParseUrl_Click(object sender, EventArgs e)
        {
            EnterSiteUrl siteDialog = new EnterSiteUrl(this);
            if (siteDialog.ShowDialog() == DialogResult.OK)
            {
                //ParseSiteUrl(siteDialog.SelectedUrl);
                //formDialog = new ProgressDialog(this);
                //ParseSingleFile(openDialog.FileName);
                formDialog = new ProgressDialog(this);
                Object[] args = new Object[] { 2, siteDialog.SelectedUrl };
                bgPriceWorker.RunWorkerAsync(args);
                formDialog.ResetParams();
                formDialog.ShowDialog();
            }
        }

        #region Функции потока обработки файлов

        private void bgPriceWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int argType = Int32.Parse(((Object[])e.Argument)[0].ToString());
            /*TableReaderApp.SetProgressDelegate = 
                new TableParserBase.SetProgressValueHandler(bgPriceWorker.ReportProgress);*/
            MultyParserApp.WorkerArgs = e;
            switch (argType)
            {
                case 1: ParseExcelFile(((Object[])e.Argument)[1].ToString()); break;
                case 2: ParseSiteUrl(((Object[])e.Argument)[1].ToString()); break;
            }
        }

        private void bgPriceWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            formDialog.SetProgressParams(e.ProgressPercentage);
        }

        private void bgPriceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            formDialog.Close();
            FillListOfFiles();
        }

        private void ParseExcelFile(string fileName)
        {
            ExcelBook book = ExcelBook.Open(fileName);
            MultyParserApp.DoParsingOfExcelBook(book, txtResultFile.Text, 
                new ParserBase.SetProgressValueHandler(bgPriceWorker.ReportProgress));
            book.Close();
        }

        private void ParseSiteUrl(string siteUrl)
        {
            MultyParserApp.DoParsingOfWebSite(1000, siteUrl, txtResultFile.Text,
                new ParserBase.SetProgressValueHandler(bgPriceWorker.ReportProgress));
        }

        private void ParseDirectory(string sDirName)
        {
            /*DirectoryInfo dirInfo = new DirectoryInfo(@sDirName);
            foreach (FileInfo file in dirInfo.GetFiles("*.xls"))
            {
                MakePriceFromFile(file.FullName);
                if (PriceResult.WorkerArgs.Cancel)
                    return;
            }
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                MakeAllPricesFromFolder(dir.FullName);
                if (PriceResult.WorkerArgs.Cancel)
                    return;
            }*/
        }

        public void CancelPocessing()
        {
            PriceProcess.CancelAsync();
            MultyParserApp.WorkerArgs.Cancel = true;
        }

        #endregion

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.InitialDirectory = Application.StartupPath;
            openDialog.Filter = "Excel (*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls|Все файлы (*.*)|*.*";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
                txtResultFile.Text = openDialog.FileName;
        }
    }
}
