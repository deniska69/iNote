using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet.Net;

namespace iNote
{
    class Authorize
    {
        public string Url; //Переменная дл занесений ссылки.      

        public string VkAuth(string Login, string Pass) //Создаём метод авторизации.
        {
            using (var req = new HttpRequest())
            {
                req.UserAgent = ".NET Framework Test Client";     //Указываем эмуляцию платформы авторизации.
                CookieDictionary cookies = new CookieDictionary(false); //Парсим куки.
                req.Cookies = cookies;

                //req.Get(string.Format("https://login.vk.com/?act=login&email={0}&pass={1}", Login, Pass)); //Отправляем Get-запрос авторизации.

                req.Get(My.UrlAuth);
                String Data = req.Get(String.Format(My.UrlAuth)).ToString(); //Заносим в переменную ссылку.

                Url = req.Response.Address.ToString(); //Присваиваем значение переменной  = ссылке, полученой в ответе от vk.com.

                //При помощи функции сплит парсим Токен
                char[] simbol = { '=', '&' };
                string[] strData = req.Response.Address.ToString().Split(simbol);

                if (Url.Contains("#"))     //Проверяем ссылку на наличие символа "#".
                {
                    My.Auth = true;        //Заносим значение в глобальную переменную.
                    My.Token = strData[1]; //Заносим значение в глобальную переменную.
                    My.ID = strData[5];    //Заносим значение в глобальную переменную.
                }

                return strData[1];
            }
        }
    }
}
