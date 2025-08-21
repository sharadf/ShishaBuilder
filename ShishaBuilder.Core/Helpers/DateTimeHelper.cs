using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaBuilder.Core.Helpers;

public class DateTimeHelper
{
    public  static DateTime GetBakuTime()
    {
        return TimeZoneInfo.ConvertTimeFromUtc(

            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time")
        );
    }
    
}
