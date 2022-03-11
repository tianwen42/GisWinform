using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;



namespace gis_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void axToolbarControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IToolbarControlEvents_OnMouseDownEvent e)
        {

        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {

        }

        private void axTOCControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ITOCControlEvents_OnMouseDownEvent e)
        {

        }

        private void btn_openmxd_Click(object sender, EventArgs e)
        {
            /// <summary>
            /// 加载Mxd
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>

            OpenFileDialog ofd = new OpenFileDialog()   //实例化打开文件功能
            {
                Title = "打开Mxd文档",
                Filter = "打开Mxd文档(*.mxd)|*.mxd"
            };
            if (ofd.ShowDialog() == DialogResult.OK)  //打开对话框，并选择OK
            {
                string filename = ofd.FileName;  //打开对话框，并选择OK
                ofd.RestoreDirectory = true;     //打开对话框，并选择OK
                axMapControl1.LoadMxFile(filename);
                axMapControl1.Refresh();
                axTOCControl1.Refresh();
            }

        }


        /// <summary>
        /// 加载Shp文件
        /// </summary>
        /// <param name="sender"></param>
        ///// <param name="e"></param>
        private void btn_openshp_Click(object sender, EventArgs e)
        {
            //    //打开选取文件的对话框
            //    OpenFileDialog ofd = new OpenFileDialog()   //实例化打开文件功能
            //    {
            //        Title = "打开shp图层",
            //        Filter = "打开shp图层(*.shp)|*.shp"
            //    };
            //    ofd.ShowDialog();
            //    ///已经获取到shp文件的路径
            //    ///
            //    string fullFileName = ofd.FileName;
            //    if (fullFileName == "") return;

            //    ///提前路径和文件名
            //    int index = fullFileName.LastIndexOf("\\");
            //    string path = fullFileName.Substring(0, index);  //路径
            //    string filename = fullFileName.Substring(index + 1);  //文件名

            //    ///创建工作空间
            //    IWorkspaceFactory pWorkspaceFactory;
            //    pWorkspaceFactory = new ShapefileWorkspaceFactory();
            //    IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(path, 0) as IFeatureWorkspace;

            //    //提前本工作空间下的要素类
            //    IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(filename);

            //    //将要素类转化成要素图层  为地图图层做准备
            //    IFeatureLayer pFeatureLayer = new FeatureLayer();  //实例化一个要素图层
            //    pFeatureLayer.FeatureClass = pFeatureClass;
            //    pFeatureLayer.Name = "我喜欢的名字";

            //    //将图层加载到地图窗口
            //    axMapControl1.Map.AddLayer(pFeatureLayer as ILayer);
            //    axMapControl1.Refresh();
            //    axTOCControl1.Refresh();
            }
        }
}
