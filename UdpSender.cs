using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Bacnet_Test_Final
{
  public class UdpSender
    {
        private Byte[] data;
        private IPEndPoint ipep;
        internal static Socket client;
        

        public UdpSender(ref Byte[] byte_send, IPEndPoint ip)
        {

            data = byte_send;
            ipep = ip;
        }
        public UdpSender(int length=1476)
        {
            data = new Byte[length];
         
        
        }
        private void Send(int length)
        {
         
            
          Byte[] data_send=new Byte[length];
          Array.Copy(data, 0, data_send, 0, length);
          client.SendTo(data_send, length, SocketFlags.None, ipep);
       
        }
        internal void Send_Forwarder(ref Byte[] data_f, int length)
        {
            Byte[] data_send = new Byte[length];
            Array.Copy(data_f, 0, data_send, 0, length);
            for (int i = 0; i < Forwarder.Forwarder_List.Count; i++)
            {
                client.SendTo(data_send, length, SocketFlags.None, Forwarder.Forwarder_List[i].address.bacnet_ip);
            }
           
        }
        private void Send_Broadcast(int length)
         {
    
            Byte[] data_send = new Byte[length];
            Array.Copy(data, 0, data_send, 0, length);
            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);     
            client.SendTo(data_send, length, SocketFlags.None, ipep);
 
        }
        public void  Send_IamService()
        {
          
            IamService iam_service = new IamService();
            int len = iam_service.Pack_I_Am_Service(ref data);
            BACNET_ADDRESS dest = new BACNET_ADDRESS();
            dest.Get_Broadcast_Address();
            ipep = dest.bacnet_ip;
            if (ipep != null)
                Send_Broadcast(len);
            BacnetRecord.Add((uint)BACNET_PDU_TYPE.PDU_TYPE_UNCONFIRMED_SERVICE_REQUEST, (uint)BACNET_UNCONFIRMED_SERVICE.SERVICE_UNCONFIRMED_I_AM, DateTime.Now, true,UInt32.MaxValue);

        }

        internal void Send_SimpleAck(Byte invoke_id, BACNET_CONFIRMED_SERVICE service_type, BACNET_ADDRESS src)
        {
            SimpleAck sim_ack = new SimpleAck();



            int len = sim_ack.SimpleAck_Pack(ref data, invoke_id, service_type, src);
            BACNET_ADDRESS dest = src;
            ipep = dest.bacnet_ip;

            if (ipep != null)

                Send(len);
        }
      public void Send_Alarm_Ack(UInt32 device_id)
        {
            AcknowledgeAlarmService ack_s = new AcknowledgeAlarmService();
            int len=ack_s.AcknowledgeAlarmService_Pack(ref data, device_id);
           ;
            BACNET_ADDRESS dest = new BACNET_ADDRESS();
            BACNET_ADDRESS.Get_Device_Address(ref dest, device_id);
            ipep = dest.bacnet_ip;


            if (ipep != null)

                Send(len);
            BacnetRecord.Add((uint)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST, (uint)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_ACKNOWLEDGE_ALARM, DateTime.Now, true, device_id);

        }
        public void Send_ReadProperty_Identifer(UInt32 device_id)
       {
           
            ReadPropertyService rp_service = new ReadPropertyService();

            BACnet_Read_Property_Data rp_data = new BACnet_Read_Property_Data();
            Device_Manager manager = Device_Manager.Get_Device_Manager();
            rp_data.object_type = manager.Type_Affirm(device_id);
            if (rp_data.object_type == BACNET_OBJECT_TYPE.BACNET_LIFT)
                rp_data.object_instance = manager.Get_Lift(device_id).lift.Get_OBJECT_ID_Number();
            if (rp_data.object_type == BACNET_OBJECT_TYPE.BACNET_ESCALATORS)
                rp_data.object_instance = manager.Get_Escalators(device_id).escalators.Get_OBJECT_ID_Number();
            rp_data.object_property = BACNET_PROPERTY_ID.PROP_Identification_Number;
           
            int len = rp_service.Pack_Read_Property_Request(ref data, device_id,  rp_data);
    
           BACNET_ADDRESS dest = new BACNET_ADDRESS();
           BACNET_ADDRESS.Get_Device_Address(ref dest, device_id);
           ipep = dest.bacnet_ip;
            
           if(ipep!=null)
           
            Send(len);
           BacnetRecord.Add((uint)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST, (uint)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_READ_PROPERTY, DateTime.Now, true, device_id);
             
           

        }
     
        public void Send_ReadProperty_Object_List(UInt32 device_id)
        {
            ReadPropertyService rp_service = new ReadPropertyService();

            BACnet_Read_Property_Data rp_data = new BACnet_Read_Property_Data();
            rp_data.object_instance = device_id;
            rp_data.object_property = BACNET_PROPERTY_ID.PROP_OBJECT_LIST;
            rp_data.object_type =BACNET_OBJECT_TYPE.OBJECT_DEVICE;
            int len = rp_service.Pack_Read_Property_Request(ref data, device_id, rp_data);
            BACNET_ADDRESS dest = new BACNET_ADDRESS();
            BACNET_ADDRESS.Get_Device_Address(ref dest, device_id);
          
            ipep = dest.bacnet_ip;

            if (ipep != null)

                Send(len);
            BacnetRecord.Add((uint)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST, (uint)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_READ_PROPERTY, DateTime.Now, true,device_id);
           
        }
        internal void Send_ReadPropertyAckService(BACNET_ADDRESS src, BACnet_Read_Property_Data rp_data, Byte invoke_id)
        {
            ReadPropertyService rp_ack = new ReadPropertyService();
            int len = rp_ack.Read_Property_Ack_Pack(ref data, src, invoke_id, rp_data);
            BACNET_ADDRESS dest = src;

            ipep = dest.bacnet_ip;

            if (ipep != null)

                Send(len);
       BacnetRecord.Add((uint)BACNET_PDU_TYPE.PDU_TYPE_COMPLEX_ACK, (uint)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_READ_PROPERTY, DateTime.Now, true,UInt32.MaxValue);

        }
        public void Send_WhoIsService(UInt32 low_limit = 0, UInt32 high_limit = 0x3FFFFF)
        {
            WhoIsService whois_service = new WhoIsService();
            int len = whois_service.Pack_Who_Is_Service(ref data, low_limit, high_limit);
            BACNET_ADDRESS dest = new BACNET_ADDRESS();
            dest.Get_Broadcast_Address();
            ipep = dest.bacnet_ip;
            if (ipep != null)
                Send_Broadcast(len);

            BacnetRecord.Add((uint)BACNET_PDU_TYPE.PDU_TYPE_UNCONFIRMED_SERVICE_REQUEST, (uint)BACNET_UNCONFIRMED_SERVICE.SERVICE_UNCONFIRMED_WHO_IS, DateTime.Now, true,UInt32.MaxValue);

           
        }
      public void Send_Read_Property_Multi_Service(UInt32 device_id)
        {
            ReadPropertyMultiService rpm_service = new ReadPropertyMultiService();
            int len = rpm_service.Pack_Read_Property_Multi_Request(ref data, device_id);

            BACNET_ADDRESS dest = new BACNET_ADDRESS();
            BACNET_ADDRESS.Get_Device_Address(ref dest, device_id);
            ipep = dest.bacnet_ip;

            if (ipep != null)

                Send(len);
            BacnetRecord.Add((uint)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST, (uint)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_READ_PROP_MULTIPLE, DateTime.Now, true, device_id);
             
        }
      public void Send_CovSubscribeService(UInt32 device_id,Boolean cancel=false)
        {
            CovSubscribeService service = new CovSubscribeService();
            BACNET_SUBSCRIBE_COV_DATA covsub_data = new BACNET_SUBSCRIBE_COV_DATA();
           

           

            covsub_data.cancellationRequest = false;
          //可能要确认时取消
            if (cancel)
            {
                covsub_data.cancellationRequest = true;
                CovProcessor.Free_SubscriberProcess(device_id);
            }
            covsub_data.subscriberProcessIdentifier = CovProcessor.next_free_id(device_id);
            BACNET_OBJECT_ID obj_id = new BACNET_OBJECT_ID();
          
            covsub_data.lifetime = 0;
            covsub_data.issueConfirmedNotifications = true;

        
           int s = 0;
           Device_Manager manager = Device_Manager.Get_Device_Manager();
           Bacnet_Lift lift;
           Bacnet_Escalators escalators;
           lift = manager.Get_Lift(device_id);
           if (lift != null)
           {
               obj_id.instance = lift.lift.Get_OBJECT_ID_Number();
               obj_id.type = (UInt16)BACNET_OBJECT_TYPE.BACNET_LIFT;
               covsub_data.monitoredObjectIdentifier = obj_id;
               ipep = lift.address.bacnet_ip;
               s = 1;

           }
           else
           {
               escalators = manager.Get_Escalators(device_id);
               if (escalators != null)
               {
                   obj_id.instance = escalators.escalators.Get_OBJECT_ID_Number();
                   ipep = escalators.address.bacnet_ip;
                   s = 1;
                   obj_id.type = (UInt16)BACNET_OBJECT_TYPE.BACNET_ESCALATORS;
                   covsub_data.monitoredObjectIdentifier = obj_id;
               }

           }

           if (s == 1)
           {
               int len = service.Cov_Subscribe_pack(ref data, device_id, ref covsub_data);
               Send(len);
               BacnetRecord.Add((uint)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST, (uint)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_SUBSCRIBE_COV, DateTime.Now, true, device_id);

           }
         
             
        }
     
    }

  }