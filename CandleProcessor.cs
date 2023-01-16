public class CandleProcessor
{
    
    public List<Candle> FormatCandles(List<Candle> candles)
    {
        List<Candle> heikinAshiCandles = new List<Candle>();
        for (int i = 0; i < candles.Count; i++)
        {
            Candle newCandle = new Candle();
            newCandle.time = candles[i].time;
            newCandle.open = (candles[i].open + candles[i].close) / 2;
            newCandle.close = (candles[i].open + candles[i].close + candles[i].high + candles[i].low) / 4;
            if (i == 0)
            {
                newCandle.high = Math.Max(newCandle.open, newCandle.close);
                newCandle.low = Math.Min(newCandle.open, newCandle.close);
            }
            else
            {
                newCandle.high = Math.Max(newCandle.open, Math.Max(newCandle.close, heikinAshiCandles[i - 1].high));
                newCandle.low = Math.Min(newCandle.open, Math.Min(newCandle.close, heikinAshiCandles[i - 1].low));
            }
            heikinAshiCandles.Add(newCandle);
        }
        return heikinAshiCandles;
       
    }

    public double CalculateMovingAverage(List<Candle> candles, int period)
    {
         double sum = 0;
        for (int i = 0; i < candles.Count; i++)
        {
            sum += candles[i].close;
        }
        return (double)(sum / candles.Count);
    }
}
