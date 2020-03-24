using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace Dialogi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        StreamReader sr = new StreamReader(@"C:\Users\Julia\Documents\test.txt");

        private void clickAction(object sender, EventArgs e)
        {
            Dialog.Text = "";
            string ln=String.Empty;
            string ln2 = String.Empty;
            
            if((ln = sr.ReadLine()) != null)
            {
                for(int i=0;i<ln.Length;i++)
                {
                    ln2 = ln2.Insert(ln2.Length, ln[i].ToString());
                    Dialog.Text = ln2;
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(50);
                }
            }
            
        }
    }
}
