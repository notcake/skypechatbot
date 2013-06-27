using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using SKYPE4COMLib;

namespace ChatBot
{
    public partial class Main : Form
    {
        Skype Skype = null;
        DateTime LastResponseTime = DateTime.Now;

        Logger Logger;
        MessageHandler MessageHandler;

        public Main()
        {
            this.InitializeComponent();

            this.Logger = new Logger();
            this.MessageHandler = new MessageHandler(this.Logger);

            this.Logger.MessageLogged += delegate(string message)
            {
                if (this.Log.TextLength > 0)
                {
                    this.Log.AppendText("\n");
                }
                this.Log.AppendText(message);
                this.Log.ScrollToCaret();
            };

            System.Windows.Forms.Application.ThreadException += delegate(object sender, ThreadExceptionEventArgs e)
            {
                this.Logger.Log("Unhandled Exception: " + e.Exception.ToString());
            };

            this.NotifyIcon.Icon = this.Icon;

            // Handlers
            this.MessageHandler.AddSpanHandler(new PastebinUrlSpanHandler());
            this.MessageHandler.AddSpanHandler(new ShortenedUrlSpanHandler());
            this.MessageHandler.AddSpanHandler(new TwitterUrlSpanHandler());
            this.MessageHandler.AddSpanHandler(new VimeoUrlSpanHandler());
            this.MessageHandler.AddSpanHandler(new YouTubeUrlSpanHandler());
            
            // Skype
            this.Skype = new SKYPE4COMLib.Skype();

            if (!Skype.Client.IsRunning)
            {
                Skype.Client.Start();
            }

            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "http://redd.it/1dko1n");
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "https://www.youtube.com/watch?v=psuRGfAaju4");
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "http://youtu.be/1uV63nApaYA");
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "http://pastebin.com/8jNskG2r");
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "http://pastebin.com/8jNskG2rasdsad");
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "http://goo.gl/jmzTY");
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "http://vimeo.com/61930364#");

            try
            {
                this.Skype.Attach();
            }
            catch (COMException)
            {
                this.Logger.Log("Failed to attach to Skype.");
            }
            this.Skype.MessageStatus += delegate(ChatMessage message, TChatMessageStatus status)
            {
                try
                {
                    if (status == TChatMessageStatus.cmsReceived || status == TChatMessageStatus.cmsSent)
                    {
                        if ((DateTime.Now - this.LastResponseTime).TotalMilliseconds < 1000) { return; }

                        MessageSink messageSink = x =>
                        {
                            this.Logger.Log("Sending message:\n\t" + x.Replace("\n", "\n\t"));
                            message.Chat.SendMessage(x);
                            this.LastResponseTime = DateTime.Now;
                        };
                        this.MessageHandler.HandleMessage(messageSink, message.Body);
                    }
                }
                catch (Exception e)
                {
                    this.Logger.Log("Unhandled Exception: " + e.ToString());
                }
            };
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
            }
        }

        private void ShowNotifyMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}