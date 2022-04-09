using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TEST.MODEL;

namespace TEST
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openMxd_Click(object sender, EventArgs e)
        {
            OpenFeatureClass op = new OpenFeatureClass();
            op.openMxd(axMapControl1, axTOCControl1);
        }

        private void openShp_Click(object sender, EventArgs e)
        {
            OpenFeatureClass op=new OpenFeatureClass();
            op.openShp(axMapControl1, axTOCControl1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MapOperate op = new MapOperate();
            op.queryLayerByAttribute();
        }
    }
}
