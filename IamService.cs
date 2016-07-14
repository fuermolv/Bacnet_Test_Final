using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class IamService
    {
       public int Pack_I_Am_Service(ref byte[] Handler_Transmit_Buffer)
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
            n_pro.Encode_NpduData(ref npdu_data, false, BACNET_MESSAGE_PRIORITY.MESSAGE_PRIORITY_NORMAL);
            pdu_len = n_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref my_address, ref npdu_data);
            Device_Object device_obj=Local_Device.this_device.device_object;

            len = iam_encode_apdu(ref Handler_Transmit_Buffer, device_obj.Get_OBJECT_ID_Number(), device_obj.Max_APDU_Length_Accepted,(int) device_obj.Segmentation_Supported, device_obj.Vendor_Identifier, pdu_len);

            pdu_len += len;
            byte_len = b_pro.Encode(ref Handler_Transmit_Buffer, ref dest, ref npdu_data, pdu_len);

            return byte_len; 

        }
        private int iam_encode_apdu(ref byte[] apdu,UInt32 device_id, uint max_apdu, int segmentation, UInt16 vendor_id, int pos)
        {
            int len = 0;        /* length of each encoding */
            int apdu_len = 0;   /* total length of the apdu, return value */

            if (apdu != null)
            {
                apdu[pos + 0] = (byte)BACNET_PDU_TYPE.PDU_TYPE_UNCONFIRMED_SERVICE_REQUEST;
                apdu[pos + 1] = (byte)BACNET_UNCONFIRMED_SERVICE.SERVICE_UNCONFIRMED_I_AM;     /* service choice */
                apdu_len = 2;
               len =BasicalProcessor.Encode_Application_Object_Id(ref apdu,(int)BACNET_OBJECT_TYPE.OBJECT_DEVICE, device_id, pos+apdu_len);
               apdu_len += len;
               len = BasicalProcessor.Encode_Application_Unsigned(ref apdu, max_apdu, pos + apdu_len);
               apdu_len += len;
               len = BasicalProcessor.Encode_Application_Enumerated(ref apdu, (UInt32)segmentation, pos + apdu_len);
               apdu_len += len;
               len = BasicalProcessor.Encode_Application_Unsigned(ref apdu, vendor_id, pos + apdu_len);
               apdu_len += len;
            }
            return apdu_len;

        }
        internal void Handler_Iam(ref byte[] service_request,UInt16 service_len,ref BACNET_ADDRESS src)
        {
            int len = 0;
            UInt32 device_id = 0; ;
            uint max_apdu = 0;
            int segmentation = 0;
            UInt16 vendor_id = 0;
            len =iam_decode_service_request(ref service_request, ref device_id, ref max_apdu, ref segmentation, ref vendor_id);
            //添加对Iam的处理
            Device_Manager manager = Device_Manager.Get_Device_Manager();
            Bacnet_Device remote_deviec = new Bacnet_Device();
            remote_deviec.address = src;
            remote_deviec.device_object.Vendor_Identifier = vendor_id;
            remote_deviec.device_object.Object_Identifier.instance = device_id;
            remote_deviec.device_object.Object_Identifier.type = (UInt16)BACNET_OBJECT_TYPE.OBJECT_DEVICE;
            remote_deviec.device_object.Max_APDU_Length_Accepted = max_apdu;
            remote_deviec.device_object.Segmentation_Supported = (BACnetSegmentation)segmentation;
            bool su = true;
            foreach( Bacnet_Device temp in manager.device_list)
            {
                if (temp.device_object.Get_OBJECT_ID_Number() == device_id)
                    su = false;
            }
            if(su)
            manager.device_list.Add(remote_deviec);
        }
        private int iam_decode_service_request(ref byte[] apdu,ref UInt32 Device_id,ref uint Max_apdu,ref int Segmentation,ref UInt16 Vendor_id)
        {
            int len = 0;
            int apdu_len = 0;   /* total length of the apdu, return value */
            UInt16 object_type = 0;   /* should be a Device Object */
            UInt32 object_instance = 0;
            byte tag_number = 0;
            UInt32 len_value = 0;
            UInt32 decoded_value = 0;

            len = BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value, apdu_len);
            apdu_len += len;
            if (tag_number != (byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_OBJECT_ID)
                return -1;
            len = BasicalProcessor.Decode_Object_Id(ref apdu, ref object_type, ref object_instance, apdu_len);
            apdu_len += len;
            if (object_type != (UInt16)BACNET_OBJECT_TYPE.OBJECT_DEVICE)
                return -1;
            if (object_instance != 0)
                Device_id = object_instance;
            /* MAX APDU - unsigned */
            len = BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value, apdu_len);
            apdu_len += len;
            len = BasicalProcessor.Decode_Unsigned(ref apdu, len_value, ref decoded_value, apdu_len);
            apdu_len += len;
            if (len_value != 0)
                Max_apdu = (uint)decoded_value;
            /* Segmentation - enumerated */
            len = BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value, apdu_len);
            apdu_len += len;
            if (tag_number != (byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_ENUMERATED)
                return -1;
            len = BasicalProcessor.Decode_Enumerated(ref apdu, len_value, ref decoded_value, apdu_len);
            apdu_len += len;
            if (decoded_value >= (uint)BACNET_SEGMENTATION.MAX_BACNET_SEGMENTATION)
                return -1;

                Segmentation = (int)decoded_value;
            /* Vendor ID - unsigned16 */
            len = BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value, apdu_len);
            apdu_len += len;

            len = BasicalProcessor.Decode_Unsigned(ref apdu, len_value, ref decoded_value, apdu_len);
            apdu_len += len;

            if (decoded_value > 0xFFFF)
                return -1;
           
                Vendor_id = (UInt16)decoded_value;
            return apdu_len;

        }
    }
}
