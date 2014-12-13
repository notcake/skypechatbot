namespace ChatBot
{
    public class Logger
    {
        public delegate void MessageLoggedHandler(string message);

        public event MessageLoggedHandler MessageLogged;

        public Logger()
        {
        }

        public void Log(string message)
        {
            if (this.MessageLogged != null)
            {
                this.MessageLogged(message);
            }
        }
    }
}