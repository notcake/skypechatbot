using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKYPE4COMLib;

namespace ChatBot
{
    public class ChatFilter
    {
        private HashSet<string> ExcludedChatBlobs = new HashSet<string>();

        public ChatFilter()
        {
        }

        public bool ChatPassesFilter(Chat chat)
        {
            return !this.ExcludedChatBlobs.Contains(chat.Blob);
        }

        public void ExcludeChat(Chat chat)
        {
            if (this.ExcludedChatBlobs.Contains(chat.Blob)) { return; }

            this.ExcludedChatBlobs.Add(chat.Blob);
        }

        public void IncludeChat(Chat chat)
        {
            if (!this.ExcludedChatBlobs.Contains(chat.Blob)) { return; }

            this.ExcludedChatBlobs.Remove(chat.Blob);
        }
    }
}
