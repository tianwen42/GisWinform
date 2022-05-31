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

namespace TEST.窗体
{
    public partial class xyToPoint : Form
    {
        DataSet resDataSet;
        private AxMapControl AxMap;
        private List<MODEL.Point.PointXY> xyList = new List<MODEL.Point.PointXY>();
        string excelFilepath;



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
                NPOI.SS.UserModel.IWorkbook workbook = new NPOI.XSSF.UserModel.XSSFWorkbook(fs);
                NPOI.SS.UserModel.ISheet sheet = workbook.GetSheetAt(0);
                NPOI.SS.UserModel.IRow row = null;

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
                }


                // 字段索引
                int fieldIndex_X = pFeatureClass.Fields.FindField(fieldValue_X);
                int fieldIndex_Y = pFeatureClass.Fields.FindField(fieldValue_Y);


                


                GenerateFeature gf = new GenerateFeature();

                // 遍历excel数据行
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);

                    // 读取单元格
                    fieldValue_X = row.Cells[0].ToString();
                    fieldValue_Y = row.Cells[1].ToString();

                    // 创建坐标点
                    ESRI.ArcGIS.Geometry.IPoint pPoint = new ESRI.ArcGIS.Geometry.Point();
                    pPoint.X = double.Parse(fieldValue_X);
                    pPoint.Y = double.Parse(fieldValue_Y);

                    // 设置字段值
                    pFeatureBuffer.Shape = pPoint;
                    pFeatureBuffer.set_Value(fieldIndex_X, fieldValue_X);
                    pFeatureBuffer.set_Value(fieldIndex_Y, fieldValue_Y);
                    pFeatureCursor.InsertFeature(pFeatureBuffer);
                }
                pFeatureCursor.Flush();

                // 释放游标
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureBuffer);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {

            

            
            try{
                // 创建要素类
                IFeatureClass pFeatureClass = CreateFeatureClass(@"C:\Users\Administrator\Desktop\GisWinform\Temp\point.shp");
                // 插入要素
                InsertFeatures(pFeatureClass, excelFilepath);

            }
            catch(Exception ex)
            {
                MessageBox.Show("执行有问题，看看数据\n"+ex.ToString());
            }
            
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
