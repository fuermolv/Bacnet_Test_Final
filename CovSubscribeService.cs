using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class CovSubscribeService
    {
        internal int Cov_Subscribe_pack(ref Byte[] Handler_Transmit_Buffer, UInt32 device_id, ref BACNET_SUBSCRIBE_COV_DATA cov_data)
        {
            Byte invoke_id = TsmProcessor.next_free_id(device_id);

            BACNET_ADDRESS my_address = new BACNET_ADDRESS();
            BACNET_ADDRESS dest = new BACNET_ADDRESS();
            my_address.Get_My_Address();
            BACNET_ADDRESS.Get_Device_Address(ref dest,device_id);


            //   uint max_apdu = 0;      
            //   bool status = false;
            int len = 0;
            int pdu_len = 0;
            int byte_len = 0;
            BACNET_NPDU_DATA npdu_data = new BACNET_NPDU_DATA();
            NpduProcessor n_pro = new NpduProcessor();
            BvlcProcessor b_pro = new BvlcProcessor();
            //status = address_get_by_device(device_id, &max_apdu, &dest);
             invoke_id = TsmProcessor.next_free_id(device_id);
          
            n_pro.Encode_NpduData(ref npdu_data, true, BACNET_MESSAGE_PRIORITY.MESSAGE_PRIORITY_NORMAL);

            pdu_len = n_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref my_address, ref npdu_data);
            len = Cov_Subscribe_Encode(ref Handler_Transmit_Buffer, invoke_id, ref cov_data, pdu_len);
            pdu_len += len;
       

            byte_len = b_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref npdu_data, pdu_len);

            return byte_len;

        }
        private int Cov_Subscribe_Encode(ref Byte[] apdu, int invoke_id, ref BACNET_SUBSCRIBE_COV_DATA cov_data, int pos)
        {
            int len = 0;        /* length of each encoding */
            int apdu_len = 0;
            apdu[pos + 0] = (byte)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST;
            apdu[pos + 1] = BasicalProcessor.Encode_MaxSegsandApdu(0, 1476);
            apdu[pos + 2] = (Byte)invoke_id;
            apdu[pos + 3] = (byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_SUBSCRIBE_COV;
            apdu_len = 4;
            len = BasicalProcessor.Encode_Context_Unsigned(ref apdu, 0, cov_data.subscriberProcessIdentifier, pos + apdu_len);
            apdu_len += len;

            len = BasicalProcessor.Encode_Context_ObjectId(ref apdu, 1, (int)cov_data.monitoredObjectIdentifier.type, cov_data.monitoredObjectIdentifier.instance, pos + apdu_len);
            apdu_len += len;

     
            if (!cov_data.cancellationRequest)
            {
                /* tag 2 - issueConfirmedNotifications */
                len =
                    BasicalProcessor.Encode_Context_Boolean(ref apdu, 2,
                    cov_data.issueConfirmedNotifications, pos + apdu_len);
                apdu_len += len;
                /* tag 3 - lifetime */
                len = BasicalProcessor.Encode_Context_Unsigned(ref apdu, 3, cov_data.lifetime, pos + apdu_len);
                apdu_len += len;
            }

            return apdu_len;
        }
    }
}
