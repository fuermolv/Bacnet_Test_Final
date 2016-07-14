using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Analog_Bacnet_lift
{
    class BacnetAddresssProcessor
    {
        public static IPEndPoint dest;
       
          
        public static void Get_My_Address(ref BACNET_ADDRESS my_address)
        {
            my_address.net = 0;
            my_address.len = 6;
            for (int i = 0; i < 7;i++ )
            {
               
            }
                my_address.mac_len = 6;
                my_address.mac[0] = 192;
                my_address.mac[1] = 168;
                my_address.mac[2] = 110;
                my_address.mac[3] = 1;
                BasicalProcessor.Encode_Unsigned16(ref my_address.mac, 50, 4);

                my_address.adr[0] = 192;
                my_address.adr[1] = 168;
                my_address.adr[2] = 110;
                my_address.adr[3] = 1;
                BasicalProcessor.Encode_Unsigned16(ref my_address.adr, 50, 4);
                       //测试用 

        }
        public static void Get_Broadcast_Address(ref  BACNET_ADDRESS dest_address,Boolean local,UInt16 port)
        {
           
                dest_address.net = 0;
                dest_address.len = 6;
                for (int u=0; u < 7; u++)
                {
                    dest_address.adr[u] = 0;
                }
                dest_address.mac_len = 6;
                for(int i=0;i<4;i++)
                {
                    dest_address.mac[i] = 255;          
                   
                    
                }
                BasicalProcessor.Encode_Unsigned16(ref dest_address.mac, port,4);
                if (!local)
                    dest_address.net = 0xFFFF;//远程广播
               

        }
        public static void Get_Device_Address(ref BACNET_ADDRESS dest_address, UInt32 device_id)
        {
            Get_My_Address(ref dest_address);
        }
    }

}
