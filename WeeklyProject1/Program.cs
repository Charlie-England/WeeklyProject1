using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace WeeklyProject1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read all text documents into arrays
            string[] basic = File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\Basic.txt");
            string[] delux = File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\Delux.txt");
            string[] total = File.ReadAllLines($"{Directory.GetCurrentDirectory()}\\Total.txt");

            int curIndex = basic.Length - 1;
            DateTime curDate = DateTime.Today;

            List<DaySale> sales = new List<DaySale>(); 
            RevenueTotals RevTotals = new RevenueTotals(); //Main reporting class

            //Loop through indexes of all the basic, delux and total text files and create a new DaySale class and append to a list
            while (curIndex > 0)
            {
                int basicSaleNum = Convert.ToInt32(basic[curIndex]);
                int deluxSaleNum = Convert.ToInt32(delux[curIndex]);
                int totalSale = Convert.ToInt32(total[curIndex]);
                DaySale newDaySale = new DaySale(basicSaleNum, deluxSaleNum, totalSale, curDate);

                sales.Add(newDaySale);
                RevTotals.addDay(newDaySale);

                curIndex--;
                curDate = curDate.AddDays(-1);
            }


            //Console.WriteLine(RevTotals.RevenueReport());
            //Console.WriteLine(RevTotals.GraphReport());
            WriteToFile("RevReport", RevTotals.RevenueReport());

            Console.ReadLine();
        }

        private static void WriteToFile(string fileName, List<string> data)
        {
            //takes a string data and writes this to a filename
            string fPath = Directory.GetCurrentDirectory() + "\\" + fileName + ".txt";

            File.WriteAllLines(fPath,data);
        }
    }

}
