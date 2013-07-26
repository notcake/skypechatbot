using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
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
        bool AttachedToSkype = false;

        DateTime LastResponseTime = DateTime.Now;

        Logger Logger;

        CommandDispatcher CommandDispatcher;
        MessageHandler MessageHandler;

        public Main()
        {
            this.InitializeComponent();

            this.Logger = new Logger();
            this.CommandDispatcher = new CommandDispatcher(this.Logger);
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

            // Command and Message Handlers
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsClass) { continue; }
                    if (type.IsAbstract) { continue; }

                    // Command Handlers
                    if (typeof(ICommandHandler).IsAssignableFrom(type))
                    {
                        ConstructorInfo constructorInfo = type.GetConstructor(new Type[0] { });
                        if (constructorInfo == null) { continue; }

                        object commandHandler = constructorInfo.Invoke(null);
                        this.CommandDispatcher.AddCommandHandler((ICommandHandler)commandHandler);
                    }

                    // Message Handlers
                    if (typeof(IMessageSpanHandler).IsAssignableFrom(type))
                    {
                        ConstructorInfo constructorInfo = type.GetConstructor(new Type[0] { });
                        if (constructorInfo == null) { continue; }

                        object messageSpanHandler = constructorInfo.Invoke(null);
                        this.MessageHandler.AddSpanHandler((IMessageSpanHandler)messageSpanHandler);
                    }
                }
            }
            
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
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "https://twitter.com/garrynewman/status/360042767461789698");
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "3000€");
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "£20");

            this.ConnectToSkype();
        }

        private static char[] randomZeroWidthCharacters = {
                                                              (char)0x200B,
                                                              (char)0x200C,
                                                              (char)0x200D,
                                                              (char)0xFEFF
                                                          };

        private void ConnectToSkype()
        {
            if (this.AttachedToSkype) { return; }

            this.ConnectToSkypeButton.Enabled = false;

            try
            {
                this.Skype.Attach();
                this.AttachedToSkype = true;

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

                                if (x.Length > 0)
                                {
                                    char firstCharacter = x[0];
                                    x = firstCharacter.ToString() + Main.randomZeroWidthCharacters[new Random().Next(Main.randomZeroWidthCharacters.Length)] + x.Substring(1);
                                }

                                message.Chat.SendMessage(x);
                                this.LastResponseTime = DateTime.Now;
                            };

                            if (!this.CommandDispatcher.HandleMessage(messageSink, message.Body))
                            {
                                this.MessageHandler.HandleMessage(messageSink, message.Body);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        this.Logger.Log("Unhandled Exception: " + e.ToString());
                    }
                };
            }
            catch (COMException)
            {
                this.Logger.Log("Failed to attach to Skype.");
                this.ConnectToSkypeButton.Enabled = true;
            }
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.BringToFront();
        }

        private void ShowNotifyMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.BringToFront();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Toolbar
        private void ConnectToSkypeButton_Click(object sender, EventArgs e)
        {
            this.ConnectToSkype();
        }
        #endregion
    }
}