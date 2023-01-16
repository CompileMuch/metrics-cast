 public class PriceData
    {
        public string Type { get; set; }
        public DateTime Time { get; set; }
        public List<Bid> Bids { get; set; }
        public List<Ask> Asks { get; set; }
        public string CloseoutBid { get; set; }
        public string CloseoutAsk { get; set; }
        public string Status { get; set; }
        public bool Tradeable { get; set; }
        public string Instrument { get; set; }
    }