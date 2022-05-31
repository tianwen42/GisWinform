using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            pFeature =pFeatureClass.CreateFeature();
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
        private void InsertFeatures(IFeatureClass pFeatureClass, IPoint pPoint)
        {
            IFeatureBuffer pFeatureBuffer = pFeatureClass.CreateFeatureBuffer();
            IFeatureCursor pFeatureCursor = pFeatureClass.Insert(true);


            pFeatureCursor.Flush();

            // 释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureBuffer);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
        }

        
    }
}