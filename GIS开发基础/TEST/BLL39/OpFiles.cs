using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST.BLL39
{
    class OpFiles
    {
        public static string fileReName(string filepath)
        {
            int i = 1;
            while (File.Exists(filepath))
            {
                filepath = filepath.Split('.')[0] + i.ToString() + ".shp";
                i++;
                if (File.Exists(filepath))
                {
                    break;
                }
            }
            return filepath;
        }
    }
}
