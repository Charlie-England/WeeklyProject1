using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyProject1
{
    public class DaySale
    {
        public int Basic { get; }
        public int Delux { get; }
        public int Total { get; }
        public DateTime Date { get; }

        public int BasicCost { get; }
        public int DeluxCost { get; }

        public DaySale(int _Basic, int _Delux, int _Total, DateTime _Date)
        {
            //Takes a Basic amount and a delux amount and creates a new class
            Basic = _Basic;
            Delux = _Delux;
            Total = _Total;
            Date = _Date;
        }
    }
}
