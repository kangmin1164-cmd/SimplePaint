using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimplePaint
{
    public partial class Form1 : Form
    {
        // 현재 그리기 상태를 저장하는 변수들
        private string currentShape = "Line";      // 도형 종류
        private Color currentColor = Color.Black;   // 선 색상
        private int currentLineWidth = 1;           // 선 굵기

        public Form1()
        {
            InitializeComponent();
            InitControls(); // 컨트롤 초기화 함수 호출
        }

        // 컨트롤 기본값 설정
        private void InitControls()
        {
            // 콤보박스 아이템 추가 및 기본값 설정
            cmbColor.Items.Add("Black");
            cmbColor.Items.Add("Red");
            cmbColor.Items.Add("Blue");
            cmbColor.Items.Add("Green");
            cmbColor.SelectedIndex = 0;

            // 트랙바 범위 설정
            trbLineWidth.Minimum = 1;
            trbLineWidth.Maximum = 10;
            trbLineWidth.Value = 1;
        }

        // 도형 선택 기능
        private void btnLine_Click(object sender, EventArgs e) => currentShape = "Line";
        private void btnRectangle_Click(object sender, EventArgs e) => currentShape = "Rectangle";
        private void btnCircle_Click(object sender, EventArgs e) => currentShape = "Circle";

        // 색상 선택 기능
        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbColor.SelectedItem == null) return;
            string colorName = cmbColor.SelectedItem.ToString();

            switch (colorName)
            {
                case "Red": currentColor = Color.Red; break;
                case "Blue": currentColor = Color.Blue; break;
                case "Green": currentColor = Color.Green; break;
                default: currentColor = Color.Black; break;
            }
        }

        //선 두께 설정
        private void trbLineWidth_ValueChanged(object sender, EventArgs e)
        {
            currentLineWidth = trbLineWidth.Value;
        }

        // 열기 및 저장 버튼
        private void btnOpenFile_Click(object sender, EventArgs e) { }
        private void btnSaveFile_Click(object sender, EventArgs e) { }
    }
}