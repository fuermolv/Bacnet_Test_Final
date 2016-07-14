using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    public class Bacnet_Lift:Bacnet_Device
    {
        public Lift lift;
      
        public Bacnet_Lift()
        {
         lift=new Lift();
        
       
           
         }
    }
}
