using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;//класс с потоками
using System.IO;//класс для ввода, вывода

namespace product_key_maker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void Button1_Click(object sender, EventArgs e)
        {
            ChooseStart();//Самый первый метод для проверки выбранных параметров. Он чуть позже...
        }
        public string CreateKey(int length)
        {
            //из чего собираем ключ
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();//объект класса StringBuilder. Будет собирать нам ключ
            Random rnd = new Random();//Объект класса Random. Будет выбирать нам из, выше приведённых символов - случайные
            //и отдавать их построителю строки
            for (int i = 0; i < length; i++)
            {
                res.Append(valid[rnd.Next(valid.Length)]);//Генерируем строку со случайными символами
            }
            //Так как наш метод должен вернуть строку - возвращаем нашу построенную строку с символами
            return res.ToString();//Не забываем про типы данных. res нужно преобразовать в строку методом ToString
        }
        //теперь опишем метод записи в файл
        public void WriteFile(string keys)//Да, опечатка. Метод ничего не должен возвращать
            //но должен получить то, что будет писать в файл - готовый ключик с "тире"
        {
            //Подключаем класс с потоками
            using (StreamWriter stream = new StreamWriter("keys.txt", true))//создаём объект класса StreamWriter
                                                                            //и можем писать что-то в файл
            {
                stream.WriteLine(keys);//keys у нас будет содержать созданный ключ а метод WriteLine запишет его как строку
                //а файл keys.txt и выполнит перевод на новую строку. Как закончим запись - закроем поток.
                stream.Close();
            }
        }
            public void KeyMaker(int num, int SymNum)//это метод создания ключа с "тире" из набора случайных символов,
                //которые нас создал построитель строк StringBuilder из предыдущего метода
            {
                string keys = "";//тут будет наша строка символов
                button1.Enabled = false;//на время работы - блокируем нашу кнопку

                //Да, к ошибкам потом вернёмся...
                for (int i = 1; i <= num; i++)//num - это сколько ключей мы хотим создать
                {
                    keys = CreateKey(SymNum);//SymNum - это сколько символов в нём будет, не считая знак "тире"
                    for (int j = 4; j < keys.Length; j += 5)
                        if (keys[j] != '-')//мы создаём группы по 5 символов и на 6-ом, если не "тире" - заменяем символ на 
                            //"тире"
                        {
                            keys = keys.Insert(++j, "-");//замена происходит тут
                        }
                    //Если оставить этот метод "как он есть", то в конце каждого ключика - тоже будет "тире".
                    //Давайте это исправим. Просто получим последний символ ключа и удалим его.
                    int x1 = keys.Length - 1;//вытаскием индекс последнего символа (это всегда будет "тире")
                    keys = keys.Remove(x1);//и просто удаляем его с помощью метода Remove().

                    WriteFile(keys);//вызываем метод записи в файл нашего готового ключика. А сколько раз записывать
                    //мы определили во внешнем цикле for.
                    //внутренний for говорит сколько групп символов в ключе нам нужно создать
                    keys = "";//обнуляем область памяти для следующего ключика
                    Thread.Sleep(15);//задержка нужна, чтобы ключи не повторялись, так как for работает быстрее, чем мы 
                    //успеваем сгенерировать новый ключ


                }
            button1.Enabled = true;//разлочили кнопку
            }
        //теперь пишем метод, который вызывали в самом начале...
        public void ChooseStart()
        {
            int kol = 0;//кол-во ключей
            int len = 0;//кол-во символов без "тире"
            string ch = "";//выбранный элемент, пренадлежащий первому GroupBox1
            string sch = "";//тоже и для второго

            //теперь нам нужно узнать, какие радио кнопки были выбраны на форме и получить из название (у нас это числа)
            foreach (Control control in groupBox1.Controls)
            {
                //Проверям принадлежность элемента в groubox1 к классу RadioButton
                if (control.GetType() == typeof(System.Windows.Forms.RadioButton))
                {
                    //создаём отдельный (именованный) объект класса RadioButton
                    RadioButton rbControl = (RadioButton)control;
                    //получаем текст, выбранной радиокнопки. Мы нажали на форме - программа об этом узнала
                    if (rbControl.Checked)
                    {
                        ch = rbControl.Text;//это просто название, которое мы дали в свойствах. 
                        //естественно далее нам нужно перевести его в целое число (это другой тип данных). 
                        //мы можем это сделать, потому что у нас название состоит из чисел. С буквами ТАК НЕЛЬЗЯ!!!!
                    }
                }
            }//foreach
            //теперь сделаем абсолютно тоже самое, но для другого GroupBox (у нас их 2 на форме)
            foreach (Control control in groupBox2.Controls)
            {
                //Проверям принадлежность элемента в groubox1 к классу RadioButton
                if (control.GetType() == typeof(System.Windows.Forms.RadioButton))
                {
                    //создаём отдельный (именованный) объект класса RadioButton
                    RadioButton rbControl = (RadioButton)control;
                    //получаем текст, выбранной радиокнопки. Мы нажали на форме - программа об этом узнала
                    if (rbControl.Checked)
                    {
                        sch = rbControl.Text;//это просто название, которое мы дали в свойствах. 
                        //естественно далее нам нужно перевести его в целое число (это другой тип данных). 
                        //мы можем это сделать, потому что у нас название состоит из чисел. С буквами ТАК НЕЛЬЗЯ!!!!
                    }
                }
            }//foreach
            //готово. Теперь производим преобразование типов (имя радио кнопка как число для машины) и
            //полученное число будем передавать в наши цыклы

            //зачем это? Как вам такая конструкция:

            //ЖУТЬ!!! Мы так делать не будем.
            kol = Convert.ToInt32(ch);//выбранная радио кнопка из первой группы (её номер)
            len = Convert.ToInt32(sch);//выбранная радио кнопка из второй группы (её номер)

            if ((kol == 0) || (len == 0))
            {
                MessageBox.Show("Вы ничего не выбрали");//тут опишите защиту от тех, кто каким-то образом ничего не выбрал
            }
            else
            {
                KeyMaker(kol, len);//вызываем наш метод создания ключей и отдаём ему кол-во ключей и кол-во символов без
                //"тире"
            }
            //А ВСЁ! Мы закончили
        }
        
    }
}
