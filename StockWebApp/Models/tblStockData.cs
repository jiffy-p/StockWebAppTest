//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblStockData
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public Nullable<int> Volume { get; set; }
        public Nullable<decimal> AdjustedClose { get; set; }
    }
}
