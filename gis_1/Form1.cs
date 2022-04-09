using System;
using System.Windows.Forms;



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

            OpenFileDialog OpenMXD = new OpenFileDialog();
            OpenMXD.Title = "打开地图";
            OpenMXD.InitialDirectory = "E:";
            OpenMXD.Filter = "Map Documents (*.mxd)|*.mxd";
            if (OpenMXD.ShowDialog() == DialogResult.OK)
            {
                string MxdPath = OpenMXD.FileName;
                axMapControl1.LoadMxFile(MxdPath);
            }
        }


        /// <summary>
        /// 打开shp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_openshp_Click(object sender, EventArgs e)
        {
            //IWorkspaceFactory pWorkspaceFactory = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(pFilePath, 0);
            //IFeatureWorkspace pFeatureWorkspace = new ShapefileWorkspaceFactory();
            //IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pFileName);
            //pFeatureLayer = new FeatureLayer();
            //pFeatureLayer.FeatureClass = pFeatureClass;
            //pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
            ////ClearAllData();//删除所有已加载的数据
            //axMapControl2.Map.AddLayer(pFeatureLayer);
        }

        public string OpenMxd()
        {
            string MxdPath = "";
            OpenFileDialog OpenMXD = new OpenFileDialog();
            OpenMXD.Title = "打开地图";
            OpenMXD.InitialDirectory = "E:";

            OpenMXD.Filter = "Map Documents (*.mxd)|*.mxd";
            if (OpenMXD.ShowDialog() == DialogResult.OK)
            {
                MxdPath = OpenMXD.FileName;
            }
            return MxdPath;
        }

        private void axLicenseControl1_Enter(object sender, EventArgs e)
        {

        }
    }
}
