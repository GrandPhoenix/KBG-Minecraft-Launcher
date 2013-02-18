using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;


namespace KBG_Launcher
{
    class ServerAccessClass
    {
        System.Windows.Forms.Label _label;
        System.Windows.Forms.ProgressBar _progressbar;
        FormMain _parent;
        string _address;
        int _port;

        public ServerAccessClass()
        { }

        public ServerAccessClass(string address,int port, System.Windows.Forms.Label resultLabel, System.Windows.Forms.ProgressBar progressBar,FormMain parent)
        {
            _label = resultLabel;
            _progressbar = progressBar;
            _parent = parent;
            _address = address;
            _port = port;
        }

        public void StartCheck()
        {
            if (!_parent.CloseAllThreads)
            {
                _parent.Invoke(new Action(delegate() { _label.Visible = false; }));
                _parent.Invoke(new Action(delegate() { _progressbar.Visible = true; }));
            }
                        
            System.Net.Sockets.TcpClient client = new TcpClient();
            try
            {                
                client.Connect(_address, _port);  
                //connected
                if (!_parent.CloseAllThreads)
                {
                    _parent.Invoke(new Action(delegate() { _label.ForeColor = Color.Green; }));
                    _parent.Invoke(new Action(delegate() { _label.Text = "Online"; }));
                }
                
            }
            catch (SocketException ex)
            {
                //not connected
                if (!_parent.CloseAllThreads)
                {
                    _parent.Invoke(new Action(delegate() { _label.ForeColor = Color.Red; }));
                    _parent.Invoke(new Action(delegate() { _label.Text = "Offline"; }));
                }
                Debug.WriteLine(string.Format("Connection to {0}:{1} could not be established due to: \n{2}",_address,_port.ToString(),ex.Message));
            }
            finally
            {
                client.Close();
                if (!_parent.CloseAllThreads)
                {
                    _parent.Invoke(new Action(delegate() { _label.Visible = true; }));
                    _parent.Invoke(new Action(delegate() { _progressbar.Visible = false; }));
                }
            }            
            //Thread.CurrentThread.Abort();
        }
    }
}
