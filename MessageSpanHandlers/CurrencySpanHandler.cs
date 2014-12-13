using Eka.Web.Google;
using System.Text.RegularExpressions;

namespace ChatBot.MessageSpanHandlers
{
    public class CurrencySpanHandler : IMessageSpanHandler
    {
        private Regex CurrencyRegex1 = new Regex("([\\$€£]|GBP|EUR|USD|CAD|PLN|SEK|NOK|YEN)\\s*([0-9\\.]+)", RegexOptions.IgnoreCase);
        private Regex CurrencyRegex2 = new Regex("([0-9\\.]+)\\s*([\\$€£]|GBP|EUR|USD|CAD|PLN|SEK|NOK|YEN)", RegexOptions.IgnoreCase);

        private string[] Currencies = new string[]
        {
            "EUR",
            "GBP",
            "USD"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (Match match in this.CurrencyRegex1.Matches(message))
            {
                actionSpanSink(match, "1");
            }
            foreach (Match match in this.CurrencyRegex2.Matches(message))
            {
                actionSpanSink(match, "2");
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            double amount = 0;
            string sourceCurrency = "";

            if (actionSpan.Data == "1")
            {
                sourceCurrency = actionSpan.Match.Groups[1].ToString();
                amount = double.Parse(actionSpan.Match.Groups[2].ToString());
            }
            else if (actionSpan.Data == "2")
            {
                sourceCurrency = actionSpan.Match.Groups[2].ToString();
                amount = double.Parse(actionSpan.Match.Groups[1].ToString());
            }

            if (sourceCurrency == "$") { sourceCurrency = "USD"; }
            else if (sourceCurrency == "£") { sourceCurrency = "GBP"; }
            else if (sourceCurrency == "€") { sourceCurrency = "EUR"; }

            string message = amount.ToString("0.00") + " " + sourceCurrency.ToUpper();

            foreach (string destinationCurrency in this.Currencies)
            {
                if (sourceCurrency.ToLower() == destinationCurrency.ToLower()) { continue; }

                message += "\n        " + CurrencyConverter.Convert(amount, sourceCurrency, destinationCurrency).ToString("0.00") + " " + destinationCurrency;
            }

            messageSink(message);
        }
    }
}