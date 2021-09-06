using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace TPR_lab6.Models
{
    static class DataImport
    {
        static public IEnumerable<StockData> ReadCSV(string fileName)
        {
            string[] lines = File.ReadAllLines(System.IO.Path.ChangeExtension(fileName, ".csv"));
            return lines.Select(line =>
            {
                string[] data = line.Split(';');
                StockData sd = new StockData();


                sd.Ticker = data[0];
                try
                {
                    sd.DateTime = DateTime.ParseExact(data[2] + data[3], "yyyyMMddHHmmss",
                        CultureInfo.InvariantCulture);
                }
                catch
                {
                    sd.DateTime = DateTime.ParseExact(data[2], "yyyyMMdd",
                       CultureInfo.InvariantCulture);
                }
                sd.Open = double.Parse(data[4], CultureInfo.InvariantCulture);
                sd.High = double.Parse(data[5], CultureInfo.InvariantCulture);
                sd.Low = double.Parse(data[6], CultureInfo.InvariantCulture);
                sd.Close = double.Parse(data[7], CultureInfo.InvariantCulture);
                sd.Vol = double.Parse(data[8], CultureInfo.InvariantCulture);

                return sd;
                // We return a person with the data in order.
            });
        }
    }

}


