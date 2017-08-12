using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.UI;
using DataTable = Google.DataTable.Net.Wrapper.DataTable;


namespace StockWebApp
{
    public partial class _default : Page
    {
        public string Json_TableChart { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Enter(nameof(Page_Load));

            try
            {
                
                Debug.WriteLine($"Getting Symbol List");
                var symbolList = DataController.GetSymbols("US");

                Debug.WriteLine($"SymbolList length: {symbolList.Count}");

                //Debug.WriteLine($"Inserting Symbol List");
                //DbConnect.InsertSymbols(symbolList);

                Debug.WriteLine($"Getting SymbolData List");
                var stockDataList = DataController.GetStockData("US", symbolList);

                Debug.WriteLine($"StockDataList length: {stockDataList.Count}");

                Debug.WriteLine($"Inserting StockDataList");
                DbConnect.InsertStockData(stockDataList);

            }
            catch (Exception exception)
            {
                Log.Exception(exception);
            }
            finally
            {
                Log.Exit(nameof(Page_Load));
            }
        }
    }
}