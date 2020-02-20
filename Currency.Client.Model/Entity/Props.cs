using System;

namespace Currency.Client.Model.Entity
{
    public class Props
    {
        public Props(DateTimeOffset tableDateTime, Rate selectedRate)
        {
            TableDateTime = tableDateTime;
            SelectedRate = selectedRate;
        }

        public DateTimeOffset TableDateTime { get; }
        public Rate SelectedRate { get; }
    }
}