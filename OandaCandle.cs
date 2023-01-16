public class OandaCandle
    {
        public class MidCandle
        {
            public double o { get; set; }
            public double h { get; set; }
            public double l { get; set; }
            public double c { get; set; }
        }
        public DateTime time { get; set; }
        public MidCandle mid { get; set; }
    }