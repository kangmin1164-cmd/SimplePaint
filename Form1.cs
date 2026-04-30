using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
// [핵심 추가] 점선 스타일(DashStyle)을 사용하기 위해 필요합니다.
using System.Drawing.Drawing2D;

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

        // [과제 4] 확대/축소 비율 변수 (float 사용)
        private float zoomScale = 1.0f;

        public Form1()
        {
            InitializeComponent();
            InitControls();
            InitCanvas();

            // [수정된 부분 1] 폼뿐만 아니라 PictureBox(도화지)에도 휠 이벤트를 연결합니다.
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            picCanvas.MouseWheel += new MouseEventHandler(Form1_MouseWheel);

            // [수정된 부분 2] 마우스가 캔버스 위로 올라올 때 포커스를 가져와 휠 이벤트를 정상적으로 받도록 처리합니다.
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

        // --- [과제 4] 통합된 확대/축소 로직 ---

        private void ApplyZoom(float scale)
        {
            zoomScale = scale;

            if (canvas != null)
            {
                // 1. 원본 이미지 크기에 배율을 곱하여 PictureBox 크기 설정
                // 이 작업이 수행되면 부모 Panel의 AutoScroll에 의해 스크롤바가 생깁니다.
                picCanvas.Width = (int)(canvas.Width * zoomScale);
                picCanvas.Height = (int)(canvas.Height * zoomScale);

                // [수정된 부분 3] Zoom 대신 StretchImage를 사용하면 
                // 소수점 오차로 인한 미세한 여백 발생을 막고 클릭 좌표를 더 정확하게 매핑할 수 있습니다.
                picCanvas.SizeMode = PictureBoxSizeMode.StretchImage;

                // 3. 화면 갱신
                picCanvas.Invalidate();
            }
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            // [수정된 부분 4] ModifierKeys 조건을 더 안전하게 체크합니다.
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

        // --- 마우스 이벤트 구현 (좌표 보정 및 점선 미리보기 구현) ---

        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            // 확대된 화면의 좌표를 원본 캔버스 크기에 맞게 환산
            startPoint = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            // [메모리 관리 수정] 이전 임시 이미지가 PictureBox에 연결되어 있다면 Dispose() 하여 메모리를 아낍니다.
            if (picCanvas.Image != canvas && picCanvas.Image != null)
            {
                picCanvas.Image.Dispose();
            }

            // 원본 캔버스를 복제하여 임시 도화지를 만듭니다.
            Bitmap tempCanvas = new Bitmap(canvas);

            using (Graphics g = Graphics.FromImage(tempCanvas))
            // 펜을 만들고 using으로 묶어 사용 후 즉시 자원 반납
            using (Pen myPen = new Pen(currentColor, currentLineWidth))
            {
                // [핵심 수정 1] 미리보기 시에는 펜의 스타일을 '점선'으로 설정합니다.
                myPen.DashStyle = DashStyle.Dash;

                Point currentPoint = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
                DrawShape(g, myPen, startPoint, currentPoint);
            }

            // PictureBox에 임시 이미지를 보여줍니다. (이제 점선 도형이 보입니다)
            picCanvas.Image = tempCanvas;
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            // [메모리 관리 수정] MouseMove에서 만들었던 마지막 임시 이미지를 Dispose() 합니다.
            if (picCanvas.Image != canvas && picCanvas.Image != null)
            {
                picCanvas.Image.Dispose();
            }

            // 최종적으로 원본 캔버스에 그립니다.
            using (Graphics g = Graphics.FromImage(canvas))
            using (Pen myPen = new Pen(currentColor, currentLineWidth))
            {
                // [핵심 수정 2] 최종 도형은 펜의 스타일을 '실선(Solid)'으로 설정하여 그립니다.
                myPen.DashStyle = DashStyle.Solid;

                Point endPoint = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
                DrawShape(g, myPen, startPoint, endPoint);
            }
            // 최종 수정된 원본 캔버스를 다시 PictureBox에 연결합니다.
            picCanvas.Image = canvas;
            isDrawing = false;
        }

        private void DrawShape(Graphics g, Pen pen, Point start, Point end)
        {
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);
            int width = Math.Abs(start.X - end.X);
            int height = Math.Abs(start.Y - end.Y);

            // [팁] GDI+는 좌표가 소수점 오차로 인해 반올림될 때 
            // 픽셀이 뭉개질 수 있습니다. Antialiasing을 켜면 점선이 더 예쁘게 보입니다.
            g.SmoothingMode = SmoothingMode.AntiAlias;

            switch (currentShape)
            {
                case "Line": g.DrawLine(pen, start, end); break;
                case "Rectangle": g.DrawRectangle(pen, x, y, width, height); break;
                case "Circle": g.DrawEllipse(pen, x, y, width, height); break;
            }
        }

        // --- [과제 4] 이미지 열기 기능 (메모리 문제 해결 버전) ---
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "이미지 파일(*.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 1. 기존 PictureBox가 쥐고 있던 이미지 연결을 먼저 끊어줍니다. (핵심)
                        picCanvas.Image = null;

                        // 2. 파일을 열어 원본 이미지를 잠시 변수에 담습니다.
                        // using을 사용하면 파일을 읽은 후 원본 파일의 '잠금' 상태를 즉시 해제합니다.
                        using (Bitmap loadedImage = new Bitmap(ofd.FileName))
                        {
                            // 3. 기존 도화지가 있다면 메모리에서 안전하게 삭제합니다.
                            if (canvas != null)
                            {
                                canvas.Dispose();
                            }

                            // 4. 불러온 이미지와 똑같은 크기와 내용을 가진 새로운 도화지를 생성합니다.
                            canvas = new Bitmap(loadedImage);
                        }

                        // 5. 새롭게 만든 도화지를 다시 PictureBox에 연결합니다. (핵심)
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