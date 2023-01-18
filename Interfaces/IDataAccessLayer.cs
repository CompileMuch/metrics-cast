public interface IDataAccessLayer
{
    Task<List<Candle>> GetCandles();
    Task<double> GetCloseoutPrice(string instrument);
}
