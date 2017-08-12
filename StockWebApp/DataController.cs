using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

        public static List<tblSymbol> GetSymbols(string exchange)
        {
            Log.Enter(nameof(GetSymbols));

            try
            {
                string csv;
                using (var web = new WebClient())
                {
                    csv = web.DownloadString($@"https://eodhistoricaldata.com/api/exchanges/{exchange}?api_token={ApiToken}");
                }

                if (string.IsNullOrEmpty(csv))
                {
                    throw new Exception($"{nameof(csv)} is null or empty.");
                }

                var symbolList = new List<tblSymbol>();

                using (var parser = new TextFieldParser(new StringReader(csv)))
                {
                    parser.HasFieldsEnclosedInQuotes = true;
                    parser.SetDelimiters(",");

                    var rowCount = 0;

                    while (!parser.EndOfData)
                    {
                        rowCount++; 
                        var fields = parser.ReadFields();
                        var fieldCount = fields.Length;
                        
                        if (fieldCount <= 1)
                        {
                            break;
                        }

                        var empty = fields.Any(string.IsNullOrEmpty);
                        if (empty)
                        {
                            continue;
                        }

                        if (fields[0].Contains("Code"))
                        {
                            continue;
                        }

                        var symbol = new tblSymbol()
                        {
                            Symbol = fields[0],
                            Name = fields[1],
                            Exchange = fields[2]
                        };
                        Debug.WriteLine($"{nameof(GetSymbols)}:\t Symbol: \t {rowCount} - {symbol.Symbol} - {symbol.Name} - {symbol.Exchange}");

                        symbolList.Add(symbol);
                    }
                }

                Debug.WriteLine($"Sorting Symbol List");
                symbolList.Sort((x, y) => string.CompareOrdinal(x.Symbol, y.Symbol));

                return symbolList;
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
                throw;
            }
            finally
            {
                Log.Exit(nameof(GetSymbols));
            }
        }

        public static List<tblStockData> GetStockData(string exchange, IEnumerable<tblSymbol> symbolList)
        {
            Log.Enter(nameof(GetStockData));

            try
            {
                var stockDataList = new List<tblStockData>();
                var rowCount = 0;

                foreach (var item in symbolList)
                {
                    rowCount++;
                    if (rowCount >= 200)
                    {
                        break;
                    }

                    var symbol = item.Symbol;
                
                    string csv;
                    var fromDate = $"{DateTime.Now.Date.AddYears(-3):yyyy-M-d}";
                    var toDate = $"{DateTime.Now.Date:yyyy-M-d}";
                    using (var web = new WebClient())
                    {
                        var url = $@"https://eodhistoricaldata.com/api/eod/{symbol}.{exchange}?from={fromDate}&to={toDate}&api_token={DataController.ApiToken}&period=d";
                        Debug.WriteLine($"{rowCount} - {symbol} - {url}");
                        csv = web.DownloadString(url);
                    }

                    if (string.IsNullOrEmpty(csv))
                    {
                        throw new Exception($"{nameof(csv)} is null or empty.");
                    }
                    
                    using (var parser = new TextFieldParser(new StringReader(csv)))
                    {
                        parser.HasFieldsEnclosedInQuotes = true;
                        parser.SetDelimiters(",");

                        while (!parser.EndOfData)
                        {
                            var fields = parser.ReadFields();
                            var fieldCount = fields.Length;

                            if (fieldCount <= 1)
                            {
                                break;
                            }
                            var empty = fields.Any(string.IsNullOrEmpty);
                            if (empty)
                            {
                                continue;
                            }
                            if (fields[0].ToLower().Contains("date"))
                            {
                                continue;
                            }

                            var symbolData = new tblStockData
                            {
                                Symbol = symbol,
                                Date = Convert.ToDateTime(fields[0]),
                                Open = decimal.Parse(fields[1]),
                                High = decimal.Parse(fields[2]),
                                Low = decimal.Parse(fields[3]),
                                Close = decimal.Parse(fields[4]),
                                AdjustedClose = decimal.Parse(fields[5]),
                                Volume = (int ?) decimal.Parse(fields[6])
                            };

                            stockDataList.Add(symbolData);
                        }
                    }
                }
                Debug.WriteLine($"Sorting StockData List");
                stockDataList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

                return stockDataList;
            }
            catch (Exception exception)
            {
                Log.Exception(exception);
                throw;
            }
            finally
            {
                Log.Exit(nameof(GetStockData));
            }
        }

    }
}