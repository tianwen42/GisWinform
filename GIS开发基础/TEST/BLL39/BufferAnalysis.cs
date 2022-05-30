using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST.BLL39
{
    class BufferAnalysis
    {
        private IActiveView pActiveView;
        private AxMapControl mapControl;

        //通过构造函数获取MapControl控件
        public BufferAnalysis(AxMapControl mainmapControl): this()
        {
            mapControl = mainmapControl;
            pActiveView = mainmapControl.ActiveView;
        }

        public BufferAnalysis()
        {
        }

        //输出路径
        private void button1_OutputPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "Shapefile(*.shp)|*.shp";
            if (saveDlg.ShowDialog() != DialogResult.OK) return;

            textBox2_OutputPath.Text = saveDlg.FileName;
        }

        //生成窗体函数
        private void BufferAnalysis_Load(object sender, EventArgs e)
        {
            if (mapControl == null || pActiveView.FocusMap.LayerCount == 0)
            {
                return;
            }
            IEnumLayer layers = pActiveView.FocusMap.get_Layers();
            layers.Reset();
            ILayer layer = layers.Next();
            while (layer != null)
            {
                comboBox1_ChooseLayer.Items.Add(layer.Name);
                layer = layers.Next();
            }
        }

        //确认分析
        private void button2_OK_Click(object sender, EventArgs e)
        {
            double bufferDistance = Convert.ToDouble(textBox1_BufferDistance.Text.Trim());
            if (bufferDistance == 0.0)
            {
                MessageBox.Show("缓冲区距离有误！");
                return;
            }

            if (comboBox1_ChooseLayer.Text == string.Empty)
            {
                MessageBox.Show("输入图层不能为空！");
                return;
            }

            if (textBox2_OutputPath.Text == string.Empty)
            {
                MessageBox.Show("输出路径不能为空！");
                return;
            }
            int index = comboBox1_ChooseLayer.SelectedIndex;
            string name = getLayerPath(pActiveView.FocusMap.get_Layer(index));
            string outPath = textBox2_OutputPath.Text.Trim();

            Geoprocessor pGp = new Geoprocessor();
            pGp.OverwriteOutput = true; //允许运算结果覆盖现有文件，可无
            ESRI.ArcGIS.AnalysisTools.Buffer pBuffer = new ESRI.ArcGIS.AnalysisTools.Buffer();
            //获取缓冲区分析图层
            ILayer pLayer = pActiveView.FocusMap.get_Layer(index);
            IFeatureLayer featLayer = pLayer as IFeatureLayer;
            //IFeatureCursor cursor = featLayer.Search(null, false);
            //IFeature feaClass = cursor.NextFeature();

            pBuffer.in_features = featLayer;

            pBuffer.out_feature_class = outPath; //输出路径
            pBuffer.buffer_distance_or_field = bufferDistance; //缓冲区参数
            pBuffer.dissolve_option = "NONE"; //融合缓冲区重叠交叉部分，如果不融合填"ALL"
            pGp.Execute(pBuffer, null); //执行

            string pFolder = System.IO.Path.GetDirectoryName(outPath); //得到字符串中文件夹位置
            string pFileName = System.IO.Path.GetFileName(outPath); //得到字符串中文件名字
            mapControl.AddShapeFile(pFolder, pFileName); //往地图控件里添加文件
            mapControl.ActiveView.Refresh(); //激活窗口刷新
            this.Close();
        }

        //获取图层源路径
        private string getLayerPath(ILayer pLayer)
        {
            IDatasetName pDatasetName = (pLayer as IDataLayer2).DataSourceName as IDatasetName;
            IWorkspaceName pWorkspaceName = pDatasetName.WorkspaceName;
            return pWorkspaceName.PathName + "\\" + pLayer.Name + ".shp";
        }
    }
}
