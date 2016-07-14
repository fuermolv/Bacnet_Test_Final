using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
 public interface Event_Handler
    {
      void Alarm_Handler(BACNET_EVENT_NOTIFICATION_DATA data);

      void Alarm_Cancel_Handler(UInt32 device_id);


      void Error_Handler(BACNET_ADDRESS src, Byte invoke_id, BACNET_ERROR_CLASS error_class, BACNET_ERROR_CODE error_code);
      void Reject_Handler(BACNET_ADDRESS src, Byte invoke_id, Byte reason);
      void Abort_Handler(BACNET_ADDRESS src, Byte invoke_id, Byte reason,Boolean server);
        
    }
}
