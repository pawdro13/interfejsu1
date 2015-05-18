using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLexample
{
    class RekordWykres
    {
        DateTime date;
        float exchangeRate;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public float ExchangeRate
        {
            get { return exchangeRate; }
            set { exchangeRate = value; }
        }

        public RekordWykres(DateTime _date, float _price)
        {
            date = _date;
            exchangeRate = _price;
        }

        public RekordWykres()
        {
            // TODO: Complete member initialization
        }

        public int DaysFrom(DateTime start)
        {
            //System.TimeSpan diff = date.Subtract(start);
            return (int)date.Subtract(start).TotalDays;
        }
    }
}

