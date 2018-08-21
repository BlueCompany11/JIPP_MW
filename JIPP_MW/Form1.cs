using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace JIPP_MW
{
    public partial class Form1 : Form
    {
        //mozliwe stany buttona
        enum ButtonStatus
        {
            Clicked,
            InProgress,
            UnClicked
        }
        //status buttona zaczynajacego gre
        ButtonStatus buttonStatus = ButtonStatus.UnClicked;
        public Form1()
        {
            InitializeComponent();
        }

        private async Task WaitForClick()
        {
            //funkcja konczy sie gdy buttonStatus == clicked
            while(buttonStatus != ButtonStatus.Clicked)
            {
                await Task.Delay(10);
            }
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            //jesli nic nie jest wybrane w comboboxie to nic nie rob
            if (comboBoxSeconds.SelectedItem == null)
            {
                return;
            }
            //jesli przycisk jest juz wcisniety to zmien status na clicked
            //wtedy w metodzie WaitForClick zakonczy sie petla
            if (buttonStatus == ButtonStatus.InProgress)
            {
                buttonStatus = ButtonStatus.Clicked;
                return;
            }
            buttonStatus = ButtonStatus.InProgress;
            int amountOfSeconds = Int32.Parse(comboBoxSeconds.SelectedItem.ToString());
            buttonStart.Text = "Wcisnij jak najpozniej, ale przed "+ amountOfSeconds.ToString() +" sekundami";
            //uruchamiam w osobynm watku metode do sprawdzania w petli jaki jest status przycisku
            var task = WaitForClick();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //czekam az ktorys task sie zakonczy
            //pierwszy polega na zmianie statusu przycisku - trzeba go ponownie by sie zmienil
            //drugi task polega na czekaniu pewnej ilosci sekund
            //metoda whenany zwraca ten task ktory sie zakonczy jako pierwszy
            //jesli tym taskiem bedzie zmienna task to znaczy, ze to przycisk zostal wcisniety zanim minal czas
            if (await Task.WhenAny(task, Task.Delay(TimeSpan.FromSeconds(amountOfSeconds))) == task)
            {
                sw.Stop();
                //ma byc podkreslone na zielono
                //nie uzywam timera tylko asynchronicznosci
                //resultAnimation to task ktory od razu sie uruchamia przy wywolaniu i kod leci dalej, a animacja sie znajduje w innym watku
                ResultAnimation(true);
                ScoreboardForm scoreboardForm = new ScoreboardForm((int)TimeSpan.FromSeconds(amountOfSeconds).TotalMilliseconds-(int)sw.ElapsedMilliseconds);
                //okienko scoreboardform ma zdarzenie IncorrectNickHandler
                // podpinamy do niego funkcje - gdy zostanite do zdarzenie wywolane - czyli osoba wcisnie przycisk na tamtym okienku nie 
                // wprowadzajac nicku
                scoreboardForm.IncorrectNickHandler += ScoreboardForm_IncorrectNickHandler;
                scoreboardForm.Show();
            }
            else
            {
                sw.Stop();
                ResultAnimation(false);
            }
            buttonStart.Text = "Start";
            buttonStatus = ButtonStatus.UnClicked;
        }

        private void ScoreboardForm_IncorrectNickHandler()
        {
            tableLayoutPanelAnimation.BackColor = Color.Red;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 1; i < 21; i++)
            {
                comboBoxSeconds.Items.Add(i);
            }
        }
        private async Task ResultAnimation(bool result)
        {
            //dx ile przesunac pixeli w prawo
            int dx = 10;
            //4 - ilosc skokow napisu
            for (int i = 0; i < 4; i++)
            {
                string strResult = "Brawo!";
                if (result == false)
                    strResult = "Przegrales";
                Bitmap bmp1 = new Bitmap(tableLayoutPanelAnimation.Width, tableLayoutPanelAnimation.Height);
                Graphics graphics = Graphics.FromImage(bmp1);
                graphics.TranslateTransform(dx, 0);
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                graphics.DrawString(strResult, drawFont, drawBrush, 110, 110, drawFormat);
                tableLayoutPanelAnimation.BackgroundImage = bmp1;
                dx += 100;
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
