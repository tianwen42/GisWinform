using System;
using System.Windows.Forms;
using TEST.窗体;

namespace TEST
{
    static class Program
    {
        public static bool isValidUser;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        
        static void Main()
        {
            Application.EnableVisualStyles();
            //绑定
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());//Form1()
            
            //if (isValidUser == true)
            //{
            //    MessageBox.Show("登录成功！");
            //    Application.Run(new Form1());
            //}
            //this.Hide();
            
        

}
    }
}
