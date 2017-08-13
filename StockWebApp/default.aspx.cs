using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;


namespace StockWebApp
{
    public partial class _default : Page
    {
        public string Json_TableChart { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            const string methodName = nameof(Page_Load);
            try
            {
                //var symbolList = DataController.GetSymbolsFromApi();

                //DbConnect.InsertSymbolsIntoDatabase(symbolList);

                var symbolsDbList = DbConnect.GetSymbolsFromDatabase();

                Log.WriteLine($"{methodName} - SymbolList length: {symbolsDbList.Count}");

                var count = 0;
                foreach (var item in symbolsDbList)
                {
                    count++;
                    Log.WriteLine($"{methodName} - {count} - {item.Symbol}");
                    var stockDataList = DataController.GetStockDataFromApi("US", item.Symbol.Trim());

                    Log.WriteLine($"{methodName} - StockDataList length: {stockDataList.Count}");

                    DbConnect.InsertStockDataIntoDatabase(stockDataList);
                }

                Log.WriteLine($"{methodName} - Done");

            }
            catch (Exception exception)
            {
                Log.Exception(exception, $"{methodName}");
            }
            finally
            {
                Log.Exit(methodName);
            }
        }
    }
}