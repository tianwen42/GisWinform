using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TEST_6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            createGraph();
            InitializeComponent();
            pictureInitialize(1);
            pictureInitialize(2);
        }
        int MAX = 20000;
        int distance = 0;
        public bool[, ,] P = new bool[N, N, N];
        public int box = 0;//标注当前光标所在框的编号
        static int N = 17; //声明共有17个节点

        //static int[] Vex_location_x = new int[N];//建立坐标横轴值
        //static int[] Vex_location_y = new int[N];//建立坐标纵轴值
        static int[] Vex_number = new int[N];//建立节点编号

        static string[] Vex_info = new string[N];//建立景点介绍信息
        static string[] Vex_sight = new string[N];//建立景点的名称信息

        static int[,] Arc_Legth = new int[N,N];//建立节点间的距离数组

        string[,] new_vexnum = new string[N,N];
        //static bool[] final = new bool[N];
        //static int[] Path = new int[N];
        //static bool[,] P = new bool[N,N];//二维逻辑数组，使用方法：若P[v,w] == true，则代表w是从v0到v当前求得最短路径上的顶点。
        static int[] newPath_value = new int[N];//用于保存新的路线带权长度

        public void createGraph()
        {
            for (int i = 1; i < N; i++)
            {
                Vex_number[i] = i;
            }
            Vex_info[1] = "靠近C区宿舍和D区宿舍";
            Vex_info[2] = "靠近A区宿舍，B区宿舍和H区宿舍";
            Vex_info[3] = "A区宿舍是女生宿舍";
            Vex_info[4] = "B区宿舍为男生宿舍";
            Vex_info[5] = "C区宿舍，是男女生宿舍";
            Vex_info[6] = "D区宿舍为女生宿舍";
            Vex_info[7] = "H区宿舍为女生宿舍";
            Vex_info[8] = "为学生提供基本生活保障";
            Vex_info[9] = "学生主要上课区域";
            Vex_info[10] = "主要是食科院和环科院实验室和学工处";
            Vex_info[11] = "是信工院和人工智能院的实验室";
            Vex_info[12] = "是文学院和英语专业的上课地点";
            Vex_info[13] = "存储图书和提供阅览";
            Vex_info[14] = "开会和上课地点";
            Vex_info[15] = "锻炼身体，保持健康";
            Vex_info[16] = "教务处和主要领导办公地点";

            Vex_sight[1] = "南食堂";
            Vex_sight[2] = "北食堂";
            Vex_sight[3] = "A宿舍区";
            Vex_sight[4] = "B宿舍区";
            Vex_sight[5] = "C宿舍区";
            Vex_sight[6] = "D宿舍区";
            Vex_sight[7] = "H宿舍区";
            Vex_sight[8] = "服务区";
            Vex_sight[9] = "行知组团";
            Vex_sight[10] = "理科组团";
            Vex_sight[11] = "工科楼";
            Vex_sight[12] = "文科组团";
            Vex_sight[13] = "图书馆";
            Vex_sight[14] = "中心报告厅";
            Vex_sight[15] = "体育馆";
            Vex_sight[16] = "行政楼";

            /*Vex_location_x[1] = 180; Vex_location_y[1] = 328;
            Vex_location_x[2] = 232; Vex_location_y[2] = 308;
            Vex_location_x[3] = 343; Vex_location_y[3] = 408;
            Vex_location_x[4] = 540; Vex_location_y[4] = 365;
            Vex_location_x[5] = 143; Vex_location_y[5] = 267;
            Vex_location_x[6] = 450; Vex_location_y[6] = 430;
            Vex_location_x[7] = 280; Vex_location_y[7] = 435;
            Vex_location_x[8] = 500; Vex_location_y[8] = 420;
            Vex_location_x[9] = 195; Vex_location_y[9] = 205;
            Vex_location_x[10] = 340; Vex_location_y[10] = 530;
            Vex_location_x[11] = 700; Vex_location_y[11] = 247;
            Vex_location_x[12] = 230; Vex_location_y[12] = 235;
            Vex_location_x[13] = 180; Vex_location_y[13] = 355;
            Vex_location_x[14] = 560; Vex_location_y[14] = 325;
            Vex_location_x[15] = 640; Vex_location_y[15] = 277;
            Vex_location_x[16] = 458; Vex_location_y[16] = 366;*/
        }
        //
        //包括地点名称、地点信息、地点间的距离设定
        //
        private void pictureInitialize(int box)
        {
            if (box == 1)
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                pictureBox5.Visible = false;
                pictureBox6.Visible = false;
                pictureBox7.Visible = false;
                pictureBox8.Visible = false;
                pictureBox9.Visible = false;
                pictureBox10.Visible = false;
                pictureBox11.Visible = false;
                pictureBox12.Visible = false;
                pictureBox13.Visible = false;
                pictureBox14.Visible = false;
                pictureBox15.Visible = false;
                pictureBox16.Visible = false;
            }
            if (box == 2)
            {
                pictureBox17.Visible = false;
                pictureBox18.Visible = false;
                pictureBox19.Visible = false;
                pictureBox20.Visible = false;
                pictureBox21.Visible = false;
                pictureBox22.Visible = false;
                pictureBox23.Visible = false;
                pictureBox24.Visible = false;
                pictureBox25.Visible = false;
                pictureBox26.Visible = false;
                pictureBox27.Visible = false;
                pictureBox28.Visible = false;
                pictureBox29.Visible = false;
                pictureBox30.Visible = false;
                pictureBox31.Visible = false;
                pictureBox32.Visible = false;
            }
        }
        //
        //绘制地标点
        //
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            box = 1;
            pictureInitialize(1);
            if (comboBox1.Text == "南食堂") { display(Vex_number[1]); pictureBox1.Visible = true; pictureBox17.Visible = true; }
            if (comboBox1.Text == "北食堂") { display(Vex_number[2]); pictureBox2.Visible = true; }
            if (comboBox1.Text == "A宿舍区") { display(Vex_number[3]); pictureBox3.Visible = true; }
            if (comboBox1.Text == "B宿舍区") {display(Vex_number[4]);pictureBox4.Visible = true;}
            if (comboBox1.Text == "C宿舍区") {display(Vex_number[5]);pictureBox5.Visible = true;}
            if (comboBox1.Text == "D宿舍区") {display(Vex_number[6]);pictureBox6.Visible = true;}
            if (comboBox1.Text == "H宿舍区") {display(Vex_number[7]);pictureBox7.Visible = true;}
            if (comboBox1.Text == "服务区") {display(Vex_number[8]);pictureBox8.Visible = true;}
            if (comboBox1.Text == "行知组团") {display(Vex_number[9]);pictureBox9.Visible = true;}
            if (comboBox1.Text == "理科组团") {display(Vex_number[10]);pictureBox10.Visible = true;}
            if (comboBox1.Text == "工科楼") {display(Vex_number[11]);pictureBox11.Visible = true;}
            if (comboBox1.Text == "文科组团") {display(Vex_number[12]);pictureBox12.Visible = true;}
            if (comboBox1.Text == "图书馆") {display(Vex_number[13]);pictureBox13.Visible = true;}
            if (comboBox1.Text == "中心报告厅") {display(Vex_number[14]);pictureBox14.Visible = true;}
            if (comboBox1.Text == "体育馆") {display(Vex_number[15]);pictureBox15.Visible = true;}
            if (comboBox1.Text == "行政楼") {display(Vex_number[16]);pictureBox16.Visible = true;}
        }
        //
        //下拉菜单1的操作
        //
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            box = 2;
            pictureInitialize(2);
            if (comboBox2.Text == "南食堂") { display(Vex_number[1]); pictureBox17.Visible = true; }
            if (comboBox2.Text == "北食堂") { display(Vex_number[2]); pictureBox18.Visible = true; }
            if (comboBox2.Text == "A宿舍区") { display(Vex_number[3]); pictureBox19.Visible = true; }
            if (comboBox2.Text == "B宿舍区") { display(Vex_number[4]); pictureBox20.Visible = true; }
            if (comboBox2.Text == "C宿舍区") { display(Vex_number[5]); pictureBox21.Visible = true; }
            if (comboBox2.Text == "D宿舍区") { display(Vex_number[6]); pictureBox22.Visible = true; }
            if (comboBox2.Text == "H宿舍区") { display(Vex_number[7]); pictureBox23.Visible = true; }
            if (comboBox2.Text == "服务区") { display(Vex_number[8]); pictureBox24.Visible = true; }
            if (comboBox2.Text == "行知组团") { display(Vex_number[9]); pictureBox25.Visible = true; }
            if (comboBox2.Text == "理科组团") { display(Vex_number[10]); pictureBox26.Visible = true; }
            if (comboBox2.Text == "工科楼") { display(Vex_number[11]); pictureBox27.Visible = true; }
            if (comboBox2.Text == "文科组团") { display(Vex_number[12]); pictureBox28.Visible = true; }
            if (comboBox2.Text == "图书馆") { display(Vex_number[13]); pictureBox29.Visible = true; }
            if (comboBox2.Text == "中心报告厅") { display(Vex_number[14]); pictureBox30.Visible = true; }
            if (comboBox2.Text == "体育馆") { display(Vex_number[15]); pictureBox31.Visible = true; }
            if (comboBox2.Text == "行政楼") { display(Vex_number[16]); pictureBox32.Visible = true; }
        }
        //
        //下拉菜单2的操作
        //
        /*private void drawdot(int x, int y)
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(new SolidBrush(Color.Red), 0.5f);
            Rectangle rg = new Rectangle(x, y, 15, 15);
            g.DrawEllipse(pen, rg);
            g.FillEllipse(new SolidBrush(Color.Yellow), rg);
        }*/
        //
        //绘制所选点的坐标
        //
        private void display(int x)
        {
            int i = 1;
            for (i = 1; i < N;i++ )
                if (i == x)
                    if(box == 1)
                        { textBox1.Text = Vex_info[i]; /*drawdot(Vex_location_x[i], Vex_location_y[i]);*/ }
                    else if(box == 2)
                        { textBox2.Text = Vex_info[i]; /*drawdot(Vex_location_x[i], Vex_location_y[i]);*/ }
        }
        //
        //选框内容进行函数调用，分别为显示信息，在地图上画点标注地理位置
        //
        private void Floyd()
        {
            distance = 0;
            Arc_Legth[1, 2] = Arc_Legth[2, 1] = 392;
            Arc_Legth[1, 13] = Arc_Legth[13, 1] = 659;
            Arc_Legth[2, 12] = Arc_Legth[12, 2] = 535;
            Arc_Legth[2, 7] = Arc_Legth[7, 2] = 158;
            Arc_Legth[3, 7] = Arc_Legth[7, 3] = 192;
            Arc_Legth[3, 10] = Arc_Legth[10, 3] = 812;
            Arc_Legth[3, 16] = Arc_Legth[16, 3] = 606;
            Arc_Legth[3, 6] = Arc_Legth[6, 3] = 528;
            Arc_Legth[4, 6] = Arc_Legth[6, 4] = 399;
            Arc_Legth[4, 16] = Arc_Legth[16, 4] = 604;
            Arc_Legth[4, 8] = Arc_Legth[8, 4] = 177;
            Arc_Legth[4, 14] = Arc_Legth[14, 4] = 273;
            Arc_Legth[5, 12] = Arc_Legth[12, 5] = 668;
            Arc_Legth[6, 8] = Arc_Legth[8, 6] = 540;
            Arc_Legth[6, 16] = Arc_Legth[16, 6] = 469;
            Arc_Legth[6, 7] = Arc_Legth[7, 6] = 644;
            Arc_Legth[6, 10] = Arc_Legth[10, 6] = 480;
            Arc_Legth[7, 10] = Arc_Legth[10, 7] = 988;
            Arc_Legth[8, 16] = Arc_Legth[16, 8] = 780;
            Arc_Legth[8, 14] = Arc_Legth[14, 8] = 450;
            Arc_Legth[9, 12] = Arc_Legth[12, 9] = 856;
            Arc_Legth[10, 13] = Arc_Legth[13, 10] = 697;
            Arc_Legth[11, 15] = Arc_Legth[15, 11] = 987;
            Arc_Legth[14, 15] = Arc_Legth[15, 14] = 581;
            Arc_Legth[14, 16] = Arc_Legth[16, 14] = 344;
            for (int c = 1; c < N; c++)
                for (int d = 1; d < N; d++)
                    if (Arc_Legth[c, d] == 0 && c != d)
                        Arc_Legth[c, d] = MAX;
            for (int v = 1; v < N; v++)
                for (int w = 1; w < N; w++)
                {
                    for (int u = 0; u < N; u++)
                        P[v, w, u] = false;
                    if (Arc_Legth[v, w] < MAX)
                    {
                        P[v, w, v] = true;
                        P[v, w, w] = true;
                    }
                }
            for(int v = 1;v<N;v++)
                for(int w = 1;w<N;w++)
                    for(int u = 1;u<N;u++)
                        if (Arc_Legth[v, u] + Arc_Legth[u, w] < Arc_Legth[v, w])
                        {
                            Arc_Legth[v, w] = Arc_Legth[v, u] + Arc_Legth[u, w];
                            new_vexnum[v, w] = new_vexnum[v, u] + u + "/" + new_vexnum[u, w];
                            for (int i = 1; i < N; i++)
                                P[v, w, i] = P[v, u, i] || P[u, w, i];
                        }
        }
        //
        //求出最短加权路径以及路线顺序,佛洛依德算法
        //
        private void showroad()
        {
            //Floyd();
            int n = 0, k = 0 ;
            int p = 0, q = 0;
            for (int m = 1; m < N; m++)
            {
                if (comboBox1.Text == Vex_sight[m])
                    n = m;
                if (comboBox2.Text == Vex_sight[m])
                    k = m;
            }

            textBox4.Text = Vex_sight[n];
            textBox4.Text += "→";
            if (new_vexnum[n, k] != null)
            {
                string[] new_path = new_vexnum[n, k].Split('/');
                int[] connect = new int[new_path.Length+1];
                connect[0] = n; connect[new_path.Length] = k;
                for (int i = 1; i < new_path.Length; i++)
                {
                    connect[i] = Convert.ToInt16(new_path[i-1]);
                }
                for (int i = 0; i < new_path.Length; i++)
                {
                    distance += Arc_Legth[connect[i], connect[i + 1]];
                }
                    for (int i = 0; i < new_path.Length - 1; i++)
                    {
                        p = Convert.ToInt16(new_path[i]);
                        if (i == 0)
                            if (i + 1 < new_path.Length - 1 && i != 0)
                            {
                                p = Convert.ToInt16(new_path[i]);
                            }
                        textBox4.Text += Vex_sight[p];
                        textBox4.Text += "→";
                    }
            }
            distance += Arc_Legth[q, k];
            textBox4.Text += Vex_sight[k];
            textBox3.Text = Convert.ToString(distance);
        }
        //
        //显示路线
        //
        private void button1_Click(object sender, EventArgs e)
        {
            Floyd();
            showroad();
        }
        //
        //点击按钮触发事件
        //
        private void 初始化复位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox1.Text = null;
            comboBox2.Text = null;
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            pictureInitialize(1);
            pictureInitialize(2);
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本产品是提供给大家介绍云南大学校园导航的导航系统软件，详细使用规则，请参见说明书使用操作。\r\n\n\n谢谢使用！");
        }

        private void 版权ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version :0.0.1(2012-11-28)\n\rCopyright:Private Product(本产品版权归本团队所有！)");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox27_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //
        //调用函数进行输出最优路线和路径长度
        //
    }
}
