using System;
using System.Drawing;
using System.Windows.Forms;
using ChatBot.Properties;
using SKYPE4COMLib;

namespace ChatBot
{
    public partial class ChatFilterDialog : Form
    {
        private readonly ChatFilter ChatFilter;
        private readonly Skype Skype;

        public ChatFilterDialog(Skype skype, ChatFilter chatFilter)
        {
            InitializeComponent();

            Icon = Icon.FromHandle(Resources.comments.GetHicon());
            DialogResult = DialogResult.OK;

            Skype = skype;
            ChatFilter = chatFilter;
        }

        private void ChatFilterDialog_Load(object sender, EventArgs e)
        {
            foreach (Chat chat in Skype.RecentChats)
            {
                ChatListBox.Items.Add(new ChatItem(chat), ChatFilter.ChatPassesFilter(chat));
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < ChatListBox.Items.Count; i++)
            {
                var chat = ((ChatItem) ChatListBox.Items[i]).Chat;
                var itemChecked = ChatListBox.GetItemChecked(i);

                if (itemChecked)
                {
                    ChatFilter.IncludeChat(chat);
                }
                else
                {
                    ChatFilter.ExcludeChat(chat);
                }
            }

            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private class ChatItem
        {
            public ChatItem(Chat chat)
            {
                Chat = chat;
            }

            public Chat Chat { get; }

            public override string ToString()
            {
                var chatString = Chat.Topic;

                if (chatString == "")
                {
                    chatString = Chat.FriendlyName;
                }

                return chatString;
            }
        }
    }
}