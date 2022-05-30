using System;

using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

using ESRI.ArcGIS.Geodatabase;

using TEST.BLL;
using TEST.窗口;
using TEST.窗体;



namespace TEST
{
    public partial class Form1 : Form
    {
        #region 全局变量
        int flag;  //功能键


        #endregion

        public Form1()
        {
            InitializeComponent();
        }



        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {

        }
      

        // 地图点击
        private void axMapControl1_OnMouseDown_1(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            //MessageBox.Show("测试点击");
            // 放大
            if (flag == 1)
            {
                axMapControl1.Extent = axMapControl1.TrackRectangle();
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
            // 缩小
            else if (flag == 2)
            {
                MapOperate op = new MapOperate();
                op.conTractWindow(axMapControl1);
            }

            // 移动
            else if (flag == 3)
            {
                axMapControl1.Pan();
            }
        }



        #region 网络分析这块
        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("假装做了！");
        }
        #endregion

        #region 符号化
        private void button13_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("KKKK");
            try
            {
                //输入唯一值符号化的图层（我这里默认第一个图层【高耦合】）
                IFeatureLayer pFeatLyr = axMapControl1.get_Layer(0) as IFeatureLayer;
                //输入唯一值符号化的字段（我这里默认FID字段【高耦合】）
                string sFieldName = "FID";

                IGeoFeatureLayer pGeoFeatLyr = pFeatLyr as IGeoFeatureLayer;
                ITable pTable = pFeatLyr as ITable;
                IUniqueValueRenderer pUniqueValueRender = new UniqueValueRenderer();

                int intFieldNumber = pTable.FindField(sFieldName);
                pUniqueValueRender.FieldCount = 1;//设置唯一值符号化的关键字段为一个
                pUniqueValueRender.set_Field(0, sFieldName);//设置唯一值符号化的第一个关键字段

                IRandomColorRamp pRandColorRamp = new RandomColorRamp();
                pRandColorRamp.StartHue = 0;
                pRandColorRamp.MinValue = 0;
                pRandColorRamp.MinSaturation = 15;
                pRandColorRamp.EndHue = 360;
                pRandColorRamp.MaxValue = 100;
                pRandColorRamp.MaxSaturation = 30;
                //根据渲染字段的值的个数，设置一组随机颜色，如某一字段有5个值，则创建5个随机颜色与之匹配
                IQueryFilter pQueryFilter = new QueryFilter();
                pRandColorRamp.Size = pFeatLyr.FeatureClass.FeatureCount(pQueryFilter);
                bool bSuccess = false;
                pRandColorRamp.CreateRamp(out bSuccess);

                IEnumColors pEnumRamp = pRandColorRamp.Colors;
                IColor pNextUniqueColor = null;
                //查询字段的值
                pQueryFilter = new QueryFilter();
                pQueryFilter.AddField(sFieldName);
                ICursor pCursor = pTable.Search(pQueryFilter, true);
                IRow pNextRow = pCursor.NextRow();
                object codeValue = null;
                IRowBuffer pNextRowBuffer = null;

                //遍历要素类中的所有要素
                while (pNextRow != null)
                {
                    pNextRowBuffer = pNextRow as IRowBuffer;
                    //获取渲染字段的每一个值
                    codeValue = pNextRowBuffer.get_Value(intFieldNumber);

                    pNextUniqueColor = pEnumRamp.Next();
                    if (pNextUniqueColor == null)
                    {
                        pEnumRamp.Reset();
                        pNextUniqueColor = pEnumRamp.Next();
                    }
                    IFillSymbol pFillSymbol = null;
                    ILineSymbol pLineSymbol;
                    IMarkerSymbol pMarkerSymbol;
                    switch (pGeoFeatLyr.FeatureClass.ShapeType)
                    {
                        //如何该要素类 类型为面状要素，就创建面状填充符号
                        case esriGeometryType.esriGeometryPolygon:
                            {
                                pFillSymbol = new SimpleFillSymbol();
                                pFillSymbol.Color = pNextUniqueColor;
                                pUniqueValueRender.AddValue(codeValue.ToString(), "", pFillSymbol as ISymbol);//添加渲染字段的值和渲染样式
                                pNextRow = pCursor.NextRow();
                                break;
                            }
                        //如何该要素类 类型为线状要素，就创建简单线符号
                        case esriGeometryType.esriGeometryPolyline:
                            {
                                pLineSymbol = new SimpleLineSymbol();
                                pLineSymbol.Color = pNextUniqueColor;
                                pUniqueValueRender.AddValue(codeValue.ToString(), "", pLineSymbol as ISymbol);//添加渲染字段的值和渲染样式
                                pNextRow = pCursor.NextRow();
                                break;
                            }
                        //如何该要素类 类型为点状要素，就创建简单标记符号
                        case esriGeometryType.esriGeometryPoint:
                            {
                                pMarkerSymbol = new SimpleMarkerSymbol();
                                pMarkerSymbol.Color = pNextUniqueColor;
                                pUniqueValueRender.AddValue(codeValue.ToString(), "", pMarkerSymbol as ISymbol);//添加渲染字段的值和渲染样式
                                pNextRow = pCursor.NextRow();
                                break;
                            }
                    }
                }
                //渲染并刷新地图窗口与目录栏窗口
                pGeoFeatLyr.Renderer = pUniqueValueRender as IFeatureRenderer;
                axMapControl1.Refresh();
                axTOCControl1.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show("该字段不存在！");
            }
        }
        #endregion





        private void button20_Click(object sender, EventArgs e)
        {
            //1、准备的第一个参数输入图层要素类
            IFeatureLayer inputFeaturLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
            IFeatureClass inputFeatureClass = inputFeaturLayer.FeatureClass;
            //2、准备的第二个参数剪切图层要素类
            IFeatureLayer clipFeaturLayer = axMapControl1.get_Layer(1) as IFeatureLayer;
            IFeatureClass clipFeatureClass = clipFeaturLayer.FeatureClass;
            //3、准备容差
            double tor = 0.01;
            //4、输出位置（IFeatureClassname)
            IFeatureClassName pOutPut = new FeatureClassNameClass();
            pOutPut.ShapeType = inputFeatureClass.ShapeType;
            pOutPut.FeatureType = esriFeatureType.esriFTSimple;
            pOutPut.ShapeFieldName = inputFeatureClass.ShapeFieldName;

            //获取shapeFile数据工作空间
            IWorkspaceName pWsN = new WorkspaceNameClass();
            pWsN.WorkspaceFactoryProgID = "esriDataSourcesFile.ShapefileWorkspaceFactory";

            pWsN.PathName = "C:\\Temp";
            //通过IDatasetName设置输出结果相关参数
            IDatasetName pDatasetName = pOutPut as IDatasetName;
            pDatasetName.Name = "结果";
            pDatasetName.WorkspaceName = pWsN;

            //5、执行
            IBasicGeoprocessor pBasicGeo = new BasicGeoprocessorClass();
            pBasicGeo.SpatialReference = axMapControl1.SpatialReference;
            //pBasicGeo.Clip(输入图层的要素类、剪切图层的要素类、容差、输出位置）
            IFeatureClass result = pBasicGeo.Clip(inputFeatureClass as ITable, false, clipFeatureClass as ITable, false, tor, pOutPut);
            //pBasicGeo.Dissolve(

            //6、将结果加载到map
            IFeatureLayer pFeatueLayer = new FeatureLayerClass();
            pFeatueLayer.FeatureClass = result;
            pFeatueLayer.Name = result.AliasName;

            axMapControl1.AddLayer(pFeatueLayer);
            axMapControl1.Refresh();
        }

        private void 打开mxdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFeatureClass op = new OpenFeatureClass();
            op.openMxd(axMapControl1, axTOCControl1);
        }

        private void 导入shpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFeatureClass op = new OpenFeatureClass();
            op.openShp(axMapControl1, axTOCControl1);
        }

        private void 导入mdbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFeatureClass op = new OpenFeatureClass();
            op.loadMdb(axMapControl1, axTOCControl1);
        }

        private void 导入gdbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFeatureClass op = new OpenFeatureClass();
            op.loadGDataBase(axMapControl1, axTOCControl1);
        }

        private void 空间查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新创建空间查询窗体
            frmQueryBySpatial frmQueryBySpatial = new frmQueryBySpatial();
            //将当前主窗体中MapControl控件中的Map对象赋值给frmQueryBySpatial窗体的CurrentMap属性
            frmQueryBySpatial.CurrentMap = axMapControl1.Map;
            //显示空间查询窗体
            frmQueryBySpatial.Show();
        }

        private void 属性查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新创建属性查询窗体
            frmQueryByAttribute frmQueryByAttribute = new frmQueryByAttribute();
            //将当前主窗体中MapControl控件中的Map对象赋值给frmQueryByAttribute窗体的CurrentMap属性
            frmQueryByAttribute.CurrentMap = axMapControl1.Map;
            //显示属性查询窗体
            frmQueryByAttribute.Show();
        }

        private void 查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新创建属性查询窗体
            gugu frmQueryByAttribute = new gugu();
            //显示属性查询窗体
            frmQueryByAttribute.Show();
        }

        private void 融合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新创建属性查询窗体
            gugu frmQueryByAttribute = new gugu();
            //显示属性查询窗体
            frmQueryByAttribute.Show();
        }

        private void shp转gdb要素ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("不好意思，我鸽了，咕咕！");
        }

        private void 缩放ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 点击放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;     //设置地图窗口的鼠标样式
            IEnvelope pEnvelope;                   //定义包络线为地图外围四个顶点的xy值，放大缩小后面为其内部范围；获取当前地图控件地图窗口的范围，并转为包络线
            pEnvelope = axMapControl1.Extent;      //获取Envelope后，对这个包络线进行放大操作
            pEnvelope.Expand(0.5, 0.5, true);      //这里设置放大为2倍，可以根据需要具体设置，true是按照比例值放大，false就是按照坐标值来放大；（放大其实就是包络线范围缩小，所以此处为0.5）
            axMapControl1.Extent = pEnvelope;      //将新生成的包络线赋值给地图的范围，形成新的范围
            axMapControl1.ActiveView.Refresh();
        }

        private void 选框缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = 2;
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
        }

        private void 图层居中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.Extent = axMapControl1.FullExtent;
        }

        private void 选框放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = 1;
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
        }

        private void panToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = 3;
            axMapControl1.CurrentTool = null;
        }

        private void 停止操作状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = 0;//地图浏览设置为取消地图浏览 
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
        }

        private void 点击缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;     //设置地图窗口的鼠标样式
            IEnvelope pEnvelope;                   //定义包络线为地图外围四个顶点的xy值，放大缩小后面为其内部范围；获取当前地图控件地图窗口的范围，并转为包络线
            pEnvelope = axMapControl1.Extent;      //获取Envelope后，对这个包络线进行放大操作
            pEnvelope.Expand(1.5, 1.5, true);      //这里设置放大为2倍，可以根据需要具体设置，true是按照比例值放大，false就是按照坐标值来放大；（放大其实就是包络线范围缩小，所以此处为0.5）
            axMapControl1.Extent = pEnvelope;      //将新生成的包络线赋值给地图的范围，形成新的范围
            axMapControl1.ActiveView.Refresh();
        }

        private void 点符号化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新创建属性查询窗体
            gugu frmQueryByAttribute = new gugu();
            //显示属性查询窗体
            frmQueryByAttribute.Show();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新创建属性查询窗体
            gugu unDoStatement = new gugu();
            //显示属性查询窗体
            unDoStatement.Show();
        }

        private void xY转点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //EXCEL转点
            xyToPoint xyTopointForm = new xyToPoint();
            xyTopointForm.Show();
        }
    }
}
