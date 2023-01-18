public interface ICandleProcessor
{
    List<Candle> FormatCandles(List<Candle> candles);
    double CalculateMovingAverage(List<Candle> candles, int period);
}