using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace iNote
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        Form2 Form2 = new Form2();                             //Переменная для обращения к Form2 (форма авторизации через браузер).
        Authorize Authorize = new Authorize();                 //Переменная для обращения к классу авторизации через логин-пароль.
        double Opcty = My.Opacity;                             //См. Program.cs

        private void button1_Click(object sender, EventArgs e) //Обработчик события нажатая на кнопку "Вход".
        {
            label3.Text = "           Авторизация...";         //Статус процесса авторизации.
            Authorize.VkAuth(textBox1.Text, textBox2.Text);    //Обращение к методу "VkAuth" из класса "Authorize", передача ему логина и пароля из textBox1/textBox2.

            if(My.Auth == true)                                //Проверка глобальной переменной статуса авторизации.
            {
                label3.Text = "Авторизация прошла успешно!";   //Вывод на форму статуса авторизации.
            }

            else                                               //При условии, что авторизация "логин-пароль" не прошла, (это происходит только в том случае,
            {                                                  //если учётная запись Вконтакте вводится впервые в эту программу). Потом работает вход через логин-пароль.
                Form2.ShowDialog();                            //Открывает форму авторизации через браузер
            }
        }

        #region Открытие/закрытие формы
        private void Auth_Shown(object sender, EventArgs e)    //Событие открытия формы.
        {
            if (My.ShownClosing == true)                       //Проверка глобольной переменной (см. Program.cs).
            {
                ((Control)sender).Refresh();                   //Обрабочтик плавного открытия.
                for (Opacity = 0; Opacity < 1; Opacity += Opcty, System.Threading.Thread.Sleep(10)) ;
            }
        }

        private void Auth_FormClosing(object sender, FormClosingEventArgs e)               //Событие закрытия формы.
        {
            if (My.ShownClosing == true)                                                   //Проверка глобольной переменной (см. Program.cs).
            {
                for (; Opacity > 0; Opacity -= Opcty, System.Threading.Thread.Sleep(10)) ; //Обрабочтик плавного закрытия.
            }

            if(checkBox2.Checked == true)
            {
                Properties.Settings.Default.SaveAuth = true;
                Properties.Settings.Default.Login = textBox1.Text;
                Properties.Settings.Default.Password = textBox2.Text;
            }

            if(checkBox2.Checked == false)
            {
                Properties.Settings.Default.SaveAuth = false;
                Properties.Settings.Default.Login = "";
                Properties.Settings.Default.Password = "";
            }

            Properties.Settings.Default.Save();
        }    
        #endregion

        private void Auth_Load(object sender, EventArgs e)     //Обрабочтик события загрузки формы.
        {
            label3.Text = "      Введите свои данные:";        //Вывод текста в label3-статус авторизации.

            textBox2.UseSystemPasswordChar = true;
            checkBox1.Checked = false;

            if(Properties.Settings.Default.SaveAuth == true)
            {
                checkBox2.Checked = true;
                textBox1.Text = Properties.Settings.Default.Login;
                textBox2.Text = Properties.Settings.Default.Password;
            }

            if(Properties.Settings.Default.SaveAuth == false)
            {
                checkBox2.Checked = false;
                textBox1.Text = "";
                textBox2.Text = "";
            }

            timer1.Start();                                    //Запуска таймера.
        }

        private void timer1_Tick(object sender, EventArgs e)   //Обрабочтик тика таймера.
        {
            if (My.Auth == true)                               //Проверка глобальной переменной (см. Program.cs).
            {
                label3.Text = "Авторизация прошла успешно!";   //Вывод текста в label3-статус авторизации.
            }

            if(checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }

            if(checkBox1.Checked == false)
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
    }
}
