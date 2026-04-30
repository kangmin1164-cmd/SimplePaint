using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimplePaint
{
    public partial class Form1 : Form
    {
        // 그리기 상태 변수
        private string currentShape = "Line";
        private Color currentColor = Color.Black;
        private int currentLineWidth = 1;

        private Bitmap canvas;          // 최종 그림이 저장되는 도화지
        private Point startPoint;       // 마우스 클릭 시작점
        private bool isDrawing = false; // 그리기 상태 체크

        public Form1()
        {
            InitializeComponent();
            InitControls();
            InitCanvas(); // 캔버스 초기화 호출
        }

        private void InitCanvas()
        {
            // picCanvas 크기의 비트맵 생성 및 흰색 배경 초기화 
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
        }

        // 마우스 이벤트 구현

        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startPoint = e.Location; // 시작 좌표 저장
        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            // 잔상 효과 구현 (임시 그리기)
            // 원본 캔버스를 복사하여 임시 비트맵 생성
            Bitmap tempCanvas = (Bitmap)canvas.Clone();
            using (Graphics g = Graphics.FromImage(tempCanvas))
            {
                Pen myPen = new Pen(currentColor, currentLineWidth);
                DrawShape(g, myPen, startPoint, e.Location);
            }
            picCanvas.Image = tempCanvas; // 화면에 임시 그림 표시
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            // 최종적으로 원본 캔버스에 그림 고정
            using (Graphics g = Graphics.FromImage(canvas))
            {
                Pen myPen = new Pen(currentColor, currentLineWidth);
                DrawShape(g, myPen, startPoint, e.Location);
            }
            picCanvas.Image = canvas; // 최종 이미지 갱신
            isDrawing = false;
        }

        // [도형 그리기 공통 함수]
        private void DrawShape(Graphics g, Pen pen, Point start, Point end)
        {
            // 사각형/원 그리기용 영역 계산
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);
            int width = Math.Abs(start.X - end.X);
            int height = Math.Abs(start.Y - end.Y);

            switch (currentShape)
            {
                case "Line":
                    g.DrawLine(pen, start, end);
                    break;
                case "Rectangle":
                    g.DrawRectangle(pen, x, y, width, height);
                    break;
                case "Circle":
                    g.DrawEllipse(pen, x, y, width, height);
                    break;
            }
        }

        // 기존 컨트롤 이벤트들
        private void btnLine_Click(object sender, EventArgs e) => currentShape = "Line";
        private void btnRectangle_Click(object sender, EventArgs e) => currentShape = "Rectangle";
        private void btnCircle_Click(object sender, EventArgs e) => currentShape = "Circle";

        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbColor.SelectedItem != null)
                currentColor = Color.FromName(cmbColor.SelectedItem.ToString());
        }

        private void trbLineWidth_ValueChanged(object sender, EventArgs e)
        {
            currentLineWidth = trbLineWidth.Value;
        }

        private void btnOpenFile_Click(object sender, EventArgs e) { }
        private void btnSaveFile_Click(object sender, EventArgs e) { }
    }
}