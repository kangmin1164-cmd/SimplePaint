using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace SimplePaint
{
    public partial class Form1 : Form
    {
        // 그리기 상태 변수 (과제 1, 2 반영)
        private string currentShape = "Line";
        private Color currentColor = Color.Black;
        private int currentLineWidth = 1;

        private Bitmap canvas;          // 최종 그림이 저장되는 원본 도화지 (실제 크기)
        private Point startPoint;       // 마우스 클릭 시작점
        private bool isDrawing = false; // 그리기 상태 체크

        // [과제 4] 확대/축소 배율 변수
        private float zoomScale = 1.0f;

        public Form1()
        {
            InitializeComponent();
            InitControls();
            InitCanvas();

            // picCanvas에 직접 휠 이벤트를 연결하여 인식률 극대화
            picCanvas.MouseWheel += new MouseEventHandler(picCanvas_MouseWheel);

            // 마우스가 영역에 들어오면 포커스를 주어 휠이 즉시 작동하게 함
            picCanvas.MouseEnter += (s, e) => picCanvas.Focus();
        }

        private void InitCanvas()
        {
            // 캔버스 초기화 (흰색 배경)
            canvas = new Bitmap(picCanvas.Width, picCanvas.Height);
            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.Clear(Color.White);
            }
            picCanvas.Image = canvas;
        }

        private void InitControls()
        {
            cmbColor.Items.AddRange(new string[] { "Black", "Red", "Blue", "Green" });
            cmbColor.SelectedIndex = 0;
            trbLineWidth.Minimum = 1;
            trbLineWidth.Maximum = 10;
            trbLineWidth.Value = 1;

            // 스크롤바 활성화를 위해 픽처박스 크기가 이미지에 맞춰지도록 설정
            picCanvas.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        // --- [과제 4] 마우스 휠 확대/축소 (Ctrl + 휠) ---
        private void picCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                // e.Delta가 양수면 확대, 음수면 축소
                if (e.Delta > 0) zoomScale += 0.1f;
                else if (e.Delta < 0 && zoomScale > 0.2f) zoomScale -= 0.1f;

                UpdateCanvasDisplay(); // 배율에 맞춰 화면 갱신

                // 폼의 기본 스크롤 동작 방지
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        // [핵심] 화면을 배율에 맞게 다시 그려주는 함수
        private void UpdateCanvasDisplay()
        {
            if (canvas == null) return;

            // 1. 픽처박스 크기를 배율에 맞춰 변경 (부모 Panel에 스크롤바 생성됨)
            picCanvas.Width = (int)(canvas.Width * zoomScale);
            picCanvas.Height = (int)(canvas.Height * zoomScale);

            // 2. 현재 배율이 적용된 화면용 비트맵 생성
            Bitmap displayBitmap = new Bitmap(picCanvas.Width, picCanvas.Height);
            using (Graphics g = Graphics.FromImage(displayBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                // 원본 canvas를 배율이 적용된 크기로 그림
                g.DrawImage(canvas, 0, 0, picCanvas.Width, picCanvas.Height);
            }

            // 3. 화면에 표시 (기존 이미지를 Dispose하여 메모리 관리 가능)
            var oldImage = picCanvas.Image;
            picCanvas.Image = displayBitmap;
            if (oldImage != null && oldImage != canvas) oldImage.Dispose();
        }

        // --- 마우스 드로잉 (확대 상태 유지 보완) ---

        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            // 확대된 좌표를 실제 원본 비트맵의 좌표로 환산
            startPoint = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            // 임시 캔버스 생성 (잔상 효과)
            Bitmap tempCanvas = (Bitmap)canvas.Clone();
            using (Graphics g = Graphics.FromImage(tempCanvas))
            {
                Pen myPen = new Pen(currentColor, currentLineWidth);
                Point currentPoint = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
                DrawShape(g, myPen, startPoint, currentPoint);
            }

            // [오류 수정] 임시 화면을 보여줄 때도 현재 배율(zoomScale)을 유지해야 함
            Bitmap displayTemp = new Bitmap((int)(tempCanvas.Width * zoomScale), (int)(tempCanvas.Height * zoomScale));
            using (Graphics g = Graphics.FromImage(displayTemp))
            {
                g.DrawImage(tempCanvas, 0, 0, displayTemp.Width, displayTemp.Height);
            }
            tempCanvas.Dispose();

            var oldImage = picCanvas.Image;
            picCanvas.Image = displayTemp;
            if (oldImage != null && oldImage != canvas) oldImage.Dispose();
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            // 원본 캔버스에 최종 그림 기록
            using (Graphics g = Graphics.FromImage(canvas))
            {
                Pen myPen = new Pen(currentColor, currentLineWidth);
                Point endPoint = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
                DrawShape(g, myPen, startPoint, endPoint);
            }

            // [오류 수정] 마우스를 뗐을 때 원본으로 돌아가지 않고 현재 배율 화면을 유지
            UpdateCanvasDisplay();
            isDrawing = false;
        }

        private void DrawShape(Graphics g, Pen pen, Point start, Point end)
        {
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);
            int width = Math.Abs(start.X - end.X);
            int height = Math.Abs(start.Y - end.Y);

            switch (currentShape)
            {
                case "Line": g.DrawLine(pen, start, end); break;
                case "Rectangle": g.DrawRectangle(pen, x, y, width, height); break;
                case "Circle": g.DrawEllipse(pen, x, y, width, height); break;
            }
        }

        // --- 이미지 열기 (과제 4) ---
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "이미지 파일(*.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap loadedImage = new Bitmap(ofd.FileName);
                    if (canvas != null) canvas.Dispose();
                    canvas = new Bitmap(loadedImage); // 원본 크기 저장
                    loadedImage.Dispose();

                    zoomScale = 1.0f; // 초기 배율
                    UpdateCanvasDisplay();
                }
            }
        }

        // --- 이미지 저장 (과제 3) ---
        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG 파일(*.png)|*.png|JPG 파일(*.jpg)|*.jpg|BMP 파일(*.bmp)|*.bmp";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string ext = System.IO.Path.GetExtension(sfd.FileName).ToLower();
                    ImageFormat format = (ext == ".jpg") ? ImageFormat.Jpeg : (ext == ".bmp" ? ImageFormat.Bmp : ImageFormat.Png);
                    canvas.Save(sfd.FileName, format); // 원본 고화질 저장
                }
            }
        }

        // 컨트롤 이벤트
        private void btnLine_Click(object sender, EventArgs e) => currentShape = "Line";
        private void btnRectangle_Click(object sender, EventArgs e) => currentShape = "Rectangle";
        private void btnCircle_Click(object sender, EventArgs e) => currentShape = "Circle";
        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbColor.SelectedItem != null)
                currentColor = Color.FromName(cmbColor.SelectedItem.ToString());
        }
        private void trbLineWidth_ValueChanged(object sender, EventArgs e) => currentLineWidth = trbLineWidth.Value;
    }
}