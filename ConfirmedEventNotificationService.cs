using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacnet_Test_Final
{
    class ConfirmedEventNotificationService
    {
        internal void Cevent_Notify_Handler(ref Byte[] service_request,UInt16 service_len,ref BACNET_ADDRESS src,BACNET_CONFIRMED_SERVICE_DATA request_data)
        {
            int len = 0;
            BACNET_EVENT_NOTIFICATION_DATA data=new BACNET_EVENT_NOTIFICATION_DATA();
            len=Uevent_Notify_Decode(ref service_request,service_len,ref data );

            Alarm_Event_Manager alarm_manager = Alarm_Event_Manager.Get_Alarm_Manager();
            ACKNOWLEDGE_ALARM_DATA ack_alaram_data=new ACKNOWLEDGE_ALARM_DATA();
            ack_alaram_data.EventIdentifier=data.eventObjectIdentifier;
            ack_alaram_data.ProcessIdentifier=data.processIdentifier;
            ack_alaram_data.TimeStamp=data.timeStamp;
            ack_alaram_data.StateAcknowledged=data.toState;
 
            alarm_manager.Add_Alarm_Event(ack_alaram_data, data.initiatingObjectIdentifier.instance);

            Device_Manager device_manager;
            device_manager = Device_Manager.Get_Device_Manager();
            device_manager.Event_Handler.Alarm_Handler(data);

            BacnetRecord.Add((uint)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST, (uint)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_EVENT_NOTIFICATION, DateTime.Now, false, data.initiatingObjectIdentifier.instance);
          
             //此处添加对事件的处理
            UdpSender send = new UdpSender(1476);
            send.Send_SimpleAck(request_data.invoke_id, BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_EVENT_NOTIFICATION, src);
           

        }
        private int Uevent_Notify_Decode(ref Byte[] apdu, uint apdu_len, ref BACNET_EVENT_NOTIFICATION_DATA data)
        {
            int len = 0;        /* return value */
            int section_length = 0;
            UInt32 value = 0;
            Byte tag_number = 0;
            uint len_value = 0;
            /* tag 0 - processIdentifier */
            section_length = BasicalProcessor.Decode_Context_Unsigned(ref apdu, 0, ref data.processIdentifier, len);
            len += section_length;

            /* tag 1 - initiatingObjectIdentifier */
            len += BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref  len_value, len);
            section_length = BasicalProcessor.Decode_Object_Id(ref apdu, ref data.initiatingObjectIdentifier.type, ref data.initiatingObjectIdentifier.instance, len);
            len += section_length;
            /* tag 2 - eventObjectIdentifier */
            len += BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref  len_value, len);
            section_length = BasicalProcessor.Decode_Object_Id(ref apdu, ref data.eventObjectIdentifier.type, ref data.eventObjectIdentifier.instance, len);
            len += section_length;
            /* tag 3 - timeStamp */
            section_length = BasicalProcessor.Decode_Context_Timestamp(ref apdu, 3, ref data.timeStamp, len);
            len += section_length;
            /* tag 4 - noticicationClass */
            section_length = BasicalProcessor.Decode_Context_Unsigned(ref apdu, 4, ref data.notificationClass, len);
            len += section_length;
            /* tag 5 - priority */
            section_length = BasicalProcessor.Decode_Context_Unsigned(ref apdu, 5, ref value, len);
            data.priority = (Byte)value;
            len += section_length;
            /* tag 6 - eventType */
            section_length = BasicalProcessor.Decode_Context_Enumerated(ref apdu, 6, ref value, len);
            data.eventType = (BACNET_EVENT_TYPE)value;
            len += section_length;
            /* tag 7 - messageText */
            //option
            //此处没写

            /* tag 8 - notifyType */
            section_length = BasicalProcessor.Decode_Context_Enumerated(ref apdu, 8, ref value, len);
            data.notifyType = (BACNET_NOTIFY_TYPE)value;
            len += section_length;

            switch (data.notifyType)
            {

                case BACNET_NOTIFY_TYPE.NOTIFY_ALARM:
                case BACNET_NOTIFY_TYPE.NOTIFY_EVENT:
                    /* tag 9 - ackRequired */
                    data.ackRequired = BasicalProcessor.Decode_Context_Boolean(ref apdu, len);
                    len++;
                    len++;
                    /* tag 10 - fromState */
                    section_length = BasicalProcessor.Decode_Context_Enumerated(ref apdu, 10, ref value, len);
                    data.fromState = (BACNET_EVENT_STATE)value;

                    len += section_length;
                    break;
                default:
                    break;
            }
            /* tag 11 - toState */
            section_length = BasicalProcessor.Decode_Context_Enumerated(ref apdu, 11, ref value, len);
            data.toState = (BACNET_EVENT_STATE)value;
            len += section_length;

            /* tag 12 - eventValues */
            if (BasicalProcessor.Decode_Is_Opening_Tag_Number(ref apdu, 12, len))
                len++;
            if (BasicalProcessor.Decode_Is_Opening_Tag_Number(ref apdu, (Byte)data.eventType, len))
                len++;
            if ((Byte)data.eventType > 14)
                len++;

            if (data.notifyType == BACNET_NOTIFY_TYPE.NOTIFY_ALARM || data.notifyType == BACNET_NOTIFY_TYPE.NOTIFY_EVENT)
            {
                switch (data.eventType)
                {
                    case BACNET_EVENT_TYPE.EVENT_CHANGE_OF_STATE:
                        {
                            section_length = BasicalProcessor.Decode_Context_Poroperty_State(ref apdu, 0, ref data.notificationParams.change_of_state.newState, len);
                            len += section_length;

                            section_length = BasicalProcessor.Decode_Context_Bitstring(ref apdu, 1, ref data.notificationParams.change_of_state.statusFlags, len);
                            len += section_length;

                            break;
                        }
                    case BACNET_EVENT_TYPE.EVENT_LIFT_SAFE_BUTTON:
                        {
                            uint decode_value = 0; ;
                            section_length = BasicalProcessor.Decode_Context_Enumerated(ref apdu, 0, ref decode_value, len);
                            data.notificationParams.lift_safe_button.code=(BACnetMessageCode)decode_value;
                            len += section_length;

                            break;
                        }
                    case BACNET_EVENT_TYPE.EVENT_CHANGE_OF_LIFE_SAFETY:
                          {
                             section_length = BasicalProcessor.Decode_Context_Enumerated(ref apdu, 0, ref value, len);
                             data.notificationParams.change_of_lifesafety.newState = (BACNET_LIFE_SAFETY_STATE)value;
                             len += section_length;
                             section_length = BasicalProcessor.Decode_Context_Enumerated(ref apdu, 1, ref value, len);
                             data.notificationParams.change_of_lifesafety.newMode = (BACNET_LIFE_SAFETY_MODE)value;
                             len += section_length;
                             section_length = BasicalProcessor.Decode_Context_Bitstring(ref apdu, 2, ref data.notificationParams.change_of_state.statusFlags, len);
                             len += section_length;
                             section_length = BasicalProcessor.Decode_Context_Enumerated(ref apdu, 3, ref value, len);
                             data.notificationParams.change_of_lifesafety.operationExpected = (BACNET_LIFE_SAFETY_OPERATION)value;
                             len += section_length;


                             break;
                         }
                    default:
                        break;

                }
                if (BasicalProcessor.Decode_Is_Closing_Tag_Number(ref apdu, (Byte)data.eventType, len))
                    len++;
                if ((Byte)data.eventType > 14)
                    len++;
                if (BasicalProcessor.Decode_Is_Closing_Tag_Number(ref apdu, 12, len))
                    len++;
            }


            return len;
        }
    }
}
