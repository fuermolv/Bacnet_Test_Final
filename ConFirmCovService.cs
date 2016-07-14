using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class ConFirmCovService
    {
        public void Confirm_Cov_Handler(ref Byte[] service_request, UInt16 service_len, ref BACNET_ADDRESS src,BACNET_CONFIRMED_SERVICE_DATA request_data)
        {
            int len = 0;
            BACNET_COV_DATA cov_data = new BACNET_COV_DATA();
            /* create linked list to store data if more
            than one property value is expected */

            len = Cnconfirm_Cov_Decode(ref service_request, service_len, ref cov_data);
            Update_data(ref cov_data);
            //可以对得到的数据进行操作

            BacnetRecord.Add((uint)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST, (uint)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_COV_NOTIFICATION, DateTime.Now, false, cov_data.initiatingDeviceIdentifier);
            UdpSender sender = new UdpSender(1476);
            sender.Send_SimpleAck(request_data.invoke_id,BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_COV_NOTIFICATION,src);

        }
        private int Cnconfirm_Cov_Decode(ref Byte[] request, uint service_len, ref BACNET_COV_DATA data)
        {

            int len = 0;        /* return value */
            Byte tag_number = 0;
            UInt32 len_value = 0;
            UInt32 decoded_value = 0; /* for decoding */
            UInt16 decoded_type = 0;  /* for decoding */
            UInt32 property = 0;      /* for decoding */
            /* value in list */
            /* tag 0 - subscriberProcessIdentifier */
            //  if (decode_is_context_tag(&apdu[len], 0))
            len += BasicalProcessor.Decode_Tag_number_and_Value(ref request, ref tag_number, ref len_value, len);
            len += BasicalProcessor.Decode_Unsigned(ref request, len_value, ref decoded_value, len);
            data.subscriberProcessIdentifier = decoded_value;
            /* tag 1 - initiatingDeviceIdentifier */
            len += BasicalProcessor.Decode_Tag_number_and_Value(ref request, ref tag_number, ref  len_value, len);
            len += BasicalProcessor.Decode_Object_Id(ref request, ref decoded_type, ref data.initiatingDeviceIdentifier, len);
            /* tag 2 - monitoredObjectIdentifier */
            len += BasicalProcessor.Decode_Tag_number_and_Value(ref request, ref tag_number, ref len_value, len);
            len += BasicalProcessor.Decode_Object_Id(ref request, ref decoded_type, ref data.monitoredObjectIdentifier.instance, len);
            /* tag 3 - timeRemaining */
            len += BasicalProcessor.Decode_Tag_number_and_Value(ref request, ref tag_number, ref len_value, len);
            len += BasicalProcessor.Decode_Unsigned(ref request, len_value, ref decoded_value, len);
            data.timeRemaining = decoded_value;

            /* tag 4: opening context tag - listOfValues */
            //   if (!decode_is_opening_tag_number(&apdu[len], 4)) {
            len++;

            while (true) //应该设置遇到时间戳就BREAK? 下面CLOSING TAG有break
            {
                BACNET_PROPERTY_VALUE value = new BACNET_PROPERTY_VALUE();
                /* tag 0 - propertyIdentifier */

                if (BasicalProcessor.Decode_Is_Context_Tag(ref request, 0, len))
                {
                    len += BasicalProcessor.Decode_Tag_number_and_Value(ref request, ref tag_number, ref  len_value, len);
                    len += BasicalProcessor.Decode_Enumerated(ref request, len_value, ref  property, len);
                    value.propertyIdentifier = (BACNET_PROPERTY_ID)property;
                }
                else
                {
                    return -1;
                }
                /* tag 1 - propertyArrayIndex OPTIONAL */
                if (BasicalProcessor.Decode_Is_Context_Tag(ref request, 1, len))
                {
                    len += BasicalProcessor.Decode_Tag_number_and_Value(ref request, ref tag_number, ref  len_value, len);
                    len += BasicalProcessor.Decode_Unsigned(ref request, len_value, ref decoded_value, len);
                    value.propertyArrayIndex = (UInt32)decoded_value;
                }
                else
                {
                    value.propertyArrayIndex = BacnetConst.BACNET_ARRAY_ALL;//#define BACNET_ARRAY_ALL (~(unsigned int)0)

                }
                /* tag 2: opening context tag - value */
                if (!BasicalProcessor.Decode_Is_Opening_Tag_Number(ref request, 2, len))
                {
                    return -1;
                }
                len++;
                len += BasicalProcessor.Decode_Application_Data(ref request,value.value, len);

                if (!BasicalProcessor.Decode_Is_Closing_Tag_Number(ref request, 2, len))
                {
                    return -1;
                }
                len++;

                /* tag 3 - priority OPTIONAL */
                if (BasicalProcessor.Decode_Is_Context_Tag(ref request, 3, len))
                {
                    len += BasicalProcessor.Decode_Tag_number_and_Value(ref request, ref tag_number, ref  len_value, len);
                    len += BasicalProcessor.Decode_Unsigned(ref request, len_value, ref decoded_value, len);
                    value.priority = (Byte)decoded_value;

                }
                else
                {
                    value.priority = 0;

                }
                data.listOfValues.Add(value);
                if (BasicalProcessor.Decode_Is_Closing_Tag_Number(ref request, 4, len))
                {
                    break;
                }
            }

            return len;
        }
        private void Update_data(ref BACNET_COV_DATA data)
        {
         // UInt32 device_id = CovProcessor.Get_Device_Id((Byte)data.subscriberProcessIdentifier);
            
            UInt32 device_id = data.initiatingDeviceIdentifier;
            Device_Manager manager = Device_Manager.Get_Device_Manager();
            bool new_message_code = false;
            Bacnet_Lift lift;
            Bacnet_Escalators escalators;
            lift = manager.Get_Lift(device_id);
            if (lift != null)
            {
                List<BACNET_PROPERTY_ID> property_list = new List<BACNET_PROPERTY_ID>();
                BACNET_TIMESTAMP time=new BACNET_TIMESTAMP();
                lift.monitored = true;
                List<BACNET_PROPERTY_VALUE> value_list = data.listOfValues;
                BACNET_PROPERTY_VALUE value = new BACNET_PROPERTY_VALUE();
                List<BACnetMessageCode> message_list = new List<BACnetMessageCode>();
                for (int count = 0; count < value_list.Count; count++)
                {
                    value = value_list[count];
                    switch (value.propertyIdentifier)
                    {
                        case BACNET_PROPERTY_ID.PROP_Service_Mode:
                            {
                                lift.lift.Set_Service_Mode((BACnetLiftServiceMode)value.value.value.Unsigned_Int);
                                property_list.Add(BACNET_PROPERTY_ID.PROP_Service_Mode);
                                break;
                            }
                        case BACNET_PROPERTY_ID.PROP_Car_Status:
                            {
                                lift.lift.Set_Car_Status((Byte)value.value.value.Unsigned_Int);
                                property_list.Add(BACNET_PROPERTY_ID.PROP_Car_Status);
                                break;
                            }
                        case BACNET_PROPERTY_ID.PROP_Car_Direction:
                            {
                                lift.lift.Set_Car_Direction((Byte)value.value.value.Unsigned_Int);
                                property_list.Add(BACNET_PROPERTY_ID.PROP_Car_Direction);
                                break;
                            }
                        case BACNET_PROPERTY_ID.PROP_Door_Status:
                            {
                                lift.lift.Set_Door_Status(value.value.value.Boolean);
                                property_list.Add(BACNET_PROPERTY_ID.PROP_Door_Status);
                                break;
                            }
                        case BACNET_PROPERTY_ID.PROP_Door_Zone:
                            {
                                lift.lift.Set_Door_Zone(value.value.value.Boolean);
                                property_list.Add(BACNET_PROPERTY_ID.PROP_Door_Zone);
                                break;
                            }
                        case BACNET_PROPERTY_ID.PROP_Car_Position:
                            {
                                lift.lift.Set_Car_Position((Byte)value.value.value.Unsigned_Int);
                                property_list.Add(BACNET_PROPERTY_ID.PROP_Car_Position);

                                break;
                            }
                        case BACNET_PROPERTY_ID.PROP_Passenger_Status:
                            {
                                lift.lift.Set_Passenger_Status(value.value.value.Boolean);
                                property_list.Add(BACNET_PROPERTY_ID.PROP_Passenger_Status);
                                break;
                            }
                        case BACNET_PROPERTY_ID.PROP_LOCAL_DATE:
                            {
                                lift.lift.Set_Time_Stamps(value.value.value.Date);
                                lift.lift.Set_Time_Stamps(value.value.value.Date, ref time);
                                
                                break;
                            }
                        case BACNET_PROPERTY_ID.PROP_LOCAL_TIME:
                            {
                                lift.lift.Set_Time_Stamps(value.value.value.Time);
                                lift.lift.Set_Time_Stamps(value.value.value.Time, ref time);
                                break;
                            }
                        case BACNET_PROPERTY_ID.PROP_LIFT_Message_Code:
                            {
                                new_message_code = true;
                                message_list.Add((BACnetMessageCode)value.value.value.Unsigned_Int);
                                property_list.Add(BACNET_PROPERTY_ID.PROP_LIFT_Message_Code);
                                break;

                            }

                    }





                }
                for (int j = 0; j < property_list.Count;j++ )
                {
                    Property_TimeStamps temp = new Property_TimeStamps();
                    temp.poperty_id = property_list[j];
                    temp.time = time;
                    lift.lift.Add_Property_Time(temp);

                 }
                    if (new_message_code)
                        lift.lift.Set_Message_Code(message_list);
            }
            else
            {
                escalators = manager.Get_Escalators(device_id);
                if (escalators != null)
                {
                    escalators.monitored = true;
                    List<BACNET_PROPERTY_ID> property_list = new List<BACNET_PROPERTY_ID>();
                    BACNET_TIMESTAMP time = new BACNET_TIMESTAMP();
                    List<BACNET_PROPERTY_VALUE> value_list = data.listOfValues;

                    BACNET_PROPERTY_VALUE value = new BACNET_PROPERTY_VALUE();
                    List<BACnetMessageCode> message_list = new List<BACnetMessageCode>();
                    for (int count = 0; count < value_list.Count; count++)
                    {
                        value = value_list[count];
                        
                        switch (value.propertyIdentifier)
                        {
                            case BACNET_PROPERTY_ID.PROP_Service_Mode:
                                {
                                    escalators.escalators.Set_Service_Mode((BACnetEscalatorServiceMode)value.value.value.Unsigned_Int);
                                    property_list.Add(BACNET_PROPERTY_ID.PROP_Service_Mode);
                                    break;
                                }

                            case BACNET_PROPERTY_ID.PROP_LOCAL_DATE:
                                {
                                    escalators.escalators.Set_Time_Stamps(value.value.value.Date);
                                     lift.lift.Set_Time_Stamps(value.value.value.Date, ref time);
                                    break;
                                }
                            case BACNET_PROPERTY_ID.PROP_LOCAL_TIME:
                                {
                                    escalators.escalators.Set_Time_Stamps(value.value.value.Time);
                                     lift.lift.Set_Time_Stamps(value.value.value.Date, ref time);
                                    break;
                                }
                            case BACNET_PROPERTY_ID.PROP_Operation_Status:
                                {
                                    escalators.escalators.Set_Operation_Status((Byte)value.value.value.Unsigned_Int);
                                    property_list.Add(BACNET_PROPERTY_ID.PROP_Operation_Status);
                                    break;
                                }
                            case BACNET_PROPERTY_ID.PROP_Operation_Direction:
                                {
                                    escalators.escalators.Set_Operation_Direction((Byte)value.value.value.Unsigned_Int);
                                     property_list.Add(BACNET_PROPERTY_ID.PROP_Operation_Direction);
                                    break;
                                }
                            case BACNET_PROPERTY_ID.PROP_LIFT_Message_Code:
                                {
                                    new_message_code = true;
                                    message_list.Add((BACnetMessageCode)value.value.value.Unsigned_Int);
                                     property_list.Add(BACNET_PROPERTY_ID.PROP_LIFT_Message_Code);
                                    break;

                                }

                        }

                    }
                    for (int j = 0; j < property_list.Count; j++)
                    {
                        Property_TimeStamps temp = new Property_TimeStamps();
                        temp.poperty_id = property_list[j];
                        temp.time = time;
                        escalators.escalators.Add_Property_Time(temp);

                    }
                    if (new_message_code)
                        lift.lift.Set_Message_Code(message_list);

                }






            }

        }
    }
}
        
    

