using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JIPP_MW
{
    public partial class Form1 : Form
    {
        enum ButtonStatus
        {
            Clicked,
            InProgress,
            UnClicked
        }
        ButtonStatus buttonStatus = ButtonStatus.UnClicked;
        public Form1()
        {
            InitializeComponent();
        }

        private async Task WaitForClick()
        {
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
            if (buttonStatus == ButtonStatus.InProgress)
            {
                buttonStatus = ButtonStatus.Clicked;
                return;
            }
            buttonStatus = ButtonStatus.InProgress;
            int amountOfSeconds = Int32.Parse(comboBoxSeconds.SelectedItem.ToString());
            buttonStart.Text = "Wcisnij jak najpozniej, ale przed "+ amountOfSeconds.ToString() +" sekundami";
            var task = WaitForClick();
            if (await Task.WhenAny(task, Task.Delay(TimeSpan.FromSeconds(amountOfSeconds))) == task)
            {
                Console.WriteLine("Brawo");
            }
            else
            {
                Console.WriteLine("Buuu");
            }
            buttonStart.Text = "Start";
            buttonStatus = ButtonStatus.UnClicked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 1; i < 21; i++)
            {
                comboBoxSeconds.Items.Add(i);
            }
        }
    }
}
