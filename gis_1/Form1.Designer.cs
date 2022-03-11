
namespace gis_1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabpage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.btn_openmxd = new System.Windows.Forms.Button();
            this.btn_openshp = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabpage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(879, 100);
            this.panel1.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.axTOCControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 100);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 463);
            this.panel3.TabIndex = 7;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(679, 100);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 463);
            this.panel4.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.axMapControl1);
            this.panel5.Controls.Add(this.axToolbarControl1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(200, 100);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(479, 463);
            this.panel5.TabIndex = 9;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabpage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(879, 100);
            this.tabControl1.TabIndex = 0;
            // 
            // tabpage1
            // 
            this.tabpage1.Controls.Add(this.btn_openshp);
            this.tabpage1.Controls.Add(this.btn_openmxd);
            this.tabpage1.Location = new System.Drawing.Point(4, 25);
            this.tabpage1.Name = "tabpage1";
            this.tabpage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabpage1.Size = new System.Drawing.Size(871, 71);
            this.tabpage1.TabIndex = 0;
            this.tabpage1.Text = "开始";
            this.tabpage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(871, 71);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(0, 28);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(479, 435);
            this.axMapControl1.TabIndex = 0;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl1.Location = new System.Drawing.Point(0, 0);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(200, 463);
            this.axTOCControl1.TabIndex = 0;
            this.axTOCControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl1_OnMouseDown);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(871, 71);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "关于";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 0);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(479, 28);
            this.axToolbarControl1.TabIndex = 0;
            this.axToolbarControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IToolbarControlEvents_Ax_OnMouseDownEventHandler(this.axToolbarControl1_OnMouseDown);
            // 
            // btn_openmxd
            // 
            this.btn_openmxd.Location = new System.Drawing.Point(8, 6);
            this.btn_openmxd.Name = "btn_openmxd";
            this.btn_openmxd.Size = new System.Drawing.Size(91, 59);
            this.btn_openmxd.TabIndex = 1;
            this.btn_openmxd.Text = "加载mxd";
            this.btn_openmxd.UseVisualStyleBackColor = true;
            this.btn_openmxd.Click += new System.EventHandler(this.btn_openmxd_Click);
            // 
            // btn_openshp
            // 
            this.btn_openshp.Location = new System.Drawing.Point(106, 7);
            this.btn_openshp.Name = "btn_openshp";
            this.btn_openshp.Size = new System.Drawing.Size(90, 58);
            this.btn_openshp.TabIndex = 2;
            this.btn_openshp.Text = "加载shp";
            this.btn_openshp.UseVisualStyleBackColor = true;
            this.btn_openshp.Click += new System.EventHandler(this.btn_openshp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 563);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabpage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabpage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel3;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.Button btn_openmxd;
        private System.Windows.Forms.Button btn_openshp;
    }
}

