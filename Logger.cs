namespace ChatBot
{
    public class Logger
    {
        public delegate void MessageLoggedHandler(string message);

        public event MessageLoggedHandler MessageLogged;

        public void Log(string message)
        {
            if (MessageLogged != null)
            {
                MessageLogged(message);
            }
        }
    }
}