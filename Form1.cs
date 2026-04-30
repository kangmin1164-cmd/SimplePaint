using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace SimplePaint
{
    public partial class Form1 : Form
    {
        // 그리기 상태 변수
        private string currentShape = "Line";
        private Color currentColor = Color.Black;
        private int currentLineWidth = 1;

        private Bitmap canvas;          // 실제 그림이 기록되는 원본 도화지
        private Point startPoint;       // 마우스 클릭 시작점
        private bool isDrawing = false; // 그리기 상태 체크

        //  확대/축소 비율 변수 (float 사용)
        private float zoomScale = 1.0f;

        public Form1()
        {
            InitializeComponent();
            InitControls();
            InitCanvas();

            // 폼뿐만 아니라 PictureBox(도화지)에도 휠 이벤트를 연결
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            picCanvas.MouseWheel += new MouseEventHandler(Form1_MouseWheel);

            // 마우스가 캔버스 위로 올라올 때 포커스를 가져와 휠 이벤트를 정상적으로 받도록 처리
            picCanvas.MouseEnter += (sender, e) => {
                if (picCanvas.Parent != null)
                {
                    picCanvas.Parent.Focus(); // 부모 컨트롤(Panel 또는 Form)에 포커스 부여
                }
                else
                {
                    this.Focus();
                }
            };
        }

        private void InitCanvas()
        {
            // 초기 캔버스 생성
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

            // 기본 SizeMode 설정
            picCanvas.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        // --- 통합된 확대/축소 로직 ---

        private void ApplyZoom(float scale)
        {
            zoomScale = scale;

            if (canvas != null)
            {
                // 1. 원본 이미지 크기에 배율을 곱하여 PictureBox 크기 설정
                // 이 작업이 수행되면 부모 Panel의 AutoScroll에 의해 스크롤바 생김
                picCanvas.Width = (int)(canvas.Width * zoomScale);
                picCanvas.Height = (int)(canvas.Height * zoomScale);

                // Zoom 대신 StretchImage를 사용하면 
                // 소수점 오차로 인한 미세한 여백 발생을 막고 클릭 좌표를 더 정확하게 매핑
                picCanvas.SizeMode = PictureBoxSizeMode.StretchImage;

                // 3. 화면 갱신
                picCanvas.Invalidate();
            }
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            // ModifierKeys 조건을 더 안전하게 체크
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.Delta > 0)
                {
                    ApplyZoom(zoomScale + 0.1f); // 10% 확대
                }
                else
                {
                    if (zoomScale > 0.2f) // 최소 배율 제한 (0이 되지 않도록 방어)
                    {
                        ApplyZoom(zoomScale - 0.1f); // 10% 축소
                    }
                }

                // 폼의 기본 스크롤(위아래 이동) 동작 방지
                if (e is HandledMouseEventArgs he) he.Handled = true;
            }
        }

        // --- 마우스 이벤트 구현 (좌표 보정 필수) ---

        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            // 확대된 화면의 좌표를 원본 캔버스 크기에 맞게 환산
            startPoint = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            Bitmap tempCanvas = (Bitmap)canvas.Clone();
            using (Graphics g = Graphics.FromImage(tempCanvas))
            {
                Pen myPen = new Pen(currentColor, currentLineWidth);
                Point currentPoint = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
                DrawShape(g, myPen, startPoint, currentPoint);
            }
            picCanvas.Image = tempCanvas;
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            using (Graphics g = Graphics.FromImage(canvas))
            {
                Pen myPen = new Pen(currentColor, currentLineWidth);
                Point endPoint = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
                DrawShape(g, myPen, startPoint, endPoint);
            }
            picCanvas.Image = canvas;
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

        // --- 이미지 열기 기능  ---
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "이미지 파일(*.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 1. 기존 PictureBox가 쥐고 있던 이미지 연결을 먼저 끊어줌
                        picCanvas.Image = null;

                        // 2. 파일을 열어 원본 이미지를 잠시 변수에 담음
                        // using을 사용하면 파일을 읽은 후 원본 파일의 '잠금' 상태를 즉시 해제
                        using (Bitmap loadedImage = new Bitmap(ofd.FileName))
                        {
                            // 3. 기존 도화지가 있다면 메모리에서 안전하게 삭제
                            if (canvas != null)
                            {
                                canvas.Dispose();
                            }

                            // 4. 불러온 이미지와 똑같은 크기와 내용을 가진 새로운 도화지를 생성
                            canvas = new Bitmap(loadedImage);
                        }

                        // 5. 새롭게 만든 도화지를 다시 PictureBox에 연결
                        picCanvas.Image = canvas;

                        // 6. 불러올 때 배율 초기화 및 화면 적용
                        ApplyZoom(1.0f);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("파일을 열 수 없습니다: " + ex.Message);
                    }
                }
            }
        }

        // --- 기타 컨트롤 이벤트 ---
        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG 파일(*.png)|*.png|JPG 파일(*.jpg)|*.jpg|BMP 파일(*.bmp)|*.bmp";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string ext = System.IO.Path.GetExtension(sfd.FileName).ToLower();
                    ImageFormat format = (ext == ".jpg") ? ImageFormat.Jpeg : (ext == ".bmp" ? ImageFormat.Bmp : ImageFormat.Png);
                    canvas.Save(sfd.FileName, format);
                }
            }
        }

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