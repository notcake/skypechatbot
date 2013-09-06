using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChatBot.Properties;
using SKYPE4COMLib;

namespace ChatBot
{
    public partial class ChatFilterDialog : Form
    {
        private Skype Skype;
        private ChatFilter ChatFilter;

        private class ChatItem
        {
            public Chat Chat { get; protected set; }

            public ChatItem(Chat chat)
            {
                this.Chat = chat;
            }

            public override string ToString()
            {
                string chatString = this.Chat.Topic;

                if (chatString == "")
                {
                    chatString = this.Chat.FriendlyName;
                }

                return chatString;
            }
        }

        public ChatFilterDialog(Skype skype, ChatFilter chatFilter)
        {
            this.InitializeComponent();

            this.Icon = Icon.FromHandle(Resources.comments.GetHicon());
            this.DialogResult = DialogResult.OK;

            this.Skype = skype;
            this.ChatFilter = chatFilter;
        }

        private void ChatFilterDialog_Load(object sender, EventArgs e)
        {
            foreach (Chat chat in this.Skype.RecentChats)
            {
                this.ChatListBox.Items.Add(new ChatItem(chat), this.ChatFilter.ChatPassesFilter(chat));
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.ChatListBox.Items.Count; i++)
            {
                Chat chat = ((ChatItem)this.ChatListBox.Items[i]).Chat;
                bool itemChecked = this.ChatListBox.GetItemChecked(i);

                if (itemChecked)
                {
                    this.ChatFilter.IncludeChat(chat);
                }
                else
                {
                    this.ChatFilter.ExcludeChat(chat);
                }
            }

            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
