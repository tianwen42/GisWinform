using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;


namespace TEST.BLL
{
    class OpenFeatureClass
    {
        /// <summary>
        /// Open SHP Document
        /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
        public void openShp(AxMapControl e, AxTOCControl a)
        {
            //OpenFileDialog op = new OpenFileDialog();
            //op.Filter = "shp图层(*.shp)|*.shp";
            //op.Title = "";
            //op.RestoreDirectory = true;
            //if (op.ShowDialog() == DialogResult.OK)
            //{
            //    string pFilePath = op.FileName;
            //    MessageBox.Show(pFilePath);
            //    IMapDocument pm = new MapDocument();
            //    pm.Open(pFilePath, "");
            //    e.Map = pm.ActiveView.FocusMap;
            //    e.ActiveView.Refresh();
            //}

            //获取当前路径和文件名
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "打开shp图层";
            dlg.Filter = "shp图层(*.shp)|*.shp";
            dlg.ShowDialog();
            string strFullPath = dlg.FileName;
            //    MessageBox.Show(strFullPath);
            if (strFullPath == "") return;
            int Index = strFullPath.LastIndexOf("\\");
            string filePath = strFullPath.Substring(0, Index);
            string fileName = strFullPath.Substring(Index + 1);
            //打开工作空间并添加shp文件
            IWorkspaceFactory pWorkspaceFactory;
            IFeatureWorkspace pFeatureWorkspace;
            IFeatureLayer pFeatureLayer;
            pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            //注意此处的路径是不能带文件名的
            pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(filePath, 0);
            pFeatureLayer = new FeatureLayerClass();
            //注意这里的文件名是不能带路径的
            pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(fileName);
            pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
            e.Map.AddLayer(pFeatureLayer);
            e.ActiveView.Refresh();
        }

        /// <summary>
        /// openMxd
        /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
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
                e.Refresh();
            }
        }

        /// <summary>
        /// 加载gdb
        /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
        public void loadGDataBase(AxMapControl e, AxTOCControl a)
        {
            IWorkspaceFactory pFileGDBWorkspaceFactory;

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.OK) return;
            string pFullPath = dlg.SelectedPath;

            if (pFullPath == "")
            {
                return;
            }
            pFileGDBWorkspaceFactory = new FileGDBWorkspaceFactoryClass(); //using ESRI.ArcGIS.DataSourcesGDB;

            ClearAllData(e);    //新增删除数据

            //获取工作空间
            IWorkspace pWorkspace = pFileGDBWorkspaceFactory.OpenFromFile(pFullPath, 0);
            AddAllDataset(pWorkspace, e);
        }

        /// <summary>
        /// CleadAllData
        /// </summary>
        private void ClearAllData(AxMapControl e)
        {
            if (e.Map != null && e.Map.LayerCount > 0)
            {
                //新建e中Map
                IMap dataMap = new MapClass();
                dataMap.Name = "Map";
                e.DocumentFilename = string.Empty;
                e.Map = dataMap;
            }
        }

        /// <summary>
        /// AddAllDataset
        /// </summary>
        /// <param name="pWorkspace"></param>
        /// <param name="mapControl"></param>
        private void AddAllDataset(IWorkspace pWorkspace, AxMapControl mapControl)
        {
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTAny);
            pEnumDataset.Reset();
            //将Enum数据集中的数据一个个读到DataSet中
            IDataset pDataset = pEnumDataset.Next();
            //判断数据集是否有数据
            while (pDataset != null)
            {
                if (pDataset is IFeatureDataset)  //是否为要素数据集
                {
                    IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                    IFeatureDataset pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(pDataset.Name);
                    IEnumDataset pEnumDataset1 = pFeatureDataset.Subsets;
                    pEnumDataset1.Reset();
                    IGroupLayer pGroupLayer = new GroupLayerClass();
                    pGroupLayer.Name = pFeatureDataset.Name;
                    IDataset pDataset1 = pEnumDataset1.Next();
                    while (pDataset1 != null)
                    {
                        if (pDataset1 is IFeatureClass)  //是否为要素类
                        {
                            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                            pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset1.Name);
                            if (pFeatureLayer.FeatureClass != null)
                            {
                                pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                                pGroupLayer.Add(pFeatureLayer);
                                mapControl.Map.AddLayer(pFeatureLayer);
                            }
                        }
                        pDataset1 = pEnumDataset1.Next();
                    }
                }
                else if (pDataset is IFeatureClass) //要素类
                {
                    IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset.Name);

                    pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                    mapControl.Map.AddLayer(pFeatureLayer);
                }
                else if (pDataset is IRasterDataset) //是否为栅格数据集
                {
                    IRasterWorkspaceEx pRasterWorkspace = (IRasterWorkspaceEx)pWorkspace;
                    IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(pDataset.Name);
                    //影像金字塔判断与创建
                    IRasterPyramid3 pRasPyrmid;
                    pRasPyrmid = pRasterDataset as IRasterPyramid3;
                    if (pRasPyrmid != null)
                    {
                        if (!(pRasPyrmid.Present))
                        {
                            pRasPyrmid.Create(); //创建金字塔
                        }
                    }
                    IRasterLayer pRasterLayer = new RasterLayerClass();
                    pRasterLayer.CreateFromDataset(pRasterDataset);
                    ILayer pLayer = pRasterLayer as ILayer;
                    mapControl.AddLayer(pLayer, 0);
                }
                pDataset = pEnumDataset.Next();
            }
        }

        /// <summary>
        /// 加载mdb
        /// </summary>
        public void loadMdb(AxMapControl e, AxTOCControl a)
        {
            IWorkspaceFactory pAccessWorkspaceFactory;

            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.Filter = "Personal Geodatabase(*.mdb)|*.mdb";
            pOpenFileDialog.Title = "打开PersonGeodatabase文件";
            pOpenFileDialog.ShowDialog();

            string pFullPath = pOpenFileDialog.FileName;
            if (pFullPath == "")
            {
                return;
            }
            pAccessWorkspaceFactory = new AccessWorkspaceFactory(); //using ESRI.ArcGIS.DataSourcesGDB;
            //获取工作空间
            IWorkspace pWorkspace = pAccessWorkspaceFactory.OpenFromFile(pFullPath, 0);

            ClearAllData(e);    //新增删除数据

            //加载工作空间里的数据
            AddAllDataset(pWorkspace, e);
        }
        
    }


}
