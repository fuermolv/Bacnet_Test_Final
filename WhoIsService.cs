using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class WhoIsService
    {
        public int Pack_Who_Is_Service(ref byte[] Handler_Transmit_Buffer,UInt32 low_limit, UInt32 high_limit)
        {
            BACNET_ADDRESS dest = new BACNET_ADDRESS();
            BACNET_ADDRESS my_address = new BACNET_ADDRESS();
            my_address.Get_My_Address();
            dest.Get_Broadcast_Address();
            int byte_len = 0;
            int len = 0;
            int pdu_len = 0;
            NpduProcessor n_pro = new NpduProcessor();
            BACNET_NPDU_DATA npdu_data = new BACNET_NPDU_DATA();
            BvlcProcessor b_pro = new BvlcProcessor();

            n_pro.Encode_NpduData(ref npdu_data, true, BACNET_MESSAGE_PRIORITY.MESSAGE_PRIORITY_NORMAL);
            pdu_len = n_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref my_address, ref npdu_data);
            len=Who_is_Encode(ref Handler_Transmit_Buffer, low_limit, high_limit,pdu_len);

            pdu_len += len;
            byte_len = b_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref npdu_data, pdu_len);

            return byte_len; 
        }
        public int Who_is_Encode(ref byte[] apdu, UInt32 low_limit, UInt32 high_limit,int pos)
        {
            int len = 0;        /* length of each encoding */
            int apdu_len = 0;   /* total length of the apdu, return value */
            if (apdu != null)
            {
                apdu[pos + 0] = (byte)BACNET_PDU_TYPE.PDU_TYPE_UNCONFIRMED_SERVICE_REQUEST;
                apdu[pos + 1] = (byte)BACNET_UNCONFIRMED_SERVICE.SERVICE_UNCONFIRMED_WHO_IS;   /* service choice */
                apdu_len = 2;

                /* optional limits - must be used as a pair */
                if ((low_limit >= 0) && (low_limit <= 0x3FFFFF) &&
                    (high_limit >= 0) && (high_limit <= 0x3FFFFF))
                {
                    len = BasicalProcessor.Encode_Context_Unsigned(ref apdu, 0, low_limit, pos + apdu_len);
                    apdu_len += len;
                    len = BasicalProcessor.Encode_Context_Unsigned(ref apdu, 1, high_limit, pos + apdu_len);
                    apdu_len += len;
                }

            }

            return apdu_len;
        }
        internal void Handler_Who_Is(ref byte[] service_request, UInt16 service_len, ref BACNET_ADDRESS src)
        {
            int len = 0;
            UInt32 low_limit = 0;
            UInt32 high_limit = 0;
   

                len =
                    whois_decode_service_request(ref service_request, service_len, ref low_limit,
                    ref high_limit);
            if(len==0)
            {
                UdpSender sender = new UdpSender(1476);
                sender.Send_IamService();

            }
            else if(len!=-1)
            {

                if (low_limit <= Local_Device.this_device.device_object.Get_OBJECT_ID_Number() && Local_Device.this_device.device_object.Get_OBJECT_ID_Number() <= high_limit)
                {  UdpSender sender = new UdpSender(1476);
                   sender.Send_IamService();
                }
             
           }

        }
         private int whois_decode_service_request(ref byte[] apdu,UInt16 apdu_len,ref UInt32 pLow_limit,ref UInt32 pHigh_limit)
        {
            int len = 0;
            byte tag_number = 0;
            UInt32 len_value = 0;
            UInt32 decoded_value = 0;

            Boolean no_limit = true;


            for (int i = 0; i < apdu_len; i++)
            {
                if (apdu[i] != 0)
                    no_limit = false;
            }
            if (no_limit)
                return 0;

            if (apdu_len!=0)
            {
                len += BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value, 0);
                if (tag_number != 0)
                    return -1;
                len += BasicalProcessor.Decode_Unsigned(ref apdu, len_value, ref decoded_value, len);
                if (decoded_value <= 0x3FFFFF)
                {
                    
                        pLow_limit = decoded_value;

                }
                len += BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value, len);
                if (tag_number != 1)
                    return -1;
                len += BasicalProcessor.Decode_Unsigned(ref apdu, len_value, ref decoded_value, len);
                if (decoded_value <= 0x3FFFFF)
                {

                    pHigh_limit = decoded_value;

                }
                

            }
            return len;
        }
    }
}
