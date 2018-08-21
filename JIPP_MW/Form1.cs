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
        bool buttonStatus = false;
        public Form1()
        {
            InitializeComponent();
        }

        private async Task WaitForClick()
        {
            while(buttonStatus != true)
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
            //buttonStatus = !buttonStatus;
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
            //var compleatedTask = Task.WhenAny(new Task[]{ WaitForClick(), Task.Delay(TimeSpan.FromSeconds(amountOfSeconds))});
            //Console.WriteLine("b");
            buttonStart.Text = "Start";
            //buttonStatus = !buttonStatus;
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
