using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class ApduProcessor
    {
        internal void Decode(ref BACNET_ADDRESS src, ref byte[] apdu, UInt16 apdu_len, int pos = 0)
        {
            BACNET_CONFIRMED_SERVICE_DATA service_data = new BACNET_CONFIRMED_SERVICE_DATA();
            BACNET_CONFIRMED_SERVICE_ACK_DATA service_ack_data = new BACNET_CONFIRMED_SERVICE_ACK_DATA();
            Byte invoke_id = 0;
            Byte service_choice = 0;
            Byte[] service_request;
            UInt32 error_code = 0;
            UInt32 error_class = 0;
            Byte reason = 0;
            Boolean server = false;
            Device_Manager device_manager;
            device_manager = Device_Manager.Get_Device_Manager();

            UInt16 service_request_len = 0;
            int len = 0;        /* counts where we are in PDU */
            uint pdu_type=(uint)apdu[pos] & 0xF0;

            //服务选择 



            switch (apdu[pos] & 0xF0)
            {
                case (Byte)BACNET_PDU_TYPE.PDU_TYPE_UNCONFIRMED_SERVICE_REQUEST:
                    {
                        service_choice = apdu[pos + 1];
                        service_request_len = (ushort)(apdu_len - 2);

                        service_request = new Byte[service_request_len];

                        Array.Copy(apdu, pos + 2, service_request, 0, service_request_len);
                        switch (service_choice)
                        {
                               
                            case (Byte)BACNET_UNCONFIRMED_SERVICE.SERVICE_UNCONFIRMED_COV_NOTIFICATION:
                                {
                                  /* ConFirmCovService un_cov = new ConFirmCovService();
                                   un_cov.Confirm_Cov_Handler(ref service_request, (ushort)service_request_len, ref src);*/
                                    break;
                                }
                            case (Byte)BACNET_UNCONFIRMED_SERVICE.SERVICE_UNCONFIRMED_EVENT_NOTIFICATION:
                                {
                                  //  ConfirmedEventNotificationService un_event_not = new ConfirmedEventNotificationService();
                                   // un_event_not.Cevent_Notify_Handler(ref service_request, (ushort)service_request_len, ref src);
//
                                    break;
                                }
                            case (Byte)BACNET_UNCONFIRMED_SERVICE.SERVICE_UNCONFIRMED_WHO_IS:
                                {
                                    WhoIsService whois = new WhoIsService();
                                    whois.Handler_Who_Is(ref service_request, (ushort)service_request_len, ref src);
                                    break;
                                }
                            case (Byte)BACNET_UNCONFIRMED_SERVICE.SERVICE_UNCONFIRMED_I_AM:
                                {
                                   
                                    IamService iam = new IamService();
                                    iam.Handler_Iam(ref service_request, (ushort)service_request_len, ref src);
                                    break;
                                }
                               
                        }
                        BacnetRecord.Add(pdu_type, apdu[pos + 1], DateTime.Now, false,UInt32.MaxValue);

                        break;


                    }
                case (Byte)BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST:
                    {

                        len = (int)Decode_confirmed_Service_Request(ref apdu, apdu_len, ref service_data, ref service_choice, ref service_request_len, pos);
                        service_request = new Byte[service_request_len];
                        Array.Copy(apdu, pos + len, service_request, 0, service_request_len);
                        switch (service_choice)
                        {
                            case (Byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_COV_NOTIFICATION:
                                {
                                    ConFirmCovService un_cov = new ConFirmCovService();
                                    un_cov.Confirm_Cov_Handler(ref service_request, (ushort)service_request_len, ref src, service_data);
                                  
                                    
                                    break;
                                }
                            case (Byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_READ_PROPERTY:
                                {
                                    BACnet_Read_Property_Data data = new BACnet_Read_Property_Data();
                                   ReadPropertyService rp_service = new ReadPropertyService();

                                   rp_service.Read_Property_Handler(ref service_request, service_request_len, ref src, ref service_data);
                                   BacnetRecord.Add(pdu_type, service_choice, DateTime.Now, false,UInt32.MaxValue);
                                    break;
                                }
                            case (Byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_SUBSCRIBE_COV:
                                {
                                   // CovSubscribeService cov_service = new CovSubscribeService();
                                   // cov_service.Cov_Subscribe_Handler(ref service_request, service_request_len, ref src, ref service_data);
                                    break;
                                }
                            case (Byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_ACKNOWLEDGE_ALARM:
                                {
                                  //  AcknowledgeAlarmService ack_al = new AcknowledgeAlarmService();
                                   // ack_al.Acknowledge_Alarm_Handler(ref service_request, service_request_len, ref src, ref service_data);
                                    break;
                                }
                            case (Byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_EVENT_NOTIFICATION:
                                {
                                    ConfirmedEventNotificationService eve_service = new ConfirmedEventNotificationService();
                                    eve_service.Cevent_Notify_Handler(ref service_request, service_request_len, ref src, service_data);
                                   
                                    break;

                                }

                        }

                       
                        break;
                    }
                case (Byte)BACNET_PDU_TYPE.PDU_TYPE_SIMPLE_ACK:
                    {

                        invoke_id = apdu[pos + 1];
                        service_choice = apdu[pos + 2];

                        if (service_choice ==(Byte) BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_SUBSCRIBE_COV)
                        {
                            UInt32 device_id=TsmProcessor.Get_Device_Id(invoke_id);
                            Device_Manager manager = Device_Manager.Get_Device_Manager();
                            if (manager.Type_Affirm(device_id) == BACNET_OBJECT_TYPE.BACNET_LIFT)
                            {
                                if (manager.Get_Lift(device_id).monitored == false)
                                    manager.Get_Lift(device_id).monitored = true;
                                else
                                    manager.Get_Lift(device_id).monitored = false;
                            }
                            if (manager.Type_Affirm(device_id) == BACNET_OBJECT_TYPE.BACNET_ESCALATORS)
                            {
                                if (manager.Get_Escalators(device_id).monitored == false)
                                    manager.Get_Escalators(device_id).monitored = true;
                                else
                                    manager.Get_Escalators(device_id).monitored = false;
                            }
                        }
                        if (service_choice == (Byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_ACKNOWLEDGE_ALARM)
                        {
                            UInt32 device_id = TsmProcessor.Get_Device_Id(invoke_id);
                            Device_Manager manager = Device_Manager.Get_Device_Manager();
                            manager.Event_Handler.Alarm_Cancel_Handler(device_id);
                        }

                        TsmProcessor.free_invoke_id(invoke_id);
                        break;
                    }
                case (Byte)BACNET_PDU_TYPE.PDU_TYPE_COMPLEX_ACK:
                    {

                        if ((apdu[pos + 0] & BacnetConst.BIT3) != 0)
                            service_ack_data.segmented_message = true;

                        if ((apdu[pos + 0] & BacnetConst.BIT2) != 0)
                            service_ack_data.more_follows = true;
                        invoke_id = service_ack_data.invoke_id = apdu[pos + 1];
                        len = 2;
                        if (service_ack_data.segmented_message)
                        {
                            service_ack_data.sequence_number = apdu[pos + len++];
                            service_ack_data.proposed_window_number = apdu[pos + len++];
                        }
                        service_choice = apdu[pos + len++];
                        service_request_len = (UInt16)(apdu_len - len);
                        service_request = new Byte[service_request_len];
                        Array.Copy(apdu, pos + len, service_request, 0, service_request_len);

                        switch (service_choice)
                        {
                            case (Byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_READ_PROPERTY:
                                { ReadPropertyService rp_ack = new ReadPropertyService();
                                rp_ack.Read_Property_Ack_Handler(ref service_request, service_request_len, ref src, ref service_ack_data);
                                break;
                                }
                            case (Byte)BACNET_CONFIRMED_SERVICE.SERVICE_CONFIRMED_READ_PROP_MULTIPLE:
                                {
                                    ReadPropertyMultiService rp_ack = new ReadPropertyMultiService();
                                    rp_ack.Read_Property_Multi_Ack_Handler(ref service_request, service_request_len, ref src, ref service_ack_data);
                                    break;

                                }
                               
                        }
                        BacnetRecord.Add(pdu_type, service_choice, DateTime.Now, false, TsmProcessor.Get_Device_Id(invoke_id));

                        TsmProcessor.free_invoke_id(invoke_id);



                        break;
                    }
                case (Byte)BACNET_PDU_TYPE.PDU_TYPE_ERROR:
                    {
                        invoke_id = apdu[pos+1];
                        service_choice = apdu[pos+2];
                        len = 3;
                        //fix me
                        BacnetRecord.Add(pdu_type, service_choice, DateTime.Now, false,TsmProcessor.Get_Device_Id(invoke_id));
                        device_manager.Event_Handler.Error_Handler(src, invoke_id, (BACNET_ERROR_CLASS)error_class, (BACNET_ERROR_CODE)error_code);
                        TsmProcessor.free_invoke_id(invoke_id);

                        break;
                    }

                case (Byte)BACNET_PDU_TYPE.PDU_TYPE_REJECT:
                    {
                        invoke_id = apdu[pos+1];
                        reason = apdu[pos+2];
                        BacnetRecord.Add(pdu_type, service_choice, DateTime.Now, false, TsmProcessor.Get_Device_Id(invoke_id));
                        device_manager.Event_Handler.Reject_Handler(src, invoke_id, reason);
                        TsmProcessor.free_invoke_id(invoke_id);
                        break;
                    }

                case (Byte)BACNET_PDU_TYPE.PDU_TYPE_ABORT:
                    {
                        server = (apdu[pos+0] & 0x01)!=0;
                        invoke_id = apdu[pos + 1];
                        reason = apdu[pos + 2];
                        BacnetRecord.Add(pdu_type, service_choice, DateTime.Now, false, TsmProcessor.Get_Device_Id(invoke_id));
                        device_manager.Event_Handler.Abort_Handler(src, invoke_id, reason, server);
                        TsmProcessor.free_invoke_id(invoke_id);
                        break;
                    }
            }
        }

        internal UInt16 Decode_confirmed_Service_Request(ref Byte[] apdu, UInt16 apdu_len, ref BACNET_CONFIRMED_SERVICE_DATA service_data, ref Byte service_choice,
            ref UInt16 service_requst_len, int pos)
        {
            UInt16 len = 0;   /* counts where we are in PDU */

            service_data.segmented_message = (apdu[0 + pos] & 0x08) != 0 ? true : false;
            service_data.more_follows = (apdu[0] & 0x04) != 0 ? true : false;
            service_data.segmented_response_accepted =
                (apdu[pos] & 0x02) != 0 ? true : false;
            service_data.max_segs = BasicalProcessor.Decode_Max_Segs(ref apdu, pos + 1);
            service_data.max_resp = BasicalProcessor.Decode_Max_Apdu(ref apdu, pos + 1);
            service_data.invoke_id = apdu[pos + 2];



            len = 3;
            if (service_data.segmented_message)
            {
                service_data.sequence_number = apdu[pos + len++];
                service_data.proposed_window_number = apdu[pos + len++];
            }
            service_choice = apdu[pos + len++];

            ;
            service_requst_len = (UInt16)(apdu_len - len);

            return len;
        }
        internal int Encode_Simple_Ack(ref Byte[] apdu, Byte invoke_id, Byte service_choice, int pos)
        {
            apdu[pos + 0] = (Byte)BACNET_PDU_TYPE.PDU_TYPE_SIMPLE_ACK;
            apdu[pos + 1] = invoke_id;
            apdu[pos + 2] = service_choice;

            return 3;
        }
    }
}
