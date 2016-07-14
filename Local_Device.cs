using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class Local_Device
    {

        public static Bacnet_Device this_device=new Bacnet_Device("Control_Centre");

       public static void Get_Local_Time(ref BACNET_TIMESTAMP time )
       {
         
           DateTime now = DateTime.Now;
           time.value.date.day = (Byte)now.Day;
           time.value.date.year = (UInt16)now.Year;
           time.value.date.month = (Byte)now.Month;
           time.value.date.wday = (Byte)now.DayOfWeek;
           time.value.time.hour = (Byte)now.Hour;
           time.value.time.min = (Byte)now.Minute;
           time.value.time.sec = (Byte)now.Second;
           time.value.time.hundredths = (Byte)now.Millisecond;
           time.tag = 2;

           
       }


    }
}
