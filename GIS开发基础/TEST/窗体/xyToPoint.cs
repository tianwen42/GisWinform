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

namespace TEST.窗体
{
    public partial class xyToPoint : Form
    {
        DataSet resDataSet;
        private AxMapControl AxMap;
        private List<MODEL.Point.PointXY> xyList = new List<MODEL.Point.PointXY>();
        string excelFilepath;



        public xyToPoint(AxMapControl Map)
        {
            InitializeComponent();
            AxMap = Map;
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


            // 创建要素类
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath), 0);
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pFeatureClass = pFeatureWorkspace.CreateFeatureClass(System.IO.Path.GetFileName(filePath), pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
            return pFeatureClass;
        }

        /// <summary>
        /// 插入点数据
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <param name="filePath"></param>
        private void InsertFeatures(IFeatureClass pFeatureClass, string filePath)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {

                int index_X ;
                int index_Y ;
                // 要素游标
                IFeatureBuffer pFeatureBuffer = pFeatureClass.CreateFeatureBuffer();
                IFeatureCursor pFeatureCursor = pFeatureClass.Insert(true);


                // 字段值
                string fieldValue_X = string.Empty;
                string fieldValue_Y = string.Empty;

                if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
                {
                    MessageBox.Show("选择xy字段");
                }
                else
                {
                    fieldValue_X = comboBox1.SelectedItem.ToString();
                    fieldValue_Y = comboBox2.SelectedItem.ToString();
                    //字段索引
                    index_X = resDataSet.Tables[0].Columns.IndexOf(fieldValue_X);
                    index_Y = resDataSet.Tables[0].Columns.IndexOf(fieldValue_Y);

                    GenerateFeature gf = new GenerateFeature();


                    int rows = resDataSet.Tables[0].Rows.Count;
                    // 遍历excel数据行
                    for (int i = 0; i < rows; i++)
                    {
                        string strValue = resDataSet.Tables[0].Rows[i][index_X].ToString();
                        // 读取单元格
                        fieldValue_X = resDataSet.Tables[0].Rows[i][index_X].ToString();
                        fieldValue_Y = resDataSet.Tables[0].Rows[i][index_Y].ToString();

                        // 创建坐标点
                        IPoint pPoint = new Point();
                        pPoint.X = double.Parse(fieldValue_X);
                        pPoint.Y = double.Parse(fieldValue_Y);

                        // 设置字段值
                        pFeatureBuffer.Shape = pPoint;
                        pFeatureBuffer.set_Value(2, fieldValue_X);
                        pFeatureBuffer.set_Value(3, fieldValue_Y);
                        pFeatureCursor.InsertFeature(pFeatureBuffer);
                    }
                    pFeatureCursor.Flush();

                    // 释放游标
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureBuffer);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                }
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (resDataSet != null)
            {
                try
                {
                    DirectoryInfo topDir = Directory.GetParent(Environment.CurrentDirectory);
                    string filepath = topDir.Parent.Parent.Parent.FullName;
                    filepath += "\\Temp\\point.shp";
                    while (File.Exists(filepath))
                    {
                        filepath = fileReName(filepath);
                    }
                    
                    // 创建要素类
                    IFeatureClass pFeatureClass = CreateFeatureClass(filepath);
                    // 插入要素
                    InsertFeatures(pFeatureClass, excelFilepath);
                    MessageBox.Show("excel转点成功");

                    //插入
                    importShp(AxMap,filepath);

                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("执行有问题，看看数据\n" + ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("请先输入数据");
            }

        }

        void importShp(AxMapControl e,string strFullPath)
        {
            int Index = strFullPath.LastIndexOf("\\");
            string filePath = strFullPath.Substring(0, Index);
            string fileName = strFullPath.Substring(Index + 1);
            //打开工作空间并添加shp文件
            IWorkspaceFactory pWorkspaceFactory;
            IFeatureWorkspace pFeatureWorkspace;
            IFeatureLayer pFeatureLayer;
            pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            //注意此处的路径是不能带文件名的
            pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(filePath, 0);
            pFeatureLayer = new FeatureLayerClass();
            //注意这里的文件名是不能带路径的
            pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(fileName);
            pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
            e.Map.AddLayer(pFeatureLayer);
            e.ActiveView.Refresh();
        }





        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Excel数据|*.xls;*.xlsx";
            if (op.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            excelFilepath = op.FileName;

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
