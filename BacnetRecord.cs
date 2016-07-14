using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
  public class BacnetRecord
    {
      public static List<ServiceInformation> inforamtion_list = new List<ServiceInformation>();
      private static int max_infromation=100;


      public void SetMaxInformation(int n)
      {
          max_infromation=n;
      }
      static internal void clear()
      {
          if (inforamtion_list.Count >BacnetRecord. max_infromation)
          {
              inforamtion_list.Clear();
          }
      }
      internal static void Add(uint pdu_type,uint service_choice,DateTime time,Boolean issend,UInt32 device_id)
      {
          clear();
          ServiceInformation information = new ServiceInformation();
          information.pdu_type = (BACNET_PDU_TYPE)pdu_type;
          if (pdu_type == 0 || pdu_type == 0x30)
          {
              information.cnf_type = (BACNET_CONFIRMED_SERVICE)(service_choice);
          }
          if(pdu_type==0x10)
          {
              information.uncon_type = (BACNET_UNCONFIRMED_SERVICE)(service_choice);
          }
          information.send = issend;
          information.time = time;
        
          information.device_id = device_id;
          inforamtion_list.Add(information);



      }
        
    }
  public struct ServiceInformation
  {
    public DateTime time;
    public Boolean send;
    public BACNET_PDU_TYPE pdu_type;
    public BACNET_UNCONFIRMED_SERVICE uncon_type;
    public BACNET_CONFIRMED_SERVICE cnf_type;

    public UInt32 device_id;//UInt32.MaxValue indicate	broadcast

  }
}
