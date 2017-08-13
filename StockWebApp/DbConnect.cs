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
        public static List<tblSymbol> GetSymbolsFromDatabase()
        {
            const string methodName = nameof(GetSymbolsFromDatabase);
            try
            {
                Log.Enter(methodName);

                using (var db = new dbStockDataEntities())
                {
                    var query = from item in db.tblSymbols
                                orderby item.Symbol
                                select item;

                    if (!query.Any())
                    {
                        Log.WriteLine($"{methodName} - No Symbols found in database.");
                        return null;
                    }

                    Log.WriteLine($"{methodName} - {query.Count()} records found in tblSymbols.");
                    return query.ToList();
                }
            }
            catch (Exception exception)
            {
                Log.Exception(exception, $"{methodName} - An error occurred while getting Symbols from database.");
                throw;
            }
            finally
            {
                Log.Exit(methodName);
            }
        }

        public static void InsertSymbolsIntoDatabase(IEnumerable<tblSymbol> symbolList)
        {
            const string methodName = nameof(InsertSymbolsIntoDatabase);

            try
            {
                Log.Enter(methodName);

                var tblSymbolsList = symbolList as IList<tblSymbol> ?? symbolList.ToList();

                if (!tblSymbolsList.Any())
                {
                    Log.WriteLine($"{methodName} - {nameof(symbolList)} is empty.");
                    return;
                }

                Log.WriteLine($"{methodName} - Removing any existing symbol records.");
                using (var db = new dbStockDataEntities())
                {
                    foreach (var tblSymbol in tblSymbolsList)
                    {
                        var symbol = tblSymbol.Symbol;
                        var query = from item in db.tblSymbols
                                    where item.Symbol.Equals(symbol)
                                    select item;

                        if (!query.Any())
                        {
                            continue;
                        }
                        foreach (var item in query)
                        {
                            Log.WriteLine($"{methodName} - Existing entry found - {item.Id} - {item.Symbol}");
                        }

                        db.tblSymbols.RemoveRange(query);
                        db.SaveChanges();
                    } 
                }
                
                Log.WriteLine($"{methodName} - Inserting Symbols into Database.");
                using (var db = new dbStockDataEntities())
                {
                    db.tblSymbols.AddRange(tblSymbolsList);
                    db.SaveChanges();

                    Log.WriteLine($"{methodName} - Symbols inserted into Database.");
                }
            }
            catch (Exception exception)
            {
                Log.Exception(exception, $"{methodName} - An error occurred while adding the the Symbol record to the database.");
                throw;
            }
            finally
            {
                Log.Exit(methodName);
            }
        }

        public static List<tblStockData> GetStockDataFromDatabase(string symbol)
        {
            const string methodName = nameof(GetStockDataFromDatabase);
            try
            {
                Log.Enter(methodName);

                using (var db = new dbStockDataEntities())
                {
                    var query = from item in db.tblStockDatas
                                where item.Symbol.Equals(symbol)
                                orderby item.Symbol
                                select item;

                    return query.ToList();
                }
            }
            catch (Exception exception)
            {
                Log.Exception(exception, $"{methodName} - An error occurred while getting StockData from database.");
                throw;
            }
            finally
            {
                Log.Exit(methodName);
            }
        }
        
        public static void InsertStockDataIntoDatabase(IEnumerable<tblStockData> stockDataList)
        {
            const string methodName = nameof(InsertStockDataIntoDatabase);

            try
            {
                Log.Enter(methodName);

                var tblstockDataList = stockDataList as IList<tblStockData> ?? stockDataList.ToList();

                if (!tblstockDataList.Any())
                {
                    Log.WriteLine($"{methodName} - {nameof(tblstockDataList)} is empty.");
                    return;
                }

                Log.WriteLine($"{methodName} - Removing any existing StockData records.");
                using (var db = new dbStockDataEntities())
                {
                    foreach (var tblStockData in tblstockDataList)
                    {
                        var symbol = tblStockData.Symbol.Trim();
                        var date = tblStockData.Date;

                        var query = from item in db.tblStockDatas
                                    where item.Symbol.Equals(symbol) && item.Date.Equals(date)
                                    select item;

                        if (!query.Any())
                        {
                            continue;
                        }
                        foreach (var item in query)
                        {
                            Log.WriteLine($"{methodName} - Existing entry found - {item.Id} - {item.Symbol} - {item.Date}");
                        }

                        db.tblStockDatas.RemoveRange(query);
                        db.SaveChanges();
                    }
                }

                Log.WriteLine($"{methodName} - Inserting StockData into Database.");
                using (var db = new dbStockDataEntities())
                {
                    db.tblStockDatas.AddRange(tblstockDataList);
                    db.SaveChanges();

                    Log.WriteLine($"{methodName} - StockData inserted into Database.");
                }
            }
            catch (Exception exception)
            {
                Log.Exception(exception, $"{methodName} - An error occurred while adding the StockData records to the database.");
                throw;
            }
            finally
            {
                Log.Exit(methodName);
            }
        }
    }
}