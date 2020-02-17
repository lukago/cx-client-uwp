using System;

namespace Currency.Client.Model.Entity
{
    public class Rate
    {
        public Rate(string code, string currency, decimal mid, Uri flag)
        {
            Code = code;
            Currency = currency;
            Mid = mid;
            Flag = flag;
        }

        public string Code { get; }
        public string Currency { get; }
        public decimal Mid { get; }
        public Uri Flag { get; }
    }
}