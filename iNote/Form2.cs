using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xNet.Net;

namespace iNote
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e) //Событие загрузки формы.
        {
            string Url = My.UrlAuth;                        //Получение ссылки для запроса авторизации.
            webBrowser1.Navigate(Url);                      //Передача ссылки в браузер.
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e) //Событие загрузки браузера.
        {
            toolStripStatusLabel1.Text = "Загрузка"; //Вывод статуса загрузки.
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) //Событие завершения загрузки браузера.
        {
            toolStripStatusLabel1.Text = "Загружено"; //Вывод статуса загрузки.

            try //Избегание ошибки
            {
                string Url = webBrowser1.Url.ToString(); //Получение ответной ссылки из браузера.  
                string UrlSplit = Url.Split('#')[1];     //Обрезаем ссылку по символы "#".

                if (UrlSplit[0] == 'a')                  //Определяем начало строки.
                {
                    string Token = UrlSplit.Split('&')[0].Split('=')[1]; //Вырезаем из ссылки Токен.
                    string id = UrlSplit.Split('=')[3];                  //Вырезаем из ссылки id, учетнйо записи, с каторой был выполнен вход.

                    My.Token = Token; //Занесение значения в глобальную переменную.
                    My.ID = id;       //Занесение значения в глобальную переменную.
                    My.Auth = true;   //Занесение значения в глобальную переменную.

                    MessageBox.Show("Авторизация прошла успешно!"); //Вывод диалоговог сообщения.

                    this.Close(); //Закрытие формы браузера.
                }
            }
            catch { }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Чистим историю.
            System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 255"); //— полная очистка кэша браузера.
            System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 16");  //— удаление данных веб-форм.
            System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 32");  //— удаление сохраненных паролей.
        }
    }
}
