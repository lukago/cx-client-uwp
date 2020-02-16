using System;

namespace Currency.Client.Model
{
    public class Props
    {
        public Props(DateTime tableDateTime)
        {
            TableDateTime = tableDateTime;
        }

        public DateTime TableDateTime { get; }
    }
}