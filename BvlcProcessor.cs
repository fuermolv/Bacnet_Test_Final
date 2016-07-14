using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class BvlcProcessor
    { 
        internal int Encode(ref byte[] buf, ref BACNET_ADDRESS dest, ref BACNET_NPDU_DATA npdu, int pdu_len)
        {
            
            UInt16 BVLC_length=0;
            byte[] data = new byte[1024];
            Array.Copy(buf, 0, data, 4, pdu_len);
            buf = data;
            buf[0] = 0x81;//BVLL_TYPE_BACNET_IP
            if (dest.net == 0xFFFF)//全局广播
            
             {
                 buf[1] =(byte)BACNET_BVLC_FUNCTION.BVLC_DISTRIBUTE_BROADCAST_TO_NETWORK;
                
             }
            else if (dest.mac_len == 0)
            {
                buf[1] = (byte)BACNET_BVLC_FUNCTION.BVLC_ORIGINAL_BROADCAST_NPDU;
            }
            else if(dest.mac_len==6)
                buf[1] = (byte)BACNET_BVLC_FUNCTION.BVLC_ORIGINAL_UNICAST_NPDU;

        
            

            BVLC_length = (UInt16)(pdu_len + 4) ;
            BasicalProcessor.Encode_Unsigned16(ref buf, BVLC_length, 2);
            return BVLC_length;
           
            }
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal int Decode(ref BACNET_ADDRESS src, ref byte[] buf)
        {   
            UInt16 npdu_len=0;
            BasicalProcessor.Decode_Unsigned16(ref buf, ref npdu_len, 2);
            npdu_len=(UInt16)(npdu_len-4);

            int function_type = buf[1];
            switch (function_type)
            {
             
                case (byte)BACNET_BVLC_FUNCTION.BVLC_ORIGINAL_BROADCAST_NPDU:
                    {
                        byte[] data = new byte[1024];
                        Array.Copy(buf, 4, data, 0, npdu_len);
                        buf = data;
                        break;
                    }
                case (byte)BACNET_BVLC_FUNCTION.BVLC_DISTRIBUTE_BROADCAST_TO_NETWORK:
                    {
                        byte[] data = new byte[1024];
                        Array.Copy(buf, 4, data, 0, npdu_len);
                        buf = data;
                        break;
                    }

                case (byte)BACNET_BVLC_FUNCTION.BVLC_ORIGINAL_UNICAST_NPDU:
                    {
                        byte[] data = new byte[1024];
                        Array.Copy(buf, 4, data, 0, npdu_len);
                        buf = data;
                        break;
                    }
            }
            return npdu_len;
            }
        }
    }

