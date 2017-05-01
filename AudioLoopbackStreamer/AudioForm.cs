using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioLoopbackStreamer
{
    public partial class MainForm : Form
    {
        private AudioHandler _handler;
        private TcpStream _tcpStream;
        private bool _recording = false;

        public MainForm()
        {
            InitializeComponent();
            _handler = new AudioHandler();
            _tcpStream = new TcpStream();
            _tcpStream.Listen();
        }

        private void goBtn_Click(object sender, EventArgs e)
        {
            if (!_recording)
            {
                goBtn.Text = "Stop";
                statusLbl.Text = "status: yes";

                _handler.StartRecording(_tcpStream);
            } else
            {
                goBtn.Text = "Go";
                statusLbl.Text = "status: no";
                _handler.StopRecording();
            }
            _recording = !_recording;
        }
    }
}
