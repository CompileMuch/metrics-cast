public class BusinessLogic
{
    public void ProcessTradeDecision(double movingAverage, double lastCandleClosePrice)
    {
        if (movingAverage > lastCandleClosePrice)
        {
            Console.WriteLine("Sell");
        }
        else if (movingAverage < lastCandleClosePrice)
        {
            Console.WriteLine("Buy");
        }
        else
        {
            Console.WriteLine("Do nothing");
        }
    }
}
