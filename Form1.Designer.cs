namespace SimplePaint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            picCanvas = new PictureBox();
            lblAppName = new Label();
            groupBox1 = new GroupBox();
            btnCircle = new Button();
            btnRectangle = new Button();
            btnLine = new Button();
            groupBox2 = new GroupBox();
            cmbColor = new ComboBox();
            groupBox3 = new GroupBox();
            trbLineWidth = new TrackBar();
            btnOpenFile = new Button();
            btnSaveFile = new Button();
            ((System.ComponentModel.ISupportInitialize)picCanvas).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trbLineWidth).BeginInit();
            SuspendLayout();
            // 
            // picCanvas
            // 
            picCanvas.Location = new Point(26, 211);
            picCanvas.Name = "picCanvas";
            picCanvas.Size = new Size(990, 410);
            picCanvas.TabIndex = 0;
            picCanvas.TabStop = false;
            // 
            // lblAppName
            // 
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("한컴 고딕", 20F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblAppName.ForeColor = Color.Blue;
            lblAppName.Location = new Point(26, 9);
            lblAppName.Name = "lblAppName";
            lblAppName.Size = new Size(252, 52);
            lblAppName.TabIndex = 1;
            lblAppName.Text = "Simple Paint";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnCircle);
            groupBox1.Controls.Add(btnRectangle);
            groupBox1.Controls.Add(btnLine);
            groupBox1.Font = new Font("한컴 고딕", 10.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBox1.Location = new Point(26, 78);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(300, 127);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "도형 선택";
            // 
            // btnCircle
            // 
            btnCircle.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnCircle.Image = (Image)resources.GetObject("btnCircle.Image");
            btnCircle.ImageAlign = ContentAlignment.TopCenter;
            btnCircle.Location = new Point(196, 32);
            btnCircle.Name = "btnCircle";
            btnCircle.Size = new Size(79, 87);
            btnCircle.TabIndex = 2;
            btnCircle.Text = "원";
            btnCircle.TextAlign = ContentAlignment.BottomCenter;
            btnCircle.UseVisualStyleBackColor = true;
            btnCircle.Click += btnCircle_Click;
            // 
            // btnRectangle
            // 
            btnRectangle.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnRectangle.Image = (Image)resources.GetObject("btnRectangle.Image");
            btnRectangle.ImageAlign = ContentAlignment.TopCenter;
            btnRectangle.Location = new Point(111, 32);
            btnRectangle.Name = "btnRectangle";
            btnRectangle.Size = new Size(79, 87);
            btnRectangle.TabIndex = 1;
            btnRectangle.Text = "사각형";
            btnRectangle.TextAlign = ContentAlignment.BottomCenter;
            btnRectangle.UseVisualStyleBackColor = true;
            btnRectangle.Click += btnRectangle_Click;
            // 
            // btnLine
            // 
            btnLine.Font = new Font("한컴 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnLine.Image = (Image)resources.GetObject("btnLine.Image");
            btnLine.ImageAlign = ContentAlignment.TopCenter;
            btnLine.Location = new Point(21, 32);
            btnLine.Name = "btnLine";
            btnLine.Size = new Size(79, 87);
            btnLine.TabIndex = 0;
            btnLine.Text = "직선";
            btnLine.TextAlign = ContentAlignment.BottomCenter;
            btnLine.UseVisualStyleBackColor = true;
            btnLine.Click += btnLine_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cmbColor);
            groupBox2.Font = new Font("한컴 고딕", 10.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBox2.Location = new Point(332, 78);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(204, 127);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "색 선택";
            // 
            // cmbColor
            // 
            cmbColor.FormattingEnabled = true;
            cmbColor.Location = new Point(6, 59);
            cmbColor.Name = "cmbColor";
            cmbColor.Size = new Size(192, 37);
            cmbColor.TabIndex = 7;
            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(trbLineWidth);
            groupBox3.Font = new Font("한컴 고딕", 10.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBox3.Location = new Point(542, 78);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(257, 127);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "선 두깨";
            // 
            // trbLineWidth
            // 
            trbLineWidth.Location = new Point(6, 42);
            trbLineWidth.Name = "trbLineWidth";
            trbLineWidth.Size = new Size(251, 69);
            trbLineWidth.TabIndex = 8;
            trbLineWidth.ValueChanged += trbLineWidth_ValueChanged;
            // 
            // btnOpenFile
            // 
            btnOpenFile.BackColor = Color.FromArgb(255, 255, 128);
            btnOpenFile.Font = new Font("한컴 고딕", 11.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnOpenFile.ForeColor = Color.Chocolate;
            btnOpenFile.Location = new Point(805, 100);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(98, 86);
            btnOpenFile.TabIndex = 5;
            btnOpenFile.Text = "열기";
            btnOpenFile.UseVisualStyleBackColor = false;
            btnOpenFile.Click += btnOpenFile_Click;
            // 
            // btnSaveFile
            // 
            btnSaveFile.BackColor = Color.FromArgb(192, 192, 255);
            btnSaveFile.Font = new Font("한컴 고딕", 11.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnSaveFile.ForeColor = Color.Blue;
            btnSaveFile.Location = new Point(918, 100);
            btnSaveFile.Name = "btnSaveFile";
            btnSaveFile.Size = new Size(98, 86);
            btnSaveFile.TabIndex = 6;
            btnSaveFile.Text = "저장";
            btnSaveFile.UseVisualStyleBackColor = false;
            btnSaveFile.Click += btnSaveFile_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1028, 633);
            Controls.Add(btnSaveFile);
            Controls.Add(btnOpenFile);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(lblAppName);
            Controls.Add(picCanvas);
            Name = "Form1";
            Text = "Simple Paint";
            ((System.ComponentModel.ISupportInitialize)picCanvas).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trbLineWidth).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picCanvas;
        private Label lblAppName;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Button btnOpenFile;
        private Button btnSaveFile;
        private Button btnLine;
        private Button btnRectangle;
        private Button btnCircle;
        private ComboBox cmbColor;
        private TrackBar trbLineWidth;
    }
}
