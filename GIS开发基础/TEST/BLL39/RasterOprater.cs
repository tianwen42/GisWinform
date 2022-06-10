using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST.BLL39
{
    class RasterOprater
    {

        public void IDW()
        {
            double cellsize = 0.1; ////设置越小效率越低

            IRasterLayer rl = new RasterLayerClass();



            string idwpath = Application.StartupPath + "\\IDW";



            Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();

            ESRI.ArcGIS.SpatialAnalystTools.Idw pIDW = new ESRI.ArcGIS.SpatialAnalystTools.Idw(pFeaLyr, "Z", idwpath);

            pIDW.in_barrier_polyline_features = pFeaLyrShp;

            pIDW.power = 2;



            pIDW.cell_size = cellsize;

            IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(pIDW, null);



            rl.CreateFromFilePath(idwpath);
        }
        
    }
}
