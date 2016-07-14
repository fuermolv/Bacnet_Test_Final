using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacnet_Test_Final
{
   internal class Default_Event_Handler:Event_Handler
    {
       public void Alarm_Handler(BACNET_EVENT_NOTIFICATION_DATA data)
        {
            MessageBox.Show("收到来自设备" + data.initiatingObjectIdentifier.instance.ToString() + "的报警");
        
            
        }
       public void Alarm_Cancel_Handler(UInt32 device_id)
       {

       }
      public void Error_Handler(BACNET_ADDRESS src, Byte invoke_id, BACNET_ERROR_CLASS error_class, BACNET_ERROR_CODE error_code)
       {

       }
      public void Reject_Handler(BACNET_ADDRESS src, Byte invoke_id, Byte reason)
      {

      }
      public  void Abort_Handler(BACNET_ADDRESS src, Byte invoke_id, Byte reason, Boolean server)
      {

      }
    }

}
