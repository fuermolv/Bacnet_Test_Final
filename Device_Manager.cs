using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    public class Device_Manager
    {
        public Event_Handler Event_Handler;
       

        private static Device_Manager Manager;
        public  List<Bacnet_Lift> Lift_list;
        private int Lift_Count;
        public List<Bacnet_Escalators> Escalators_list;
        private int Escalators_Count;
        public List<Bacnet_Device> device_list;
        private Device_Manager()
        {
            Lift_list = new List<Bacnet_Lift>();
            Escalators_list = new List<Bacnet_Escalators>();
            Lift_Count = 0;
            Escalators_Count = 0;
            Event_Handler = new Default_Event_Handler();
            device_list = new List<Bacnet_Device>();
        }
        public void Set_Alarm_Handler(Event_Handler handler)
        {
            Event_Handler = handler;
        }
        public static Device_Manager Get_Device_Manager()
        {
            if (Manager==null)
            {
                Manager=new Device_Manager();
            }
            return Manager;
        }
        public Bacnet_Device Get_Device(UInt32 device_id)
        {
          

            for (int i = 0; i < device_list.Count; i++)
            {
                if (device_list[i].device_object.Get_OBJECT_ID_Number() == device_id)
                    return device_list[i];
            }


            return null;

        }
        internal BACNET_OBJECT_TYPE Type_Affirm(UInt32 device_id) 
        {
           
            Bacnet_Device Get;
            Get = Get_Lift(device_id);
            if (Get == null)
            {
                Get = Get_Escalators(device_id);
                if(Get == null)
                {
                    return BACNET_OBJECT_TYPE.BACNET_ESCALATORS;
                }
            }
            else 
            {
                return BACNET_OBJECT_TYPE.BACNET_LIFT;
            }



            return BACNET_OBJECT_TYPE.PROPRIETARY_BACNET_OBJECT_TYPE;
        }
        public Boolean Is_Exist(UInt32 device_id)
        {
            Boolean result = false;
            if (Is_Lift(device_id))
                result = true;
            if(Is_Escalators(device_id))
                result = true;
            return result;
        }
        public Bacnet_Lift Get_Lift(UInt32 device_id)
        {
            Bacnet_Lift Get=new Bacnet_Lift();
            Get = Lift_list.Find(delegate(Bacnet_Lift lift)
            {
                return lift.device_object.Get_OBJECT_ID_Number() == device_id;
            });
            return Get;
           
            
        }
        public Boolean Is_Lift(UInt32 device_id)
        {
            Boolean result = false;
            if (Get_Lift(device_id) != null)
                result = true;
            return result;

        }
        public Bacnet_Lift Get_Lift(String Id_Num)
        {
            Bacnet_Lift Get = new Bacnet_Lift();
            Get = Lift_list.Find(delegate(Bacnet_Lift lift)
            {
                return lift.lift.Get_Identification_Number() == Id_Num;
            });
            return Get;
           
            

        }
        public Bacnet_Escalators Get_Escalators(String Id_Num)
        {
            Bacnet_Escalators Get = new Bacnet_Escalators();
            Get = Escalators_list.Find(delegate(Bacnet_Escalators escalators)
            {
                return escalators.escalators.Get_Identification_Number() == Id_Num;
            });
            return Get;
           
        }
        public Boolean Is_Escalators(UInt32 device_id)
        {
            Boolean result = false;
            if (Get_Escalators(device_id) != null)
                result = true;
            return result;

        }
        public Bacnet_Escalators Get_Escalators(UInt32 device_id)
        {

            Bacnet_Escalators Get = new Bacnet_Escalators();
            Get = Escalators_list.Find(delegate(Bacnet_Escalators escalators)
            {
                return escalators.device_object.Get_OBJECT_ID_Number() == device_id;
            });
            return Get;
           

        }
        public void Add_Lift(Bacnet_Lift source)
        {
            Lift_list.Add(source);
            Lift_Count++;
        }

        public void Add_Escalators(Bacnet_Escalators source)
        {
            Escalators_list.Add(source);
            Escalators_Count++;
        }
        public void Clear()
        {
            Lift_list.Clear();
            Escalators_list.Clear();
            device_list.Clear();
        }


    }
}
