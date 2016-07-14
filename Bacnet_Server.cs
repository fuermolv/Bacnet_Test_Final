using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace Bacnet_Test_Final
{
    public class Bacnet_Server
    {
        private  UInt16 port;
        int BufferLength;
        Boolean Run;
        internal static  UInt16 local_send_port;
  
        public Bacnet_Server(UInt16 port=47808,int buflength=1476)
        {
            this.port = port;
            local_send_port = port;
            BufferLength=buflength;
            Run = false;
           
        }
        public static void Set_Broadcast_Port(UInt16 port)
        {
            BACNET_ADDRESS.Set_Broadcast_Port(port);
        }
      
        public void Set()
        {
            byte[] data;
            Run=true;
            int recv_length;
            EndPoint f_ip;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);//ip地址
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//建立新的socket
            newsock.Bind(ipep);
            UdpSender.client = newsock;
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender); //获得客户机地址
            while(Run)
            {
                data = new byte[1476];
                recv_length = newsock.ReceiveFrom(data, ref Remote);
                f_ip = Remote;
                IPEndPoint src_ip=(IPEndPoint)(Remote);
                BACNET_ADDRESS src = new BACNET_ADDRESS(src_ip);
                if (Forwarder.Get_Status())
                {
                    UdpSender udpsender = new UdpSender(1476);
                    udpsender.Send_Forwarder(ref data, (ushort)recv_length);

                }
                handler(ref src, ref data, (ushort)recv_length);
               


              
            }
            newsock.Close();
        }
        public void Stop()
        {
            Run = false;
        }
      void handler(ref BACNET_ADDRESS src,ref byte[] pdu,UInt16 pdu_len)
        {
              int apdu_offset = 0;//网络层数据的长度 
            BACNET_ADDRESS dest = new BACNET_ADDRESS();
            BACNET_NPDU_DATA npdu_data = new BACNET_NPDU_DATA();
            BvlcProcessor b_pro = new BvlcProcessor();
            NpduProcessor p_pro = new NpduProcessor();
            ApduProcessor a_pro =new ApduProcessor();
            b_pro.Decode(ref src, ref pdu);
            if (pdu[0] == 1) //1是版本号 
            {
                apdu_offset = p_pro.Decode(ref pdu, ref dest, ref src, ref npdu_data);//获得npdu的数据       

            }
            if (npdu_data.network_layer_message)
            {
                //尚未定义 
            }

            else
            {
                if ((dest.net == 0) || (dest.net == 0xFFFF))



                    a_pro.Decode(ref src, ref  pdu, (UInt16)(pdu_len - apdu_offset), apdu_offset);
            }
        }
       


    }
}
