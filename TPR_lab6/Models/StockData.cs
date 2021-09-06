using System;
using System.Collections.Generic;
using System.Text;

namespace TPR_lab6.Models
{
    public class StockData
    {
        public string Ticker { get; set; }
        public double Per { get; set; }
        public DateTime DateTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Vol { get; set; }
    }
}
