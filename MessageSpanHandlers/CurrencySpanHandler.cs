using System.Text.RegularExpressions;
using Eka.Web.Google;

namespace ChatBot.MessageSpanHandlers
{
    public class CurrencySpanHandler : IMessageSpanHandler
    {
        private readonly string[] Currencies =
        {
            "EUR",
            "GBP",
            "USD"
        };

        private readonly Regex CurrencyRegex1 = new Regex("([\\$€£]|GBP|EUR|USD|CAD|PLN|SEK|NOK|YEN)\\s*([0-9\\.]+)",
            RegexOptions.IgnoreCase);

        private readonly Regex CurrencyRegex2 = new Regex("([0-9\\.]+)\\s*([\\$€£]|GBP|EUR|USD|CAD|PLN|SEK|NOK|YEN)",
            RegexOptions.IgnoreCase);

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (Match match in CurrencyRegex1.Matches(message))
            {
                actionSpanSink(match, "1");
            }
            foreach (Match match in CurrencyRegex2.Matches(message))
            {
                actionSpanSink(match, "2");
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            double amount = 0;
            var sourceCurrency = "";

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

            if (sourceCurrency == "$")
            {
                sourceCurrency = "USD";
            }
            else if (sourceCurrency == "£")
            {
                sourceCurrency = "GBP";
            }
            else if (sourceCurrency == "€")
            {
                sourceCurrency = "EUR";
            }

            var message = amount.ToString("0.00") + " " + sourceCurrency.ToUpper();

            foreach (var destinationCurrency in Currencies)
            {
                if (sourceCurrency.ToLower() == destinationCurrency.ToLower())
                {
                    continue;
                }

                message += "\n        " +
                           CurrencyConverter.Convert(amount, sourceCurrency, destinationCurrency).ToString("0.00") + " " +
                           destinationCurrency;
            }

            messageSink(message);
        }
    }
}