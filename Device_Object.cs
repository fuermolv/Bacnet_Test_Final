using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
     public class Device_Object
    {
        public BACNET_OBJECT_ID Object_Identifier;
        public String Object_Name;
        private BACNET_OBJECT_TYPE Objcet_Type;
        public BACnetDeviceStatus System_Status;
        public String Vendor_Name;
        public UInt16 Vendor_Identifier;
        public String Model_Name;
        public String Firmware_Revision;
        public String Application_Software_Version;
        public uint Protocol_Version;
        public uint Protocol_Conformance_Class;
        //Protocol_Object_Types_Supported
        //Protocol_Service_Supported
        public List<BACNET_OBJECT_ID> Object_List;
        public uint Max_APDU_Length_Accepted;
        public BACnetSegmentation Segmentation_Supported;
        public uint APDU_Segment_Timeout;
        public uint APDU_Timeout;
        public uint Number_Of_APDU_Retries;
        public List<BACnetAddressBinding> Device_Address_Binding;

        /*每个 BACnet 设备有且只有一个设备对象。每个设备对象由它的对象标识符属性确定，该属
         性在 BACnet 设备中乃至整个 BACnet 互联网中都是唯一的。*/
        public Device_Object()
        {
            Object_Identifier = new BACNET_OBJECT_ID();
            Object_List = new List<BACNET_OBJECT_ID>();
            Device_Address_Binding = new List<BACnetAddressBinding>();
            Object_Identifier.type = (UInt16)BACNET_OBJECT_TYPE.OBJECT_DEVICE;
            Objcet_Type = BACNET_OBJECT_TYPE.OBJECT_DEVICE;

        }
        public Device_Object(String s)
        {
            Object_Identifier = new BACNET_OBJECT_ID();
            Object_Identifier.instance = 10;
            Object_Name = s;
            Object_Identifier.type = (UInt16)BACNET_OBJECT_TYPE.OBJECT_DEVICE;
            Objcet_Type = BACNET_OBJECT_TYPE.OBJECT_DEVICE;
            System_Status = BACnetDeviceStatus.Non_Operational;
            Vendor_Name = "GDUT";
            Vendor_Identifier = 6;
            Model_Name = "1.0";
            Firmware_Revision = "1.0";
            Application_Software_Version = "1.0";
            Protocol_Version = 1;
            Protocol_Conformance_Class = 1;
            Object_List = new List<BACNET_OBJECT_ID>();
            Object_List.Add(Object_Identifier);
            Max_APDU_Length_Accepted = 1476;
            Segmentation_Supported = BACnetSegmentation.No_Segmentation;
            APDU_Segment_Timeout = 2000;
            APDU_Timeout = 60000;
            Number_Of_APDU_Retries = 3;
            Device_Address_Binding = new List<BACnetAddressBinding>();

    
        }
        
        public void Set_OBJECT_ID(BACNET_OBJECT_ID id)
        {
            Object_Identifier.instance = id.instance;
            Object_Identifier.type = id.type;
    
        }
        public uint Get_OBJECT_ID_Number()
        {
            return Object_Identifier.instance;
        }
    }
}
