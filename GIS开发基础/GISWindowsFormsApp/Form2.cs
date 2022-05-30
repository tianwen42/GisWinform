using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace GISWindowsFormsApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void 地图文档加载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog2;
            openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Title = "打开地图文档";
            openFileDialog2.Filter = "地图文档 (*.mxd)|*.mxd";
            openFileDialog2.ShowDialog();
            string sFilePath = openFileDialog2.FileName;
            if (axMapControl1.CheckMxFile(sFilePath))
            {
                axMapControl1.MousePointer =
                esriControlsMousePointer.esriPointerHourglass;
                axMapControl1.LoadMxFile(sFilePath, 0, Type.Missing);
                axMapControl1.MousePointer =
                esriControlsMousePointer.esriPointerDefault;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void 剪切TToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
