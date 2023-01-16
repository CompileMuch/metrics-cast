using Microsoft.Extensions.Configuration;

public class OandaOptions
{
    public string AccessToken { get; set; }
    public string AccountId { get; set; }
    public string Instrument { get; set; }
    public int CandleInterval { get; set; }
    public int MAPeriod { get; set; }
}
