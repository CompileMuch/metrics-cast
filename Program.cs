using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

class Program
{

    static List<Candle> candles;



    //track state of the trade
    static bool isLong = false;
    static bool isShort = false;
    //flag to check if the last candle added to the list has the same time as the current candle
    static bool isSameTime = false;
    static DateTime lastCandleAdded;

    static double movingAverage;
    static CandleProcessor candleProcessor = new CandleProcessor();

    static OandaOptions settings;


    private static async Task Main(string[] args)
    {



        var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false);

        IConfiguration config = builder.Build();
        //check if config.GetRequiredSection("Oanda") is null
        if (config.GetRequiredSection("Oanda") == null)
        {
            Console.WriteLine("Oanda section is not provided");
            return;
        }

        // Get values from the config given their key and their target type.
         settings = config.GetRequiredSection("Oanda").Get<OandaOptions>();

        //exit if access token is not provided
        if (settings.AccessToken == null)
        {
            Console.WriteLine("Access token is not provided");
            return;
        }
        //exit if account id is default
        if (settings.AccountId == "YOUR_ACCOUNT")
        {
            Console.WriteLine("Account id is not provided");
            return;
        }
        //exit if instrument is not provided
        if (settings.Instrument == null)
        {
            Console.WriteLine("Instrument is not provided");
            return;
        }
        //exit if candle token is not provided
        if (settings.AccessToken == "YOUR_TOKEN")
        {
            Console.WriteLine("Topken interval is not provided");
            return;
        }

        //exit if APIStreamUrl is not provided
        if (settings.APIStreamUrl == null)
        {
            Console.WriteLine("APIStreamUrl is not provided");
            return;
        }

        //exit if APIUrlCandles is not provided
        if (settings.APIUrlCandles == null)
        {
            Console.WriteLine("APIUrlCandles is not provided");
            return;
        }

        TradeState tradeState = new TradeState();

        //set APIStreamUrl
        string apiStreamUrl = string.Format(settings.APIStreamUrl, settings.AccountId, settings.Instrument);

        //call FormatCandles function to format candles into heikin ashi candles
        var dataAccess = new DataAccessLayer(settings);
        candles = await dataAccess.GetCandles();

        //format candles into heikin ashi candles
        List<Candle> heikinAshiCandles = candleProcessor.FormatCandles(candles);
        //calculate moving average
        movingAverage = candleProcessor.CalculateMovingAverage(heikinAshiCandles, settings.MAPeriod);



        //output moving average and close price of the last heikin ashi candle
        Console.WriteLine("Moving average: " + movingAverage);
        Console.WriteLine("Last candle close price: " + heikinAshiCandles[heikinAshiCandles.Count - 1].close);

        BusinessLogic logic = new BusinessLogic();
        logic.ProcessTradeDecision(movingAverage, heikinAshiCandles[heikinAshiCandles.Count - 1].close);



        ///call StartStreamAsync
        await StartStreamAsync(apiStreamUrl,candleProcessor, settings.MAPeriod, settings.CandleInterval, settings.AccountId, settings.AccessToken, tradeState);

    }

    //function that processes candles into heiikin ashi candles and gets the moving average and determines if the trade should be entered

    //function that opens stream and listens for events, every 5 minutes adds a candle to the list


    static async Task StartStreamAsync(string apiStreamUrl, CandleProcessor candleProcessor, int maPeriod, int candleInterval, string accountId, string accountToken, TradeState tradeState)
    {
        try
        {
            string endpoint = apiStreamUrl;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accountToken);
                using (var stream = await client.GetStreamAsync(endpoint))
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = await reader.ReadLineAsync();
                        PriceData data = JsonConvert.DeserializeObject<PriceData>(line);
                        Console.WriteLine("Price: " + data.CloseoutAsk);
                        ProcessPrice(candleProcessor, data, maPeriod, candleInterval, tradeState);
                    }
                }
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Error connecting to the server: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }



    static void ProcessPrice(CandleProcessor candleProcessor, PriceData data, int maPeriod, int candleInterval, TradeState tradeState)
    {
        //set lastCandleAdded to current time
        lastCandleAdded = DateTime.Now;

        //if the event is a price, then add a candle to the list
        if (data.Type == "PRICE")
        {
            //add candle to list if the time of the candle is a multiple of 5 minutes and a candle has not already been added for that time
            if (data.Time.Minute % candleInterval == 0)
            {
                if (isSameTime == false)
                {
                    isSameTime = true;
                    Candle newCandle = new Candle();
                    //set newCandle to the current price data from data.Bids
                    newCandle.time = data.Time;
                    newCandle.open = Convert.ToDouble(data.Bids[0].Price);
                    newCandle.high = Convert.ToDouble(data.Bids[0].Price);
                    newCandle.low = Convert.ToDouble(data.Bids[0].Price);
                    newCandle.close = Convert.ToDouble(data.Bids[0].Price);
                    candles.Add(newCandle);

                    //check if the last candle added to the list has the same time, if so, update the high, low and close values of that candle
                    if (candles.Count > 0 && candles[candles.Count - 1].time == newCandle.time)
                    {
                        //update the high, low and close

                        //check if the last candle added to the list has the same time, if so, update the high, low and close values of that candle
                        if (candles.Count > 0 && candles[candles.Count - 1].time == newCandle.time)
                        {
                            //update the high, low and close values of the last candle
                            candles[candles.Count - 1].high = Math.Max(candles[candles.Count - 1].high, newCandle.high);
                            candles[candles.Count - 1].low = Math.Min(candles[candles.Count - 1].low, newCandle.low);
                            candles[candles.Count - 1].close = newCandle.close;
                        }
                        else
                        {
                            //otherwise add the candle
                            candles.Add(newCandle);
                        }

                        //call FormatCandles function to format candles into heikin ashi candles
                        List<Candle> heikinAshiCandles = candleProcessor.FormatCandles(candles);

                        movingAverage = candleProcessor.CalculateMovingAverage(heikinAshiCandles, settings.MAPeriod);

                        //output moving average and close price of the last heikin ashi candle
                        Console.WriteLine("Moving average: " + movingAverage);
                        Console.WriteLine("Last candle close price: " + heikinAshiCandles[heikinAshiCandles.Count - 1].close);

                         

                        //if moving average is greater than the last candle close price, then sell
                        if (movingAverage > heikinAshiCandles[heikinAshiCandles.Count - 1].close)
                        {
                            Console.WriteLine("Sell");
                             tradeState.SetShort();

                        }
                        //if moving average is less than the last candle close price, then buy
                        else if (movingAverage < heikinAshiCandles[heikinAshiCandles.Count - 1].close)
                        {
                            Console.WriteLine("Buy");
                            tradeState.SetLong();
                        }
                        //if moving average is equal to the last candle close price, then do nothing
                        else
                        {
                            Console.WriteLine("Do nothing");
                            tradeState.SetSameTime();
                        }

                    }
                }
            }
            else
            {
                isSameTime = false;
            }
        }
    }

  
    static List<Candle> GetCandles(int maPeriod, int candleInterval, string instrument, string accessToken)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            //set get url and get response 
            var url = "https://api-fxtrade.oanda.com/v3/instruments/" + instrument + "/candles?smooth=true&price=M&count=" + maPeriod + "&granularity=M" + candleInterval;
            //https://api-fxtrade.oanda.com/v3/instruments/USD_JPY/candles?count=15&price=M&granularity=M5&smooth=true
            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                List<Candle> candles = new List<Candle>();
                foreach (var candle in json["candles"])
                {
                    Candle newCandle = new Candle();
                    newCandle.time = candle["time"].Value<DateTime>();
                    newCandle.open = candle["mid"]["o"].Value<double>();
                    newCandle.high = candle["mid"]["h"].Value<double>();
                    newCandle.low = candle["mid"]["l"].Value<double>();
                    newCandle.close = candle["mid"]["c"].Value<double>();
                    candles.Add(newCandle);
                }
                return candles;
            }
            else
            {
                Console.WriteLine("Error in getting candles: " + response.ReasonPhrase);
                return null;
            }
        }
    }



}
