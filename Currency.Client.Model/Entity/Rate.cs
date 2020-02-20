using System;

namespace Currency.Client.Model.Entity
{
    public class Rate
    {
        public Rate(string code, string currency, decimal mid, Uri flag, DateTime effectiveDate)
        {
            Code = code;
            Currency = currency;
            Mid = mid;
            Flag = flag;
            EffectiveDate = effectiveDate;
        }

        public string Code { get; }
        public string Currency { get; }
        public decimal Mid { get; }
        public Uri Flag { get; }
        public DateTime EffectiveDate { get; }
    }
}