using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class SimpleAck
    {
      public int SimpleAck_Pack(ref Byte[] buffer,Byte invoke_id,BACNET_CONFIRMED_SERVICE service_type,BACNET_ADDRESS dest)
        {
            //   bool status = false;
            int len = 0;
            int pdu_len = 0;
            int byte_len = 0;
            BACNET_ADDRESS my_address = new BACNET_ADDRESS();
            my_address.Get_My_Address();
            BACNET_NPDU_DATA npdu_data = new BACNET_NPDU_DATA();
            NpduProcessor n_pro = new NpduProcessor();
            BvlcProcessor b_pro = new BvlcProcessor();
           
            n_pro.Encode_NpduData(ref npdu_data, true, BACNET_MESSAGE_PRIORITY.MESSAGE_PRIORITY_NORMAL);
            pdu_len = n_pro.Encode(ref buffer, ref dest, ref my_address, ref npdu_data);

            ApduProcessor a_pro=new ApduProcessor();
             len=a_pro.Encode_Simple_Ack(ref buffer,invoke_id,(Byte)service_type,pdu_len);
              
            pdu_len += len;
            byte_len = b_pro.Encode(ref buffer, ref dest, ref npdu_data, pdu_len);

            return byte_len; 

        }
    }
}
