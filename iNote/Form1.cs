using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace iNote
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Переменные для обращения к формам и классам.
        
        Authors Authors = new Authors();
        Auth Auth = new Auth();
        SendForm SendForm = new SendForm();
        
        #endregion

        #region Файл
        
        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Текстовый документ(.txt)|*.txt";
            sfd.FileName = "Безымянный iNote";

            if (richTextBox1.TextLength != 0)
            {
                if (MessageBox.Show("В iNote есть текст, сохранить его?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox1.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText);
                        this.Text = sfd.FileName;
                    }
                }
                else
                {
                    richTextBox1.Clear();
                    this.Text = "iNote";
                }
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Текстовый документ(.txt)|*.txt";
            sfd.FileName = "Безымянный iNote";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Текстовый документ(.txt)|*.txt";

            if (richTextBox1.TextLength != 0)
            {
                if (MessageBox.Show("В iNote есть текст, сохранить его?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox1.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText);
                        this.Text = sfd.FileName;
                    }

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox1.LoadFile(ofd.FileName, RichTextBoxStreamType.PlainText);
                        this.Text = ofd.FileName;
                    }
                }
            }

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(ofd.FileName, RichTextBoxStreamType.PlainText);
                this.Text = ofd.FileName;
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Текстовый документ(.txt)|*.txt";
            sfd.FileName = "Безымянный iNote";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText);
                this.Text = sfd.FileName;
            }  
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        #endregion

        #region Правка
        
        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void повторитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }

        private void выделитьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void датаВремяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + System.DateTime.Now.ToString();
        }
        
        #endregion

        #region Формат
       
        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = richTextBox1.SelectionFont;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionFont = fd.Font;
            }
        }

        private void цветТекстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = cd.Color;
            }
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = cd.Color;
            }
        }

        private void сбросНастроекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.White;
            richTextBox1.ForeColor = Color.Black;
            richTextBox1.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Regular);      
        }
       
        #endregion

        #region Открытие/закрытие формы
        
        private void Form1_Shown(object sender, EventArgs e)
        {
            if (My.ShownClosing == true)
            {
                ((Control)sender).Refresh();
                for (Opacity = 0; Opacity < 1; Opacity += .03, System.Threading.Thread.Sleep(10));
            }      
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (My.ShownClosing == true)
            {
                if (MessageBox.Show("Вы уверены, что хотите выйти?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }

                else
                {
                    e.Cancel = false;
                }
            }

            //Перед закрытием приложения заносим в настроки:
            Properties.Settings.Default.CrlBack = this.richTextBox1.BackColor;  //Цвет фона
            Properties.Settings.Default.CrlFont = this.richTextBox1.ForeColor;  //Цвет текста
            Properties.Settings.Default.Font = this.richTextBox1.SelectionFont; //Шрифт
            My.Token = "";
            My.ID = "";
            My.Auth = false;

            Properties.Settings.Default.Save();

            timer1.Stop();

            if(My.ShownClosing == true)
            {
                for (; Opacity > 0; Opacity -= .03, System.Threading.Thread.Sleep(10));
            }
        }
        
        #endregion

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Authors.ShowDialog();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Кол-во строк: " + richTextBox1.Lines.Count();
            toolStripStatusLabel2.Text = "Кол-во символов: " + richTextBox1.Text.Length.ToString();
            My.Text = richTextBox1.Text;
        }    

        private void входToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Auth.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)  //Событие загрузки формы.
        {
            //Подгружаем настройки.
            richTextBox1.BackColor = Properties.Settings.Default.CrlBack; //Цвет фона richTextBox1.
            richTextBox1.ForeColor = Properties.Settings.Default.CrlFont; //Цвет шрифта.
            richTextBox1.Font = Properties.Settings.Default.Font;         //Шрифт.

            //Обнуляем переменные.
            My.Token = "";
            My.ID = "";

            timer1.Start(); //Запускаем таймер.

            отправитьToolStripMenuItem.Enabled = false; //Вырубаем кнопку "Отправить..".

        }

        private void timer1_Tick(object sender, EventArgs e) //Событие тика таймера.
        {
            if (My.Auth == true) //Проверка глобальной переменной (см. Programm.cs).
            {
                отправитьToolStripMenuItem.Enabled = true; //Вкл. кнопку "Отправить".
            }
        }

        private void отправитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendForm.ShowDialog();
        }
    }
}