using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using xNet.Net;

namespace iNote
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }  
    }

    static class My                               //Объявляем "свой" класс, для глобальных переменных, дабы упростить передачу данных между формами/классами
    {
        internal static bool Auth;                //Состояние авторизация (выполнена/нет).
        internal static string Token;             //Токен.
        internal static string ID;                //id учётной записи, с которой был выполнен вход.
                                                  //Сылка-запрос на авторизацию-получение прав для выполнения дальнейших функций при взаимодействии с vk.com.
        internal static string UrlAuth = "https://oauth.vk.com/authorize?client_id=4918488&scope=friends,wall,groups,messages&redirect_uri=https://oauth.vk.com/blank.html&display=popup&v=5.34&response_type=token";
        internal static string UrlGet;            //Переменная, в которую заносится url(ссылка) для Get-запроса.
        internal static bool ShownClosing = false; //Вкл/выкл. плавное появление/закрытие форм, а также подтверждение выхода из приложения.
        internal static double Opacity = 0.03;    //Переменная отвечающая за скорость плавного открытия/закрытия форм.
        internal static string Text;              //Переменная, в которую "в реальном" времени заносится текст из richTextBox1.
        internal static string UID;               //Переменная, в которую заноссится ID друга/группы.
        internal static string UrlGroups;         //Переменная, в которую заносится Url для получения списка групп.
    }
}
