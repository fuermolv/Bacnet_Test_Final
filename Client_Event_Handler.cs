using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacnet_Test_Final
{
    class Client_Event_Handler : Event_Handler
    {
       public void Alarm_Handler(BACNET_EVENT_NOTIFICATION_DATA data)
        {
            UInt32 device_id=CovProcessor.Get_Device_Id((Byte)data.processIdentifier);
            int result = 0;
            for (int i = 0; i < Form1.detail_list_Lift.Count;i++ )
            {
                if (Form1.detail_list_Lift[i].device_id == device_id)
                    result = i;
            }
            Form1.listview.Items[result].UseItemStyleForSubItems = false;
            Form1.listview.Items[result].SubItems[7].BackColor = System.Drawing.Color.Red;
            Form1.listview.Items[result].SubItems[7].Text = "是";
        
     
            Device_Manager manager = Device_Manager.Get_Device_Manager();

            manager.Get_Lift(device_id).lift.SetAlaram_Status(true);
        }

     public  void Alarm_Cancel_Handler(UInt32 device_id)
       {
        
           int result = 0;
           for (int i = 0; i < Form1.detail_list_Lift.Count; i++)
           {
               if (Form1.detail_list_Lift[i].device_id == device_id)
                   result = i;
           }
           Form1.listview.Items[result].UseItemStyleForSubItems = false;
           Form1.listview.Items[result].SubItems[7].BackColor = System.Drawing.Color.Gray;
           Form1.listview.Items[result].SubItems[7].Text = "否";


           Device_Manager manager = Device_Manager.Get_Device_Manager();

           manager.Get_Lift(device_id).lift.SetAlaram_Status(false);
       }
       public void Error_Handler(BACNET_ADDRESS src, Byte invoke_id, BACNET_ERROR_CLASS error_class, BACNET_ERROR_CODE error_code)
        {

        }
       public void Reject_Handler(BACNET_ADDRESS src, Byte invoke_id, Byte reason)
        {

        }
       public void Abort_Handler(BACNET_ADDRESS src, Byte invoke_id, Byte reason, Boolean server)
        {

        }
    }

}
