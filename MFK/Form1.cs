using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MFK
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        long loop = 8;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        private void rjButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {
            this.WindowState =  FormWindowState.Minimized;
        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.AddRange(openFileDialog1.FileNames);
            }
        }
        public void shredder()
        {
            BinaryWriter yazar = null;
            long wfc = 0;
            long wd = 0;
            long cwd = 0;
            short nstate = 0;
            try
            {
                foreach (string dosyalar in listBox1.Items)
                {
                    nstate = 1;
                    wfc = loop * new System.IO.FileInfo(dosyalar).Length;
                    string fname = new System.IO.FileInfo(dosyalar).Name;
                    long fsize = new System.IO.FileInfo(dosyalar).Length;
                    System.IO.File.WriteAllBytes(dosyalar, getrandombytes(1000000));
                    yazar = new BinaryWriter(File.Open(dosyalar, FileMode.Open));
                    while (wd < wfc)
                    {
                        if (cwd >= fsize)
                        {
                            yazar.Close();
                            cwd = 0;
                            System.IO.File.WriteAllBytes(dosyalar, getrandombytes(1000000));
                            yazar = new BinaryWriter(File.Open(dosyalar, FileMode.Open));
                        }
                        yazar.Write(getrandombytes(1000000));
                        yazar.Flush();
                        wd = wd + 1000000;
                        cwd = cwd + 1000000;
                        label4.Text = "Shredding: " + fname + " | %" + Math.Round(((double)wd / (double)wfc) * 100, 0).ToString();
                    }
                    yazar.Close();
                    wd = 0;
                    wfc = 0;
                    if (checkBox1.Checked == true)
                    {
                        System.IO.File.Delete(dosyalar);
                    }
                }

            }
            catch (Exception exc)
            {
                    nstate = 0;
                    if (yazar != null)
                    {
                        yazar.Close();
                    }
                    label4.Text = "";
                    MessageBox.Show("An error occurred in Shredder! Error = " + exc.Message + exc.StackTrace, "Multron File Killer", MessageBoxButtons.OK, MessageBoxIcon.Error);

                
            }
            if (nstate == 1)
            {
                label4.Text = "";
                listBox1.Items.Clear();
                MessageBox.Show("Operation Done!", "Multron File Killer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public static byte[] getrandombytes(long size)
        {
            byte[] randombs = new byte[size];
            System.Security.Cryptography.RandomNumberGenerator.Create().GetNonZeroBytes(randombs);
            return randombs;
        }
        private void rjButton2_Click(object sender, EventArgs e)
        {
            System.Threading.Thread s = new System.Threading.Thread(() => shredder());
            s.Start();
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            loop = 4;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            loop = 8;
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            loop = 16;
        }

        private void rjButton4_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Operation ended with a error, these may cause an error's occure: you didnot select any file, error = " + exc.Message + " | You can contact programmer, Discord = dexter_morgan3169", "Multron File Killer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rjButton5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            loop = 1;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("If you want to Strength-Speed balance, Then We recommend Standard. Quick is fast and less secure but it's still ressistant to File-Recovery Programs. One-Time Pass is fastest but it's much less secure than others. Strong is strongest but it's slowest.", "Multron File Killer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
