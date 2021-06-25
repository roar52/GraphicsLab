using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace GraphicsLab
{
    public partial class GraphicWin : Form
    {
        int beginY;
        int width;
        int max = 0;
        int buildmax;
        float koef;
        MainWin mainWin;
        public GraphicWin()
        {
            InitializeComponent();
        }
        public GraphicWin(MainWin form)
        {
            InitializeComponent();
            this.mainWin = form;
        }
        //изменение размеров окна графика
        private void GraphicWin_Resize(object sender, EventArgs e)
        {
            this.Width = Width - 24;
            this.Height = Height - 55;
            this.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {

                //сделать еще ниже....................................................
                beginY = this.ClientSize.Height / 5 * 4;
                buildmax = Convert.ToInt32(beginY * 0.9);
                mainWin.dataGridView1.AllowUserToAddRows = false;
                //ширина пробелов и столбцов
                width = this.Width / (mainWin.dataGridView1.RowCount);
                //ищем маkсимальное значение из данных
                for (int i = 0; i < mainWin.dataGridView1.RowCount; i++)
                {
                    int tp = Math.Abs(Convert.ToInt32(mainWin.dataGridView1.Rows[i].Cells[1].Value.ToString()));
                    if (max < tp)
                    {
                        max = tp;
                    }
                }

                koef = (float)buildmax / max;
                for (int i = 0; i < mainWin.dataGridView1.RowCount; i++)
                {
                    int val = Convert.ToInt32(Convert.ToInt32(mainWin.dataGridView1.Rows[i].Cells[1].Value.ToString()) * koef);
                    DrawLine(FillPolygon(val, i), i);
                }
                mainWin.dataGridView1.AllowUserToAddRows = true;
            }
            catch
            { mainWin.dataGridView1.AllowUserToAddRows = true; }
        }

        private void GraphicWin_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainWin.Graphic.Enabled = true;
        }

        Point[] FillPolygon(int Value, int i)
        {
            Point[] polygon = new Point[4];

            polygon[0].X = width * i;
            polygon[0].Y = beginY;

            polygon[3].X = polygon[0].X + width - 10;
            polygon[3].Y = beginY;

            polygon[1].X = polygon[0].X;
            polygon[1].Y = beginY - Value;

            polygon[2].X = polygon[0].X + width - 10;
            polygon[2].Y = polygon[1].Y;

            return polygon;
        }
        void DrawLine(Point[] polygon, int i)
        {
            Graphics formGraphics;
            formGraphics = this.CreateGraphics();
            SolidBrush brush = new SolidBrush(Color.DarkSeaGreen);
            GraphicsPath path = new GraphicsPath();
            path.AddLine(polygon[0], polygon[1]);
            path.AddLine(polygon[1], polygon[2]);
            path.AddLine(polygon[2], polygon[3]);
            path.AddLine(polygon[3], polygon[0]);
            formGraphics.FillPath(brush, path);

            string name = mainWin.dataGridView1.Rows[i].Cells[0].Value.ToString();
            string value = mainWin.dataGridView1.Rows[i].Cells[1].Value.ToString();
            Font drawFont = new Font("Segoe UI", 10);
            SolidBrush NameBrush = new SolidBrush(Color.Indigo);
            SolidBrush ValueBrush = new SolidBrush(Color.Black);
            int x = polygon[0].X + ((polygon[3].X - polygon[0].X) / 8);
            int y;
            if (Convert.ToInt32(value) < 0)
            {
                y = polygon[1].Y + 10;
            }
            else
            {
                y = polygon[1].Y - 30;
            }
            formGraphics.DrawString(value, drawFont, NameBrush, x, y);
            x = polygon[0].X + ((polygon[3].X - polygon[0].X) / 3);
            y = polygon[0].Y + 5;
            formGraphics.DrawString(name, drawFont, ValueBrush, x, y);
        }
    }
}
