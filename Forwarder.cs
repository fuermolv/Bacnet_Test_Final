using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Bacnet_Test_Final
{
   public class Forwarder
    {
       public static List<Forward_Address> Forwarder_List = new List<Forward_Address>();
       private static Boolean Forwarder_Status;
       
     
        public static void Set_Status(Boolean s)
        {
            Forwarder_Status=s;

        }
        public static Boolean Get_Status()
        {
            return Forwarder_Status;
        }
    
          
            
        }
    public class Forward_Address
    {
       public BACNET_ADDRESS address;
       public String ID;
        public Forward_Address()
        {
            address = new BACNET_ADDRESS();

        }
        public Forward_Address(String id, String ip, UInt16 port = 47808)
        {
            address = new BACNET_ADDRESS(ip, port);
            ID = id;

        }



    }
    
}
