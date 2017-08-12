using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using StockWebApp.Models;


namespace StockWebApp
{
    public static class DbConnect
    {
        public static bool InsertSymbols(IEnumerable<tblSymbol> symbolsList)
        {
            try
            {
                Log.Enter(nameof(InsertSymbols));

                using (var db = new dbStockDataEntities())
                {
                    db.tblSymbols.AddRange(symbolsList);
                    db.SaveChanges();

                    Debug.WriteLine($"Items inserted into Database.");
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Exception(exception, $"An error occurred while adding the the Symbol record to the database.");
                throw;
            }
            finally
            {
                Log.Exit(nameof(InsertSymbols));
            }
        }

        public static bool InsertStockData(IEnumerable<tblStockData> stockDataList)
        {
            try
            {
                Log.Enter(nameof(InsertStockData));

                using (var db = new dbStockDataEntities())
                {
                    db.tblStockDatas.AddRange(stockDataList);
                    db.SaveChanges();

                    Debug.WriteLine($"Items inserted into Database.");

                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Exception(exception, $"An error occurred while adding the the Symbol record to the database.");
                throw;
            }
            finally
            {
                Log.Exit(nameof(InsertStockData));
            }
        }

    }
}