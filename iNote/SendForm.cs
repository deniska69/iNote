using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using xNet.Net;

namespace iNote
{
    public partial class SendForm : Form
    {
        #region Объявление переменных
        
        double Opcty = My.Opacity;                                        //Переменная для плавного перехода форм (см. Program.cs).
        public List<Friends> FriendsList;                                 //Объявлем класс-лист для списка друзей.
        public List<Groups> GroupList;                                    //Объявлем класс-лист для списка групп.
        string ID;                                                        //ID учётной записи, с которой была выполнена авторизация.
        string Token;                                                     //Токен.
        string TextSend;                                                  //Переменная, в которую заносится текст из блокнота, который отправляется.
        string UID;                                                       //ID друга, группы.
        int SU;
        #endregion

        public SendForm()
        {
            InitializeComponent(); 
        }

        #region Открытие/закрытие формы

        private void SendForm_Shown(object sender, EventArgs e)                            //Событие открытия формы.
        {
            if (My.ShownClosing == true)                        //Проверка глобольной переменной.
            {
                ((Control)sender).Refresh();                    //Обрабочтик плавного открытия.
                for (Opacity = 0; Opacity < 1; Opacity += Opcty, System.Threading.Thread.Sleep(10)) ;
            }
        }

        private void SendForm_FormClosing(object sender, FormClosingEventArgs e)           //Событие закрытия формы.
        {
            if (My.ShownClosing == true)                                                   //Проверка глобольной переменной (см. Program.cs).
            {
                for (; Opacity > 0; Opacity -= Opcty, System.Threading.Thread.Sleep(10)) ; //Обрабочтик плавного закрытия.
            }
        } 
        #endregion

        #region Проверка кол-ва адресатов
        private void ProvAdUs() //Проверка кол-ва выбранных друзей
        {
            
            if (SU > 0)
            {
                label1.Text = "Кол-во выбранных друзей: " + Convert.ToString(SU);
                button1.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                label1.Text = "Выберите хотя бы одного друга!";            //Проверка не выполнена, вывод сообщения.
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }
        private void ProvAdGr() //Проверка кол-ва выбранных групп
        {
            if (SU > 0)
            {
                label1.Text = "Кол-во выбранных групп: " + Convert.ToString(SU);
                button1.Enabled = true;
                
            }
            else
            {
                label1.Text = "Выберите хотя бы одну группу!";            //Проверка не выполнена, вывод сообщения.
                button1.Enabled = false;
                
            }
        }
        #endregion
        
        private string ApiMethod()                                        //Метод Get-запрос и получение JSON-ответа.
        {
            using (var req = new HttpRequest())
            {
                return req.Get(My.UrlGet).ToString();
            }
        }

        #region Объявление форм для списков
        
        public class Friends                                              //Задаём форму для списка друзей.
        {
            public int UID { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
        }
          
        public class Groups                                               //Задаём форму для списка групп.
        {
            public int GID { get; set; }
            public string Name { get; set; }
        }
        
        #endregion

        #region Отправка сообщений
        
        private void button1_Click(object sender, EventArgs e)            //Событе отправки текста на стену.
        {
            Token = My.Token;
            TextSend = My.Text;
            foreach (int SUi in checkedListBox1.CheckedIndices)           //Отправка сообщений друзъям по 1.
            {
                My.UID = "";                                              //Обнуление списка друзей.
                My.UID = FriendsList[SUi].UID.ToString();                 //Добавление адресата
                    label1.Text = "Отправляется...";
                    using (var req = new HttpRequest())                   //отправка сообщений.
                    {
                        req.Get(String.Format("https://api.vk.com/method/wall.post?owner_id={0}&message={1}&access_token={2}", My.UID, TextSend, Token)).ToString();
                    }
                    label1.Text = "Отправлено";                           //Отображение статуса отправки.
                
            }
        }

        private void button2_Click(object sender, EventArgs e)            //Событе отправки текста в ЛС.
        {
            My.UID = "";                                                  //Обнуление списка друзей

            foreach (int SUi in checkedListBox1.CheckedIndices)           //Создание списка друзей по Checklistbox'у
            {
                My.UID = FriendsList[SUi].UID.ToString() + "," + My.UID;
            }

            Token = My.Token;
            TextSend = My.Text;
            UID = My.UID;

            if (UID != "")                                                //Проверка кол-ва выбранных друзей
            {
                label1.Text = "Отправляется...";
                
                using (var req = new HttpRequest())                       //Проверка выполнена, отправка сообщений
                {
                    req.Get(String.Format("https://api.vk.com/method/messages.send?user_ids={0}&message={1}&access_token={2}", UID, TextSend, Token)).ToString();
                }
                
                label1.Text = "Отправлено";                               //Отображение статуса сообщений
            }

            else
            {
                label1.Text = "Выберите хотя бы одного друга!";           //Проверка не выполнена, вывод сообщения
            }
        }  
        
        #endregion

        #region События
        private void SendForm_Load(object sender, EventArgs e)            //Событие загрузки формы.
        {
            ID = My.ID;                                                   //Получаем значения из глобальных переменных.
            Token = My.Token;                                             //Получаем значения из глобальных переменных.
            TextSend = My.Text;                                           //Получаем значения из глобальных переменных.

            #region Получаем список друзей в checkedListBox1

            //Формируем в глобальную переменную - ссылку для запроса списка друзей.
            My.UrlGet = "https://api.vk.com/method/friends.get?user_id=" + ID + "&order=hints&fields=name&access_token=" + Token;

            checkedListBox1.Items.Clear();                                //Очищаем listBox1 (на всякий случай).
            string UrlFriends = ApiMethod();                              //Заносим в строковую переменную ответ полученный из метода "ApiMethod".

            JToken UrlParseFriends = JToken.Parse(UrlFriends);            //Определяем переменную, дял парсинга.

                                                                          //Производим парсинг переменной.
            FriendsList = UrlParseFriends["response"].Children().Select(c => c.ToObject<Friends>()).ToList();

            this.Invoke((MethodInvoker)delegate                           //Заносим в checkedListBox1 список друзей.
            {
                for (int i = 0; i < FriendsList.Count(); i++)
                {
                    checkedListBox1.Items.Add(FriendsList[i].First_Name + " " + FriendsList[i].Last_Name);
                }
            });
            
            #endregion

            #region Получаем список групп в checkedListBox2

            //Формируем в глобальную переменную - ссылку для запроса списка групп.
            My.UrlGet = "https://api.vk.com/method/groups.get?user_id=" + ID + "&extended=1&access_token=" + Token;

            checkedListBox2.Items.Clear();                                //Очищаем checkedListBox2 (на всякий случай).
            
            //Ужас, а не метод (получаем "нормальный" JSON-ответ от сервера):
            string UrlGroups = ApiMethod();
            string UrlGroups1 = UrlGroups.Substring(0, 13);                        
            int n = UrlGroups.IndexOf("[");
            string UrlGroups2 = UrlGroups.Substring(n, UrlGroups.Length - n);
            n = UrlGroups2.IndexOf("{");
            UrlGroups2 = UrlGroups2.Substring(n, UrlGroups2.Length - n);

            My.UrlGroups = UrlGroups1 + UrlGroups2;

            JToken UrlParseGroups = JToken.Parse(My.UrlGroups);           //Парсим полученный ответ.
                                                                          //Формируес список групп.
            GroupList = UrlParseGroups["response"].Children().Select(c => c.ToObject<Groups>()).ToList();

            this.Invoke((MethodInvoker)delegate                           //Заносим в checkedListBox2 список друзей.
            {
                for (int i = 0; i < GroupList.Count(); i++)
                {
                    checkedListBox2.Items.Add(GroupList[i].Name);
                }
            });
            
            #endregion
        }
        
        private void tabPage1_Enter(object sender, EventArgs e)           //Событие выбора (в фокус) списка друзей.
        {
            foreach (int i in checkedListBox2.CheckedIndices)             //Снимаем выделение с галочек в checkedListBox2.
            {
                checkedListBox2.SetItemChecked(i, false);
            }

            My.UID = "";                                                  //Обнуляем переменную.
            SU = 0;
            ProvAdUs();
            //Отключаем кнопки.
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void tabPage2_Enter(object sender, EventArgs e)           //Событие выбора (в фокус) списка групп.
        {
            foreach (int i in checkedListBox1.CheckedIndices)             //Снимаем выделение с галочек в checkedListBox1.
            {
                checkedListBox1.SetItemChecked(i, false);
            }

            My.UID = "";                                                  //Обнуляем переменную.
            SU = 0;
            ProvAdGr();
            //Отключаем кнопки.
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)            //Кнопка выбрать всех.
        {
            if(tabPage1.Focus())
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
                SU = checkedListBox1.Items.Count;
                ProvAdUs();
            }

            else
            {
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    checkedListBox2.SetItemChecked(i, true);
                }
                SU = checkedListBox2.Items.Count;
                ProvAdGr();
            }
        }

        private void button4_Click(object sender, EventArgs e)            //Кнпока убрать выделение.
        {
            if (tabPage1.Focus())
            {
                foreach (int i in checkedListBox1.CheckedIndices)        //Снимаем выделение с галочек в checkedListBox1.
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
                label1.Text = "Выберите хотя бы одного друга!";
                SU = 0;
            }
            else
            {
                foreach (int i in checkedListBox2.CheckedIndices)        //Снимаем выделение с галочек в checkedListBox2.
                {
                    checkedListBox2.SetItemChecked(i, false);
                }
                label1.Text = "Выберите хотя бы одну группу!";
                SU = 0;
            }

            //Отключаем кнопки.
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)//Событие выбора друга из CheckedListBox1
        {
            if (e.NewValue == CheckState.Checked)//проверка состояния выбранного пункта
            {
                SU++;
                ProvAdUs();
            }
            else
            {
                SU--;
                ProvAdUs();
            }
        }

        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e) //Событие выбора группы из CheckedListBox2
        {
            if (e.NewValue == CheckState.Checked) //проверка состояния выбранного пункта
            {
                SU++;
                ProvAdGr();
            }
            else
            {
                SU--;
                ProvAdGr();
            }
        }
        #endregion




    }
}
