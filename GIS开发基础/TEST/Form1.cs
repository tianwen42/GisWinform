using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

using TEST.BLL40;
using TEST.MODEL;


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

        //测试
        private void button1_Click(object sender, EventArgs e)
        {
            MapOperate op = new MapOperate();
            op.queryLayerByAttribute();
        }

        #region 加载这块
        //打开mxd
        private void openMxd_Click(object sender, EventArgs e)
        {
            OpenFeatureClass op = new OpenFeatureClass();
            op.openMxd(axMapControl1, axTOCControl1);
        }

        //打开shp
        private void openShp_Click(object sender, EventArgs e)
        {
            OpenFeatureClass op = new OpenFeatureClass();
            op.openShp(axMapControl1, axTOCControl1);
        }

        //加载gdb
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFeatureClass op = new OpenFeatureClass();
            op.loadGDataBase(axMapControl1, axTOCControl1);
        }

        //加载mdb
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFeatureClass op = new OpenFeatureClass();
            op.loadMdb(axMapControl1, axTOCControl1);
        }

        #endregion

        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {

        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {

        }

        #region 操作这块
        //缩小
        private void button5_Click(object sender, EventArgs e)
        {
            flag = 2;
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
        }

        //放大
        private void button4_Click(object sender, EventArgs e)
        {
            flag = 1;
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
        }

        //平移
        private void button6_Click(object sender, EventArgs e)
        {
            flag = 3;
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
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


        //取消操作状态
        private void button7_Click(object sender, EventArgs e)
        {
            flag = 0;//地图浏览设置为取消地图浏览 
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
        }

        //点击缩小
        private void button9_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;     //设置地图窗口的鼠标样式
            IEnvelope pEnvelope;                   //定义包络线为地图外围四个顶点的xy值，放大缩小后面为其内部范围；获取当前地图控件地图窗口的范围，并转为包络线
            pEnvelope = axMapControl1.Extent;      //获取Envelope后，对这个包络线进行放大操作
            pEnvelope.Expand(1.5, 1.5, true);      //这里设置放大为2倍，可以根据需要具体设置，true是按照比例值放大，false就是按照坐标值来放大；（放大其实就是包络线范围缩小，所以此处为0.5）
            axMapControl1.Extent = pEnvelope;      //将新生成的包络线赋值给地图的范围，形成新的范围
            axMapControl1.ActiveView.Refresh();
        }

        //点击放大
        private void 点击放大_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;      //设置当前地图控件窗口工具为空
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;     //设置地图窗口的鼠标样式
            IEnvelope pEnvelope;                   //定义包络线为地图外围四个顶点的xy值，放大缩小后面为其内部范围；获取当前地图控件地图窗口的范围，并转为包络线
            pEnvelope = axMapControl1.Extent;      //获取Envelope后，对这个包络线进行放大操作
            pEnvelope.Expand(0.5, 0.5, true);      //这里设置放大为2倍，可以根据需要具体设置，true是按照比例值放大，false就是按照坐标值来放大；（放大其实就是包络线范围缩小，所以此处为0.5）
            axMapControl1.Extent = pEnvelope;      //将新生成的包络线赋值给地图的范围，形成新的范围
            axMapControl1.ActiveView.Refresh();
        }

        //ZOOM IN
        private void button8_Click(object sender, EventArgs e)
        {
            axMapControl1.Extent = axMapControl1.FullExtent;
        }
        #endregion

        #region 查询
        //属性查
        private void button10_Click(object sender, EventArgs e)
        {
            //新创建属性查询窗体
            frmQueryByAttribute frmQueryByAttribute = new frmQueryByAttribute();
            //将当前主窗体中MapControl控件中的Map对象赋值给frmQueryByAttribute窗体的CurrentMap属性
            frmQueryByAttribute.CurrentMap = axMapControl1.Map;
            //显示属性查询窗体
            frmQueryByAttribute.Show();
        }


        //空间查
        private void button11_Click(object sender, EventArgs e)
        {
            //新创建空间查询窗体
            frmQueryBySpatial frmQueryBySpatial = new frmQueryBySpatial();
            //将当前主窗体中MapControl控件中的Map对象赋值给frmQueryBySpatial窗体的CurrentMap属性
            frmQueryBySpatial.CurrentMap = axMapControl1.Map;
            //显示空间查询窗体
            frmQueryBySpatial.Show();
        }

        #endregion

        #region 网络分析这块
        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("假装做了！");
        }
        #endregion

        #region 符号化
        private void button13_Click(object sender, EventArgs e)
        {
            MessageBox.Show("KKKK");
        }
        #endregion
    }
}
