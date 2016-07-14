using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Bacnet_Test_Final
{
     enum BACNET_BVLC_FUNCTION
    {
        BVLC_RESULT = 0,
        BVLC_WRITE_BROADCAST_DISTRIBUTION_TABLE = 1,
        BVLC_READ_BROADCAST_DIST_TABLE = 2,
        BVLC_READ_BROADCAST_DIST_TABLE_ACK = 3,
        BVLC_FORWARDED_NPDU = 4,
        BVLC_REGISTER_FOREIGN_DEVICE = 5,
        BVLC_READ_FOREIGN_DEVICE_TABLE = 6,
        BVLC_READ_FOREIGN_DEVICE_TABLE_ACK = 7,
        BVLC_DELETE_FOREIGN_DEVICE_TABLE_ENTRY = 8,
        BVLC_DISTRIBUTE_BROADCAST_TO_NETWORK = 9,
        BVLC_ORIGINAL_UNICAST_NPDU = 10,
        BVLC_ORIGINAL_BROADCAST_NPDU = 11,
        MAX_BVLC_FUNCTION = 12
    }

   public  enum BACNET_PDU_TYPE
    {
        PDU_TYPE_CONFIRMED_SERVICE_REQUEST = 0,
        PDU_TYPE_UNCONFIRMED_SERVICE_REQUEST = 0x10,
        PDU_TYPE_SIMPLE_ACK = 0x20,
        PDU_TYPE_COMPLEX_ACK = 0x30,
        PDU_TYPE_SEGMENT_ACK = 0x40,
        PDU_TYPE_ERROR = 0x50,
        PDU_TYPE_REJECT = 0x60,
        PDU_TYPE_ABORT = 0x70
    }
    enum BACNET_APPLICATION_TAG
    {
        BACNET_APPLICATION_TAG_NULL = 0,
        BACNET_APPLICATION_TAG_BOOLEAN = 1,
        BACNET_APPLICATION_TAG_UNSIGNED_INT = 2,
        BACNET_APPLICATION_TAG_SIGNED_INT = 3,
        BACNET_APPLICATION_TAG_REAL = 4,
        BACNET_APPLICATION_TAG_DOUBLE = 5,
        BACNET_APPLICATION_TAG_OCTET_STRING = 6,
        BACNET_APPLICATION_TAG_CHARACTER_STRING = 7,
        BACNET_APPLICATION_TAG_BIT_STRING = 8,
        BACNET_APPLICATION_TAG_ENUMERATED = 9,
        BACNET_APPLICATION_TAG_DATE = 10,
        BACNET_APPLICATION_TAG_TIME = 11,
        BACNET_APPLICATION_TAG_OBJECT_ID = 12,
        BACNET_APPLICATION_TAG_RESERVE1 = 13,
        BACNET_APPLICATION_TAG_RESERVE2 = 14,
        BACNET_APPLICATION_TAG_RESERVE3 = 15,
        MAX_BACNET_APPLICATION_TAG = 16
    }
     enum BACNET_MESSAGE_PRIORITY
    {
        MESSAGE_PRIORITY_NORMAL = 0,
        MESSAGE_PRIORITY_URGENT = 1,
        MESSAGE_PRIORITY_CRITICAL_EQUIPMENT = 2,
        MESSAGE_PRIORITY_LIFE_SAFETY = 3
    }
   public enum BACnetDeviceStatus
    {
        Operational=0,
        Operational_Read_Only=1,
        Download_Required=2,
        Download_In_Progress=3,
        Non_Operational=4,
    }
     enum BACNET_NETWORK_MESSAGE_TYPE
    {
        NETWORK_MESSAGE_WHO_IS_ROUTER_TO_NETWORK = 0,
        NETWORK_MESSAGE_I_AM_ROUTER_TO_NETWORK = 1,
        NETWORK_MESSAGE_I_COULD_BE_ROUTER_TO_NETWORK = 2,
        NETWORK_MESSAGE_REJECT_MESSAGE_TO_NETWORK = 3,
        NETWORK_MESSAGE_ROUTER_BUSY_TO_NETWORK = 4,
        NETWORK_MESSAGE_ROUTER_AVAILABLE_TO_NETWORK = 5,
        NETWORK_MESSAGE_INIT_RT_TABLE = 6,
        NETWORK_MESSAGE_INIT_RT_TABLE_ACK = 7,
        NETWORK_MESSAGE_ESTABLISH_CONNECTION_TO_NETWORK = 8,
        NETWORK_MESSAGE_DISCONNECT_CONNECTION_TO_NETWORK = 9,
        NETWORK_MESSAGE_INVALID = 0x100
    }
    public enum BACNET_UNCONFIRMED_SERVICE
    {
        SERVICE_UNCONFIRMED_I_AM = 0,
        SERVICE_UNCONFIRMED_I_HAVE = 1,
        SERVICE_UNCONFIRMED_COV_NOTIFICATION = 2,
        SERVICE_UNCONFIRMED_EVENT_NOTIFICATION = 3,
        SERVICE_UNCONFIRMED_PRIVATE_TRANSFER = 4,
        SERVICE_UNCONFIRMED_TEXT_MESSAGE = 5,
        SERVICE_UNCONFIRMED_TIME_SYNCHRONIZATION = 6,
        SERVICE_UNCONFIRMED_WHO_HAS = 7,
        SERVICE_UNCONFIRMED_WHO_IS = 8,
        SERVICE_UNCONFIRMED_UTC_TIME_SYNCHRONIZATION = 9,
        MAX_BACNET_UNCONFIRMED_SERVICE = 10
    }
    public enum BACnetSegmentation
    {
        Segmented_Both=0,
        Segmented_Transmit=1,
        Segmented_Receive=2,
        No_Segmentation=3
    }

   public  enum BACNET_CONFIRMED_SERVICE
    {
        /* Alarm and Event Services */
        SERVICE_CONFIRMED_ACKNOWLEDGE_ALARM = 0,
        SERVICE_CONFIRMED_COV_NOTIFICATION = 1,
        SERVICE_CONFIRMED_EVENT_NOTIFICATION = 2,
        SERVICE_CONFIRMED_GET_ALARM_SUMMARY = 3,
        SERVICE_CONFIRMED_GET_ENROLLMENT_SUMMARY = 4,
        SERVICE_CONFIRMED_GET_EVENT_INFORMATION = 29,
        SERVICE_CONFIRMED_SUBSCRIBE_COV = 5,
        SERVICE_CONFIRMED_SUBSCRIBE_COV_PROPERTY = 28,
        SERVICE_CONFIRMED_LIFE_SAFETY_OPERATION = 27,
        /* File Access Services */
        SERVICE_CONFIRMED_ATOMIC_READ_FILE = 6,
        SERVICE_CONFIRMED_ATOMIC_WRITE_FILE = 7,
        /* Object Access Services */
        SERVICE_CONFIRMED_ADD_LIST_ELEMENT = 8,
        SERVICE_CONFIRMED_REMOVE_LIST_ELEMENT = 9,
        SERVICE_CONFIRMED_CREATE_OBJECT = 10,
        SERVICE_CONFIRMED_DELETE_OBJECT = 11,
        SERVICE_CONFIRMED_READ_PROPERTY = 12,
        SERVICE_CONFIRMED_READ_PROP_CONDITIONAL = 13,
        SERVICE_CONFIRMED_READ_PROP_MULTIPLE = 14,
        SERVICE_CONFIRMED_READ_RANGE = 26,
        SERVICE_CONFIRMED_WRITE_PROPERTY = 15,
        SERVICE_CONFIRMED_WRITE_PROP_MULTIPLE = 16,
        /* Remote Device Management Services */
        SERVICE_CONFIRMED_DEVICE_COMMUNICATION_CONTROL = 17,
        SERVICE_CONFIRMED_PRIVATE_TRANSFER = 18,
        SERVICE_CONFIRMED_TEXT_MESSAGE = 19,
        SERVICE_CONFIRMED_REINITIALIZE_DEVICE = 20,
        /* Virtual Terminal Services */
        SERVICE_CONFIRMED_VT_OPEN = 21,
        SERVICE_CONFIRMED_VT_CLOSE = 22,
        SERVICE_CONFIRMED_VT_DATA = 23,
        /* Security Services */
        SERVICE_CONFIRMED_AUTHENTICATE = 24,
        SERVICE_CONFIRMED_REQUEST_KEY = 25,
        /* Services added after 1995 */
        /* readRange (26) see Object Access Services */
        /* lifeSafetyOperation (27) see Alarm and Event Services */
        /* subscribeCOVProperty (28) see Alarm and Event Services */
        /* getEventInformation (29) see Alarm and Event Services */
        MAX_BACNET_CONFIRMED_SERVICE = 30
    }
    /*    public class BACnet_Date
        {   
   
           public UInt16 year;
           public byte month;
           public byte day;
           public byte wday;     
        
        }
       public class BACnet_Time {
           public byte hour;
           public byte min;
           public byte sec;
           public byte hundredths;
    }*/
     struct BACNET_NPDU_DATA
    {

        public byte protocol_version;
        public bool data_expecting_reply;
        public bool network_layer_message;
        public BACNET_MESSAGE_PRIORITY priority;
        public BACNET_NETWORK_MESSAGE_TYPE network_message_type;
        public UInt16 vendor_id;
        public byte hop_count;
    }
   public struct BACnetAddressBinding
    {
        BACNET_OBJECT_ID obj_id;
        BACNET_ADDRESS address;
    }
     class ACKNOWLEDGE_ALARM_DATA
    {
        public UInt32 ProcessIdentifier;
        public BACNET_OBJECT_ID EventIdentifier;
        public BACNET_EVENT_STATE StateAcknowledged;
        public BACNET_TIMESTAMP TimeStamp;
        public BACNET_CHARACTER_STRING Source;
        public BACNET_TIMESTAMP TimeOfAcknowledgment;
        public ACKNOWLEDGE_ALARM_DATA()
        {
            Source = new BACNET_CHARACTER_STRING(10);
        }


    }
  
     class BACNET_COV_DATA
    {
        public UInt32 subscriberProcessIdentifier;
        public UInt32 initiatingDeviceIdentifier;
        public BACNET_OBJECT_ID monitoredObjectIdentifier;
        public UInt32 timeRemaining;     /* seconds */
        /* simple linked list of values */
        public List<BACNET_PROPERTY_VALUE> listOfValues;//一个链表
        public BACNET_COV_DATA()
        {
            listOfValues = new List<BACNET_PROPERTY_VALUE>();
        }
    }

     class BACNET_COV_SUBSCRIPTION
    {
        public Boolean valid;
        public Boolean issueConfirmedNotifications;
        public Boolean send_requested;
        public BACNET_ADDRESS dest;
        public UInt32 subscriberProcessIdentifier;
        public BACNET_OBJECT_ID monitoredObjectIdentifier;
        public Byte invokeID;   /* for confirmed COV */
        public UInt32 lifetime;  /* optional */

    }
     class BACNET_PROPERTY_VALUE
    {
        public BACNET_PROPERTY_ID propertyIdentifier;
        public UInt32 propertyArrayIndex;
        public BACNET_APPLICATION_DATA_VALUE value;
        public Byte priority;

        public BACNET_PROPERTY_VALUE()
        {
            value = new BACNET_APPLICATION_DATA_VALUE();

        }

    }
     struct BACNET_CONFIRMED_SERVICE_DATA
    {
        public Boolean segmented_message;
        public Boolean more_follows;
        public Boolean segmented_response_accepted;
        public int max_segs;
        public int max_resp;
        public Byte invoke_id;
        public Byte sequence_number;
        public Byte proposed_window_number;
    }
     struct BACNET_SUBSCRIBE_COV_DATA
    {
        public UInt32 subscriberProcessIdentifier;
        public BACNET_OBJECT_ID monitoredObjectIdentifier;
        public Boolean cancellationRequest;   /* true if this is a cancellation request */
        public Boolean issueConfirmedNotifications;   /* optional */
        public UInt32 lifetime;  /* seconds, optional */
        public BACNET_PROPERTY_REFERENCE monitoredProperty;
        public Boolean covIncrementPresent;   /* true if present */
        public float covIncrement; /* optional */
        // BACNET_ERROR_CLASS error_class;
        // BACNET_ERROR_CODE error_code;


    }
    public struct BACNET_OBJECT_ID
    {
        public UInt16 type;
        public UInt32 instance;

    }
     struct BACNET_PROPERTY_REFERENCE
    {
        public BACNET_PROPERTY_ID propertyIdentifier;
        public UInt32 propertyArrayIndex;
        public BACNET_APPLICATION_DATA_VALUE value;
        public BACnet_Access_Error error;
       
      
    }
      struct BACnet_Access_Error
    {
      public BACNET_ERROR_CLASS error_class;
      public BACNET_ERROR_CODE error_code;
    }

     class BACNET_APPLICATION_DATA_VALUE
    {
        public Boolean context_specific;      /* true if context specific data */
        public Byte context_tag;        /* only used for context specific data */
        public Byte tag;
        public struct type
        {
            public bool Boolean;
            public UInt32 Unsigned_Int;
            public Int32 Signed_Int;
            public float Real;
            public double Double;
            public BACNET_OCTET_STRING Octet_String;
            public BACNET_CHARACTER_STRING Character_String;
            public BACNET_BIT_STRING Bit_String;
            public UInt32 Enumerated;
            public BACNET_DATE Date;
            public BACNET_TIME Time;
            public BACNET_OBJECT_ID Object_Id;
        }
        public type value;
        public BACNET_APPLICATION_DATA_VALUE next;
        public void add(ref BACNET_APPLICATION_DATA_VALUE section)
        {
            next = section;
        }

    }
     class BACNET_BIT_STRING
    {
        const Byte MAX_BITSTRING_BYTES = 15;
        public Byte bits_used;
        public Byte[] value;
        public BACNET_BIT_STRING()
        {
            value = new Byte[MAX_BITSTRING_BYTES];
            bits_used = 0;
        }
        public Boolean set_octet(Byte index, Byte octet)
        {
            bool status = false;
            if (index < MAX_BITSTRING_BYTES)
            {
                value[index] = octet;
                status = true;
            }

            return status;
        }

        public Boolean set_bits_used(Byte bytes_used, Byte unused_bits)
        {
            bool status = false;
            bits_used = (Byte)(bytes_used * 8);
            bits_used -= unused_bits;
            status = true;
            return status;
        }
        public Byte bitstring_octet(Byte octet_index)
        {
            Byte octet;
            octet = value[octet_index];
            return octet;

        }
        public Byte byte_reverse_bits(Byte in_byte)
        {  //此处判断可能有问题
            Byte out_byte = 0;

            if ((in_byte & BacnetConst.BIT0) != 0)
            {
                out_byte |= BacnetConst.BIT7;
            }
            if ((in_byte & BacnetConst.BIT1) != 0)
            {
                out_byte |= BacnetConst.BIT6;
            }
            if ((in_byte & BacnetConst.BIT2) != 0)
            {
                out_byte |= BacnetConst.BIT5;
            }
            if ((in_byte & BacnetConst.BIT3) != 0)
            {
                out_byte |= BacnetConst.BIT4;
            }
            if ((in_byte & BacnetConst.BIT4) != 0)
            {
                out_byte |= BacnetConst.BIT3;
            }
            if ((in_byte & BacnetConst.BIT5) != 0)
            {
                out_byte |= BacnetConst.BIT2;
            }
            if ((in_byte & BacnetConst.BIT6) != 0)
            {
                out_byte |= BacnetConst.BIT1;
            }
            if ((in_byte & BacnetConst.BIT7) != 0)
            {
                out_byte |= BacnetConst.BIT0;
            }

            return out_byte;
        }
        public Byte bytes_used()
        {

            Byte len = 0;    /* return value */
            Byte used_bytes = 0;
            Byte last_bit = 0;

            last_bit = (Byte)(bits_used - 1);
            used_bytes = (Byte)(last_bit / 8);
            used_bytes++;
            len = used_bytes;
            return len;


        }
    }
   public class BACNET_CHARACTER_STRING
    {
        public uint size;
        public Byte encoding;
        public char[] value;
        public BACNET_CHARACTER_STRING(int n)
        {
            value = new char[n];
        }
        public bool init(ref Byte[] apdu, Byte encoding, uint length, int pos)
        {
            bool status = false;        /* return value */
            uint i = 0;   /* counter */
            size = 0;
            this.encoding = encoding;
            if (value != null)
            {
                for (i = 0; i < length; i++)
                {
                    if (i < length)
                    {
                         value[size] = (char)apdu[pos + i];
                        size++;
                    }
                    else
                    {
                        value[i] = (char)0;
                    }
                }
                status = true;
            }
            else
            {
                for (i = 0; i < BacnetConst.MAX_CHARACTER_STRING_BYTES; i++)
                {
                    value[i] = (char)0;
                }
            }
            return status;
        }
    }
     class BACNET_OCTET_STRING
    {
        public uint size;
        Byte[] value;
        public BACNET_OCTET_STRING()
        {
            Byte[] value = new Byte[1024];
        }
    }
     struct BACNET_CONFIRMED_SERVICE_ACK_DATA
    {
        public Boolean segmented_message;
        public Boolean more_follows;
        public Byte invoke_id;
        public Byte sequence_number;
        public Byte proposed_window_number;
    }
     public class BACNET_EVENT_NOTIFICATION_DATA
    {
        public UInt32 processIdentifier;
        public BACNET_OBJECT_ID initiatingObjectIdentifier;
        public BACNET_OBJECT_ID eventObjectIdentifier;
        public BACNET_TIMESTAMP timeStamp;
        public UInt32 notificationClass;
        public Byte priority;
        public BACNET_EVENT_TYPE eventType;
        public BACNET_CHARACTER_STRING messageText;
        public BACNET_NOTIFY_TYPE notifyType;
        public bool ackRequired;
        public BACNET_EVENT_STATE fromState;
        public BACNET_EVENT_STATE toState;
        public NotificationParams notificationParams;
        /* OPTIONAL - Set to NULL if not being used */
        public BACNET_EVENT_NOTIFICATION_DATA()
        {
            notificationParams = new NotificationParams();
        }





    }
     public class NotificationParams
     {
         internal CHANGE_OF_STATE change_of_state;
         internal Change_Of_LifeSafety change_of_lifesafety;
         internal Lift_Safe_Button lift_safe_button;
         public NotificationParams()
         {
             change_of_state.statusFlags = new BACNET_BIT_STRING();
             change_of_lifesafety.statusFlags = new BACNET_BIT_STRING();
         }
     }
     struct Change_Of_LifeSafety
     {
         public BACNET_LIFE_SAFETY_STATE newState;
         public BACNET_LIFE_SAFETY_MODE newMode;
         public BACNET_BIT_STRING statusFlags;
         public BACNET_LIFE_SAFETY_OPERATION operationExpected;
     }
    struct Lift_Safe_Button
    {
        public BACnetMessageCode code;
    }
         
     struct CHANGE_OF_STATE
    {
        public BACNET_PROPERTY_STATE newState;
        public BACNET_BIT_STRING statusFlags;
    }
     struct BACNET_PROPERTY_STATE
    {
        public BACNET_PROPERTY_STATE_TYPE tag;
        public UInt32 value;//uint32_t unsignedValue; 此处只用到这个

    }
    public struct BACNET_TIMESTAMP
    {
        public Byte tag;//TYPE_BACNET_TIMESTAMP_TYPE
        public BACNET_DATE_TIME value;

    }
    public struct BACNET_DATE_TIME
    {

        public BACNET_DATE date;
        public BACNET_TIME time;
    }
     public struct BACNET_DATE
    {
        public UInt16 year;      /* AD */
        public Byte month;      /* 1=Jan */
        public Byte day;        /* 1..31 */
        public Byte wday;
        /* 1=Monday-7=Sunday */

    }
     public struct BACNET_TIME
    {
        public Byte hour;
        public Byte min;
        public Byte sec;
        public Byte hundredths;

    }

     public enum BACNET_EVENT_STATE
    {
        EVENT_STATE_NORMAL = 0,
        EVENT_STATE_FAULT = 1,
        EVENT_STATE_OFFNORMAL = 2,
        EVENT_STATE_HIGH_LIMIT = 3,
        EVENT_STATE_LOW_LIMIT = 4
    }
   public  enum BACNET_NOTIFY_TYPE
    {
        NOTIFY_ALARM = 0,
        NOTIFY_EVENT = 1,
        NOTIFY_ACK_NOTIFICATION = 2
    }

     enum BACNET_SEGMENTATION
    {
        SEGMENTATION_BOTH = 0,
        SEGMENTATION_TRANSMIT = 1,
        SEGMENTATION_RECEIVE = 2,
        SEGMENTATION_NONE = 3,
        MAX_BACNET_SEGMENTATION = 4
    }
   public enum BACNET_EVENT_TYPE
    {
        EVENT_CHANGE_OF_BITSTRING = 0,
        EVENT_CHANGE_OF_STATE = 1,
        EVENT_CHANGE_OF_VALUE = 2,
        EVENT_COMMAND_FAILURE = 3,
        EVENT_FLOATING_LIMIT = 4,
        EVENT_OUT_OF_RANGE = 5,
        /*  complex-event-type        (6), -- see comment below */
        /*  event-buffer-ready   (7), -- context tag 7 is deprecated */
        EVENT_CHANGE_OF_LIFE_SAFETY = 8,
        EVENT_EXTENDED = 9,
        EVENT_BUFFER_READY = 10,
        EVENT_UNSIGNED_RANGE = 11,
        EVENT_LIFT_SAFE_BUTTON=64
        /* Enumerated values 0-63 are reserved for definition by ASHRAE.  */
        /* Enumerated values 64-65535 may be used by others subject to  */
    }
  public   enum BACNET_PROPERTY_STATE_TYPE
    {
        BOOLEAN_VALUE,
        BINARY_VALUE,
        EVENT_TYPE,
        POLARITY,
        PROGRAM_CHANGE,
        PROGRAM_STATE,
        REASON_FOR_HALT,
        RELIABILITY,
        STATE,
        SYSTEM_STATUS,
        UNITS,
        UNSIGNED_VALUE,
        LIFE_SAFETY_MODE,
        LIFE_SAFETY_STATE,
    }
    enum BACNET_OBJECT_TYPE
    {
        OBJECT_ANALOG_INPUT = 0,
        OBJECT_ANALOG_OUTPUT = 1,
        OBJECT_ANALOG_VALUE = 2,
        OBJECT_BINARY_INPUT = 3,
        OBJECT_BINARY_OUTPUT = 4,
        OBJECT_BINARY_VALUE = 5,
        OBJECT_CALENDAR = 6,
        OBJECT_COMMAND = 7,
        OBJECT_DEVICE = 8,
        OBJECT_EVENT_ENROLLMENT = 9,
        OBJECT_FILE = 10,
        OBJECT_GROUP = 11,
        OBJECT_LOOP = 12,
        OBJECT_MULTI_STATE_INPUT = 13,
        OBJECT_MULTI_STATE_OUTPUT = 14,
        OBJECT_NOTIFICATION_CLASS = 15,
        OBJECT_PROGRAM = 16,
        OBJECT_SCHEDULE = 17,
        OBJECT_AVERAGING = 18,
        OBJECT_MULTI_STATE_VALUE = 19,
        OBJECT_TRENDLOG = 20,
        OBJECT_LIFE_SAFETY_POINT = 21,
        OBJECT_LIFE_SAFETY_ZONE = 22,
        OBJECT_ACCUMULATOR = 23,
        OBJECT_PULSE_CONVERTER = 24,
        OBJECT_EVENT_LOG = 25,
        OBJECT_GLOBAL_GROUP = 26,
        OBJECT_TREND_LOG_MULTIPLE = 27,
        OBJECT_LOAD_CONTROL = 28,
        OBJECT_STRUCTURED_VIEW = 29,
        OBJECT_ACCESS_DOOR = 30,
        OBJECT_LIGHTING_OUTPUT = 31,
        OBJECT_ACCESS_CREDENTIAL = 32,      /* Addendum 2008-j */
        OBJECT_ACCESS_POINT = 33,
        OBJECT_ACCESS_RIGHTS = 34,
        OBJECT_ACCESS_USER = 35,
        OBJECT_ACCESS_ZONE = 36,
        OBJECT_CREDENTIAL_DATA_INPUT = 37,  /* authentication-factor-input */
        OBJECT_NETWORK_SECURITY = 38,       /* Addendum 2008-g */
        OBJECT_BITSTRING_VALUE = 39,        /* Addendum 2008-w */
        OBJECT_CHARACTERSTRING_VALUE = 40,
        OBJECT_DATE_PATTERN_VALUE = 41,
        OBJECT_DATE_VALUE = 42,
        OBJECT_DATETIME_PATTERN_VALUE = 43,
        OBJECT_DATETIME_VALUE = 44,
        OBJECT_INTEGER_VALUE = 45,
        OBJECT_LARGE_ANALOG_VALUE = 46,
        OBJECT_OCTETSTRING_VALUE = 47,
        OBJECT_POSITIVE_INTEGER_VALUE = 48,
        OBJECT_TIME_PATTERN_VALUE = 49,
        OBJECT_TIME_VALUE = 50,

        MAX_ASHRAE_OBJECT_TYPE = 51,
        /* used for bit string loop */
        PROPRIETARY_BACNET_OBJECT_TYPE = 128,
        BACNET_LIFT = 129,
        BACNET_ESCALATORS = 130,
        MAX_BACNET_OBJECT_TYPE = 1024
        /* Enumerated values 0-127 are reserved for definition by ASHRAE. */
        /* Enumerated values 128-1023 may be used by others subject to  */
        /* the procedures and constraints described in Clause 23. */
    }
     enum BACNET_PROPERTY_ID
    {

        PROP_ACKED_TRANSITIONS = 0,
        PROP_ACK_REQUIRED = 1,
        PROP_ACTION = 2,
        PROP_ACTION_TEXT = 3,
        PROP_ACTIVE_TEXT = 4,
        PROP_ACTIVE_VT_SESSIONS = 5,
        PROP_ALARM_VALUE = 6,
        PROP_ALARM_VALUES = 7,
        PROP_ALL = 8,
        PROP_ALL_WRITES_SUCCESSFUL = 9,
        PROP_APDU_SEGMENT_TIMEOUT = 10,
        PROP_APDU_TIMEOUT = 11,
        PROP_APPLICATION_SOFTWARE_VERSION = 12,
        PROP_ARCHIVE = 13,
        PROP_BIAS = 14,
        PROP_CHANGE_OF_STATE_COUNT = 15,
        PROP_CHANGE_OF_STATE_TIME = 16,
        PROP_NOTIFICATION_CLASS = 17,
        PROP_BLANK_1 = 18,
        PROP_CONTROLLED_VARIABLE_REFERENCE = 19,
        PROP_CONTROLLED_VARIABLE_UNITS = 20,
        PROP_CONTROLLED_VARIABLE_VALUE = 21,
        PROP_COV_INCREMENT = 22,
        PROP_DATE_LIST = 23,
        PROP_DAYLIGHT_SAVINGS_STATUS = 24,
        PROP_DEADBAND = 25,
        PROP_DERIVATIVE_CONSTANT = 26,
        PROP_DERIVATIVE_CONSTANT_UNITS = 27,
        PROP_DESCRIPTION = 28,
        PROP_DESCRIPTION_OF_HALT = 29,
        PROP_DEVICE_ADDRESS_BINDING = 30,
        PROP_DEVICE_TYPE = 31,
        PROP_EFFECTIVE_PERIOD = 32,
        PROP_ELAPSED_ACTIVE_TIME = 33,
        PROP_ERROR_LIMIT = 34,
        PROP_EVENT_ENABLE = 35,
        PROP_EVENT_STATE = 36,
        PROP_EVENT_TYPE = 37,
        PROP_EXCEPTION_SCHEDULE = 38,
        PROP_FAULT_VALUES = 39,
        PROP_FEEDBACK_VALUE = 40,
        PROP_FILE_ACCESS_METHOD = 41,
        PROP_FILE_SIZE = 42,
        PROP_FILE_TYPE = 43,
        PROP_FIRMWARE_REVISION = 44,
        PROP_HIGH_LIMIT = 45,
        PROP_INACTIVE_TEXT = 46,
        PROP_IN_PROCESS = 47,
        PROP_INSTANCE_OF = 48,
        PROP_INTEGRAL_CONSTANT = 49,
        PROP_INTEGRAL_CONSTANT_UNITS = 50,
        PROP_ISSUE_CONFIRMED_NOTIFICATIONS = 51,
        PROP_LIMIT_ENABLE = 52,
        PROP_LIST_OF_GROUP_MEMBERS = 53,
        PROP_LIST_OF_OBJECT_PROPERTY_REFERENCES = 54,
        PROP_LIST_OF_SESSION_KEYS = 55,
        PROP_LOCAL_DATE = 56,
        PROP_LOCAL_TIME = 57,
        PROP_LOCATION = 58,
        PROP_LOW_LIMIT = 59,
        PROP_MANIPULATED_VARIABLE_REFERENCE = 60,
        PROP_MAXIMUM_OUTPUT = 61,
        PROP_MAX_APDU_LENGTH_ACCEPTED = 62,
        PROP_MAX_INFO_FRAMES = 63,
        PROP_MAX_MASTER = 64,
        PROP_MAX_PRES_VALUE = 65,
        PROP_MINIMUM_OFF_TIME = 66,
        PROP_MINIMUM_ON_TIME = 67,
        PROP_MINIMUM_OUTPUT = 68,
        PROP_MIN_PRES_VALUE = 69,
        PROP_MODEL_NAME = 70,
        PROP_MODIFICATION_DATE = 71,
        PROP_NOTIFY_TYPE = 72,
        PROP_NUMBER_OF_APDU_RETRIES = 73,
        PROP_NUMBER_OF_STATES = 74,
        PROP_OBJECT_IDENTIFIER = 75,
        PROP_OBJECT_LIST = 76,
        PROP_OBJECT_NAME = 77,
        PROP_OBJECT_PROPERTY_REFERENCE = 78,
        PROP_OBJECT_TYPE = 79,
        PROP_OPTIONAL = 80,
        PROP_OUT_OF_SERVICE = 81,
        PROP_OUTPUT_UNITS = 82,
        PROP_EVENT_PARAMETERS = 83,
        PROP_POLARITY = 84,
        PROP_PRESENT_VALUE = 85,
        PROP_PRIORITY = 86,
        PROP_PRIORITY_ARRAY = 87,
        PROP_PRIORITY_FOR_WRITING = 88,
        PROP_PROCESS_IDENTIFIER = 89,
        PROP_PROGRAM_CHANGE = 90,
        PROP_PROGRAM_LOCATION = 91,
        PROP_PROGRAM_STATE = 92,
        PROP_PROPORTIONAL_CONSTANT = 93,
        PROP_PROPORTIONAL_CONSTANT_UNITS = 94,
        PROP_PROTOCOL_CONFORMANCE_CLASS = 95,       /* deleted in version 1 revision 2 */
        PROP_PROTOCOL_OBJECT_TYPES_SUPPORTED = 96,
        PROP_PROTOCOL_SERVICES_SUPPORTED = 97,
        PROP_PROTOCOL_VERSION = 98,
        PROP_READ_ONLY = 99,
        PROP_REASON_FOR_HALT = 100,
        PROP_RECIPIENT = 101,
        PROP_RECIPIENT_LIST = 102,
        PROP_RELIABILITY = 103,
        PROP_RELINQUISH_DEFAULT = 104,
        PROP_REQUIRED = 105,
        PROP_RESOLUTION = 106,
        PROP_SEGMENTATION_SUPPORTED = 107,
        PROP_SETPOINT = 108,
        PROP_SETPOINT_REFERENCE = 109,
        PROP_STATE_TEXT = 110,
        PROP_STATUS_FLAGS = 111,
        PROP_SYSTEM_STATUS = 112,
        PROP_TIME_DELAY = 113,
        PROP_TIME_OF_ACTIVE_TIME_RESET = 114,
        PROP_TIME_OF_STATE_COUNT_RESET = 115,
        PROP_TIME_SYNCHRONIZATION_RECIPIENTS = 116,
        PROP_UNITS = 117,
        PROP_UPDATE_INTERVAL = 118,
        PROP_UTC_OFFSET = 119,
        PROP_VENDOR_IDENTIFIER = 120,
        PROP_VENDOR_NAME = 121,
        PROP_VT_CLASSES_SUPPORTED = 122,
        PROP_WEEKLY_SCHEDULE = 123,
        PROP_ATTEMPTED_SAMPLES = 124,
        PROP_AVERAGE_VALUE = 125,
        PROP_BUFFER_SIZE = 126,
        PROP_CLIENT_COV_INCREMENT = 127,
        PROP_COV_RESUBSCRIPTION_INTERVAL = 128,
        PROP_CURRENT_NOTIFY_TIME = 129,
        PROP_EVENT_TIME_STAMPS = 130,
        PROP_LOG_BUFFER = 131,
        PROP_LOG_DEVICE_OBJECT_PROPERTY = 132,
        /* The enable property is renamed from log-enable in
           Addendum b to ANSI/ASHRAE 135-2004(135b-2) */
        PROP_ENABLE = 133,
        PROP_LOG_INTERVAL = 134,
        PROP_MAXIMUM_VALUE = 135,
        PROP_MINIMUM_VALUE = 136,
        PROP_NOTIFICATION_THRESHOLD = 137,
        PROP_PREVIOUS_NOTIFY_TIME = 138,
        PROP_PROTOCOL_REVISION = 139,
        PROP_RECORDS_SINCE_NOTIFICATION = 140,
        PROP_RECORD_COUNT = 141,
        PROP_START_TIME = 142,
        PROP_STOP_TIME = 143,
        PROP_STOP_WHEN_FULL = 144,
        PROP_TOTAL_RECORD_COUNT = 145,
        PROP_VALID_SAMPLES = 146,
        PROP_WINDOW_INTERVAL = 147,
        PROP_WINDOW_SAMPLES = 148,
        PROP_MAXIMUM_VALUE_TIMESTAMP = 149,
        PROP_MINIMUM_VALUE_TIMESTAMP = 150,
        PROP_VARIANCE_VALUE = 151,
        PROP_ACTIVE_COV_SUBSCRIPTIONS = 152,
        PROP_BACKUP_FAILURE_TIMEOUT = 153,
        PROP_CONFIGURATION_FILES = 154,
        PROP_DATABASE_REVISION = 155,
        PROP_DIRECT_READING = 156,
        PROP_LAST_RESTORE_TIME = 157,
        PROP_MAINTENANCE_REQUIRED = 158,
        PROP_MEMBER_OF = 159,
        PROP_MODE = 160,
        PROP_OPERATION_EXPECTED = 161,
        PROP_SETTING = 162,
        PROP_SILENCED = 163,
        PROP_TRACKING_VALUE = 164,
        PROP_ZONE_MEMBERS = 165,
        PROP_LIFE_SAFETY_ALARM_VALUES = 166,
        PROP_MAX_SEGMENTS_ACCEPTED = 167,
        PROP_PROFILE_NAME = 168,
        PROP_AUTO_SLAVE_DISCOVERY = 169,
        PROP_MANUAL_SLAVE_ADDRESS_BINDING = 170,
        PROP_SLAVE_ADDRESS_BINDING = 171,
        PROP_SLAVE_PROXY_ENABLE = 172,
        PROP_LAST_NOTIFY_RECORD = 173,
        PROP_SCHEDULE_DEFAULT = 174,
        PROP_ACCEPTED_MODES = 175,
        PROP_ADJUST_VALUE = 176,
        PROP_COUNT = 177,
        PROP_COUNT_BEFORE_CHANGE = 178,
        PROP_COUNT_CHANGE_TIME = 179,
        PROP_COV_PERIOD = 180,
        PROP_INPUT_REFERENCE = 181,
        PROP_LIMIT_MONITORING_INTERVAL = 182,
        PROP_LOGGING_DEVICE = 183,
        PROP_LOGGING_RECORD = 184,
        PROP_PRESCALE = 185,
        PROP_PULSE_RATE = 186,
        PROP_SCALE = 187,
        PROP_SCALE_FACTOR = 188,
        PROP_UPDATE_TIME = 189,
        PROP_VALUE_BEFORE_CHANGE = 190,
        PROP_VALUE_SET = 191,
        PROP_VALUE_CHANGE_TIME = 192,
        /* enumerations 193-206 are new */
        PROP_ALIGN_INTERVALS = 193,
        PROP_GROUP_MEMBER_NAMES = 194,
        PROP_INTERVAL_OFFSET = 195,
        PROP_LAST_RESTART_REASON = 196,
        PROP_LOGGING_TYPE = 197,
        PROP_MEMBER_STATUS_FLAGS = 198,
        PROP_NOTIFICATION_PERIOD = 199,
        PROP_PREVIOUS_NOTIFY_RECORD = 200,
        PROP_REQUESTED_UPDATE_INTERVAL = 201,
        PROP_RESTART_NOTIFICATION_RECIPIENTS = 202,
        PROP_TIME_OF_DEVICE_RESTART = 203,
        PROP_TIME_SYNCHRONIZATION_INTERVAL = 204,
        PROP_TRIGGER = 205,
        PROP_UTC_TIME_SYNCHRONIZATION_RECIPIENTS = 206,
        /* enumerations 207-211 are used in Addendum d to ANSI/ASHRAE 135-2004 */
        PROP_NODE_SUBTYPE = 207,
        PROP_NODE_TYPE = 208,
        PROP_STRUCTURED_OBJECT_LIST = 209,
        PROP_SUBORDINATE_ANNOTATIONS = 210,
        PROP_SUBORDINATE_LIST = 211,
        /* enumerations 212-225 are used in Addendum e to ANSI/ASHRAE 135-2004 */
        PROP_ACTUAL_SHED_LEVEL = 212,
        PROP_DUTY_WINDOW = 213,
        PROP_EXPECTED_SHED_LEVEL = 214,
        PROP_FULL_DUTY_BASELINE = 215,
        /* enumerations 216-217 are used in Addendum i to ANSI/ASHRAE 135-2004 */
        PROP_BLINK_PRIORITY_THRESHOLD = 216,
        PROP_BLINK_TIME = 217,
        /* enumerations 212-225 are used in Addendum e to ANSI/ASHRAE 135-2004 */
        PROP_REQUESTED_SHED_LEVEL = 218,
        PROP_SHED_DURATION = 219,
        PROP_SHED_LEVEL_DESCRIPTIONS = 220,
        PROP_SHED_LEVELS = 221,
        PROP_STATE_DESCRIPTION = 222,
        /* enumerations 223-225 are used in Addendum i to ANSI/ASHRAE 135-2004 */
        PROP_FADE_TIME = 223,
        PROP_LIGHTING_COMMAND = 224,
        PROP_LIGHTING_COMMAND_PRIORITY = 225,
        /* enumerations 226-235 are used in Addendum f to ANSI/ASHRAE 135-2004 */
        PROP_DOOR_ALARM_STATE = 226,
        PROP_DOOR_EXTENDED_PULSE_TIME = 227,
        PROP_DOOR_MEMBERS = 228,
        PROP_DOOR_OPEN_TOO_LONG_TIME = 229,
        PROP_DOOR_PULSE_TIME = 230,
        PROP_DOOR_STATUS = 231,
        PROP_DOOR_UNLOCK_DELAY_TIME = 232,
        PROP_LOCK_STATUS = 233,
        PROP_MASKED_ALARM_VALUES = 234,
        PROP_SECURED_STATUS = 235,
        /* enumerations 236-243 are used in Addendum i to ANSI/ASHRAE 135-2004 */
        PROP_OFF_DELAY = 236,
        PROP_ON_DELAY = 237,
        PROP_POWER = 238,
        PROP_POWER_ON_VALUE = 239,
        PROP_PROGRESS_VALUE = 240,
        PROP_RAMP_RATE = 241,
        PROP_STEP_INCREMENT = 242,
        PROP_SYSTEM_FAILURE_VALUE = 243,
        /* enumerations 244-311 are used in Addendum j to ANSI/ASHRAE 135-2004 */
        PROP_ABSENTEE_LIMIT = 244,
        PROP_ACCESS_ALARM_EVENTS = 245,
        PROP_ACCESS_DOORS = 246,
        PROP_ACCESS_EVENT = 247,
        PROP_ACCESS_EVENT_AUTHENTICATION_FACTOR = 248,
        PROP_ACCESS_EVENT_CREDENTIAL = 249,
        PROP_ACCESS_EVENT_TIME = 250,
        PROP_ACCESS_TRANSACTION_EVENTS = 251,
        PROP_ACCOMPANIMENT = 252,
        PROP_ACCOMPANIMENT_TIME = 253,
        PROP_ACTIVATION_TIME = 254,
        PROP_ACTIVE_AUTHENTICATION_POLICY = 255,
        PROP_ASSIGNED_ACCESS_RIGHTS = 256,
        PROP_AUTHENTICATION_FACTORS = 257,
        PROP_AUTHENTICATION_POLICY_LIST = 258,
        PROP_AUTHENTICATION_POLICY_NAMES = 259,
        PROP_AUTHORIZATION_STATUS = 260,
        PROP_AUTHORIZATION_MODE = 261,
        PROP_BELONGS_TO = 262,
        PROP_CREDENTIAL_DISABLE = 263,
        PROP_CREDENTIAL_STATUS = 264,
        PROP_CREDENTIALS = 265,
        PROP_CREDENTIALS_IN_ZONE = 266,
        PROP_DAYS_REMAINING = 267,
        PROP_ENTRY_POINTS = 268,
        PROP_EXIT_POINTS = 269,
        PROP_EXPIRY_TIME = 270,
        PROP_EXTENDED_TIME_ENABLE = 271,
        PROP_FAILED_ATTEMPT_EVENTS = 272,
        PROP_FAILED_ATTEMPTS = 273,
        PROP_FAILED_ATTEMPTS_TIME = 274,
        PROP_LAST_ACCESS_EVENT = 275,
        PROP_LAST_ACCESS_POINT = 276,
        PROP_LAST_CREDENTIAL_ADDED = 277,
        PROP_LAST_CREDENTIAL_ADDED_TIME = 278,
        PROP_LAST_CREDENTIAL_REMOVED = 279,
        PROP_LAST_CREDENTIAL_REMOVED_TIME = 280,
        PROP_LAST_USE_TIME = 281,
        PROP_LOCKOUT = 282,
        PROP_LOCKOUT_RELINQUISH_TIME = 283,
        PROP_MASTER_EXEMPTION = 284,
        PROP_MAX_FAILED_ATTEMPTS = 285,
        PROP_MEMBERS = 286,
        PROP_MUSTER_POINT = 287,
        PROP_NEGATIVE_ACCESS_RULES = 288,
        PROP_NUMBER_OF_AUTHENTICATION_POLICIES = 289,
        PROP_OCCUPANCY_COUNT = 290,
        PROP_OCCUPANCY_COUNT_ADJUST = 291,
        PROP_OCCUPANCY_COUNT_ENABLE = 292,
        PROP_OCCUPANCY_EXEMPTION = 293,
        PROP_OCCUPANCY_LOWER_LIMIT = 294,
        PROP_OCCUPANCY_LOWER_LIMIT_ENFORCED = 295,
        PROP_OCCUPANCY_STATE = 296,
        PROP_OCCUPANCY_UPPER_LIMIT = 297,
        PROP_OCCUPANCY_UPPER_LIMIT_ENFORCED = 298,
        PROP_PASSBACK_EXEMPTION = 299,
        PROP_PASSBACK_MODE = 300,
        PROP_PASSBACK_TIMEOUT = 301,
        PROP_POSITIVE_ACCESS_RULES = 302,
        PROP_REASON_FOR_DISABLE = 303,
        PROP_SUPPORTED_FORMATS = 304,
        PROP_SUPPORTED_FORMAT_CLASSES = 305,
        PROP_THREAT_AUTHORITY = 306,
        PROP_THREAT_LEVEL = 307,
        PROP_TRACE_FLAG = 308,
        PROP_TRANSACTION_NOTIFICATION_CLASS = 309,
        PROP_USER_EXTERNAL_IDENTIFIER = 310,
        PROP_USER_INFORMATION_REFERENCE = 311,
        /* enumerations 312-313 are used in Addendum k to ANSI/ASHRAE 135-2004 */
        PROP_CHARACTER_SET = 312,
        PROP_STRICT_CHARACTER_MODE = 313,
        /* enumerations 314-316 are used in Addendum ? */
        PROP_BACKUP_AND_RESTORE_STATE = 314,
        PROP_BACKUP_PREPARATION_TIME = 315,
        PROP_RESTORE_PREPARATION_TIME = 316,
        /* enumerations 317-323 are used in Addendum j to ANSI/ASHRAE 135-2004 */
        PROP_USER_NAME = 317,
        PROP_USER_TYPE = 318,
        PROP_USES_REMAINING = 319,
        PROP_ZONE_FROM = 320,
        PROP_ZONE_TO = 321,
        PROP_ACCESS_EVENT_TAG = 322,
        PROP_GLOBAL_IDENTIFIER = 323,
        /* enumerations 324-325 are used in Addendum i to ANSI/ASHRAE 135-2004 */
        PROP_BINARY_ACTIVE_VALUE = 324,
        PROP_BINARY_INACTIVE_VALUE = 325,
        /* enumeration 326 is used in Addendum j to ANSI/ASHRAE 135-2004 */
        PROP_VERIFICATION_TIME = 326,
        /* enumerations 342-344 are defined in Addendum 2008-w */
        PROP_BIT_MASK = 342,
        PROP_BIT_TEXT = 343,
        PROP_IS_UTC = 344,

        PROP_Identification_Number = 513,
        PROP_Time_Stamps = 514,
        PROP_Service_Mode = 515,
        PROP_Car_Status = 516,
        PROP_Car_Direction = 517,
        PROP_Door_Zone = 518,
        PROP_Car_Position = 519,
        PROP_Door_Status = 520,
        PROP_Passenger_Status = 521,
        PROP_Total_Running_Time = 522,
        PROP_Present_Counter_Value = 523,
        PROP_LIFT_Message_Code = 526,
        PROP_Operation_Status=524,
        PROP_Operation_Direction = 525

        /* The special property identifiers all, optional, and required  */
        /* are reserved for use in the ReadPropertyConditional and */
        /* ReadPropertyMultiple services or services not defined in this standard. */
        /* Enumerated values 0-511 are reserved for definition by ASHRAE.  */
        /* Enumerated values 512-4194303 may be used by others subject to the  */
        /* procedures and constraints described in Clause 23.  */
    }
  public enum BACNET_ERROR_CODE
    {
        /* valid for all classes */
        ERROR_CODE_OTHER = 0,

        /* Error Class - Device */
        ERROR_CODE_DEVICE_BUSY = 3,
        ERROR_CODE_CONFIGURATION_IN_PROGRESS = 2,
        ERROR_CODE_OPERATIONAL_PROBLEM = 25,

        /* Error Class - Object */
        ERROR_CODE_DYNAMIC_CREATION_NOT_SUPPORTED = 4,
        ERROR_CODE_NO_OBJECTS_OF_SPECIFIED_TYPE = 17,
        ERROR_CODE_OBJECT_DELETION_NOT_PERMITTED = 23,
        ERROR_CODE_OBJECT_IDENTIFIER_ALREADY_EXISTS = 24,
        ERROR_CODE_READ_ACCESS_DENIED = 27,
        ERROR_CODE_UNKNOWN_OBJECT = 31,
        ERROR_CODE_UNSUPPORTED_OBJECT_TYPE = 36,

        /* Error Class - Property */
        ERROR_CODE_CHARACTER_SET_NOT_SUPPORTED = 41,
        ERROR_CODE_DATATYPE_NOT_SUPPORTED = 47,
        ERROR_CODE_INCONSISTENT_SELECTION_CRITERION = 8,
        ERROR_CODE_INVALID_ARRAY_INDEX = 42,
        ERROR_CODE_INVALID_DATA_TYPE = 9,
        ERROR_CODE_NOT_COV_PROPERTY = 44,
        ERROR_CODE_OPTIONAL_FUNCTIONALITY_NOT_SUPPORTED = 45,
        ERROR_CODE_PROPERTY_IS_NOT_AN_ARRAY = 50,
        /* ERROR_CODE_READ_ACCESS_DENIED = 27, */
        ERROR_CODE_UNKNOWN_PROPERTY = 32,
        ERROR_CODE_VALUE_OUT_OF_RANGE = 37,
        ERROR_CODE_WRITE_ACCESS_DENIED = 40,

        /* Error Class - Resources */
        ERROR_CODE_NO_SPACE_FOR_OBJECT = 18,
        ERROR_CODE_NO_SPACE_TO_ADD_LIST_ELEMENT = 19,
        ERROR_CODE_NO_SPACE_TO_WRITE_PROPERTY = 20,

        /* Error Class - Security */
        ERROR_CODE_AUTHENTICATION_FAILED = 1,
        /* ERROR_CODE_CHARACTER_SET_NOT_SUPPORTED = 41, */
        ERROR_CODE_INCOMPATIBLE_SECURITY_LEVELS = 6,
        ERROR_CODE_INVALID_OPERATOR_NAME = 12,
        ERROR_CODE_KEY_GENERATION_ERROR = 15,
        ERROR_CODE_PASSWORD_FAILURE = 26,
        ERROR_CODE_SECURITY_NOT_SUPPORTED = 28,
        ERROR_CODE_TIMEOUT = 30,

        /* Error Class - Services */
        /* ERROR_CODE_CHARACTER_SET_NOT_SUPPORTED = 41, */
        ERROR_CODE_COV_SUBSCRIPTION_FAILED = 43,
        ERROR_CODE_DUPLICATE_NAME = 48,
        ERROR_CODE_DUPLICATE_OBJECT_ID = 49,
        ERROR_CODE_FILE_ACCESS_DENIED = 5,
        ERROR_CODE_INCONSISTENT_PARAMETERS = 7,
        ERROR_CODE_INVALID_CONFIGURATION_DATA = 46,
        ERROR_CODE_INVALID_FILE_ACCESS_METHOD = 10,
        ERROR_CODE_INVALID_FILE_START_POSITION = 11,
        ERROR_CODE_INVALID_PARAMETER_DATA_TYPE = 13,
        ERROR_CODE_INVALID_TIME_STAMP = 14,
        ERROR_CODE_MISSING_REQUIRED_PARAMETER = 16,
        /* ERROR_CODE_OPTIONAL_FUNCTIONALITY_NOT_SUPPORTED = 45, */
        ERROR_CODE_PROPERTY_IS_NOT_A_LIST = 22,
        ERROR_CODE_SERVICE_REQUEST_DENIED = 29,

        /* Error Class - VT */
        ERROR_CODE_UNKNOWN_VT_CLASS = 34,
        ERROR_CODE_UNKNOWN_VT_SESSION = 35,
        ERROR_CODE_NO_VT_SESSIONS_AVAILABLE = 21,
        ERROR_CODE_VT_SESSION_ALREADY_CLOSED = 38,
        ERROR_CODE_VT_SESSION_TERMINATION_FAILURE = 39,

        /* unused */
        ERROR_CODE_RESERVED1 = 33,
        /* new error codes from new addenda */
        ERROR_CODE_ABORT_BUFFER_OVERFLOW = 51,
        ERROR_CODE_ABORT_INVALID_APDU_IN_THIS_STATE = 52,
        ERROR_CODE_ABORT_PREEMPTED_BY_HIGHER_PRIORITY_TASK = 53,
        ERROR_CODE_ABORT_SEGMENTATION_NOT_SUPPORTED = 54,
        ERROR_CODE_ABORT_PROPRIETARY = 55,
        ERROR_CODE_ABORT_OTHER = 56,
        ERROR_CODE_INVALID_TAG = 57,
        ERROR_CODE_NETWORK_DOWN = 58,
        ERROR_CODE_REJECT_BUFFER_OVERFLOW = 59,
        ERROR_CODE_REJECT_INCONSISTENT_PARAMETERS = 60,
        ERROR_CODE_REJECT_INVALID_PARAMETER_DATA_TYPE = 61,
        ERROR_CODE_REJECT_INVALID_TAG = 62,
        ERROR_CODE_REJECT_MISSING_REQUIRED_PARAMETER = 63,
        ERROR_CODE_REJECT_PARAMETER_OUT_OF_RANGE = 64,
        ERROR_CODE_REJECT_TOO_MANY_ARGUMENTS = 65,
        ERROR_CODE_REJECT_UNDEFINED_ENUMERATION = 66,
        ERROR_CODE_REJECT_UNRECOGNIZED_SERVICE = 67,
        ERROR_CODE_REJECT_PROPRIETARY = 68,
        ERROR_CODE_REJECT_OTHER = 69,
        ERROR_CODE_UNKNOWN_DEVICE = 70,
        ERROR_CODE_UNKNOWN_ROUTE = 71,
        ERROR_CODE_VALUE_NOT_INITIALIZED = 72,
        ERROR_CODE_INVALID_EVENT_STATE = 73,
        ERROR_CODE_NO_ALARM_CONFIGURED = 74,
        ERROR_CODE_LOG_BUFFER_FULL = 75,
        ERROR_CODE_LOGGED_VALUE_PURGED = 76,
        ERROR_CODE_NO_PROPERTY_SPECIFIED = 77,
        ERROR_CODE_NOT_CONFIGURED_FOR_TRIGGERED_LOGGING = 78,
        ERROR_CODE_UNKNOWN_SUBSCRIPTION = 79,
        ERROR_CODE_PARAMETER_OUT_OF_RANGE = 80,
        ERROR_CODE_LIST_ELEMENT_NOT_FOUND = 81,
        ERROR_CODE_BUSY = 82,
        ERROR_CODE_COMMUNICATION_DISABLED = 83,
        ERROR_CODE_SUCCESS = 84,
        ERROR_CODE_ACCESS_DENIED = 85,
        ERROR_CODE_BAD_DESTINATION_ADDRESS = 86,
        ERROR_CODE_BAD_DESTINATION_DEVICE_ID = 87,
        ERROR_CODE_BAD_SIGNATURE = 88,
        ERROR_CODE_BAD_SOURCE_ADDRESS = 89,
        ERROR_CODE_BAD_TIMESTAMP = 90,
        ERROR_CODE_CANNOT_USE_KEY = 91,
        ERROR_CODE_CANNOT_VERIFY_MESSAGE_ID = 92,
        ERROR_CODE_CORRECT_KEY_REVISION = 93,
        ERROR_CODE_DESTINATION_DEVICE_ID_REQUIRED = 94,
        ERROR_CODE_DUPLICATE_MESSAGE = 95,
        ERROR_CODE_ENCRYPTION_NOT_CONFIGURED = 96,
        ERROR_CODE_ENCRYPTION_REQUIRED = 97,
        ERROR_CODE_INCORRECT_KEY = 98,
        ERROR_CODE_INVALID_KEY_DATA = 99,
        ERROR_CODE_KEY_UPDATE_IN_PROGRESS = 100,
        ERROR_CODE_MALFORMED_MESSAGE = 101,
        ERROR_CODE_NOT_KEY_SERVER = 102,
        ERROR_CODE_SECURITY_NOT_CONFIGURED = 103,
        ERROR_CODE_SOURCE_SECURITY_REQUIRED = 104,
        ERROR_CODE_TOO_MANY_KEYS = 105,
        ERROR_CODE_UNKNOWN_AUTHENTICATION_TYPE = 106,
        ERROR_CODE_UNKNOWN_KEY = 107,
        ERROR_CODE_UNKNOWN_KEY_REVISION = 108,
        ERROR_CODE_UNKNOWN_SOURCE_MESSAGE = 109,
        ERROR_CODE_NOT_ROUTER_TO_DNET = 110,
        ERROR_CODE_ROUTER_BUSY = 111,
        ERROR_CODE_UNKNOWN_NETWORK_MESSAGE = 112,
        ERROR_CODE_MESSAGE_TOO_LONG = 113,
        ERROR_CODE_SECURITY_ERROR = 114,
        ERROR_CODE_ADDRESSING_ERROR = 115,
        ERROR_CODE_WRITE_BDT_FAILED = 116,
        ERROR_CODE_READ_BDT_FAILED = 117,
        ERROR_CODE_REGISTER_FOREIGN_DEVICE_FAILED = 118,
        ERROR_CODE_READ_FDT_FAILED = 119,
        ERROR_CODE_DELETE_FDT_ENTRY_FAILED = 120,
        ERROR_CODE_DISTRIBUTE_BROADCAST_FAILED = 121,
        ERROR_CODE_UNKNOWN_FILE_SIZE = 122,
        ERROR_CODE_ABORT_APDU_TOO_LONG = 123,
        ERROR_CODE_ABORT_APPLICATION_EXCEEDED_REPLY_TIME = 124,
        ERROR_CODE_ABORT_OUT_OF_RESOURCES = 125,
        ERROR_CODE_ABORT_TSM_TIMEOUT = 126,
        ERROR_CODE_ABORT_WINDOW_SIZE_OUT_OF_RANGE = 127,
        ERROR_CODE_FILE_FULL = 128,
        ERROR_CODE_INCONSISTENT_CONFIGURATION = 129,
        ERROR_CODE_INCONSISTENT_OBJECT_TYPE = 130,
        ERROR_CODE_INTERNAL_ERROR = 131,
        ERROR_CODE_NOT_CONFIGURED = 132,
        ERROR_CODE_OUT_OF_MEMORY = 133,
        ERROR_CODE_VALUE_TOO_LONG = 134,
        ERROR_CODE_ABORT_INSUFFICIENT_SECURITY = 135,
        ERROR_CODE_ABORT_SECURITY_ERROR = 136,
        /* Enumerated values 0-255 are reserved for definition by ASHRAE. */
        /* Enumerated values 256-65535 may be used by others subject to */
        /* the procedures and constraints described in Clause 23. */
        /* do the max range inside of enum so that
           compilers will allocate adequate sized datatype for enum
           which is used to store decoding */
        FIRST_PROPRIETARY_ERROR_CODE = 256,
        LAST_PROPRIETARY_ERROR_CODE = 65535
    }
    public enum BACNET_ERROR_CLASS
    {
        ERROR_CLASS_DEVICE = 0,
        ERROR_CLASS_OBJECT = 1,
        ERROR_CLASS_PROPERTY = 2,
        ERROR_CLASS_RESOURCES = 3,
        ERROR_CLASS_SECURITY = 4,
        ERROR_CLASS_SERVICES = 5,
        ERROR_CLASS_VT = 6,
        ERROR_CLASS_COMMUNICATION = 7,
        /* Enumerated values 0-63 are reserved for definition by ASHRAE. */
        /* Enumerated values 64-65535 may be used by others subject to */
        /* the procedures and constraints described in Clause 23. */
        MAX_BACNET_ERROR_CLASS = 8,
        FIRST_PROPRIETARY_ERROR_CLASS = 64,
        LAST_PROPRIETARY_ERROR_CLASS = 65535
    }
      enum BACNET_LIFE_SAFETY_STATE
     {
         MIN_LIFE_SAFETY_STATE = 0,
         LIFE_SAFETY_STATE_QUIET = 0,
         LIFE_SAFETY_STATE_PRE_ALARM = 1,
         LIFE_SAFETY_STATE_ALARM = 2,
         LIFE_SAFETY_STATE_FAULT = 3,
         LIFE_SAFETY_STATE_FAULT_PRE_ALARM = 4,
         LIFE_SAFETY_STATE_FAULT_ALARM = 5,
         LIFE_SAFETY_STATE_NOT_READY = 6,
         LIFE_SAFETY_STATE_ACTIVE = 7,
         LIFE_SAFETY_STATE_TAMPER = 8,
         LIFE_SAFETY_STATE_TEST_ALARM = 9,
         LIFE_SAFETY_STATE_TEST_ACTIVE = 10,
         LIFE_SAFETY_STATE_TEST_FAULT = 11,
         LIFE_SAFETY_STATE_TEST_FAULT_ALARM = 12,
         LIFE_SAFETY_STATE_HOLDUP = 13,
         LIFE_SAFETY_STATE_DURESS = 14,
         LIFE_SAFETY_STATE_TAMPER_ALARM = 15,
         LIFE_SAFETY_STATE_ABNORMAL = 16,
         LIFE_SAFETY_STATE_EMERGENCY_POWER = 17,
         LIFE_SAFETY_STATE_DELAYED = 18,
         LIFE_SAFETY_STATE_BLOCKED = 19,
         LIFE_SAFETY_STATE_LOCAL_ALARM = 20,
         LIFE_SAFETY_STATE_GENERAL_ALARM = 21,
         LIFE_SAFETY_STATE_SUPERVISORY = 22,
         LIFE_SAFETY_STATE_TEST_SUPERVISORY = 23,
         MAX_LIFE_SAFETY_STATE = 24,
         /* Enumerated values 0-255 are reserved for definition by ASHRAE.  */
         /* Enumerated values 256-65535 may be used by others subject to  */
         /* procedures and constraints described in Clause 23. */
         /* do the max range inside of enum so that
             compilers will allocate adequate sized datatype for enum
             which is used to store decoding */
         LIFE_SAFETY_STATE_PROPRIETARY_MIN = 256,
         LIFE_SAFETY_STATE_PROPRIETARY_MAX = 65535
     }
      enum BACNET_LIFE_SAFETY_MODE
     {
         MIN_LIFE_SAFETY_MODE = 0,
         LIFE_SAFETY_MODE_OFF = 0,
         LIFE_SAFETY_MODE_ON = 1,
         LIFE_SAFETY_MODE_TEST = 2,
         LIFE_SAFETY_MODE_MANNED = 3,
         LIFE_SAFETY_MODE_UNMANNED = 4,
         LIFE_SAFETY_MODE_ARMED = 5,
         LIFE_SAFETY_MODE_DISARMED = 6,
         LIFE_SAFETY_MODE_PREARMED = 7,
         LIFE_SAFETY_MODE_SLOW = 8,
         LIFE_SAFETY_MODE_FAST = 9,
         LIFE_SAFETY_MODE_DISCONNECTED = 10,
         LIFE_SAFETY_MODE_ENABLED = 11,
         LIFE_SAFETY_MODE_DISABLED = 12,
         LIFE_SAFETY_MODE_AUTOMATIC_RELEASE_DISABLED = 13,
         LIFE_SAFETY_MODE_DEFAULT = 14,
         MAX_LIFE_SAFETY_MODE = 15,
         /* Enumerated values 0-255 are reserved for definition by ASHRAE.  */
         /* Enumerated values 256-65535 may be used by others subject to  */
         /* procedures and constraints described in Clause 23. */
         /* do the max range inside of enum so that
             compilers will allocate adequate sized datatype for enum
             which is used to store decoding */
         LIFE_SAFETY_MODE_PROPRIETARY_MIN = 256,
         LIFE_SAFETY_MODE_PROPRIETARY_MAX = 65535
     }
      enum BACNET_LIFE_SAFETY_OPERATION
     {
         LIFE_SAFETY_OP_NONE = 0,
         LIFE_SAFETY_OP_SILENCE = 1,
         LIFE_SAFETY_OP_SILENCE_AUDIBLE = 2,
         LIFE_SAFETY_OP_SILENCE_VISUAL = 3,
         LIFE_SAFETY_OP_RESET = 4,
         LIFE_SAFETY_OP_RESET_ALARM = 5,
         LIFE_SAFETY_OP_RESET_FAULT = 6,
         LIFE_SAFETY_OP_UNSILENCE = 7,
         LIFE_SAFETY_OP_UNSILENCE_AUDIBLE = 8,
         LIFE_SAFETY_OP_UNSILENCE_VISUAL = 9,
         /* Enumerated values 0-63 are reserved for definition by ASHRAE.  */
         /* Enumerated values 64-65535 may be used by others subject to  */
         /* procedures and constraints described in Clause 23. */
         /* do the max range inside of enum so that
             compilers will allocate adequate sized datatype for enum
             which is used to store decoding */
         LIFE_SAFETY_OP_PROPRIETARY_MIN = 64,
         LIFE_SAFETY_OP_PROPRIETARY_MAX = 65535
     } 
}
