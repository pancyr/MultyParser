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
    public partial class EnterSiteUrl : Form
    {
        public string SelectedUrl
        {
            get
            {
                return txtSiteUrl.Text;
            }
            set
            {
                txtSiteUrl.Text = value;
            }
        }

        public EnterSiteUrl(MainForm form)
        {
            InitializeComponent();
            this.Owner = form;
        }
    }
}
