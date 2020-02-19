using System;

namespace Currency.Client.Model.Entity
{
    public class Props
    {
        public Props(DateTimeOffset tableDateTime)
        {
            TableDateTime = tableDateTime;
        }

        public DateTimeOffset TableDateTime { get; }
    }
}