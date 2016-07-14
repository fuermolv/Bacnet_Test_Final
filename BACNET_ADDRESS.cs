using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace Bacnet_Test_Final
{
    public class BACNET_ADDRESS
    {

        public byte mac_len;
        private static UInt16 b_port=47808;
        public byte[] mac;
        public UInt16 net;      //网络号 
        public byte len;        /* length of MAC address */                   //MAC地址 
        public byte[] adr;
        public IPEndPoint bacnet_ip;

        public BACNET_ADDRESS()
        {

            net = 0;
            len = 6;
            mac_len = 6;
            adr = new byte[7];
            mac = new byte[7];
        }

        override public  String ToString()

        {
            return bacnet_ip.ToString();
        }
        public BACNET_ADDRESS(String ip, UInt16 port, UInt16 net = 0)
        {
            adr = new byte[7];
            mac = new byte[7];
            len = 6;
            mac_len = 6;
            this.net = net;
            String[] address = ip.Split('.');
            for (int i = 0; i < 4; i++)
            {
                adr[i] = Convert.ToByte(address[i]);
                mac[i] = Convert.ToByte(address[i]);
            }
            BasicalProcessor.Encode_Unsigned16(ref adr, port, 4);
            BasicalProcessor.Encode_Unsigned16(ref mac, port, 4);

            Get_Bacnet_Ip();

        }
        public static void Set_Broadcast_Port(UInt16 port)
        {
            b_port=port;
        }
        public BACNET_ADDRESS(IPEndPoint ip, UInt16 net = 0)
        {
            bacnet_ip = ip;
            len = 0;
            mac_len = 6;
            adr = new byte[7];
            mac = new byte[7];
            this.net = net;
            String address = ip.Address.ToString();
            String[] ip_number= address.Split('.');
            UInt16 port;
            for (int i = 0; i < 4; i++)
            {
                adr[i] = Convert.ToByte(ip_number[i]);
                mac[i] = Convert.ToByte(ip_number[i]);
            }
            port = (UInt16)ip.Port;
            BasicalProcessor.Encode_Unsigned16(ref adr, port, 4);
            BasicalProcessor.Encode_Unsigned16(ref mac, port, 4);
            Get_Bacnet_Ip();


        }
        public void Get_Bacnet_Ip()
        {
            UInt16 port = 0;
            String temp = mac[0].ToString() + ".";
            for (int i = 1; i < 4; i++)
            {
                if (i==3)
                  temp += mac[i].ToString();
               else
                temp += mac[i].ToString() + ".";
            }
            BasicalProcessor.Decode_Unsigned16(ref mac, ref port, 4);

            bacnet_ip = new IPEndPoint(
                 IPAddress.Parse(temp), port);
        }
        public void Get_My_Address()
        {

            this.net = 0;
            this.len = 0;
            this.mac_len = 6;



            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                    bacnet_ip = new IPEndPoint(_IPAddress, Bacnet_Server.local_send_port);
                }
            }
           
            String[] ip = AddressIP.Split('.');
            this.mac[0] = Convert.ToByte(ip[0]);
          
            this.mac[1] = Convert.ToByte(ip[1]);
          
            this.mac[2] = Convert.ToByte(ip[2]);
          
            this.mac[3] = Convert.ToByte(ip[3]);


            BasicalProcessor.Encode_Unsigned16(ref this.mac, Bacnet_Server.local_send_port, 4);

        }
        public void Get_Broadcast_Address()
        {
            bacnet_ip = new IPEndPoint(IPAddress.Broadcast, b_port);
            len = 0;
            mac_len = 6;
            net = 0;
            for (int i = 0; i < 4; i++)
            {   adr[i] = 0;
                mac[i] = 255;
            }
            BasicalProcessor.Encode_Unsigned16(ref this.mac, b_port, 4);
            


          }
        public static void Get_Device_Address(ref BACNET_ADDRESS address,UInt32 device_id)
        {
            Device_Manager manager = Device_Manager.Get_Device_Manager();
            foreach(Bacnet_Device device in manager.device_list)
            {
                if (device.device_object.Get_OBJECT_ID_Number() == device_id)
                    address = device.address;
            }


        }


    }
}
