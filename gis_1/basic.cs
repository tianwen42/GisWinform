using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gis_1
{
    class basic
    {
        public string OpenMxd()
        {
            string MxdPath = "";
            OpenFileDialog OpenMXD = new OpenFileDialog();
            OpenMXD.Title = "打开地图";
            OpenMXD.InitialDirectory = "E:";

            OpenMXD.Filter = "Map Documents (*.mxd)|*.mxd";
            if (OpenMXD.ShowDialog() == DialogResult.OK)
            {
                MxdPath = OpenMXD.FileName;
            }
            return MxdPath;
        }
    }
}
