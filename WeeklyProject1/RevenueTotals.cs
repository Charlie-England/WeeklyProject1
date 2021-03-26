using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyProject1
{
    public class RevenueTotals
    {
        public Dictionary<int, Dictionary<int, Dictionary<string,int>>> RevTotals; //Year: {Month:{basic:int, delux:int, total:int}}
        public Dictionary<int,int> YearsTotal { get; }
        
        public RevenueTotals()
        {
            //Create new revenue totals
            RevTotals = new Dictionary<int, Dictionary<int, Dictionary<string,int>>>();
            YearsTotal = new Dictionary<int, int>();
        }

        public void addDay(DaySale daySales)
        {
            //receive a daysale object, check to make sure year (from Date) is a key
            //If year is a key, check to make sure month is a key
            //if month is a key, add basic, delux and total
            //if month is not a key, create key
            //if year is not a key, add in new dictionary key and new month
            int year = daySales.Date.Year;
            int month = daySales.Date.Month;

            if (RevTotals.Keys.Contains(year))
            {
                //contains year
                if (RevTotals[year].Keys.Contains(month))
                {
                    //contains month, add to year:month:(basic, delux, totals)
                    AddRevTotals(year, month, daySales);

                } else
                {
                    //add month
                    var dictTotals = new Dictionary<string, int>();
                    CreateNewDictTotals(dictTotals); //Add base values for new month dictionary totals

                    RevTotals[year].Add(month, dictTotals);

                    AddRevTotals(year, month, daySales);
                }
            } else
            {
                //add year and new month
                var dictTotals = new Dictionary<string, int>();
                CreateNewDictTotals(dictTotals); //Add base values for new month dictionary totals

                var yearDict = new Dictionary<int, Dictionary<string, int>>();
                yearDict.Add(daySales.Date.Month, dictTotals); //Add new month, and dictTotals to new dictionary month

                RevTotals.Add(daySales.Date.Year, yearDict); //Add new year (and above yearDict) to the RevTotals

                AddRevTotals(year, month, daySales);

            }
        }

        private void AddRevTotals(int year, int month, DaySale daySales)
        {
            RevTotals[year][month]["basic"] += daySales.Basic;
            RevTotals[year][month]["delux"] += daySales.Delux;
            RevTotals[year][month]["total"] += daySales.Total;

            if (YearsTotal.Keys.Contains(year))
            {
                YearsTotal[year] += daySales.Total;
            } else
            {
                YearsTotal.Add(year,daySales.Total);
            }
        }

        private void CreateNewDictTotals(Dictionary<string,int> dictTotals)
        {
            //takes a new dictionary object and adds in starter key and values
            dictTotals.Add("basic", 0);
            dictTotals.Add("delux", 0);
            dictTotals.Add("total", 0);
        }

        public List<string> RevenueReport()
        {
            //Returns a string of the report for the yearly revenue broken down by month/year
            List<string> report = new List<string>();
            


            const int monthSpacing = 8;
            const int otherSpacing = 15;
            report.Add($"{"Month",monthSpacing} | {"Total Sales",otherSpacing} |  {"Total Basic",otherSpacing}  |  {"Total Delux",otherSpacing}  |");
            report.Add("_________________________________________________________________");


            foreach (KeyValuePair<int, Dictionary<int,Dictionary<string,int>>> yearRep in RevTotals)
            {
                foreach(KeyValuePair<int,Dictionary<string,int>> monthRep in RevTotals[yearRep.Key])
                {
                    var subString = new StringBuilder();
                    switch (monthRep.Key)
                    {
                        case 1:
                            subString.Append($"{"Jan-"+$"{yearRep.Key}",monthSpacing}");
                            break;
                        case 2:
                            subString.Append($"{"Feb-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                        case 3:
                            subString.Append($"{"Mar-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                        case 4:
                            subString.Append($"{"Apr-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                        case 5:
                            subString.Append($"{"May-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                        case 6:
                            subString.Append($"{"Jun-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                        case 7:
                            subString.Append($"{"Jul-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                        case 8:
                            subString.Append($"{"Aug-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                        case 9:
                            subString.Append($"{"Sep-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                        case 10:
                            subString.Append($"{"Oct-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                        case 11:
                            subString.Append($"{"Nov-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                        case 12:
                            subString.Append($"{"Dec-" + $"{yearRep.Key}",monthSpacing}");
                            break;
                    }

                    subString.Append(" | ");
                    subString.Append($"{monthRep.Value["total"],otherSpacing:C2}");
                    subString.Append(" | ");
                    subString.Append($"{monthRep.Value["basic"],otherSpacing}");
                    subString.Append(" | ");
                    subString.Append($"{monthRep.Value["delux"],otherSpacing}");
                    subString.Append(" | ");
                    report.Add(subString.ToString());
                }
            }
            report.Add("Year Report:");

            //Add year summary to report
            report.Add($"{"Year",monthSpacing} | {"Totals:",monthSpacing}");
            foreach (KeyValuePair<int, int> yearTotal in YearsTotal)
            {
                report.Add($"{yearTotal.Key,monthSpacing} | {yearTotal.Value,monthSpacing:C2}");
            }

            return report;
        }

        public string GraphReport()
        {
            StringBuilder graphReport = new StringBuilder();

            int curYearIndex = 1;
            var graphDict = new Dictionary<int, List<int>>();
            

            //Iterate through each
            foreach (KeyValuePair<int, Dictionary<int, Dictionary<string, int>>> yearRep in RevTotals)
            {

                foreach (KeyValuePair<int, Dictionary<string, int>> monthRep in RevTotals[yearRep.Key])
                {
                    int horizontalVal = curYearIndex * monthRep.Key;
                    int totalVal = RevTotals[yearRep.Key][monthRep.Key]["total"];
                    int compareVal = CheckRange(graphDict, totalVal);

                    if (graphDict.Keys.Contains(compareVal))
                    {
                        //key already exists, add new value to list
                        graphDict[compareVal].Add(horizontalVal);
                    } else
                    {
                        //key does not exist, add new dictionary with new list
                        var horzValList = new List<int>();
                        horzValList.Add(horizontalVal);

                        graphDict.Add(totalVal,horzValList);
                    }
                    curYearIndex++;
                }
            }

            List<int> sortedList = graphDict.Keys.ToList<int>();
            sortedList.Sort();

            foreach (int key in sortedList)
            {
                foreach (int horzVal in graphDict[key])
                {
                    for (int i = 0; i < horzVal; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write("*");
                }
                Console.WriteLine();
            }

            return "";
        }

        private int CheckRange(Dictionary<int, List<int>> graphList, int value)
        {


            int modVal = 1500;
            int valueHigh = value + modVal;
            int valueLow = value - modVal;

            foreach (int valCheck in graphList.Keys)
            {
                if (valCheck > valueLow && valCheck < valueHigh)
                {
                    return valCheck;
                }
            }

            return value;
        }
    }
}
