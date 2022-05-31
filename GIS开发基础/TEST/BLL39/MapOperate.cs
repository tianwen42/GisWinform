using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;


namespace TEST.BLL
{
    class MapOperate
    {
        public void queryLayerByAttribute()
        {
            MessageBox.Show("还没做完呢！");
        }

        public void enLargeWindow()
        {

        }


        /// <summary>
        /// 选框缩小窗口
        /// </summary>
        public void conTractWindow(AxMapControl e)
        {
            // 获取到拉框的范围
            IEnvelope pEnvelopRec = e.TrackRectangle();
            //获取到视窗的范围
            IEnvelope pEnvelopActiveView = e.Extent;
            //计算缩小后的窗口的宽度
            double dWidth = pEnvelopActiveView.Width * pEnvelopActiveView.Width / pEnvelopRec.Width;
            //计算缩小后的窗口的高度
            double dHeight = pEnvelopActiveView.Height * pEnvelopActiveView.Height / pEnvelopRec.Height;

            //计算中心点
            double recCenter_X = (pEnvelopRec.XMax + pEnvelopRec.XMin) / 2;
            double recCenter_Y = (pEnvelopRec.YMax + pEnvelopRec.YMin) / 2;

            //计算出缩小后包络线的最大最小XY值
            double xMin = recCenter_X - dWidth / 2;
            double xMax = recCenter_X + dWidth / 2;
            double yMin = recCenter_Y - dHeight / 2;
            double yMax = recCenter_Y + dHeight / 2;

            //根据最大最小XY值生成的包络线
            IEnvelope pEnvelopResult = new EnvelopeClass();//(将引用中的Geometry互操作模式设置成Falese) 
            pEnvelopResult.PutCoords(xMin, yMin, xMax, yMax);

            e.Extent = pEnvelopResult;          //拉框的范围赋值给地图缩放范围
            e.ActiveView.Refresh();
        }


    }
}
