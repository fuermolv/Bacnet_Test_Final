using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class Alarm_Event_Manager
    {
        private List<Device_ALARM_DATA> Alarm_List;
        private static Alarm_Event_Manager Manager;
        private Alarm_Event_Manager()
        {
            Alarm_List = new List<Device_ALARM_DATA>();

        }
        public static Alarm_Event_Manager Get_Alarm_Manager()
        {
            if (Manager == null)
            {
                Manager = new Alarm_Event_Manager();
            }
            return Manager;
        }
        internal void Add_Alarm_Event( ACKNOWLEDGE_ALARM_DATA data,UInt32 device_id)
        {
            Device_ALARM_DATA alarm_data = new Device_ALARM_DATA();
            alarm_data.data = data;
            alarm_data.device_id = device_id;
            Alarm_List.Add(alarm_data);

        }
        internal ACKNOWLEDGE_ALARM_DATA Get_Alarm_Event(UInt32 device)
        {
            Device_ALARM_DATA alarm_data;
            alarm_data = Alarm_List.Find(delegate(Device_ALARM_DATA data)
            {
                return data.device_id == device;
            }
            );
            if (alarm_data != null)
                return alarm_data.data;
            else
                return null;
        }
    }


    class Device_ALARM_DATA
    {
       public ACKNOWLEDGE_ALARM_DATA data;
       public UInt32 device_id;
        public Device_ALARM_DATA()
        {
            data=new ACKNOWLEDGE_ALARM_DATA();
        }
       
    }
}
