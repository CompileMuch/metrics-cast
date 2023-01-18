public interface IBusinessLogic
{
    void ProcessTradeDecision(double movingAverage, double lastCandleClosePrice);
}