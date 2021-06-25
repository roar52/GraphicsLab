using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicsLab
{
    public partial class MainWin : Form
    {
        public bool WideScreen;
        public MainWin()
        {
            InitializeComponent();
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "Value";
            Save.Click += Saving;
            Clear.Click += Creating;
            Options.Click += ChangeWinSizeClick;
            Graphic.Click += Graphing;
            textBox1.Text = "5000";
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files[0].Substring(files[0].Length - 4) != ".csv")
            {
                MessageBox.Show("Перетяните файл с расширением .csv");
            }
            else
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                TextFieldParser parser = new TextFieldParser(files[0]);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                string[] fields = parser.ReadFields();
                dataGridView1.ColumnCount = fields.Length;
                for (int i = 0; i < fields.Length; i++)
                {
                    dataGridView1.Columns[i].Name = fields[i];
                }
                //вносим значения
                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();
                    dataGridView1.Rows.Add(fields);
                }
            }
        }
        //добавляет в эффекты таблицы
        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }
        private void Saving(object sender, EventArgs e)
        {

            //запрет на добавление строк
            dataGridView1.AllowUserToAddRows = false;
            var table = "";
            //добавление заголовков
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                if (i != dataGridView1.ColumnCount - 1)
                {
                    table += dataGridView1.Columns[i].Name + ", ";
                }
                else
                {
                    table += dataGridView1.Columns[i].Name + "\n";
                }
            }
            //добавление элементов
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int k = 0; k < dataGridView1.ColumnCount; k++)
                {
                    if (k == dataGridView1.ColumnCount - 1)
                    {
                        try
                        {
                            table += dataGridView1.Rows[i].Cells[k].Value.ToString() + "\n";
                        }
                        catch
                        {
                            table += "" + "\n";
                        }
                    }
                    else
                    {
                        try
                        {
                            table += dataGridView1.Rows[i].Cells[k].Value.ToString() + ", ";
                        }
                        catch
                        {
                            table += "" + ", ";
                        }
                    }
                }
            }
            //сохраняем файл по данным и пути
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.ShowDialog();
            string filename = saveFileDialog.FileName;
            System.IO.File.WriteAllText(filename, table);
            //разрешаем вносить элементы в таблицу
            dataGridView1.AllowUserToAddRows = true;

            MessageBox.Show("Файл сохранен");
        }
        private void Creating(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "Value";
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AllowUserToAddRows = false;
                var table = "";
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    if (i != dataGridView1.ColumnCount - 1)
                    {
                        table += dataGridView1.Columns[i].Name + ", ";
                    }
                    else
                    {
                        table += dataGridView1.Columns[i].Name + "\n";
                    }
                }
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for (int k = 0; k < dataGridView1.ColumnCount; k++)
                    {
                        if (k == dataGridView1.ColumnCount - 1)
                        {
                            try
                            {
                                table += dataGridView1.Rows[i].Cells[k].Value.ToString() + "\n";
                            }
                            catch
                            {
                                table += "" + "\n";
                            }
                        }
                        else
                        {
                            try
                            {
                                table += dataGridView1.Rows[i].Cells[k].Value.ToString() + ", ";
                            }
                            catch
                            {
                                table += "" + ", ";
                            }
                        }
                    }
                }
                System.IO.File.WriteAllText("C:\\Users\\Ruslan\\source\\repos\\GraphicsLab\\autosave.csv", table);
                dataGridView1.AllowUserToAddRows = true;
            }
            catch
            { }
        }
        private void Graphing(object sender, EventArgs e)
        {
            Graphic.Enabled = false;
            GraphicWin form3 = new GraphicWin(this);
            form3.Show();
        }

        private void ChangeWinSizeClick(object sender, EventArgs e)
        {
            if (!WideScreen)
            {
                this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                this.panel1.Width = Screen.PrimaryScreen.Bounds.Width;
                this.dataGridView1.Size = new Size(Screen.PrimaryScreen.Bounds.Width-500, 1000);
                WideScreen = true;
            }
            else 
            {
                this.SetBounds(0, 0, 800, 496);
                this.panel1.Width = 800;
                this.dataGridView1.Size = new Size(426, 426);
                WideScreen = false;
            }
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoSave.Checked)
            {
                timer1.Start();
                timer1.Interval = Convert.ToInt32(textBox1.Text);
            }
            else
            {
                timer1.Stop();
            }
        }
    }
}
