using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class NpduProcessor
    { internal void Encode_NpduData(ref BACNET_NPDU_DATA npdu_data, bool data_expecting_reply, BACNET_MESSAGE_PRIORITY priority)
        {

            npdu_data.data_expecting_reply = data_expecting_reply;
            npdu_data.protocol_version = 1;
            npdu_data.network_layer_message = false;
            npdu_data.network_message_type = BACNET_NETWORK_MESSAGE_TYPE.NETWORK_MESSAGE_INVALID;      /* optional */
            npdu_data.vendor_id = 0;       /* optional, if net message type is > 0x80 */
            npdu_data.priority = priority;
            npdu_data.hop_count = 255;//默认

        }
        internal int Encode(ref byte[] npdu, ref BACNET_ADDRESS dest, ref BACNET_ADDRESS src, ref BACNET_NPDU_DATA npdu_data, int pos = 0)
        {
            int len = 0;        /* return value - number of octets loaded in this function */
            byte i = 0;      /* counter  */
            if (npdu != null)
            {
                npdu[pos + 0] = npdu_data.protocol_version;
                //控制号
                npdu[pos + 1] = 0;
                //控制信息
                if (npdu_data.network_layer_message)
                    npdu[pos + 1] |= BacnetConst.BIT7;
                //是否是网络型报文
                if (dest.net != 0)
                    npdu[pos + 1] |= BacnetConst.BIT5;
                //0 表示DNET DLEN DADR HPO COUNT不存在
                // 1 表示存在，
                if (src.len != 0&&src.net!=0)
                    npdu[pos + 1] |= BacnetConst.BIT3;
                //0代表SNET SLEN 和SADR都不存在
                // 1表示都存在
                if (npdu_data.data_expecting_reply)
                    npdu[pos + 1] |= BacnetConst.BIT2;
                //是否需要回复
                npdu[pos + 1] |= (byte)((byte)npdu_data.priority & 0x03);
                //优先级
                len = 2;
                if (dest.len!=0&&dest.net!=0)
                {
                    len += BasicalProcessor.Encode_Unsigned16(ref npdu, dest.net, pos + len);
                    npdu[pos + len] = dest.len;
                    len++;
                    //当DNET=FFFF 表示全局广播
                    //缺省表示本地广播
                    if (dest.len != 0 &&dest.net!=0)
                    {
                        for (i = 0; i < dest.len; i++)
                        {
                            npdu[pos + len++] = dest.adr[i];
                            
                        }
                    }
                }
                if (src.len != 0 && src.net != 0)
                {
                    len += BasicalProcessor.Encode_Unsigned16(ref npdu, src.net, pos + len);
                    npdu[pos + len] = src.len;
                  
                    len++;
                    if (src.len != 0)
                    {
                        for (i = 0; i < src.len; i++)
                        {
                            npdu[pos + len++] = src.adr[i];                           
                        }
                    }
                }
                if (dest.net != 0)
                {
                    npdu[pos + len] = (byte)npdu_data.hop_count;
                    len++;
                    //转发计数
                }
                if (npdu_data.network_layer_message)
                {
                    npdu[pos + len] = (byte)npdu_data.network_message_type;
                    len++;
                    if ((int)npdu_data.network_message_type >= 0x80)
                        len += BasicalProcessor.Encode_Unsigned16(ref npdu, npdu_data.vendor_id, pos + len);
                    //报文类型
                    //0x80以后为用户扩展类型
                }
            }
            return len;
        }
        /**********************************************************************************************************************************************************************/
        internal int Decode(ref byte[] npdu, ref BACNET_ADDRESS dest, ref BACNET_ADDRESS src, ref BACNET_NPDU_DATA npdu_data)
        {
            int len = 0;
            byte i = 0; 
            UInt16 src_net = 0;
            UInt16 dest_net = 0;
            byte address_len = 0;
            byte mac_octet = 0;
            npdu_data.protocol_version = npdu[0];
            npdu_data.network_layer_message = ((npdu[1] & BacnetConst.BIT7) != 0) ? true : false; //是否是网络层的数据报文或者网络层协议报文
            npdu_data.data_expecting_reply = ((npdu[1] & BacnetConst.BIT2) != 0) ? true : false;
            npdu_data.priority = (BACNET_MESSAGE_PRIORITY)(npdu[1] & 0x03);
            len = 2;
            if ((npdu[1] & BacnetConst.BIT5) != 0)
            {
                len += BasicalProcessor.Decode_Unsigned16(ref npdu, ref dest_net, len);
                address_len = npdu[len++];
                dest.net = dest_net;
                dest.len = address_len;
                if (address_len > 0)
                {


                    for (i = 0; i < address_len; i++)
                    {
                        mac_octet = npdu[len++];//11

                        dest.adr[i] = mac_octet;
                    }
                }
            }
            else
            {
                dest.net = 0;
                dest.len = 0;
                for (i = 0; i < 7; i++)
                {
                    dest.adr[i] = 0;
                }
            }
                //源mac

                if ((npdu[1] & BacnetConst.BIT3) != 0)
                {
                    len += BasicalProcessor.Decode_Unsigned16(ref npdu, ref src_net, len);//13          
                    address_len = npdu[len++];//14
                    src.net = src_net;
                    src.len = address_len;
                    if (address_len > 0)
                    {
                       

                        for (i = 0; i < address_len; i++)
                        {
                            mac_octet = npdu[len++];//21

                            src.adr[i] = mac_octet;                                                    
                        }
                    }
                }
                else
                {
                    src.net = 0;
                    src.len = 0;
                    for (i = 0; i < 7; i++)
                    {
                        src.adr[i] = 0;
                    }
                }
               //hop count

                if (dest_net != 0)
                {
                    npdu_data.hop_count = npdu[len++];
                }
                else
                {
                    npdu_data.hop_count = 0;
                }
                //对于网络层协议无处理功能
                if (npdu_data.network_layer_message)
                {
                    npdu_data.network_message_type=
                (BACNET_NETWORK_MESSAGE_TYPE)npdu[len++];
                }
                else
                    npdu_data.network_message_type = BACNET_NETWORK_MESSAGE_TYPE.NETWORK_MESSAGE_INVALID;
                dest.Get_Bacnet_Ip();
                src.Get_Bacnet_Ip();
                return len;

            }



        }
    }

