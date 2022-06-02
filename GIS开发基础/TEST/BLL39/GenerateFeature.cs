using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace TEST.BLL39
{

    /// <summary>
    /// 用于生成点线面等要素
    /// </summary>
    class GenerateFeature
    {
        /// <summary>
        /// 构建解析函数
        /// </summary>
        public GenerateFeature()
        {

        }

        /// <summary>
        /// 生成点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IPoint GeneratePoint(double x, double y)
        {
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(x, y);
            return pPoint;
        }

        public void AddPointToFeatureLayer(IPoint pPoint, IFeatureLayer pFeatureLayer)
        {
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            IFeature pFeature;//添加的一个IFeature实例，用于添加到当前图层上
            IFeatureClassWrite featureWrite = (IFeatureClassWrite)pFeatureClass;
            pFeature = pFeatureClass.CreateFeature();
            pFeature.Shape = pPoint;
            pFeature.Store();
            featureWrite.WriteFeature(pFeature);
        }


        /// <summary>
        /// 创建WGS-84参考
        /// </summary>
        /// <returns></returns>
        private ISpatialReference CreateSpatialReference()
        {
            ISpatialReferenceFactory pSpatialReferenceFactory = new SpatialReferenceEnvironment();
            ISpatialReference pSpatialReference = pSpatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            return pSpatialReference;
        }

        /// <summary>
        /// 创建要素类
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
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

                // 字段索引
                int fieldIndex_X = pFeatureClass.Fields.FindField("经度");
                int fieldIndex_Y = pFeatureClass.Fields.FindField("纬度");

                // 字段值
                string fieldValue_X = string.Empty;
                string fieldValue_Y = string.Empty;

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


    }
}