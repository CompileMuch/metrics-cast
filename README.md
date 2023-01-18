![alt text](https://raw.githubusercontent.com/CompileMuch/metrics-cast/master/assets/logo-mc-2.png)
# About
## Oanda API
This sample code demonstrates how to access the pricing endpoint of the Oanda API and process the pricing data for a given instrument, and also illustrates the implementation of Heikin Ashi and Moving Average indicators using that data.


## Heikin Ashi and Moving Average
The Heikin Ashi indicator is a type of chart that is used to identify trends in financial markets. It's a form of candle stick chart that is different from a traditional candle stick chart in that it uses a different formula to calculate the open, high, low, and close values for each period. Heikin Ashi chart is smoother than a normal candle stick chart (https://www.investopedia.com/trading/heikin-ashi-better-candlestick/).



The moving average is a commonly used indicator in technical analysis that helps smooth out price action by filtering out the “noise” from random price fluctuations. A moving average is calculated by taking the average of a certain number of data points, typically the closing prices of a security, over a certain period of time. This can help to identify the overall direction of the trend and also provide support and resistance levels (https://www.investopedia.com/ask/answers/071414/whats-difference-between-moving-average-and-weighted-moving-average.asp).

In this code, we use the pricing data from the Oanda API to create Heikin Ashi and Moving Average indicators. The Heikin Ashi indicator is created by applying the Heikin Ashi formula to the open, high, low, and close values of the pricing data. The Moving Average indicator is created by calculating the average of the closing prices over a certain period of time.

## Strategy
The strategy involves monitoring the relationship between the Heikin Ashi candles and the moving average indicator. When the moving average indicator crosses below the Heikin Ashi candles, it generates a buy signal, indicating that the market is in an uptrend. Conversely, when the moving average indicator crosses above the Heikin Ashi candles, it generates a sell signal, indicating that the market is in a downtrend.

![alt text](https://raw.githubusercontent.com/CompileMuch/metrics-cast/master/assets/buy-sell.jpg)

It is important to note that this strategy is based on the assumption that past market trends will continue into the future, which may not always be the case. Therefore, it is important to use other forms of analysis and risk management techniques in conjunction with this strategy. Additionally, it's important to consider the market conditions and the volatility of the asset being traded before making any buying or selling decisions.

This strategy is based on the interpretation of the Moving Average and Heikin Ashi candles. However, the interpretation can change depending on the context of the market, so it's important to use this strategy in combination with other forms of analysis and risk management techniques.

Please note that this code is just an example and it may not work without providing the valid Oanda API key.



# Getting started
To get started, you will need to sign up for a free Oanda practice account by following the instructions on the Oanda website.

Once you have an account, you will need to generate an API key to access the Oanda API. You can find more information on how to generate an API key in the Oanda API documentation: https://developer.oanda.com/rest-live-v20/introduction/

Open the project in Visual Studio and replace YOUR_ACCOUNT_ID and YOUR_API_KEY with your actual account ID and API key. These settings are located in appsettings.json
```
{
    "Oanda": {
      "AccessToken": "YOUR_TOKEN",
      "AccountId": "YOUR_ACCOUNT",
      "Instrument": "USD_JPY",
      "CandleInterval": 5,
      "MAPeriod": 10
    }
  }
```

Run the program to start streaming the pricing data from the Oanda API.

Run this command to install package dependencies
```
dotnet restore
```

To execute run
```
dotnet run
```

# Project Files
## gitignore
 generated using:
```
dotnet new gitignore
```

# Financial Risk Disclaimer

The contents of this repository are for informational purposes only and do not constitute financial advice or a recommendation to buy or sell any securities. None of the information contained in this repository is intended to be, and shall not be deemed to be, an offer to sell or a solicitation of an offer to buy any securities.

The information provided in this repository is not intended for distribution to, or use by, any person or entity in any jurisdiction or country where such distribution or use would be contrary to law or regulation.

Trading in financial instruments and/or cryptocurrencies involves high risks including the risk of losing some, or all, of your investment amount, and may not be suitable for all investors. Prices of cryptocurrencies are extremely volatile and may be affected by external factors such as financial, regulatory or political events. Trading on margin increases the financial risks.

Before deciding to trade in financial instruments or cryptocurrencies you should carefully consider your investment objectives, level of experience, and risk appetite. Under no circumstances shall the creator(s) of this repository be liable for any direct or indirect, special, incidental, or consequential damages, arising out of the use of the contents of this repository.

You should always consult a licensed financial advisor or conduct your own due diligence and research before making any investment decisions. The contents of this repository do not constitute financial, legal, or tax advice.
 