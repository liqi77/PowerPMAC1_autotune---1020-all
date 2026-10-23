namespace PMacLearn
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.pixConStatus = new System.Windows.Forms.PictureBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnServerOn = new System.Windows.Forms.Button();
            this.btnServerOff = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pixHomeStatus = new System.Windows.Forms.PictureBox();
            this.btnHome = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFeedbackAngle = new System.Windows.Forms.TextBox();
            this.txtRotateSpeed = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCmdAngle = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerUpdateStatus = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pixConStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixHomeStatus)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "连接状态:";
            // 
            // pixConStatus
            // 
            this.pixConStatus.Location = new System.Drawing.Point(116, 22);
            this.pixConStatus.Name = "pixConStatus";
            this.pixConStatus.Size = new System.Drawing.Size(32, 32);
            this.pixConStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pixConStatus.TabIndex = 1;
            this.pixConStatus.TabStop = false;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(257, 20);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(84, 36);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(382, 20);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(84, 36);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "断开";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "使能状态:";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(116, 77);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // btnServerOn
            // 
            this.btnServerOn.Location = new System.Drawing.Point(257, 75);
            this.btnServerOn.Name = "btnServerOn";
            this.btnServerOn.Size = new System.Drawing.Size(84, 36);
            this.btnServerOn.TabIndex = 2;
            this.btnServerOn.Text = "使能";
            this.btnServerOn.UseVisualStyleBackColor = true;
            this.btnServerOn.Click += new System.EventHandler(this.btnServerOn_Click);
            // 
            // btnServerOff
            // 
            this.btnServerOff.Location = new System.Drawing.Point(382, 75);
            this.btnServerOff.Name = "btnServerOff";
            this.btnServerOff.Size = new System.Drawing.Size(84, 36);
            this.btnServerOff.TabIndex = 2;
            this.btnServerOff.Text = "不使能";
            this.btnServerOff.UseVisualStyleBackColor = true;
            this.btnServerOff.Click += new System.EventHandler(this.btnServerOff_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "回零状态";
            // 
            // pixHomeStatus
            // 
            this.pixHomeStatus.Location = new System.Drawing.Point(116, 137);
            this.pixHomeStatus.Name = "pixHomeStatus";
            this.pixHomeStatus.Size = new System.Drawing.Size(32, 32);
            this.pixHomeStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pixHomeStatus.TabIndex = 1;
            this.pixHomeStatus.TabStop = false;
            // 
            // btnHome
            // 
            this.btnHome.Location = new System.Drawing.Point(258, 135);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(84, 36);
            this.btnHome.TabIndex = 2;
            this.btnHome.Text = "转台回零";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "反馈角度:";
            // 
            // txtFeedbackAngle
            // 
            this.txtFeedbackAngle.Location = new System.Drawing.Point(116, 214);
            this.txtFeedbackAngle.Name = "txtFeedbackAngle";
            this.txtFeedbackAngle.Size = new System.Drawing.Size(100, 21);
            this.txtFeedbackAngle.TabIndex = 3;
            // 
            // txtRotateSpeed
            // 
            this.txtRotateSpeed.Location = new System.Drawing.Point(116, 241);
            this.txtRotateSpeed.Name = "txtRotateSpeed";
            this.txtRotateSpeed.Size = new System.Drawing.Size(100, 21);
            this.txtRotateSpeed.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 289);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "指令角度:";
            // 
            // txtCmdAngle
            // 
            this.txtCmdAngle.Location = new System.Drawing.Point(116, 286);
            this.txtCmdAngle.Name = "txtCmdAngle";
            this.txtCmdAngle.Size = new System.Drawing.Size(100, 21);
            this.txtCmdAngle.TabIndex = 3;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(257, 214);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(84, 36);
            this.btnRead.TabIndex = 2;
            this.btnRead.Text = "读取";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(257, 277);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(84, 36);
            this.btnWrite.TabIndex = 2;
            this.btnWrite.Text = "写入";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 339);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(530, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssStatus
            // 
            this.tssStatus.Name = "tssStatus";
            this.tssStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // timerUpdateStatus
            // 
            this.timerUpdateStatus.Interval = 300;
            this.timerUpdateStatus.Tick += new System.EventHandler(this.timerUpdateStatus_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(45, 244);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "反馈速度:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 361);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtRotateSpeed);
            this.Controls.Add(this.txtCmdAngle);
            this.Controls.Add(this.txtFeedbackAngle);
            this.Controls.Add(this.btnServerOff);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.pixHomeStatus);
            this.Controls.Add(this.btnServerOn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pixConStatus);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "PMAC运动";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pixConStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixHomeStatus)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pixConStatus;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnServerOn;
        private System.Windows.Forms.Button btnServerOff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pixHomeStatus;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFeedbackAngle;
        private System.Windows.Forms.TextBox txtRotateSpeed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCmdAngle;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssStatus;
        private System.Windows.Forms.Timer timerUpdateStatus;
        private System.Windows.Forms.Label label6;
    }
}

