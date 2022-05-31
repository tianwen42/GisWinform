using ExcelDataReader;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace TEST.窗体
{
    public partial class xyToPoint : Form
    {
        public xyToPoint()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //comboBox1.SelectedIndex;
            //comboBox2.SelectedIndex;
            MessageBox.Show("gg");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Excel数据|*.xls;*.xlsx";
            if (op.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            string excelFilepath = op.FileName;

            DataSet resDataSet = ReadExcelToDataSet(excelFilepath);
            dataGridView1.DataSource = resDataSet.Tables[0];

            foreach (DataColumn col in resDataSet.Tables[0].Columns)
            {
                //选择项
                comboBox1.Items.Add(col.ColumnName);
                comboBox2.Items.Add(col.ColumnName);
            }

        }

        private DataSet ReadExcelToDataSet(string fileNmaePath)
        {
            FileStream stream = null;
            IExcelDataReader excelReader = null;
            DataSet dataSet = null;
            try
            {
                //stream = File.Open(fileNmaePath, FileMode.Open, FileAccess.Read);
                stream = new FileStream(fileNmaePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch
            {
                return null;
            }
            string extension = Path.GetExtension(fileNmaePath);

            if (extension.ToUpper() == ".XLS")
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (extension.ToUpper() == ".XLSX")
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            else
            {
                MessageBox.Show("格式错误");
                return null;

            }
            //dataSet = excelReader.AsDataSet();//第一行当作数据读取
            dataSet = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });//第一行当作列名读取
            excelReader.Close();
            return dataSet;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
