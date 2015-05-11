using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ChatBot.CommandHandlers;
using ChatBot.Commands;
using ChatBot.MessageSpanHandlers;
using SKYPE4COMLib;
using Application = System.Windows.Forms.Application;

namespace ChatBot
{
    public partial class Main : Form
    {
        private static readonly char[] randomZeroWidthCharacters =
        {
            (char) 0x200B,
            (char) 0x200C,
            (char) 0x200D,
            (char) 0xFEFF
        };

        private readonly ChatFilter ChatFilter = new ChatFilter();
        private readonly CommandDispatcher CommandDispatcher;
        private readonly Logger Logger;
        private readonly MessageHandler MessageHandler;
        private readonly Skype Skype;
        private bool AttachedToSkype;
        private DateTime LastResponseTime = DateTime.Now;

        public Main()
        {
            InitializeComponent();

            Logger = new Logger();
            CommandDispatcher = new CommandDispatcher(Logger);
            MessageHandler = new MessageHandler(Logger);

            Logger.MessageLogged += delegate(string message)
            {
                if (this.Log.TextLength > 0)
                {
                    this.Log.AppendText("\n");
                }
                this.Log.AppendText(message);
                this.Log.ScrollToCaret();
            };

            Application.ThreadException +=
                delegate(object sender, ThreadExceptionEventArgs e)
                {
                    this.Logger.Log("Unhandled Exception: " + e.Exception);
                };

            NotifyIcon.Icon = Icon;

            // Command and Message Handlers
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsClass)
                    {
                        continue;
                    }
                    if (type.IsAbstract)
                    {
                        continue;
                    }

                    // Command Handlers
                    if (typeof (ICommandHandler).IsAssignableFrom(type))
                    {
                        var constructorInfo = type.GetConstructor(new Type[0] {});
                        if (constructorInfo == null)
                        {
                            continue;
                        }

                        var commandHandler = constructorInfo.Invoke(null);
                        CommandDispatcher.AddCommandHandler((ICommandHandler) commandHandler);
                    }

                    // Message Handlers
                    if (typeof (IMessageSpanHandler).IsAssignableFrom(type))
                    {
                        var constructorInfo = type.GetConstructor(new Type[0] {});
                        if (constructorInfo == null)
                        {
                            continue;
                        }

                        var messageSpanHandler = constructorInfo.Invoke(null);
                        MessageHandler.AddSpanHandler((IMessageSpanHandler) messageSpanHandler);
                    }
                }
            }

            // Skype
            Skype = new Skype();

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
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "https://twitter.com/thronecast/status/548132192988442624/photo/1");
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "3000€");
            // this.MessageHandler.HandleMessage(x => Debug.Print(x), "£20");
            // this.CommandDispatcher.HandleMessage(x => Debug.Print(x), "!w asdfasdf");

            ConnectToSkype();
        }

        private void ConnectToSkype()
        {
            if (AttachedToSkype)
            {
                return;
            }

            ConnectToSkypeButton.Enabled = false;

            try
            {
                Skype.Attach();
                AttachedToSkype = true;

                Skype.MessageStatus += delegate(ChatMessage message, TChatMessageStatus status)
                {
                    try
                    {
                        if (status == TChatMessageStatus.cmsReceived || status == TChatMessageStatus.cmsSent)
                        {
                            if (!this.ChatFilter.ChatPassesFilter(message.Chat))
                            {
                                return;
                            }
                            if ((DateTime.Now - this.LastResponseTime).TotalMilliseconds < 1000)
                            {
                                return;
                            }

                            MessageSink messageSink = x =>
                            {
                                this.Logger.Log("Sending message:\n\t" + x.Replace("\n", "\n\t"));
                                x = x.Trim();

                                if (x.Length > 0)
                                {
                                    var firstCharacter = x[0];
                                    x = firstCharacter.ToString() +
                                        randomZeroWidthCharacters[new Random().Next(randomZeroWidthCharacters.Length)] +
                                        x.Substring(1);
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
                        this.Logger.Log("Unhandled Exception: " + e);
                    }
                };
            }
            catch (COMException)
            {
                Logger.Log("Failed to attach to Skype.");
                ConnectToSkypeButton.Enabled = true;
            }
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Visible = false;
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Visible = true;
            BringToFront();
        }

        private void ShowNotifyMenuItem_Click(object sender, EventArgs e)
        {
            Visible = true;
            BringToFront();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Toolbar

        private void ConnectToSkypeButton_Click(object sender, EventArgs e)
        {
            ConnectToSkype();
        }

        private void ChatFilterButton_Click(object sender, EventArgs e)
        {
            new ChatFilterDialog(Skype, ChatFilter).ShowDialog();
        }

        #endregion Toolbar
    }
}