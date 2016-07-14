using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class BasicalProcessor
    {
           internal  static int Decode_Unsigned32(ref byte[] apdu, ref UInt32 value, int pos = 0)
        {
            if (value != null)
            {
                value = ((UInt32)((((UInt32)apdu[pos + 0]) << 24) & 0xff000000));
                value |= ((UInt32)((((UInt32)apdu[pos + 1]) << 16) & 0x00ff0000));
                value |= ((UInt32)((((UInt32)apdu[pos + 2]) << 8) & 0x0000ff00));
                value |= ((UInt32)(((UInt32)apdu[pos + 3]) & 0x000000ff));
            }

            return 4;
        }//解码32位

          internal static int Decode_Unsigned16(ref byte[] apdu, ref UInt16 value, int pos = 0)
        {
            if (value != null)
            {
                value = (UInt16)((((UInt16)apdu[pos + 0]) << 8) & 0xff00);
                value |= ((UInt16)(((UInt16)apdu[pos + 1]) & 0x00ff));
            }

            return 2;
        }//解码16位
          internal static int Decode_Unsigned24(ref byte[] apdu, ref UInt32 value, int pos = 0)
        {
            if (value != null)
            {


                value = ((UInt32)((((UInt32)apdu[pos + 0]) << 16) & 0x00ff0000));
                value |= (UInt32)((((UInt32)apdu[pos + 1]) << 8) & 0x0000ff00);
                value |= ((UInt32)(((UInt32)apdu[pos + 2]) & 0x000000ff));


            }
            return 3;
        }
          internal static Boolean Decode_Boolean(UInt32 len_value)
        {

            bool boolean_value = false;

            if (len_value!=0)
            {
                boolean_value = true;
            }

            return boolean_value;
        }
          internal static Boolean Decode_Context_Boolean(ref Byte[] apdu,int pos)
        {
            bool boolean_value = false;

            if (apdu[pos+0]!=0)
            {
                boolean_value = true;
            }

            return boolean_value;
        }
          internal static int Decode_Tag_number_and_Value(ref byte[] apdu, ref byte tag_number, ref UInt32 value, int pos)
        {
            int len = 1;
            UInt16 value16 = new UInt16();
            UInt32 value32 = new UInt32();
            len = Decode_Tag_Number(ref apdu, ref tag_number, pos);

            if ((apdu[pos + 0] & 0x07) == 5)
            {

                if (apdu[pos + len] == 255)
                {
                    len++;
                    len += Decode_Unsigned32(ref apdu, ref value32, pos + len);

                    if (value != null)
                    {
                        value = value32;
                    }
                }
                /* tagged as uint16_t */
                else if (apdu[pos + len] == 254)
                {
                    len++;
                    len += Decode_Unsigned16(ref apdu, ref value16, pos + len);
                    if (value != null)
                    {
                        value = value16;
                    }
                }
                /* no tag - must be uint8_t */
                else
                {
                    if (value != null)
                    {
                        value = apdu[pos + len];
                    }
                    len++;
                }
            }

            else if ((apdu[pos + 0] & 0x07) == 6)
            {
                value = 0;
            }
            else if ((apdu[pos + 0] & 0x07) == 7)
            {

                value = 0;
            }
            else if (value != null)
            {
                /* small value */
                value = (UInt32)apdu[pos + 0] & 0x07;
            }
            return len;
        }

          internal static int Decode_Tag_Number(ref byte[] apdu, ref byte tag_number, int pos = 0)
        {
            int len = 1;

            if ((apdu[pos + 0] & 0xF0) == 0xF0)
            {

                if (tag_number != null)
                {
                    tag_number = apdu[pos + 1];
                }
                len++;
            }
            else
            {
                if (tag_number != null)
                {
                    tag_number = (byte)(apdu[pos + 0] >> 4);
                }
            }

            return len;
        }
          internal static int Decode_Unsigned(ref byte[] apdu, UInt32 len_value, ref UInt32 value, int pos)
        {
            UInt16 unsigned16_value = 0;

            if (value != null)
            {
                switch (len_value)
                {
                    case 1:
                        value = apdu[pos];
                        break;
                    case 2:
                        Decode_Unsigned16(ref apdu, ref unsigned16_value, pos);
                        value = unsigned16_value;
                        break;
                    case 3:
                        Decode_Unsigned24(ref apdu, ref value, pos);
                        break;
                    case 4:
                        Decode_Unsigned32(ref apdu, ref value, pos);
                        break;
                    default:
                        value = 0;
                        break;
                }
            }

            return (int)len_value;
        }
          internal static int Decode_Max_Segs(ref Byte[] buf,int pos)
        {
            int max_segs = 0;

            switch (buf[pos] & 0xF0)
            {
                case 0:
                    max_segs = 0;
                    break;
                case 0x10:
                    max_segs = 2;
                    break;
                case 0x20:
                    max_segs = 4;
                    break;
                case 0x30:
                    max_segs = 8;
                    break;
                case 0x40:
                    max_segs = 16;
                    break;
                case 0x50:
                    max_segs = 32;
                    break;
                case 0x60:
                    max_segs = 64;
                    break;
                case 0x70:
                    max_segs = 65;
                    break;
                default:
                    break;
            }
            return max_segs;
    }

         internal static int Decode_Max_Apdu(ref Byte[] buf,int pos)
        {
            int max_apdu = 0;

            switch (buf[pos] & 0x0F)
            {
                case 0:
                    max_apdu = 50;
                    break;
                case 1:
                    max_apdu = 128;
                    break;
                case 2:
                    max_apdu = 206;
                    break;
                case 3:
                    max_apdu = 480;
                    break;
                case 4:
                    max_apdu = 1024;
                    break;
                case 5:
                    max_apdu = 1476;
                    break;
                default:
                    break;
            }

            return max_apdu;
        }
          internal static int Decode_Object_Id(ref Byte[]apdu,ref UInt16 object_type,ref UInt32 instance,int pos)
       {
           UInt32 value = 0;
           int len = 0;

           len = Decode_Unsigned32(ref apdu, ref value,pos);
           object_type =
               (UInt16)(((value >> BacnetConst.BACNET_INSTANCE_BITS) & BacnetConst.BACNET_MAX_OBJECT));
           instance = (value & BacnetConst.BACNET_MAX_INSTANCE);

           return len;
       }
          internal static int Decode_Context_Enumerated(ref Byte[] apdu,Byte tag_value,ref UInt32 value,int pos)
        {
            int len = 0;
            Byte tag_number=0;
            UInt32 len_value=0;
             // if (decode_is_context_tag(&apdu[len], tag_value))
            len +=Decode_Tag_number_and_Value(ref apdu,ref tag_number,ref len_value,pos+len);
            len += Decode_Enumerated(ref apdu, len_value, ref value,pos+len);
            return len;
        }
          internal static int Decode_Enumerated(ref Byte[] apdu, UInt32 len_value,ref UInt32 value,int pos )
        {
            UInt32 unsigned_value = 0;
            int len;

            len = Decode_Unsigned(ref apdu, len_value, ref unsigned_value,pos);
            if (value!=null)
            {
                value = unsigned_value;
            }

            return len;
        }
        internal static int Decode_Application_Data(ref Byte[] apdu,BACNET_APPLICATION_DATA_VALUE value,int pos)
          {
              int len = 0;
              int tag_len = 0;
              int decode_len = 0;
              Byte tag_number = 0;
              UInt32 len_value_type = 0;

              value.context_specific = false;
              tag_len = BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value_type, pos + len);
              len += tag_len;
              value.tag = tag_number;
              decode_len = BasicalProcessor.Decode_Data(ref apdu, tag_number, len_value_type, ref  value, pos + len);
              len += decode_len;
              return len;


          }
          internal static int Decode_Application_Data(ref Byte[] apdu,uint max_apdu_len, BACNET_APPLICATION_DATA_VALUE value,int pos)
          {
              int app_len=0;
            int len = 0;
            int tag_len = 0;
            int decode_len = 0;
            Byte tag_number = 0;
            UInt32 len_value_type = 0;
            // (apdu && value && !IS_CONTEXT_SPECIFIC(*apdu))

            while (true)
          {
                value.context_specific = false;
                tag_len = BasicalProcessor.Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value_type, pos+len);
                len += tag_len;
                value.tag = tag_number;
                decode_len = BasicalProcessor.Decode_Data(ref apdu, tag_number, len_value_type, ref  value, pos + len);
                // if (value->tag != MAX_BACNET_APPLICATION_TAG)
                len += decode_len;
             app_len = app_len + len;
              if (app_len < max_apdu_len)
               {
                   value.next = new BACNET_APPLICATION_DATA_VALUE();
                   value = value.next;
              }
                else
               {
                    value.next = null;
                    break;
               }

            }
            return len;
        }
          internal static int Decode_Data(ref Byte[] apdu, Byte tag_data_type, UInt32 len_value_type,ref BACNET_APPLICATION_DATA_VALUE value,int pos)
        {
            int len = 0;
            switch (tag_data_type)
            {
                case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_NULL:
                    break;
                case  (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_BOOLEAN:
                    value.value.Boolean = Decode_Boolean(len_value_type);
                    break;
                case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_UNSIGNED_INT:
                    len = Decode_Unsigned(ref apdu, len_value_type, ref value.value.Unsigned_Int, pos);
                      break;
                case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_SIGNED_INT:
                      len = Decode_Signed(ref apdu, len_value_type, ref value.value.Signed_Int, pos);
                      break;
                case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_REAL:

                    break;
               case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_DOUBLE:
                    break;
                case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_OCTET_STRING:
                    break;
               case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_CHARACTER_STRING:
                    len = Decode_Character_String(ref apdu, len_value_type, ref value.value.Character_String, pos);
                    break;

               case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_BIT_STRING:
                    break;

               case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_ENUMERATED:
                    break;
               case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_DATE:
                    len = Decode_Date(ref apdu, ref value.value.Date, pos);
                    break;
               case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_TIME:
                    len = Decode_Time(ref apdu, ref value.value.Time, pos);
                    break;
               case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_OBJECT_ID:
                    len = Decode_Object_Id(ref apdu, ref value.value.Object_Id.type, ref value.value.Object_Id.instance, pos);
                    break;



            }
            if (len == 0 && tag_data_type != (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_NULL &&
             tag_data_type != (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_BOOLEAN)
            {
                /* indicate that we were not able to decode the value */
                value.tag = (Byte)BACNET_APPLICATION_TAG.MAX_BACNET_APPLICATION_TAG;
            }
            return len;
        }
           internal static Boolean Decode_Is_Closing_Tag_Number(ref Byte[] apdu,Byte tag_number,int pos)
        {
             Byte my_tag=0;
             Decode_Tag_Number(ref apdu, ref my_tag, pos);
             return ((apdu[pos] & 0x07) == 7) && (my_tag == tag_number);
        }
          internal static Boolean Decode_Is_Context_Tag(ref Byte[] apdu,Byte tag_number,int pos)
         {
             Byte my_tag = 0;
             Decode_Tag_Number(ref apdu, ref my_tag, pos);
             return ((apdu[pos] & 0x08) == 0x08) && (my_tag == tag_number);
         }
          internal static Boolean Decode_Is_Opening_Tag_Number(ref Byte[] apdu, Byte tag_number, int pos)
        {
            Byte my_tag = 0;
            Decode_Tag_Number(ref apdu, ref my_tag, pos);
            return ((apdu[pos] & 0x07) == 6) && (my_tag == tag_number);
        }

         internal static int Decode_Context_Unsigned(ref Byte[] apdu,Byte tag_number,ref UInt32 value,int pos)
        {
            UInt32 len_value=0;
            int len = 0;
            len += Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value, pos + len);
            len += Decode_Unsigned(ref apdu, len_value, ref value, pos + len);
            return len;

        }
   
         internal static int  Decode_Context_Timestamp(ref Byte[] apdu,Byte tag_number,ref BACNET_TIMESTAMP value,int pos)
       {

           int len = 0;
            int section_len;

           if (Decode_Is_Opening_Tag_Number(ref apdu,tag_number,pos+len))
           len++;
           section_len=Decode_Timestamp(ref apdu,ref value,pos+len);
           
           len += section_len;
            if (Decode_Is_Closing_Tag_Number(ref apdu,tag_number,pos+len))
           len++;
           return len;
       }
           internal static int  Decode_Timestamp(ref Byte[] apdu,ref BACNET_TIMESTAMP value,int pos)
       {
           int len = 0;
           int section_len;
           UInt32 len_value_type=0;
         //  UInt32 sequenceNum=0;
           section_len = Decode_Tag_number_and_Value(ref apdu, ref value.tag, ref len_value_type, pos + len);
           len += section_len;//此处可能有问题
           switch (value.tag) //BACNET_TIMESTAMP_TAG
           {
               case 2:
                   section_len = Decode_Context_Datetime(ref apdu, 2, ref value.value, pos + len);
                           
                     len += section_len;

                   break;
           }
          
          return len;

       }
         internal static int Decode_Context_Datetime(ref Byte[] apdu,Byte tag_number,ref BACNET_DATE_TIME value,int pos)
         {
             int apdu_len = 0;
             int len;
             if (Decode_Is_Opening_Tag_Number(ref apdu, tag_number,pos+apdu_len))
             apdu_len++;
              len = Decode_Datetime(ref apdu, ref value,apdu_len+pos);
             apdu_len += len;
            if (Decode_Is_Closing_Tag_Number(ref apdu, tag_number,pos+apdu_len))
             apdu_len++;

            return apdu_len;
          
         }
          internal static int Decode_Datetime(ref Byte[] apdu,ref BACNET_DATE_TIME value,int pos )
       {
           int len = 0;
           int section_len;
           section_len = Decode_Application_Date(ref apdu, ref value.date, pos + len);
           len += section_len;
           section_len = Decode_Application_Time(ref apdu, ref value.time, pos + len);
           len += section_len;

           return len;
       }
   
          internal static int Decode_Application_Date(ref Byte[] apdu,ref  BACNET_DATE bdate,int pos)
        {
            int len = 0;
            Byte tag_number=0;
            Decode_Tag_Number(ref apdu, ref tag_number, pos + len);
            tag_number = (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_DATE;
            len++;
            len += Decode_Date(ref apdu, ref bdate,pos+len);
            return len;

          
        }
          internal static int Decode_Date(ref Byte[] apdu, ref BACNET_DATE bdate, int pos)
        {
            bdate.year = (UInt16)apdu[pos+0];
            bdate.month = apdu[pos+1];
            bdate.day = apdu[pos+2];
            bdate.wday = apdu[pos+3];

            return 4;
        }
   

          internal static int Decode_Application_Time(ref Byte[] apdu, ref  BACNET_TIME btime, int pos)
        {
            int len = 0;
            Byte tag_number = 0;
            Decode_Tag_Number(ref apdu, ref tag_number, pos + len);
            tag_number = (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_TIME;
            len++;
            len += Decode_Time(ref apdu, ref btime, pos + len);
            return len;

        }
          internal static int Decode_Time(ref Byte[] apdu, ref BACNET_TIME btime, int pos)
        {
            btime.hour = apdu[pos+0];
            btime.min = apdu[pos+1];
            btime.sec = apdu[pos+2];
            btime.hundredths = apdu[pos+3];

            return 4;

            
        }
          internal static int Decode_Context_Poroperty_State(ref Byte[] apdu,Byte tag_number,ref BACNET_PROPERTY_STATE value,int pos)
        {
            int len = 0;
            int section_length;
            if (Decode_Is_Opening_Tag_Number(ref apdu, tag_number, pos + len))
                len++;
            section_length = Decode_Property_State(ref apdu, ref value, pos + len);
           len += section_length;


            if (Decode_Is_Closing_Tag_Number(ref apdu, tag_number, pos + len))
                len++;
            return len;
        }
          internal static int Decode_Property_State(ref Byte[] apdu,ref BACNET_PROPERTY_STATE value,int pos)
        {
            int len = 0;
            UInt32 len_value_type=0;
            int section_length;
           // UInt32 enumValue;
            Byte tagnum=0;

            section_length = Decode_Tag_number_and_Value(ref apdu, ref tagnum, ref len_value_type, pos + len);
            len += section_length;
            value.tag = (BACNET_PROPERTY_STATE_TYPE)tagnum;
            switch( value.tag)
            {
                case BACNET_PROPERTY_STATE_TYPE.UNSIGNED_VALUE:
                    section_length = Decode_Unsigned(ref apdu, len_value_type, ref value.value, len + pos);
                    len += section_length;
                    break;
            }
            return len;


        }
   
    
            internal static int Decode_Context_Bitstring(ref Byte[] apdu,Byte tag_number,ref BACNET_BIT_STRING bit_string, int pos)
        {
            UInt32 len_value=0;
            int len = 0;
            len+=Decode_Tag_number_and_Value(ref apdu,ref tag_number,ref len_value,pos+len);
            len += Decode_Bitstring(ref apdu, len_value, ref bit_string,pos+len);
            return len;


        }
          internal static int Decode_Signed(ref Byte[] apdu,UInt32 len_value,ref Int32 value,int pos)
          {
            //未完成
             return (int)len_value;
          }

           internal static int Decode_Bitstring(ref Byte[] apdu,UInt32 len_value,ref BACNET_BIT_STRING bit_string, int pos)
          {
              
              int len = 1;
              Byte unused_bits = 0;
              UInt32 i = 0;
              UInt32 bytes_used = 0;
              //bitstring_init(bit_string);
              bytes_used = len_value - 1;
             
              for (i = 0; i < bytes_used; i++)
              {
               bit_string.set_octet((Byte)i,bit_string.byte_reverse_bits(apdu[pos + len++]));
              }
              unused_bits = (Byte)(apdu[pos] & 0x07);
              bit_string.set_bits_used((Byte)bytes_used, unused_bits);
              return len;
          }
          internal static int Decode_Context_Character_String(ref Byte[] apdu,Byte tag_number,ref BACNET_CHARACTER_STRING char_string,int pos)
        {
            int len = 0;        /* return value */
            bool status = false;
            UInt32 len_value = 0;
            len += Decode_Tag_number_and_Value(ref apdu, ref tag_number, ref len_value, pos + len);
            char_string.init(ref apdu, apdu[pos+len], len_value-1, len + pos + 1);
            

            return len;

        }
          internal static int Decode_Character_String(ref Byte[] apdu,UInt32 len_value, ref BACNET_CHARACTER_STRING char_string, int pos)
          {

              int len = 0;        /* return value */
              bool status = false;
              char_string = new BACNET_CHARACTER_STRING((int)(len_value - 1));
              status=char_string.init(ref apdu, apdu[pos + len], len_value-1, len + pos + 1);
              if (status)
              {
                  len = (int)len_value;
              }

              return len;

          }


        /* 加码部分*************************************************************************************************************************************************************************/
       
          internal static int Encode_Unsigned(ref byte[] apdu,UInt32 value,int pos)
        {
            int len = 0;
            if (value < 0x100)
            {
                apdu[pos+0] = (Byte)value;
                len = 1;
            }
            else if (value < 0x10000)
            {
                len = Encode_Unsigned16(ref apdu, (UInt16)value,pos);
            }
            else if (value < 0x1000000)
            {
                len = Encode_Unsigned24(ref apdu, value, pos);
            }
            else
            {
                len = Encode_Unsigned32(ref apdu, value, pos);
            }
            return len;
        }
          internal static int Encode_Tag(ref byte[] apdu, byte tag_number, bool context_specific, UInt32 len_value_type, int pos)
        {
            int len = 1;        /* return value */

            apdu[pos + 0] = 0;
            if (context_specific)
                apdu[pos + 0] =BacnetConst.BIT3;

            /* additional tag byte after this byte */
            /* for extended tag byte */
            if (tag_number <= 14)
            {
                apdu[pos + 0] |= (byte)(tag_number << 4);
            }
            else
            {
                apdu[pos + 0] |= 0xF0;
                apdu[pos + 1] = tag_number;
                len++;
            }


            /* NOTE: additional len byte(s) after extended tag byte */
            /* if larger than 4 */
            if (len_value_type <= 4)
            {
                apdu[pos + 0] |= (byte)len_value_type;

            }
            else
            {
                apdu[pos + 0] |= 5;
                if (len_value_type <= 253)
                {
                    apdu[pos + len] = (byte)len_value_type;
                    len++;
                }
                else if (len_value_type <= 65535)
                {
                    apdu[pos + len] = 254;
                    len++;
                    len += Encode_Unsigned16(ref apdu, (UInt16)len_value_type, len);
                }
                else
                {
                    apdu[pos + len] = 255;
                    len++;
                    len += Encode_Unsigned32(ref apdu, len_value_type, len);
                }
            }


            return len;
        }
          internal static int Encode_Unsigned16(ref byte[] apdu, UInt16 value, int pos)
        {
            apdu[pos + 0] = (byte)((value & 0xff00) >> 8);
            apdu[pos + 1] = (byte)(value & 0x00ff);

            return 2;
        }
          internal static int Encode_Unsigned24(ref byte[] apdu, UInt32 value,int pos)
        {


    apdu[pos+0] = (Byte) ((value & 0xff0000) >> 16);
    apdu[pos+1] = (Byte) ((value & 0x00ff00) >> 8);
    apdu[pos+2] = (Byte) (value & 0x0000ff);

    return 3;
}
        
        internal static int Encode_Unsigned32(ref byte[] apdu, UInt32 value, int pos)
        {
            apdu[pos + 0] = (byte)((value & 0xff000000) >> 24);
            apdu[pos + 1] = (byte)((value & 0x00ff0000) >> 16);
            apdu[pos + 2] = (byte)((value & 0x0000ff00) >> 8);
            apdu[pos + 3] = (byte)(value & 0x000000ff);

            return 4;
        }

          internal static Byte Encode_MaxSegsandApdu(
       int max_segs,
       int max_apdu)
        {
            Byte octet = 0;

            if (max_segs < 2)
                octet = 0;
            else if (max_segs < 4)
                octet = 0x10;
            else if (max_segs < 8)
                octet = 0x20;
            else if (max_segs < 16)
                octet = 0x30;
            else if (max_segs < 32)
                octet = 0x40;
            else if (max_segs < 64)
                octet = 0x50;
            else if (max_segs == 64)
                octet = 0x60;
            else
                octet = 0x70;

            /* max_apdu must be 50 octets minimum */
            if (max_apdu <= 50)
                octet |= 0x00;
            else if (max_apdu <= 128)
                octet |= 0x01;
            /*fits in a LonTalk frame */
            else if (max_apdu <= 206)
                octet |= 0x02;
            /*fits in an ARCNET or MS/TP frame */
            else if (max_apdu <= 480)
                octet |= 0x03;
            else if (max_apdu <= 1024)
                octet |= 0x04;
            /* fits in an ISO 8802-3 frame */
            else if (max_apdu <= 1476)
                octet |= 0x05;

            return octet;
        }
          internal static int Encode_Context_ObjectId(ref Byte[] apdu, Byte tag_number, int object_type, UInt32 instance,int pos)
        {
            int len = 0;
            UInt32 value = 0;
            UInt32 type = 0;
            len = Encode_Tag(ref apdu, tag_number, true, 4, pos);

            type = (UInt32)object_type;
            
            value =
            ((type & BacnetConst.BACNET_MAX_OBJECT) << BacnetConst.BACNET_INSTANCE_BITS) | (instance &
            BacnetConst.BACNET_MAX_INSTANCE);
            len += Encode_Unsigned32(ref apdu, value,pos+len);
           


            return len;  
            
        }
       
       internal static int Encode_Context_Enumerate(ref Byte[] apdu,Byte tag_number,UInt32 value,int pos)
    {
        int len=0;
         
        if (value < 0x100) {
        len = 1;
    } else if (value < 0x10000) {
        len = 2;
    } else if (value < 0x1000000) {
        len = 3;
    } else {
        len = 4;
    }
        len = Encode_Tag(ref apdu, tag_number, true, (UInt32)len,pos);
        len += Encode_Unsigned(ref apdu,value,pos+len);
        
      
         return len;
    }
       internal static int Encode_Context_Boolean(ref Byte[] apdu,Byte tag_number,Boolean value,int pos)
     {
         int len = 0;        /* return value */

         len = Encode_Tag(ref apdu, (Byte)tag_number, true, 1,pos+len);
         if (value)
             apdu[pos + len] = 1;
         else
             apdu[pos + len] = 0;
         len++;

         return len;
     }
      internal static int Encode_Context_Unsigned(ref Byte[] apdu,Byte tag_number,UInt32 value,int pos)
     {
         int len = 0;
        

         /* length of unsigned is variable, as per 20.2.4 */
         if (value < 0x100)
         {
             len = 1;
         }
         else if (value < 0x10000)
         {
             len = 2;
         }
         else if (value < 0x1000000)
         {
             len = 3;
         }
         else
         {
             len = 4;
         }

         len = Encode_Tag(ref apdu, tag_number, true, (UInt32)len,pos);
         len += Encode_Unsigned(ref apdu, value,pos+len);



         return len; 
     }
      internal static int Encode_Opening_Tag(ref Byte[] apdu,Byte tag_number,int pos )
    {
        int len = 1;

        /* set class field to context specific */
        apdu[pos + 0] = 0x08;//BIT3
        /* additional tag byte after this byte for extended tag byte */
        if (tag_number <= 14)
        {
            apdu[pos+0] |= (Byte)(tag_number << 4);
        }
        else
        {
            apdu[pos+0] |= 0xF0;
            apdu[pos+1] = tag_number;
            len++;
        }
        /* set type field to opening tag */
        apdu[pos+0] |= 6;

        return len;
    }
      internal static int Encode_Closing_Tag(ref Byte[] apdu,Byte tag_number,int pos)
    {
        int len = 1;

        /* set class field to context specific */
        apdu[pos + 0] = 0x08;
        /* additional tag byte after this byte for extended tag byte */
        if (tag_number <= 14)
        {
            apdu[pos+0] |=(Byte) (tag_number << 4);
        }
        else
        {
            apdu[pos+0] |= 0xF0;
            apdu[pos+1] = tag_number;
            len++;
        }
        /* set type field to closing tag */
        apdu[pos+0] |= 7;

        return len;
    }
      internal static int Encode_Application_Object_Id(ref Byte[] apdu, int object_type, UInt32 instance, int pos)
    
      {    
           int len = 0;
           UInt32 value = 0;
           UInt32 type = 0;
          type = (UInt32)object_type;
            
          value =
           ((type & BacnetConst.BACNET_MAX_OBJECT) << BacnetConst.BACNET_INSTANCE_BITS) | (instance &
            BacnetConst.BACNET_MAX_INSTANCE);
            len += Encode_Unsigned32(ref apdu, value,pos+1);


          len +=Encode_Tag(ref apdu, (byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_OBJECT_ID, false,
                (UInt32)len, pos + 0);
          return len;
          

      }
      internal static int Encode_Application_Unsigned(ref byte[] apdu, UInt32 value, int pos)
      {
          int len = 0;
          len = Encode_Unsigned(ref apdu, value, pos + 1);
          len +=
              Encode_Tag(ref apdu, (byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_UNSIGNED_INT, false,
              (UInt32)len, pos + 0);

          return len;

      }
     
        
       internal static int Encode_Application_Data(ref Byte[] apdu,ref BACNET_APPLICATION_DATA_VALUE value,int pos)
    {
        int apdu_len = 0;
        switch (value.tag)
        {
            case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_UNSIGNED_INT:
                {
                    apdu_len =BasicalProcessor.Encode_Unsigned(ref apdu,value.value.Unsigned_Int,pos+1);
                    apdu_len+=BasicalProcessor.Encode_Tag(ref apdu,(Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_UNSIGNED_INT,false,(uint)apdu_len,pos);

                    break;
                }
            case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_BOOLEAN:
                {
                     apdu_len+=BasicalProcessor.Encode_Application_Boolbean(ref apdu,value.value.Boolean, pos);
                    break;
                }
            case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_DATE:
                {
                    apdu_len = BasicalProcessor.Encode_Application_Date(ref apdu,ref value.value.Date, pos);
               
                    break;
                }
            case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_TIME:
                {
                    apdu_len = BasicalProcessor.Encode_Application_Time(ref apdu, ref value.value.Time, pos);
                    break;
                }
            case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_CHARACTER_STRING:
                {
                    break;
                }
            case (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_OBJECT_ID:
                {
                    apdu_len = BasicalProcessor.Encode_Application_Object_Id(ref apdu, value.value.Object_Id.type, value.value.Object_Id.instance, pos);
                    break;
                }
        }

        
        return apdu_len;
    }

        internal static int Encode_Context_Timestamp(ref Byte[] apdu,Byte tag_number,ref BACNET_TIMESTAMP value,int pos)
     {
         int len = 0;        /* length of each encoding */
         int apdu_len = 0;
         len = Encode_Opening_Tag(ref apdu,tag_number,pos+apdu_len);
         apdu_len += len;
         //timestamp
         len = Encode_Context_Datetime(ref apdu, 2, ref value.value, pos + apdu_len);


         apdu_len += len;
          //
         len = Encode_Closing_Tag(ref apdu, tag_number, pos + apdu_len);
         apdu_len += len;

         return apdu_len;
     }

        internal static int Encode_Context_Datetime(ref Byte[] apdu, Byte tag_number,ref BACNET_DATE_TIME value,int pos)
      {
          int len = 0;
          int apdu_len = 0;
          len = Encode_Opening_Tag(ref apdu, tag_number, pos+apdu_len);
          apdu_len += len;

          len = Encode_Datetime(ref apdu, ref value, pos + apdu_len);
         
          apdu_len += len;

          len = Encode_Closing_Tag(ref apdu, tag_number, pos + apdu_len);
          apdu_len += len;

          return apdu_len;

      }
         internal static int Encode_Datetime(ref Byte[] apdu,ref BACNET_DATE_TIME value,int pos)
      {
          int len = 0;
          int apdu_len = 0;
            len = Encode_Application_Date(ref apdu, ref value.date,apdu_len+pos);
              apdu_len += len;

          len = Encode_Application_Time(ref apdu,ref value.time,apdu_len+pos);
        apdu_len += len;
   
       return apdu_len;
      }
      internal static int Encode_Application_Date(ref Byte[] apdu,ref BACNET_DATE bdate,int pos)
       {
           int len = 0;

           
           if (bdate.year >= 1900)
           {
               apdu[pos+1] = (Byte)(bdate.year - 1900);
           }
           else if (bdate.year < 0x100)
           {
               apdu[pos + 1] = (Byte)bdate.year;

           }
            apdu[pos+2] = bdate.month;
            apdu[pos+3] = bdate.day;
            apdu[pos+4] = bdate.wday;
            len = 4;
            len+= Encode_Tag(ref apdu, (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_DATE, false,
            (UInt32)len,pos+0);

           return len;
       }
      internal static int Encode_Application_Time(ref Byte[] apdu,ref BACNET_TIME btime,int pos)
    {

        int len = 0;
        apdu[pos+1] = btime.hour;

        apdu[pos+2] = btime.min;
        apdu[pos+3] = btime.sec;
        apdu[pos+4] = btime.hundredths;
        len = 4;
        len += Encode_Tag(ref apdu, (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_TIME, false,
            (UInt32)len, pos + 0);

        return len;
    }
      internal static int Encode_Context_Bitstring(ref Byte[] apdu,Byte tag_number,ref BACNET_BIT_STRING bit_string ,int pos)
    {
       int len = 0;
       UInt32 bit_string_encoded_length = 1;
       bit_string_encoded_length += bit_string.bytes_used();
       len = Encode_Tag(ref apdu, tag_number, true, bit_string_encoded_length,pos+len);
       len += Encode_Bitstring(ref apdu,ref  bit_string,pos+len);


       return len;
    }
          internal static int Encode_Bitstring(ref Byte[] apdu,ref  BACNET_BIT_STRING bit_string,int pos)
    {
        int len = 0;
        Byte remaining_used_bits = 0;
        Byte used_bytes = 0;
        Byte i = 0;
        if(bit_string.bits_used==0)
        {
            apdu[pos] = 0;
            len++;
        }
        else
        {
            used_bytes = bit_string.bytes_used();
            remaining_used_bits =(Byte)(bit_string.bits_used-((used_bytes -
                    1) * 8));
            apdu[pos + len++] =(Byte)(8 - remaining_used_bits);
            for (i = 0; i < used_bytes; i++)
            {
                apdu[pos + len++] = bit_string.byte_reverse_bits(bit_string.bitstring_octet(i));
               
            }
        }
        return len;

    }
        internal static int Encode_Property_State(ref Byte[] apdu,ref BACNET_PROPERTY_STATE value,int pos )
      {
          int len = 0;
          switch (value.tag)
          {

             case BACNET_PROPERTY_STATE_TYPE.UNSIGNED_VALUE:
                  len =
                   Encode_Context_Unsigned(ref apdu, 11,
                   value.value,pos);

                  break;

          }
          return len;
      }
          internal static int Encode_Context_Character_String(ref Byte[] apdu,Byte tag_number,ref BACNET_CHARACTER_STRING char_string,int pos)
      {
          int len = 0;
          int string_len = 0;
          string_len = (int)char_string.size + 1;
          len += Encode_Tag(ref apdu, tag_number, true, (uint)string_len,pos+len);
          len += Encode_Character_String(ref apdu, ref char_string,pos+len);
          return len;


      }
          internal static int Encode_Character_String(ref Byte[] apdu, ref BACNET_CHARACTER_STRING char_string,int pos)
        {
           UInt32 apdu_len = 1 /*encoding */ ;
           UInt32 i;
           apdu_len += char_string.size;
           apdu[pos+0] = char_string.encoding;
           for (i = 0; i < char_string.size; i++)
           {
               apdu[pos + 1 + i] = (Byte)char_string.value[i];
           }
           return (int)apdu_len;
        }
          internal static int Encode_Application_Boolbean(ref Byte[] apdu,Boolean value,int pos)
        {
            int len = 0;
            UInt32 len_value = 0;
            if(value)
            {
                len_value = 1;
            }
            len += Encode_Tag(ref apdu, (Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_BOOLEAN, false,len_value,pos);
            return len;

        }
          internal static int Encode_Application_Enumerated(ref Byte[] apdu,UInt32 value,int pos)
        {
            int len = 0;  
            len+=Encode_Unsigned(ref apdu,value,pos+1);
            len += Encode_Tag(ref apdu,(Byte)BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_ENUMERATED, false, (uint)len, pos);
            return len;


       


        }

}

    }

