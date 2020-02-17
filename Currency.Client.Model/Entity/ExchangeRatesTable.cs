using System;
using System.Collections.Generic;

namespace Currency.Client.Model.Entity
{
    public class ExchangeRatesTable
    {
        public ExchangeRatesTable(string tableType, string tableNo, DateTime effectiveDateTime, List<Rate> rates)
        {
            TableType = tableType;
            TableNo = tableNo;
            EffectiveDateTime = effectiveDateTime;
            Rates = rates;
        }

        public string TableType { get; }

        public string TableNo { get; }

        public DateTime EffectiveDateTime { get; }

        public List<Rate> Rates { get; }
    }
}