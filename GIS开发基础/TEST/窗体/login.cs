using DAL.must;
using System;
using System.Windows.Forms;
using DAL.SqlHelper;
using DAL.must;
using MySql.Data.MySqlClient;
using System.Data;

namespace TEST.窗体
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String connetStr = "server=rm-uf65380fg1n8r622t8o.mysql.rds.aliyuncs.com;port=3306;user=yanglin;password=!gis2022; database=gis;";
            // server=127.0.0.1/localhost 代表本机，端口号port默认是3306可以不写
            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句

                //获取输入
                if(txtUserName.Text=="" || txtPassword.Text == "")
                {
                    MessageBox.Show("请输入账号密码");
                    return;
                }
                string userName = txtUserName.Text;
                string passward = txtPassword.Text;


                string sqlSelect = "SELECT Name,Passward from stuinfo WHERE Name=@name;";
                MySqlParameter parms = new MySqlParameter("@name", userName);
                //string sql = @"SELECT Name,Password from stuinfo where Name={} AND Password={}";
                MySqlDataReader dr =DAL.must.MySqlHelper.ExecuteReader(conn, CommandType.Text, sqlSelect, parms);
                //DAL.must.MySqlHelper.ExecuteReader(conn, CommandType.Text, sql);

                bool flag = false;
                while (dr.Read())
                {
                    if (passward == dr[1].ToString()) {
                        flag = true;
                    };
                    
                }
                if (flag)
                {
                    this.Hide();
                    MessageBox.Show("登录成功！");
                    new Form1().Show(); 
                }
                else
                {
                    MessageBox.Show("密码错误");
                }
                //SELECT Name,Password INTO stuinfo VALUES('杨林',213,212)
                //在这里使用代码对数据库进行增删查改
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
