using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using StockWebApp.Models;


namespace StockWebApp
{
    public static class DataController
    {
        private const string ApiToken = "597a77fe02135";

        public static List<tblSymbol> GetSymbolsFromApi()
        {
            const string methodName = nameof(GetSymbolsFromApi);

            try
            {
                Log.Enter(methodName);

                var symbolList = new List<tblSymbol>();
                var csv = $@"C:\StockWebAppTest\StockWebAppTest\cboesymboldir2.csv";
                using (var parser = new TextFieldParser(csv))
                {
                    if (string.IsNullOrEmpty(csv))
                    {
                        Log.WriteLine($"{methodName} - {nameof(csv)} is null or empty.");
                        return symbolList;
                    }

                    parser.HasFieldsEnclosedInQuotes = false;
                    parser.TrimWhiteSpace = true;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        var fields = parser.ReadFields();

                        var hasEmptyField = false;

                        foreach (var field in fields)
                        {
                            if (string.IsNullOrEmpty(field))
                            {
                                hasEmptyField = true;
                                break;
                            }
                        }
                        if (hasEmptyField)
                        {
                            continue;
                        }

                        var symbol = new tblSymbol();
                        symbol.Symbol = fields[0];
                        symbol.Name = fields[1];
                        symbol.Exchange = "US";

                        symbolList.Add(symbol);
                    }
                }

                Log.WriteLine($"{methodName} - Sorting Symbol List");
                symbolList.Sort((x, y) => string.CompareOrdinal(x.Symbol, y.Symbol));

                return symbolList;
            }
            catch (Exception exception)
            {
                Log.Exception(exception, $"{methodName} - Error occured while getting Symbols from Api.");
                throw;
            }
            finally
            {
                Log.Exit(methodName);
            }
        }

        public static List<tblStockData> GetStockDataFromApi(string exchange, string symbol)
        {
            const string methodName = nameof(GetStockDataFromApi);

            try
            {
                Log.Enter(methodName);
                
                string csv;
                var fromDate = $"{DateTime.Now.Date.AddYears(-3):yyyy-M-d}";
                var toDate = $"{DateTime.Now.Date:yyyy-M-d}";
                using (var web = new WebClient())
                {
                    var url = $@"https://eodhistoricaldata.com/api/eod/{symbol.Trim()}.{exchange}?from={fromDate}&to={toDate}&api_token={DataController.ApiToken}&period=d";
                    Log.WriteLine($"{methodName} - {symbol} - {fromDate} - {toDate} - {url}");
                    csv = web.DownloadString(url);
                }

                if (string.IsNullOrEmpty(csv))
                {
                    Log.WriteLine($"{methodName} - {nameof(csv)} is null or empty.");

                }

                var stockDataList = new List<tblStockData>();

                using (var parser = new TextFieldParser(new StringReader(csv)))
                {
                    parser.HasFieldsEnclosedInQuotes = true;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        var fields = parser.ReadFields();
                        if (fields == null || !fields.Any() || fields.Length < 7)
                        {
                            break;
                        }
                        var hasEmptyField = false;

                        foreach (var field in fields)
                        {
                            if (string.IsNullOrEmpty(field) || field.ToLower().Equals("date"))
                            {
                                hasEmptyField = true;
                                break;
                            }
                        }
                        if (hasEmptyField)
                        {
                            continue;
                        }
                        Log.WriteLine($"{fields[0]} - {fields[1]} - {fields[2]} - {fields[3]} - {fields[4]} - {fields[5]} - {fields[6]}");
                        var symbolData = new tblStockData();
                        symbolData.Symbol = symbol.Trim();
                        symbolData.Date = Convert.ToDateTime(fields[0]);
                        symbolData.Open = decimal.Parse(fields[1]);
                        symbolData.High = decimal.Parse(fields[2]);
                        symbolData.Low = decimal.Parse(fields[3]);
                        symbolData.Close = decimal.Parse(fields[4]);
                        symbolData.AdjustedClose = decimal.Parse(fields[5]);
                        symbolData.Volume = (int ?) decimal.Parse(fields[6]);

                        stockDataList.Add(symbolData);
                    }
                }

                Log.WriteLine($"{methodName} - Sorting StockData List");
                stockDataList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

                return stockDataList;
            }
            catch (Exception exception)
            {
                Log.Exception(exception, $"{methodName} - Error occured while getting stockdata from API.");
                throw;
            }
            finally
            {
                Log.Exit(methodName);
            }
        }
    }
}