using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    public class Bacnet_Device
    {
        public Device_Object device_object;
        public BACNET_ADDRESS address;
    
        public bool monitored;
        public Bacnet_Device()
        {
            device_object = new Device_Object();
            address = new BACNET_ADDRESS();
            
            monitored = false;
        }
        public Bacnet_Device(String s)
        {
            device_object = new Device_Object(s);
            address = new BACNET_ADDRESS();
            
            address.Get_My_Address();
            monitored = false;
        }
   
    }
}
