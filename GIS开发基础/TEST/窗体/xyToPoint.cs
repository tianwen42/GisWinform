using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using TEST.BLL39;
using TEST.MODEL;
using static TEST.MODEL.Point;

namespace TEST.窗体
{
    public partial class xyToPoint : Form
    {
        DataSet resDataSet;
        private AxMapControl AxMap;
        private List<MODEL.Point.PointXY> xyList = new List<MODEL.Point.PointXY>();



        public xyToPoint(AxMapControl AxMap)
        {
            InitializeComponent();
            AxMap = AxMap;
        }

        private ISpatialReference CreateSpatialReference()
        {
            ISpatialReferenceFactory pSpatialReferenceFactory = new SpatialReferenceEnvironment();
            ISpatialReference pSpatialReference = pSpatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            return pSpatialReference;
        }

        private IFeatureClass CreateFeatureClass(string filePath)
        {
            IGeometryDef pGeometryDef = new GeometryDef();
            IGeometryDefEdit pGeometryDefEdit = pGeometryDef as IGeometryDefEdit;
            pGeometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
            pGeometryDefEdit.HasM_2 = false;
            pGeometryDefEdit.HasZ_2 = false;
            pGeometryDefEdit.SpatialReference_2 = CreateSpatialReference();

            // 字段集合
            IFields pFields = new Fields();
            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

            // Shape
            IField pField = new Field();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            pFieldEdit.GeometryDef_2 = pGeometryDef;
            pFieldEdit.AliasName_2 = "Shape";
            pFieldEdit.Name_2 = "Shape";
            pFieldEdit.IsNullable_2 = false;
            pFieldEdit.Required_2 = true;
            pFieldsEdit.AddField(pField);

            // 经度
            pField = new Field();
            pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.AliasName_2 = "经度";
            pFieldEdit.Name_2 = "经度";
            pFieldsEdit.AddField(pField);

            // 纬度
            pField = new Field();
            pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.AliasName_2 = "纬度";
            pFieldEdit.Name_2 = "纬度";
            pFieldsEdit.AddField(pField);

            // 名称
            pField = new Field();
            pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.AliasName_2 = "名称";
            pFieldEdit.Name_2 = "名称";
            pFieldsEdit.AddField(pField);

            // 创建要素类
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath), 0);
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pFeatureClass = pFeatureWorkspace.CreateFeatureClass(System.IO.Path.GetFileName(filePath), pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
            return pFeatureClass;
        }


        public void button1_Click(object sender, EventArgs e)
        {










        // 字段值
            string fieldValue_X = string.Empty;
            string fieldValue_Y = string.Empty;
            string fieldValue_Z = string.Empty;

            

            fieldValue_X = comboBox1.SelectedItem.ToString();
            fieldValue_Y = comboBox2.SelectedItem.ToString();
            //fieldValue_Z = row.Cells[2].ToString();



            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
            GenerateFeature gFeature = new GenerateFeature();

            int FID =1;
            try{
                foreach (DataRow dr in resDataSet.Tables[0].Rows)
                {
                    MODEL.Point.PointXY pPoint = new MODEL.Point.PointXY();
                    pPoint.dX = double.Parse(dr[fieldValue_X].ToString());
                    pPoint.dY = Math.Round(double.Parse(dr[fieldValue_Y].ToString()), 2); 
                    //pPoint.FID = FID;
                    FID+=1;
                    IGeometry pGeometry = pPoint as IGeometry;
                    
                    xyList.Add(pPoint);
                }
                bool isSuccessful = AddPointsToLayer(pFeatureLayer, xyList);
                if (isSuccessful)
                {
                    AxMap.Map.AddLayer(pFeatureLayer);
                    AxMap.Refresh();
                    MessageBox.Show("ok");
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("执行有问题，看看数据\n"+ex.ToString());
            }
            
        }

        public bool AddPointsToLayer(ILayer pLayer, List<PointXY> pointCol)
        {
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            if (pFeatureLayer == null)
            {
                MessageBox.Show(pLayer.Name + "不是矢量图层!");
                return false;
            }
            //
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            if (pFeatureClass.ShapeType != esriGeometryType.esriGeometryPoint)
            {
                System.Windows.Forms.MessageBox.Show(pLayer.Name + "不是点图层!");
                return false;
            }
            //
            IFeatureCursor pFeatureCursor = pFeatureClass.Insert(true);
            IFeatureBuffer pFeatureBuffer = null;
            foreach (PointXY one in pointCol)
            {
                pFeatureBuffer = pFeatureClass.CreateFeatureBuffer();
                IFeature pNewFeature = pFeatureBuffer as IFeature;
                pNewFeature.Shape = CreatePoint(one);
                //
                pFeatureCursor.InsertFeature(pFeatureBuffer);
            }
            pFeatureCursor.Flush();

            return true;
        }


        /// <summary>
        /// 建立 ESRI中的 点类型 并 将其转化为基类接口 IGeometry
        /// </summary>
        /// <param name="point">点坐标 结构体</param>
        /// <returns></returns>
        public IGeometry CreatePoint(PointXY point)
        {
            IPoint pPoint = new PointClass();
            pPoint.X = point.dX;
            pPoint.Y = point.dY;
            IGeometry pGeometry = pPoint as IGeometry;
            return pGeometry;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Excel数据|*.xls;*.xlsx";
            if (op.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            string excelFilepath = op.FileName;

            resDataSet = ReadExcelToDataSet(excelFilepath);
            dataGridView1.DataSource = resDataSet.Tables[0];

            foreach (DataColumn col in resDataSet.Tables[0].Columns)
            {
                //选择项
                comboBox1.Items.Add(col.ColumnName);
                comboBox2.Items.Add(col.ColumnName);
            }

        }

        private DataSet ReadExcelToDataSet(string fileNmaePath)
        {
            FileStream stream = null;
            IExcelDataReader excelReader = null;
            DataSet dataSet = null;
            try
            {
                //stream = File.Open(fileNmaePath, FileMode.Open, FileAccess.Read);
                stream = new FileStream(fileNmaePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch
            {
                return null;
            }
            string extension = System.IO.Path.GetExtension(fileNmaePath);

            if (extension.ToUpper() == ".XLS")
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (extension.ToUpper() == ".XLSX")
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            else
            {
                MessageBox.Show("格式错误");
                return null;

            }
            //dataSet = excelReader.AsDataSet();//第一行当作数据读取
            dataSet = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });//第一行当作列名读取
            excelReader.Close();
            return dataSet;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}
