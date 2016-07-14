using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class AcknowledgeAlarmService
    {
          internal int AcknowledgeAlarmService_Pack(ref byte[] Handler_Transmit_Buffer, UInt32 device_id)
        {
            BACNET_ADDRESS my_address = new BACNET_ADDRESS();
            my_address.Get_My_Address();
            BACNET_ADDRESS dest = new BACNET_ADDRESS();
            BACNET_ADDRESS.Get_Device_Address(ref dest, device_id);
            ACKNOWLEDGE_ALARM_DATA Ack_data = Get_Ack_Data(device_id);
            
            int byte_len = 0;
            Byte invoke_id;
            int len = 0;
            int pdu_len = 0;
            NpduProcessor n_pro = new NpduProcessor();
            BACNET_NPDU_DATA npdu_data = new BACNET_NPDU_DATA();
            BvlcProcessor b_pro = new BvlcProcessor();
      
            invoke_id = TsmProcessor.next_free_id(device_id);
            n_pro.Encode_NpduData(ref npdu_data, true, BACNET_MESSAGE_PRIORITY.MESSAGE_PRIORITY_NORMAL);
  
            pdu_len = n_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref my_address, ref npdu_data);
            len = Acknowledge_Alarm_Encode(ref Handler_Transmit_Buffer, invoke_id, ref Ack_data, pdu_len);
            pdu_len += len;
            byte_len = b_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref npdu_data, pdu_len);

            return byte_len; ;


        }
          private  ACKNOWLEDGE_ALARM_DATA Get_Ack_Data(UInt32 device_id)
          {
              Alarm_Event_Manager manager=Alarm_Event_Manager.Get_Alarm_Manager();
              ACKNOWLEDGE_ALARM_DATA Ack_data = manager.Get_Alarm_Event(device_id);
              String temp = "Known";
              Ack_data.Source = new BACNET_CHARACTER_STRING(temp.Length);
              Ack_data.Source.encoding = 1;
              Ack_data.Source.size = (uint)temp.Length;
              Ack_data.Source.value=temp.ToCharArray();
              Local_Device.Get_Local_Time(ref  Ack_data.TimeOfAcknowledgment);
              return Ack_data;

          }
          private int Acknowledge_Alarm_Encode(ref Byte[] apdu, Byte invoke_id, ref ACKNOWLEDGE_ALARM_DATA Ack_data, int pos)
          {
              int len = 0;        /* length of each encoding */
              int apdu_len = 0;
              apdu[pos + 0] = (byte)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST;
              apdu[pos + 1] = BasicalProcessor.Encode_MaxSegsandApdu(0, 1476);
              apdu[pos + 2] = invoke_id;
              apdu[pos + 3] = (byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_ACKNOWLEDGE_ALARM;
              apdu_len = 4;

              /* tag 0 - processIdentifier */
              len = BasicalProcessor.Encode_Context_Unsigned(ref apdu, 0, Ack_data.ProcessIdentifier, pos + apdu_len);
              apdu_len += len;
              /* tag 1 - eventObjectIdentifier */
              len = BasicalProcessor.Encode_Context_ObjectId(ref apdu, 1, (int)Ack_data.EventIdentifier.type, Ack_data.EventIdentifier.instance, pos + apdu_len);
              apdu_len += len;
              /* tag 2 - toState */
              len = BasicalProcessor.Encode_Context_Enumerate(ref apdu, 2, (uint)Ack_data.StateAcknowledged, apdu_len + pos);
              apdu_len += len;
              /* tag 3 - timeStamp */
              len = BasicalProcessor.Encode_Context_Timestamp(ref apdu, 3, ref Ack_data.TimeStamp, pos + apdu_len);
              apdu_len += len;
              /* tag 4 - Acknowledgment Source */

              len = BasicalProcessor.Encode_Context_Character_String(ref apdu, 4, ref Ack_data.Source, pos + apdu_len);
              apdu_len += len;
              /* tag 5- timeStamp */
              len = BasicalProcessor.Encode_Context_Timestamp(ref apdu, 5, ref Ack_data.TimeOfAcknowledgment, pos + apdu_len);
              apdu_len += len;
              return apdu_len;


          }
    }
}
