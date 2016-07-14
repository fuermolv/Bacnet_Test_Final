using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class ReadPropertyMultiService
    {
        public int Pack_Read_Property_Multi_Request(ref Byte[] Handler_Transmit_Buffer,UInt32 device_id)
        {
           
            BACNET_ADDRESS dest = new BACNET_ADDRESS();
            BACNET_ADDRESS my_address = new BACNET_ADDRESS();
            BACNET_READ_ACCESS_DATA access_data = new BACNET_READ_ACCESS_DATA(true);
         
            Device_Manager manager = Device_Manager.Get_Device_Manager();
            access_data.obj_type = manager.Type_Affirm(device_id);
            if (access_data.obj_type == BACNET_OBJECT_TYPE.BACNET_LIFT)
                access_data.object_instance = manager.Get_Lift(device_id).lift.Get_OBJECT_ID_Number();
            if (access_data.obj_type==BACNET_OBJECT_TYPE.BACNET_ESCALATORS)
                access_data.object_instance = manager.Get_Escalators(device_id).escalators.Get_OBJECT_ID_Number();
           

            BACNET_ADDRESS.Get_Device_Address(ref dest, device_id);
            my_address.Get_My_Address();

            int byte_len = 0;
            byte invoke_id;
            int len = 0;
            int pdu_len = 0;
            NpduProcessor n_pro = new NpduProcessor();
            BACNET_NPDU_DATA npdu_data = new BACNET_NPDU_DATA();
            BvlcProcessor b_pro = new BvlcProcessor();
            invoke_id = TsmProcessor.next_free_id(device_id);

            n_pro.Encode_NpduData(ref npdu_data, true, BACNET_MESSAGE_PRIORITY.MESSAGE_PRIORITY_NORMAL);

            pdu_len = n_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref my_address, ref npdu_data);
            len = Read_Property_Multi_Encode(ref Handler_Transmit_Buffer, invoke_id, access_data, pdu_len);
            pdu_len += len;
            byte_len = b_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref npdu_data, pdu_len);

            return byte_len; 


        }
        internal int Read_Property_Multi_Encode(ref byte[] apdu, byte invoke_id, BACNET_READ_ACCESS_DATA access_data, int pos)
        {
            int len = 0;       
            int apdu_len = 0;
            apdu[pos + 0] = (byte)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST;
            apdu[pos + 1] = BasicalProcessor.Encode_MaxSegsandApdu(0, 1476);
            apdu[pos + 2] = invoke_id;
            apdu[pos + 3] = (byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_READ_PROP_MULTIPLE;
            apdu_len = 4;
            len = BasicalProcessor.Encode_Context_ObjectId(ref apdu, 0, (int)access_data.obj_type, access_data.object_instance, pos + apdu_len);
            apdu_len += len;
            len = BasicalProcessor.Encode_Opening_Tag(ref apdu, 1, pos+apdu_len);
            apdu_len += len;
            BACNET_PROPERTY_REFERENCE data;
            for(int i=0;i<access_data.listOfProperties.Count;i++)
            {
                data = access_data.listOfProperties[i];
                if ((int)data.propertyIdentifier <= 4194303)
                {
                    len = BasicalProcessor.Encode_Context_Enumerate(ref apdu, 0, (uint)data.propertyIdentifier, pos + apdu_len);
                    apdu_len += len;
                }
                if (data.propertyArrayIndex != BacnetConst.BACNET_ARRAY_ALL)
                {
                    len =
                        BasicalProcessor.Encode_Context_Unsigned(ref apdu, 1,
                        (UInt32)data.propertyArrayIndex, pos + apdu_len);
                    apdu_len += len;
                }
            }
            len = BasicalProcessor.Encode_Closing_Tag(ref apdu, 1, pos+apdu_len);
            apdu_len += len;

            return apdu_len;


           
        }
        internal void Read_Property_Multi_Ack_Handler(ref Byte[]request, UInt16 request_len, ref BACNET_ADDRESS src, ref BACNET_CONFIRMED_SERVICE_ACK_DATA service_data)
    {
        int len = 0;
        BACNET_READ_ACCESS_DATA multi_data = new BACNET_READ_ACCESS_DATA();

        len = Decode_Ack_Service_Request(ref request, request_len, ref multi_data);

        Byte invoke_id = service_data.invoke_id;
        UInt32 device_id = TsmProcessor.Get_Device_Id(invoke_id);
        {
            if (device_id != 0)
            {
                Device_Manager manager = Device_Manager.Get_Device_Manager();
                Bacnet_Lift lift;
                Bacnet_Escalators escalators;
                lift = manager.Get_Lift(device_id);
                if (lift != null)
                {
                    for (int i = 0; i < multi_data.listOfProperties.Count; i++)
                    {
                        if (multi_data.listOfProperties[i].propertyIdentifier==BACNET_PROPERTY_ID.PROP_Total_Running_Time)
                        lift.lift.Set_Total_Running_Time(multi_data.listOfProperties[i].value.value.Unsigned_Int);
                        if (multi_data.listOfProperties[i].propertyIdentifier == BACNET_PROPERTY_ID.PROP_Present_Counter_Value)
                        lift.lift.Set_Present_Counter_Value(multi_data.listOfProperties[i].value.value.Unsigned_Int);
                    }
                }
                else
                {
                    escalators = manager.Get_Escalators(device_id);
                    for (int i = 0; i < multi_data.listOfProperties.Count; i++)
                    {
                        if (multi_data.listOfProperties[i].propertyIdentifier == BACNET_PROPERTY_ID.PROP_Total_Running_Time)
                            escalators.escalators.Set_Total_Running_Time(multi_data.listOfProperties[i].value.value.Unsigned_Int);
                        if (multi_data.listOfProperties[i].propertyIdentifier == BACNET_PROPERTY_ID.PROP_Present_Counter_Value)
                            escalators.escalators.Set_Present_Counter_Value(multi_data.listOfProperties[i].value.value.Unsigned_Int);
                    }

                }
            }
        }

    }
        internal int Decode_Ack_Service_Request(ref Byte[] apdu, UInt16 request_len, ref BACNET_READ_ACCESS_DATA multi_data)
        {
            int len = 0;        /* total length of decodes */
            
            UInt16 object_type = 0;        /* object type */
            Byte tag_number = 0;
            UInt32 len_value_type = 0;
             UInt32 property = 0;      /* for decoding */
             uint application_len = 0;
             int application_data_pos=0;
             int application_data_len=0;
             len++;
            len += BasicalProcessor.Decode_Object_Id(ref apdu, ref object_type, ref multi_data.object_instance, len);
            multi_data.obj_type = (BACNET_OBJECT_TYPE)object_type;
            if (BasicalProcessor.Decode_Is_Opening_Tag_Number(ref apdu, 1, len))
                len++;
            else
                return -1;
       
            while(!BasicalProcessor.Decode_Is_Closing_Tag_Number(ref apdu,1,len))
            {

                len += BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value_type, len);
                if (tag_number != 2)
                    return -1;
                len += BasicalProcessor.Decode_Enumerated(ref apdu, len_value_type, ref property, len);
                BACNET_PROPERTY_REFERENCE temp = new BACNET_PROPERTY_REFERENCE();
                BACNET_APPLICATION_DATA_VALUE temp_data = new BACNET_APPLICATION_DATA_VALUE();
                temp.value = temp_data;
                temp.propertyIdentifier = (BACNET_PROPERTY_ID)property;
                temp.propertyArrayIndex = BacnetConst.BACNET_ARRAY_ALL;
               

                if (BasicalProcessor.Decode_Is_Opening_Tag_Number(ref apdu,4,  len))
                    len++;
                application_len=(UInt32)(request_len - len - 1);
               len+= BasicalProcessor.Decode_Application_Data(ref apdu, temp.value, len);

                if (BasicalProcessor.Decode_Is_Closing_Tag_Number(ref apdu, 4, len))
                    len++;

                multi_data.listOfProperties.Add(temp);

               
            }

            len++;
            return len;


        }

    } 
    class BACNET_READ_ACCESS_DATA
    {
        public BACNET_OBJECT_TYPE obj_type;
        public UInt32 object_instance;
        public List<BACNET_PROPERTY_REFERENCE> listOfProperties;

      public BACNET_READ_ACCESS_DATA(bool send=false)
        {
            listOfProperties = new List<BACNET_PROPERTY_REFERENCE>();
          if(send)
            {


                BACNET_PROPERTY_REFERENCE data = new BACNET_PROPERTY_REFERENCE();
                BACNET_PROPERTY_REFERENCE data_a = new BACNET_PROPERTY_REFERENCE();
                data_a.propertyArrayIndex = BacnetConst.BACNET_ARRAY_ALL;
                data_a.propertyIdentifier = BACNET_PROPERTY_ID.PROP_Present_Counter_Value;
                data.propertyArrayIndex = BacnetConst.BACNET_ARRAY_ALL;
                data.propertyIdentifier = BACNET_PROPERTY_ID.PROP_Total_Running_Time;
                listOfProperties.Add(data);
                listOfProperties.Add(data_a);
            }


        }
    }
}
