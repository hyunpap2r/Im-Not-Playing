namespace Im_Not_Playing
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pictureBox1 = new PictureBox();
            timer1 = new System.Windows.Forms.Timer(components);
            CatchTime = new TextBox();
            ResultBox = new TextBox();
            BTN_P_S = new Button();
            button2 = new Button();
            groupBox1 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 16);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(649, 483);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
            // 
            // CatchTime
            // 
            CatchTime.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            CatchTime.Location = new Point(6, 504);
            CatchTime.Name = "CatchTime";
            CatchTime.ReadOnly = true;
            CatchTime.Size = new Size(217, 25);
            CatchTime.TabIndex = 1;
            // 
            // ResultBox
            // 
            ResultBox.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            ResultBox.Location = new Point(229, 504);
            ResultBox.Name = "ResultBox";
            ResultBox.ReadOnly = true;
            ResultBox.Size = new Size(84, 25);
            ResultBox.TabIndex = 2;
            // 
            // BTN_P_S
            // 
            BTN_P_S.Location = new Point(325, 506);
            BTN_P_S.Name = "BTN_P_S";
            BTN_P_S.Size = new Size(156, 23);
            BTN_P_S.TabIndex = 3;
            BTN_P_S.Text = "일시정지/시작\r\n";
            BTN_P_S.UseVisualStyleBackColor = true;
            BTN_P_S.Click += BTN_P_S_Click;
            // 
            // button2
            // 
            button2.Location = new Point(487, 505);
            button2.Name = "button2";
            button2.Size = new Size(156, 23);
            button2.TabIndex = 4;
            button2.Text = "종료 및 저장";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Location = new Point(0, 1);
            groupBox1.Margin = new Padding(0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(0);
            groupBox1.Size = new Size(649, 499);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(650, 535);
            Controls.Add(groupBox1);
            Controls.Add(button2);
            Controls.Add(BTN_P_S);
            Controls.Add(ResultBox);
            Controls.Add(CatchTime);
            Name = "Form1";
            Text = "Im Not Playing";
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private TextBox CatchTime;
        private TextBox ResultBox;
        private Button BTN_P_S;
        private Button button2;
        private GroupBox groupBox1;
    }
}
