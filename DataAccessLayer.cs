using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

public class DataAccessLayer : IDataAccessLayer
{
    private readonly HttpClient _client;
    private readonly string _accountId;
    private readonly string _accessToken;
    private readonly string _instrument;
    private readonly int _interval;
    private string _apiUrl;

    public DataAccessLayer(OandaOptions settings)
    {

        //check if the config is null
        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings));
        }
        //check if the config is valid
        if (string.IsNullOrEmpty(settings.AccountId))
        {
            throw new ArgumentException("AccountId is null or empty", nameof(settings.AccountId));
        }
        if (string.IsNullOrEmpty(settings.AccessToken))
        {
            throw new ArgumentException("AccessToken is null or empty", nameof(settings.AccessToken));
        }
        if (string.IsNullOrEmpty(settings.Instrument))
        {
            throw new ArgumentException("Instrument is null or empty", nameof(settings.Instrument));
        }
        if (settings.CandleInterval <= 0)
        {
            throw new ArgumentException("Interval is less than or equal to zero", nameof(settings.CandleInterval));
        }
        if (string.IsNullOrEmpty(settings.APIUrlCandles))
        {
            throw new ArgumentException("APIUrlCandles is null or empty", nameof(settings.APIUrlCandles));
        }


       
        _client = new HttpClient();
        _accountId = settings.AccountId;
        _accessToken = settings.AccessToken;
        _instrument = settings.Instrument;
        _interval = settings.CandleInterval;
        _apiUrl = string.Format(settings.APIUrlCandles, _instrument, _interval);
          

    }

    public async Task<List<Candle>> GetCandles()
    {
        
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
        var response = await _client.GetAsync(_apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(jsonString);
            //check if candles are null
            if (jsonObject["candles"] == null)
            {
                throw new Exception("No candles found");
            }
            //check if there are any candles
            if (jsonObject["candles"].Count() == 0)
            {
                throw new Exception("No candles found");
            }
            var oandaCandle = jsonObject["candles"].ToObject<List<OandaCandle>>();
            //convert oandaCandle to Candle
            List<Candle> candles = new List<Candle>();
            foreach (var candle in oandaCandle)
            {
                Candle newCandle = new Candle();
                newCandle.time = candle.time;
                newCandle.open = candle.mid.o;
                newCandle.high = candle.mid.h;
                newCandle.low = candle.mid.l;
                newCandle.close = candle.mid.c;
                candles.Add(newCandle);
            }
            return candles;
        }
        else
        {
            throw new Exception("Failed to retrieve candles");
        }
    }

    public Task<double> GetCloseoutPrice(string instrument)
    {
        throw new NotImplementedException();
    }
}