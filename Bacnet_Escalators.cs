using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
   public class Bacnet_Escalators:Bacnet_Device
    {
        public Escalators escalators;
       
        public Bacnet_Escalators()
        {
         escalators=new Escalators();
     
         }
    }
}
