using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Linq;
using System.IO;

namespace LTDT_Project_NhomAnhSang
{
    public partial class FormDijkstraFile : Form
    {
        // Khởi tạo ban đầu 
        public List<Point> listDiem = new List<Point>();
        public List<Point> listDiem1 = new List<Point>();
        public int[,] MaTran = new int[100, 100];
        public Dijkstra dijk = new Dijkstra();
        string textLuuV = "0", textLuuU = "0";

        // Khai báo bảng màu color 
        Pen penDen = new Pen(Color.Black, 1);
        Pen penTrang = new Pen(Color.White, 2);
        Pen penDo = new Pen(Color.Red, 3);
        SolidBrush slbrTrang = new SolidBrush(Color.White);
        SolidBrush slbrDen = new SolidBrush(Color.Black);
        SolidBrush slbrDarkCyan = new SolidBrush(Color.DarkCyan);
        SolidBrush slbrPaleTurquoise = new SolidBrush(Color.PaleTurquoise);
        SolidBrush slbrBlue = new SolidBrush(Color.Blue);
        SolidBrush slbrRed = new SolidBrush(Color.Red);

        public FormDijkstraFile()
        {
            InitializeComponent();
        }

        private void btn_KetQua_Click(object sender, EventArgs e)
        {
            string textMT = tBox_MaTranKe.Text;

            // Dùng split cắt string để lấy dòng đầu tiên là số đỉnh 
            string[] lines1 = textMT.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string _soDinh = lines1[0];
            // Ép kiểu về số nguyên 
            int soDinh = Convert.ToInt32(_soDinh);

            // Dùng split cắt string để bỏ dòng đầu tiên lấy các dòng sau đó 
            string[] lines2 = textMT.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            // Xoá dòng đầu tiên khỏi mảng lines2 
            lines2 = lines2.Skip(1).ToArray();
            // Kết hợp các chuỗi con lại thành 1 chuỗi đơn 
            string _arr = string.Join(Environment.NewLine, lines2);
            // Tách chuỗi thành từng hàng 
            string[] rows = _arr.Split('\n');
            // Khởi tạo mảng 2 chiều 
            int[,] arr = new int[rows.Length, rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                // Tách hàng thành các phần tử 
                string[] columns = rows[i].Split(' ');
                for (int j = 0; j < columns.Length; j++)
                {
                    // Chuyển đổi chuỗi thành giá trị số nguyên 
                    arr[i, j] = Convert.ToInt32(columns[j]);
                }
            }

            Dijkstra.GRAPH g = new Dijkstra.GRAPH(soDinh);
            g.maTran = arr;

            int x, y;
            if (int.TryParse(tBox_ChayTuDinh.Text, out x) && int.TryParse(tBox_ChayDenDinh.Text, out y))
            {
                tBox_KetQuaDijkstra.Text = dijk.ketQuaChay(g, x - 1, y - 1);
                tBox_TongGiaTri.Text = dijk.tongDuongDi(g, x - 1, y - 1);
                textLuuV = dijk.luuv(g, x - 1, y - 1);
                textLuuU = dijk.luuu(g, x - 1, y - 1);
            }

            StartDrawing(); 
            pictureBox2.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            // Bật chế độ này để làm mượt đường vẽ, giảm thiểu hiện tượng răng cưa 
            g.SmoothingMode = SmoothingMode.AntiAlias; 

            int soDinh = listBoxDinh.Items.Count;
            int banKinh = 20;
            int centerX = pictureBox1.Width / 2;
            int centerY = pictureBox1.Height / 2;
            int khoangCanh = 110;

            // Thao tác với cạnh 
            for (int i = 0; i < soDinh; i++)
            {
                for (int j = i + 1; j < soDinh; j++)
                {
                    if (MaTran[i, j] != 0)
                    {
                        int x1 = centerX + (int)(khoangCanh * Math.Cos(2 * Math.PI * i / soDinh));
                        int y1 = centerY + (int)(khoangCanh * Math.Sin(2 * Math.PI * i / soDinh));
                        int x2 = centerX + (int)(khoangCanh * Math.Cos(2 * Math.PI * j / soDinh));
                        int y2 = centerY + (int)(khoangCanh * Math.Sin(2 * Math.PI * j / soDinh));

                        // Vẽ đường nối giữa 2 điểm 
                        g.DrawLine(penDen, x1, y1, x2, y2);

                        int midX = (x1 + x2) / 2;
                        int midY = (y1 + y2) / 2;

                        // Ghi trọng số 
                        SizeF distanceSize = g.MeasureString(MaTran[i, j].ToString(), Font);
                        Font font = new Font("Tahoma", 9, FontStyle.Bold);
                        g.DrawString(MaTran[i, j].ToString(), font, slbrBlue, midX - distanceSize.Width / 2, midY - distanceSize.Height / 2);
                    }
                }
            }

            // Thao tác với đỉnh 
            for (int i = 0; i < soDinh; i++)
            {
                int x = centerX + (int)(khoangCanh * Math.Cos(2 * Math.PI * i / soDinh)) - banKinh;
                int y = centerY + (int)(khoangCanh * Math.Sin(2 * Math.PI * i / soDinh)) - banKinh;

                // Vẽ đỉnh 
                g.FillEllipse(slbrDarkCyan, x, y, banKinh * 2, banKinh * 2);
                g.DrawEllipse(penTrang, x, y, banKinh * 2, banKinh * 2);

                // Ghi tên đỉnh 
                SizeF labelSize = g.MeasureString((i + 1).ToString(), Font);
                Font font = new Font("UTM Aptima", 12, FontStyle.Bold);
                g.DrawString((i + 1).ToString(), font, slbrTrang, x + banKinh - labelSize.Width / 2 - 3, y + banKinh - labelSize.Height / 2 - 6);
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            // Bật chế độ này để làm mượt đường vẽ, giảm thiểu hiện tượng răng cưa 
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Lấy dữ liệu số đỉnh từ tBox_MaTranKe 
            string textMT = tBox_MaTranKe.Text;
            string[] lines1 = textMT.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string _soDinh = lines1[0];
            int soDinh = Convert.ToInt32(_soDinh);

            // Khai báo 1 graph từ class Prim 
            Dijkstra.GRAPH graph = new Dijkstra.GRAPH(soDinh);

            // Đưa textLuuV thành mảng 1 chiều số nguyên 
            string[] strarrayV = textLuuV.Split(new[] { "\r\n" }, StringSplitOptions.None);
            int[] intarrayV = new int[strarrayV.Length];
            for (int i = 0; i < strarrayV.Length; i++)
            {
                intarrayV[i] = Convert.ToInt32(strarrayV[i]);
            }

            // Đưa textLuuU thành mảng 1 chiều số nguyên 
            string[] strarrayU = textLuuU.Split(new[] { "\r\n" }, StringSplitOptions.None);
            int[] intarrayU = new int[strarrayU.Length];
            for (int i = 0; i < strarrayV.Length; i++)
            {
                intarrayU[i] = Convert.ToInt32(strarrayU[i]);
            }

            // Gộp 2 mảng lại 
            int[] intarr = intarrayV.Union(intarrayU).ToArray();

            int banKinh = 20;
            int centerX = pictureBox1.Width / 2;
            int centerY = pictureBox1.Height / 2;
            int khoangCanh = 110;

            // Tạo các đỉnh dựa theo Kết quả 
            for (int i = 0; i < listBoxDinh.Items.Count; i++)
            {
                int x = centerX + (int)(khoangCanh * Math.Cos(2 * Math.PI * i / listBoxDinh.Items.Count)) - banKinh;
                int y = centerY + (int)(khoangCanh * Math.Sin(2 * Math.PI * i / listBoxDinh.Items.Count)) - banKinh;

                // Vẽ đỉnh 
                if (i == Convert.ToInt32(tBox_ChayTuDinh.Text) - 1 || i == Convert.ToInt32(tBox_ChayDenDinh.Text) - 1)
                {
                    g.FillEllipse(slbrBlue, x, y, banKinh * 2, banKinh * 2);
                }
                else
                {
                    g.FillEllipse(slbrDarkCyan, x, y, banKinh * 2, banKinh * 2); 
                }
                g.DrawEllipse(penTrang, x, y, banKinh * 2, banKinh * 2);

                // Ghi tên đỉnh 
                SizeF labelSize = g.MeasureString((i + 1).ToString(), Font);
                Font font = new Font("UTM Aptima", 12, FontStyle.Bold);
                g.DrawString((i + 1).ToString(), font, slbrTrang, x + banKinh - labelSize.Width / 2 - 3, y + banKinh - labelSize.Height / 2 - 6);
            }

            // Thao tác với cạnh 
            for (int i = 0; i < intarrayV.Length && i < drawCounter; i++)
            {
                int j = i; // Chỉ duyệt qua phần tử tương ứng trong intarrayU
                if (MaTran[intarrayV[i], intarrayU[j]] != 0)
                {
                    int x1 = centerX + (int)(khoangCanh * Math.Cos(2 * Math.PI * intarrayV[i] / listBoxDinh.Items.Count));
                    int y1 = centerY + (int)(khoangCanh * Math.Sin(2 * Math.PI * intarrayV[i] / listBoxDinh.Items.Count));
                    int x2 = centerX + (int)(khoangCanh * Math.Cos(2 * Math.PI * intarrayU[j] / listBoxDinh.Items.Count));
                    int y2 = centerY + (int)(khoangCanh * Math.Sin(2 * Math.PI * intarrayU[j] / listBoxDinh.Items.Count));

                    // Vẽ đường nối giữa 2 đỉnh  
                    // Tạo đối tượng mũi tên
                    AdjustableArrowCap arrowCap = new AdjustableArrowCap(5, 5); 

                    // Tạo đối tượng Pen với mũi tên
                    penDo.CustomEndCap = arrowCap; 

                    // Vẽ đường dẫn với mũi tên
                    g.DrawLine(penDo, x2, y2, x1, y1); 
                }
            }
            if (drawCounter >= listBoxDinh.Items.Count && drawCounter >= intarrayV.Length)
            {
                // Hiệu ứng vẽ đã hoàn thành, dừng Timer
                drawTimer.Stop();
            }
        }
        private int drawCounter = 0; // Biến đếm thời gian 
        private void drawTimer_Tick(object sender, EventArgs e)
        {
            drawCounter++; // Tăng biến đếm thời gian
            pictureBox2.Invalidate(); // Vẽ lại hình ảnh trên pictureBox2
        }
        private void StartDrawing()
        {
            drawCounter = 0;
            drawTimer.Start();
        }
        private void btn_DocFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Thiết lập các thuộc tính của OpenFileDialog
            openFileDialog.Title = "Open Text File";
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                // Đọc nội dung của file và hiển thị vào TextBox
                string fileContent = File.ReadAllText(filePath);
                tBox_MaTranKe.Text = fileContent;
            }

            string textMT = tBox_MaTranKe.Text;

            // Dùng split cắt string để lấy dòng đầu tiên là số đỉnh 
            string[] lines1 = textMT.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string _soDinh = lines1[0];
            // Ép kiểu về số nguyên 
            int soDinh = Convert.ToInt32(_soDinh);

            // Dùng split cắt string để bỏ dòng đầu tiên lấy các dòng sau đó 
            string[] lines2 = textMT.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            // Xoá dòng đầu tiên khỏi mảng lines2 
            lines2 = lines2.Skip(1).ToArray();
            // Kết hợp các chuỗi con lại thành 1 chuỗi đơn 
            string _arr = string.Join(Environment.NewLine, lines2);
            // Tách chuỗi thành từng hàng 
            string[] rows = _arr.Split('\n');
            // Khởi tạo mảng 2 chiều 
            int[,] arr = new int[rows.Length, rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                // Tách hàng thành các phần tử 
                string[] columns = rows[i].Split(' ');
                for (int j = 0; j < columns.Length; j++)
                {
                    // Chuyển đổi chuỗi thành giá trị số nguyên 
                    arr[i, j] = Convert.ToInt32(columns[j]);
                }
            }

            // Gán MaTran = arr vừa tạo & Thêm vào listBox Đỉnh và Cạnh 
            for (int i = 0; i < soDinh; i++)
            {
                for (int j = 0; j < soDinh; j++)
                {
                    MaTran[i, j] = arr[i, j];
                    MaTran[j, i] = arr[j, i];
                    listBoxCanh.Items.Add((i + 1).ToString() + "-" + (j + 1).ToString() + ": " + MaTran[i, j].ToString());
                }
                listBoxDinh.Items.Add("Đỉnh " + (i + 1).ToString());
            }

            pictureBox1.Refresh();
        }

    }
}
