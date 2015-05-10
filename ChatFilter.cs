using System.Collections.Generic;
using SKYPE4COMLib;

namespace ChatBot
{
    public class ChatFilter
    {
        private readonly HashSet<string> ExcludedChatBlobs = new HashSet<string>();

        public bool ChatPassesFilter(Chat chat) => !ExcludedChatBlobs.Contains(chat.Blob);

        public void ExcludeChat(Chat chat)
        {
            if (ExcludedChatBlobs.Contains(chat.Blob))
            {
                return;
            }

            ExcludedChatBlobs.Add(chat.Blob);
        }

        public void IncludeChat(Chat chat)
        {
            if (!ExcludedChatBlobs.Contains(chat.Blob))
            {
                return;
            }

            ExcludedChatBlobs.Remove(chat.Blob);
        }
    }
}