using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace TEST.MODEL
{
    class OpenFeatureClass
    {
        public void openShp(AxMapControl e, AxTOCControl a)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "shp图层(*.shp)|*.shp";
            op.Title = "打开shp图层";
            op.RestoreDirectory = true;
            if (op.ShowDialog() == DialogResult.OK)
            {
                string pFilePath = op.FileName;
                IMapDocument pm = new MapDocument();
                pm.Open(pFilePath, "");
                e.Map = pm.ActiveView.FocusMap;
                e.ActiveView.Refresh();
            }
        }

        public void openMxd(AxMapControl e, AxTOCControl a)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "打开地图文档";
            op.Filter = "地图文档 (*.mxd)|*.mxd";
            op.RestoreDirectory = true;
            op.ShowDialog();
            string sFilePath = op.FileName;
            if (e.CheckMxFile(sFilePath))
            {
                e.MousePointer = esriControlsMousePointer.esriPointerHourglass;
                e.LoadMxFile(sFilePath, 0, Type.Missing);
                e.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
        }
    }
}
