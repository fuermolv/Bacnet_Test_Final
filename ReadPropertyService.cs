using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacnet_Test_Final
{
    class ReadPropertyService
    {
        
        
         public int Pack_Read_Property_Request(ref byte[] Handler_Transmit_Buffer, UInt32 device_id,BACnet_Read_Property_Data rp_data)
         {
             BACNET_ADDRESS dest = new BACNET_ADDRESS();
             BACNET_ADDRESS my_address = new BACNET_ADDRESS();
         
             rp_data.array_index = BacnetConst.BACNET_ARRAY_ALL;
             /*根据Device_id判断？*/
            

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
             len = Read_Property_Encode(ref Handler_Transmit_Buffer, invoke_id, rp_data, pdu_len);
             pdu_len += len;
             byte_len = b_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref npdu_data, pdu_len);

             return byte_len; 



         }
        internal void Read_Property_Handler(ref Byte[] request,UInt16 request_len,ref BACNET_ADDRESS src,ref BACNET_CONFIRMED_SERVICE_DATA request_data)
         {


             BACnet_Read_Property_Data rpdata = new BACnet_Read_Property_Data();
             int len = 0;
             len = Decode_Service_Request(ref request, request_len, ref rpdata);
             UdpSender sender = new UdpSender(1476);
             sender.Send_ReadPropertyAckService(src, rpdata, request_data.invoke_id);

         }
         private int Decode_Service_Request(ref Byte[] request,uint request_len,ref BACnet_Read_Property_Data rpdata  )
        {
            int len = 0;
            Byte tag_number = 0;
            UInt32 len_value_type = 0;
            UInt16 type = 0;  /* for decoding */
            UInt32 property = 0;      /* for decoding */
            //此处有对于错误请求的判断 decode_is_context_tag(&apdu[len++], 0)
            len++;
            len += BasicalProcessor.Decode_Object_Id(ref request, ref type, ref rpdata.object_instance, len);
            rpdata.object_type = (BACNET_OBJECT_TYPE)type;

            len += BasicalProcessor.Decode_Tag_number_and_Value(ref request, ref tag_number, ref len_value_type, len);
            //if (tag_number != 1)...
            len += BasicalProcessor.Decode_Enumerated(ref request, len_value_type, ref property, len);
            rpdata.object_property = (BACNET_PROPERTY_ID)property;
           // /* Tag 2: Optional Array Index */if (len < apdu_len)
            rpdata.array_index = BacnetConst.BACNET_ARRAY_ALL;
            rpdata.application_data_len = (int)request_len - len - 1;
            return len;
        }
         private int Read_Property_Encode(ref byte[] apdu, byte invoke_id, BACnet_Read_Property_Data rpdata, int pos)
         {
             int len = 0;        /* length of each encoding */
             int apdu_len = 0;
             apdu[pos + 0] = (byte)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST;
             apdu[pos + 1] = BasicalProcessor.Encode_MaxSegsandApdu(0, 1476);
             apdu[pos + 2] = invoke_id;
             apdu[pos + 3] = (byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_READ_PROPERTY;
             apdu_len = 4;

             len = BasicalProcessor.Encode_Context_ObjectId(ref apdu, 0, (int)rpdata.object_type, rpdata.object_instance, pos + apdu_len);
             apdu_len += len;
             if ((int)rpdata.object_property <= 4194303)
             {
                 len = BasicalProcessor.Encode_Context_Enumerate(ref apdu, 1, (uint)rpdata.object_property, pos + apdu_len);
                 apdu_len += len;
             }
            
             if (rpdata.array_index != BacnetConst.BACNET_ARRAY_ALL)
             {
                 len =
                     BasicalProcessor.Encode_Context_Unsigned(ref apdu, 2,
                     (UInt32)rpdata.array_index, pos + apdu_len);
                 apdu_len += len;
             }

             return apdu_len;

         }
        internal int Read_Property_Ack_Pack(ref Byte[] apdu,BACNET_ADDRESS src,Byte  invoke_id,  BACnet_Read_Property_Data rpdata)
    {
        int len = 0;
        int pdu_len = 0;
      
        int npdu_len = 0;
        int bytes_sent = 0;
             BACNET_ADDRESS my_address=new BACNET_ADDRESS();
             my_address.Get_My_Address();
            NpduProcessor n_pro = new NpduProcessor();
            BvlcProcessor b_pro = new BvlcProcessor();
            BACNET_NPDU_DATA npdu_data = new BACNET_NPDU_DATA();

            n_pro.Encode_NpduData(ref npdu_data, false, BACNET_MESSAGE_PRIORITY.MESSAGE_PRIORITY_NORMAL);
            npdu_len = n_pro.Encode(ref apdu, ref src, ref my_address, ref npdu_data);


            len = Read_Property_Ack_Encode(ref apdu, invoke_id, rpdata, npdu_len);
            pdu_len = npdu_len + len;

            bytes_sent = b_pro.Encode(ref apdu, ref src, ref npdu_data, pdu_len);
            return bytes_sent;
    }
         internal int Read_Property_Ack_Encode(ref Byte[] apdu, Byte  invoke_id,  BACnet_Read_Property_Data rpdata,int pos)
         {
             int len = 0;        /* length of each encoding */
             int apdu_len = 0;
             apdu[pos + 0] = (Byte)BACNET_PDU_TYPE.PDU_TYPE_COMPLEX_ACK; /* complex ACK service */
             apdu[pos + 1] = invoke_id;    /* original invoke id from request */
             apdu[pos + 2] = (Byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_READ_PROPERTY;      /* service choice */
             apdu_len = 3;
             len = BasicalProcessor.Encode_Context_ObjectId(ref apdu, 0, (int)rpdata.object_type, rpdata.object_instance, pos + apdu_len);
             apdu_len += len;
             len = BasicalProcessor.Encode_Context_Enumerate(ref apdu, 1, (UInt32)rpdata.object_property, pos + apdu_len);
             apdu_len += len;
             len = BasicalProcessor.Encode_Opening_Tag(ref apdu, 3, pos + apdu_len);
             apdu_len += len;

             len = Device_Read_Property(ref apdu,  rpdata, pos + apdu_len);
             apdu_len += len;

             len = BasicalProcessor.Encode_Closing_Tag(ref apdu, 3, pos + apdu_len);
             apdu_len += len;
            
             return apdu_len;
         }
         private  int Device_Read_Property(ref byte[]apdu,BACnet_Read_Property_Data rpdata,int pos)
         {
             int len = 0;
             BACNET_APPLICATION_DATA_VALUE value = new BACNET_APPLICATION_DATA_VALUE();
         
             value.context_specific = false;
           
             
          
             if(rpdata.object_property==BACNET_PROPERTY_ID.PROP_OBJECT_LIST)
             {
                 value.tag = (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_OBJECT_ID;
              

                 value.value.Object_Id = Local_Device.this_device.device_object.Object_List[0];

             }
             len += BasicalProcessor.Encode_Application_Data(ref apdu, ref value, pos+len);
          

         

             return len;
             
         }
         internal void Read_Property_Ack_Handler(ref Byte[] request, UInt16 request_len, ref BACNET_ADDRESS src, ref BACNET_CONFIRMED_SERVICE_ACK_DATA service_data)
         {
             int len = 0;
             BACnet_Read_Property_Data data = new BACnet_Read_Property_Data();

             len = Decode_Ack_Service_Request(ref request, request_len, ref data);

             BACNET_APPLICATION_DATA_VALUE value = new BACNET_APPLICATION_DATA_VALUE();
             BasicalProcessor.Decode_Application_Data(ref request, (uint)data.application_data_len,  value, data.application_data_pos);


           
           
             //此处添加对读取到的data的处理
             if (data.object_property == BACNET_PROPERTY_ID.PROP_Identification_Number&& value.tag == (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_CHARACTER_STRING)
                 {

                     Byte invoke_id = service_data.invoke_id;
                     UInt32 device_id = TsmProcessor.Get_Device_Id(invoke_id);
                     if(device_id!=0)
                     {
                          Device_Manager manager = Device_Manager.Get_Device_Manager();
                          Bacnet_Lift lift;
                          Bacnet_Escalators escalators;
                          lift = manager.Get_Lift(device_id);
                          if(lift!=null)
                          {
                              lift.lift.Set_Identification_Number(new String(value.value.Character_String.value));
                           }
                         else 
                          {
                              escalators = manager.Get_Escalators(device_id);
                              if (escalators != null)
                                  escalators.escalators.Set_Identification_Number(new String(value.value.Character_String.value));
                             
                            }
                            }
                

                 }
             if (data.object_property == BACNET_PROPERTY_ID.PROP_OBJECT_LIST && value.tag == (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_OBJECT_ID)
                 {
                     UInt32 device_id = 0;
                     Device_Manager manager = Device_Manager.Get_Device_Manager();
                     while (value != null&&value.tag == (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_OBJECT_ID)
             {
                

                 if (value.value.Object_Id.type == (Byte)BACNET_OBJECT_TYPE.OBJECT_DEVICE)
                 {
                     device_id = value.value.Object_Id.instance;
                 }
                 if (!manager.Is_Exist(device_id))
                 {
                     if (value.value.Object_Id.type == (Byte)BACNET_OBJECT_TYPE.BACNET_LIFT)
                     {
                         Bacnet_Lift new_lift = new Bacnet_Lift();
                         new_lift.device_object.Object_Identifier.instance = device_id;

                         new_lift.lift.Set_OBJECT_ID(value.value.Object_Id);
                         new_lift.address = src;

                         manager.Add_Lift(new_lift);
                     }
                     if (value.value.Object_Id.type == (Byte)BACNET_OBJECT_TYPE.BACNET_ESCALATORS)
                     {
                         Bacnet_Escalators new_escalators = new Bacnet_Escalators();
                         new_escalators.device_object.Object_Identifier.instance = device_id;
                         new_escalators.address = src;
                         manager.Add_Escalators(new_escalators);
                     }
                 }
                 value = value.next;
                


             }

                 }
                         
                     
             

         }
         private int Decode_Ack_Service_Request(ref Byte[] apdu, UInt16 apdu_len, ref BACnet_Read_Property_Data rpdata, int pos = 0)
         {
             Byte tag_number = 0;
             UInt32 len_value_type = 0;
             int len = 0;        /* total length of decodes */
             UInt16 object_type = 0;        /* object type */
             UInt32 property = 0;      /* for decoding */
           

             len = 1;
             len += BasicalProcessor.Decode_Object_Id(ref apdu, ref object_type, ref rpdata.object_instance, len);
             rpdata.object_type = (BACNET_OBJECT_TYPE)object_type;
             len += BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value_type, len);
             if (tag_number != 1)
                 return -1;
             len += BasicalProcessor.Decode_Enumerated(ref apdu, len_value_type, ref property, len);
             rpdata.object_property = (BACNET_PROPERTY_ID)property;
             /* Tag 2: Optional Array Index 没有 */
             rpdata.array_index = BacnetConst.BACNET_ARRAY_ALL;
             if (BasicalProcessor.Decode_Is_Opening_Tag_Number(ref apdu, 3, pos + len))
                 len++;
             rpdata.application_data_pos = len;
             rpdata.application_data_len = (int)apdu_len - len - 1;
             return len;

         }

           

    }




  internal class BACnet_Read_Property_Data
   {
        public BACNET_OBJECT_TYPE object_type;
        public UInt32 object_instance;
        public  BACNET_PROPERTY_ID object_property;
        public UInt32 array_index;
        public int application_data_pos;
        public int application_data_len;
        public BACnet_Read_Property_Data(BACNET_OBJECT_TYPE obj_t,UInt32 obi_i,BACNET_PROPERTY_ID obj_pr,UInt32 index=BacnetConst.BACNET_ARRAY_ALL)
        {
            object_type = obj_t;
            object_instance = obi_i;
            object_property = obj_pr;
            array_index =index;
          }
        public  BACnet_Read_Property_Data()
        {

        }
    //   BACNET_ERROR_CLASS error_class;
    //   BACNET_ERROR_CODE error_code;
   }
}

