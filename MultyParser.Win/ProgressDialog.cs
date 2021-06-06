using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultyParser.Core;

namespace MultyParser.Win
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog(MainForm form)
        {
            InitializeComponent();
            this.Owner = form;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            ((MainForm)this.Owner).CancelPocessing();
        }

        public void ResetParams()
        {
            this.Text = "Обработка...";
            this.lblPageCount.Text = string.Empty;
            this.progressBar.Value = 0;
        }

        public void SetProgressParams(int currentRow)
        {
            ParserBase parser = ((MainForm)this.Owner).ActiveParserObject;
            this.Text = parser.CurrentParsingTitle;
            this.lblPageCount.Text = String.Format("Лист {0} из {1}", parser.CurrentPageNum, parser.TotalPages);
            this.lblPagePos.Text = String.Format("Обработка строки {0} из {1}", currentRow, parser.TotalRows);
            double percent = (double)currentRow / parser.TotalRows * 100;
            progressBar.Value = (int)percent;
        }
    }
}
