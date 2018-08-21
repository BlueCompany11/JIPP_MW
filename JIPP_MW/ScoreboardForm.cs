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
    public partial class ScoreboardForm : Form
    {
        //ilosc milisekund pozostalych do zakonczenia czasu
        int miliseconds;
        public delegate void IncorrectNick();
        public event IncorrectNick IncorrectNickHandler;
        public ScoreboardForm()
        {
            InitializeComponent();
        }
        public ScoreboardForm(int miliseconds):this()
        {
            this.miliseconds = miliseconds;
        }

        private void ScoreboardForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'scoreboardDbDataSet.Table' table. You can move, or remove it, as needed.
            this.tableTableAdapter.Fill(this.scoreboardDbDataSet.Table);

        }

        private void buttonEnterNick_Click(object sender, EventArgs e)
        {
            string nick = textBoxNick.Text;
            if (nick == "")
            {
                if (IncorrectNickHandler != null)
                {
                    IncorrectNickHandler();
                }
                return;
            }
            //zapis do bazy
            Model1 model1 = new Model1();
            model1.Tables.Add(new Table { Nick = nick, Time = miliseconds });
            model1.SaveChanges();
            this.tableTableAdapter.Fill(this.scoreboardDbDataSet.Table);
        }
    }
}
