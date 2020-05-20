using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MINISwepper2
{
    public partial class frmSetName : Form
    {
        public string Name { get; private set; }
        public frmSetName()
        {
            InitializeComponent();
            btnSubmitName.Click += BtnSubmitName_Click;
        }

        private void BtnSubmitName_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Empty Text Box!");
                return;
            }
            Name = textBoxName.Text;
            this.Close();
        }
    }
}
