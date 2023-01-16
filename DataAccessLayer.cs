using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class DataAccessLayer
{
    private readonly HttpClient _client;
    private readonly string _accountId;
    private readonly string _accessToken;

    public DataAccessLayer(string accountId, string accessToken)
    {
        _client = new HttpClient();
        _accountId = accountId;
        _accessToken = accessToken;
    }

    public async Task<List<Candle>> GetCandles(string instrument, int interval)
    {
        string endpoint = $"https://api-fxtrade.oanda.com/v3/instruments/{instrument}/candles?interval={interval}";
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
        var response = await _client.GetAsync(endpoint);
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

    public async Task<double> GetCloseoutPrice(string instrument)
    {
        try
        {
            var endpoint = $"https://api-fxtrade.oanda.com/v3/accounts/{_accountId}/pricing?instruments={instrument}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
            var response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<PriceData>(json);
                return double.Parse(data.CloseoutAsk);
            }
            else
            {
                throw new Exception("Failed to retrieve closeout price data.");
            }
        }
        catch (Exception ex)
        {
            //log exception
            throw;
        }
    }

}