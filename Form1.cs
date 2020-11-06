using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoltiBGW
{
    //Garatti Elia 11/2020
    public partial class frmMain : Form
    {
        //Globali
        //Crearne uno da codice
        BackgroundWorker bgw1, bgw2;

        //Vettore con bgw
        BackgroundWorker[] bgw_vettore = new BackgroundWorker[100];

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = "Molti BGW";
        }

        private void btnAvvio2_Click(object sender, EventArgs e)
        {
            //Creiamo i due BackGroundWorker e modifichiamone le proprietà nel codice
            //Primo bgw
            bgw1 = new BackgroundWorker();
            bgw1.WorkerReportsProgress = true;
            bgw1.WorkerSupportsCancellation = true;
            bgw1.DoWork += Bgw_DoWork;
            bgw1.ProgressChanged += Bgw_ProgressChanged;
            bgw1.RunWorkerCompleted += Bgw_RunWorkerCompleted;

            //Secondo bgw
            bgw2 = new BackgroundWorker();
            bgw2.WorkerReportsProgress = true;
            bgw2.WorkerSupportsCancellation = true;
            bgw2.DoWork += Bgw_DoWork;
            bgw2.ProgressChanged += Bgw_ProgressChanged;
            bgw2.RunWorkerCompleted += Bgw_RunWorkerCompleted;

            bgw1.RunWorkerAsync(txt2);
            bgw2.RunWorkerAsync(txt2);
        }

        private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var (completato, tb) = (ValueTuple<bool, TextBox>)e.Result;
            tb.Text = completato ? "Conluso" + Environment.NewLine + tb.Text :
                "Cancellato" + Environment.NewLine + tb.Text;
        }

        private void Bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TextBox tb = e.UserState as TextBox;
            tb.Text = e.ProgressPercentage + Environment.NewLine + tb.Text;
        }

        private void btnStop2_Click(object sender, EventArgs e)
        {
            bgw1.CancelAsync();
            bgw2.CancelAsync();
        }

        private void btnAvvio100_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < bgw_vettore.Length; i++)
            {
                bgw1 = new BackgroundWorker();
                bgw1.WorkerReportsProgress = true;
                bgw1.WorkerSupportsCancellation = true;
                bgw1.DoWork += Bgw_DoWork;
                bgw1.ProgressChanged += Bgw_ProgressChanged;
                bgw1.RunWorkerCompleted += Bgw_RunWorkerCompleted;
                bgw_vettore[i] = bgw1;
            }

            foreach (var bgw in bgw_vettore)
            {
                bgw.RunWorkerAsync(txt100);
            }
        }

        private void btnStop100_Click(object sender, EventArgs e)
        {
            foreach (var bgw in bgw_vettore)
            {
                bgw.CancelAsync();
            }
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgw = sender as BackgroundWorker;
            TextBox tb = e.Argument as TextBox;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                bgw.ReportProgress(i, tb);
                if (bgw.CancellationPending)
                {
                    e.Result = (false, tb);
                    return;
                }
            }

            e.Result = (true, tb);
        }
    }
}
