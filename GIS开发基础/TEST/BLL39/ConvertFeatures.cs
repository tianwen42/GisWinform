using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Windows.Forms;

namespace TEST.BLL39
{
    class ConvertFeatures
    {
        public void shpToGdb()
        {
            string filePath;
            string fileName;
            string fileFolder;

            //shp要素类
            IFeatureClass pSourceFeatureClass;
            //过滤器
            IQueryFilter pQueryFilter;
            // GDB路径 C:\Users\DSF\Desktop\test.gdb
            string directory;
            //要素数据集名称
            string datasetName;
            //要素类名称
            string className;



            // 1设置输入过滤器  （做成界面用户输入）
            pQueryFilter = new QueryFilter();
            pQueryFilter.AddField("FID");
            pQueryFilter.WhereClause = "FID>=15";

            //2获取shp要素类
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "shp图层(*.shp)|*.shp";
            op.Title = "输入待转换的shp";
            op.RestoreDirectory = true;
            if (op.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            filePath = op.FileName;
            fileName = System.IO.Path.GetFileName(filePath).Split('.')[0];
            fileFolder = System.IO.Path.GetDirectoryName(filePath);
            MessageBox.Show(fileName);
            //filePath = @"D:\GisWinform\DATA\shp\new政区图_region.shp";
            pSourceFeatureClass = GetShapefile(filePath);
            //System.Windows.Forms.MessageBox.Show(filePath);


            //3获取GDB路径
            //OpenFileDialog targetGDBop = new OpenFileDialog();
            //targetGDBop.Filter = "gdb(*.gdb)|*.gdb";
            //targetGDBop.Title = "选一个要转入的gdb";
            //targetGDBop.RestoreDirectory = true;
            //if (targetGDBop.ShowDialog() != DialogResult.OK)
            //{
            //    return;
            //}
            //IWorkspaceFactory pFileGDBWorkspaceFactory;

            //FolderBrowserDialog dlg = new FolderBrowserDialog();
            //if (dlg.ShowDialog() != DialogResult.OK) return;
            //directory = dlg.SelectedPath;
            ////directory = targetGDBop.FileName;

            //string gdbName = System.IO.Path.GetFileName(filePath);
            //MessageBox.Show(directory);
            //MessageBox.Show(gdbName);
            //输入要素数据集名称
            datasetName = fileName + "数据集";
            //保存名称
            className = fileName + "_feature";
            //MessageBox.Show(System.IO.Directory.GetCurrentDirectory());


            bool isSuccesful = Import(pSourceFeatureClass, pQueryFilter, @"C:\Users\Administrator\Desktop\GisWinform\DATA\New File Geodatabase.gdb", "dataset", className);

            // 提示用户信息
            if (isSuccesful)
            {
                MessageBox.Show("导入成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("导入失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// 获取shp文件
        /// </summary>
        /// <param name="filePath">shp文件路径</param>
        /// <returns>要素类</returns>
        protected IFeatureClass GetShapefile(string filePath)
        {
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspaceFactoryLockControl pWorkspaceFactoryLockControl = pWorkspaceFactory as IWorkspaceFactoryLockControl;
            if (pWorkspaceFactoryLockControl.SchemaLockingEnabled)
            {
                pWorkspaceFactoryLockControl.DisableSchemaLocking();
            }
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath), 0);
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(System.IO.Path.GetFileName(filePath));
            return pFeatureClass;
        }


        /// <summary>
        /// shp导入数据GDB
        /// </summary>
        /// <param name="pSourceFeatureClass">shp要素类</param>
        /// <param name="pQueryFilter">过滤器</param>
        /// <param name="directory">GDB路径</param>
        /// <param name="datasetName">要素数据集名称</param>
        /// <param name="className">要素类名称</param>
        /// <returns>导入是否成功</returns>
        private bool Import(IFeatureClass pSourceFeatureClass, IQueryFilter pQueryFilter, string directory, string datasetName, string className)
        {
            // 源数据工作空间（SHP）
            IDataset pSourceDataset = pSourceFeatureClass as IDataset;
            IFeatureClassName pSourceFeatureClassName = pSourceDataset.FullName as IFeatureClassName;
            IWorkspace pSourceWorkspace = pSourceDataset.Workspace;

            // 目标数据工作空间（GDB）
            IWorkspaceFactory pTargetWorkspaceFactory = new FileGDBWorkspaceFactory();
            IWorkspace pTargetWorkspace = pTargetWorkspaceFactory.OpenFromFile(directory, 0);
            IFeatureWorkspace pTargetFeatureWorkspace = pTargetWorkspace as IFeatureWorkspace;

            // 若要素数据集名称不为空，则获取该数据集
            IFeatureDatasetName pTargetFeatureDatasetName = null;
            if (!string.IsNullOrWhiteSpace(datasetName))
            {
                pTargetFeatureDatasetName = pTargetFeatureWorkspace.OpenFeatureDataset(datasetName).FullName as IFeatureDatasetName;
            }

            // 设置目标数据属性
            IDataset pTargetDataSet = pTargetWorkspace as IDataset;
            IWorkspaceName pTargetWorkspaceName = pTargetDataSet.FullName as IWorkspaceName;
            IFeatureClassName pTargetFeatureClassName = new FeatureClassName() as IFeatureClassName;
            IDatasetName pTargetDatasetName = pTargetFeatureClassName as IDatasetName;
            pTargetDatasetName.WorkspaceName = pTargetWorkspaceName;
            pTargetDatasetName.Name = className;

            // 检查字段
            IFieldChecker pFieldChecker = new FieldChecker();
            pFieldChecker.InputWorkspace = pSourceWorkspace;
            pFieldChecker.ValidateWorkspace = pTargetWorkspace;

            // 字段转换
            IFields pSourceFields = pSourceFeatureClass.Fields;
            IFields pTargetFields = null;
            IEnumFieldError pEnumFieldError = null;
            pFieldChecker.Validate(pSourceFields, out pEnumFieldError, out pTargetFields);

            // 数据转换
            IFeatureDataConverter pFeatureDataConverter = new FeatureDataConverter();
            try
            {
                pFeatureDataConverter.ConvertFeatureClass(pSourceFeatureClassName, pQueryFilter, pTargetFeatureDatasetName, pTargetFeatureClassName, null, pTargetFields, "", 1000, 0);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
    }
}
