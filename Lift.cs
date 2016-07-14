using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
   public class Lift : Device
    {
        private Boolean Alaram_Status;
        private BACnetLiftServiceMode Service_Mode;//当前服务状态         
        private Byte Car_Status;//轿厢运行状态
        private Byte Car_Direction;//轿厢运行方向
        private Boolean Door_Zone;//门区
        private Byte Car_Position;//电梯当前楼层
        private Boolean Door_Status;//关门到 位
        private Boolean Passenger_Status;//轿内是否有人      
    
       public Lift()
        {
            Service_Mode = BACnetLiftServiceMode.Normal;
            Door_Zone = true;
            Alaram_Status=false;
            Property_Time_ini();
        }
       private void Property_Time_ini()
       {
           Property_TimeStamps sm = new Property_TimeStamps();
           sm.poperty_id = BACNET_PROPERTY_ID.PROP_Service_Mode;
           base.Add_Property_Time(sm);

           Property_TimeStamps cs = new Property_TimeStamps();
           cs.poperty_id = BACNET_PROPERTY_ID.PROP_Car_Status;
           base.Add_Property_Time(cs);

           Property_TimeStamps cd = new Property_TimeStamps();
           cd.poperty_id = BACNET_PROPERTY_ID.PROP_Car_Direction;
           base.Add_Property_Time(cd);

           Property_TimeStamps dz = new Property_TimeStamps();
           dz.poperty_id = BACNET_PROPERTY_ID.PROP_Door_Zone;
           base.Add_Property_Time(dz);

           Property_TimeStamps cp = new Property_TimeStamps();
           cp.poperty_id = BACNET_PROPERTY_ID.PROP_Car_Position;
           base.Add_Property_Time(cp);

           Property_TimeStamps ds = new Property_TimeStamps();
           ds.poperty_id = BACNET_PROPERTY_ID.PROP_DOOR_STATUS;
           base.Add_Property_Time(ds);

           Property_TimeStamps ps = new Property_TimeStamps();
           ps.poperty_id = BACNET_PROPERTY_ID.PROP_Passenger_Status;
           base.Add_Property_Time(ps);

       }
       public bool GetAlaram_Status()
       {
           return Alaram_Status;

       }
       public void SetAlaram_Status(bool status)
       {
          Alaram_Status=status;

       }
   
        public void Set_Service_Mode(BACnetLiftServiceMode mode)
        {
            Service_Mode = mode;
        }
        public BACnetLiftServiceMode Get_Service_Mode()
        {
            return Service_Mode;
        }
        public void Set_Car_Status(Byte status)
        {
            Car_Status = status;
        }

        public Byte Get_Car_Status()
        {
            return Car_Status ;
        }

        public void Set_Car_Direction(Byte status)
        {
            Car_Direction = status;
        }

        public Byte Get_Car_Direction()
        {
            return Car_Direction;
        }
        public void Set_Car_Position(Byte status)
        {
           Car_Position  = status;
        }

        public Byte Get_Car_Position()
        {
            return Car_Position;
        }
        public void Set_Door_Zone(Boolean status)
        {
            Door_Zone = status;
        }
        public Boolean Get_Door_Zone()
        {
          return  Door_Zone;
        }
        public void Set_Door_Status(Boolean status)
        {
            Door_Status = status;
        }
        public Boolean Get_Door_Status()
        {
            return Door_Status;
        }
        public void Set_Passenger_Status(Boolean status)
        {
            Passenger_Status = status;
        }
        public Boolean Get_Passenger_Status()
        {
            return Passenger_Status;
        }
     
    }
   public enum BACnetLiftServiceMode
    {
        Stop = 0,
        Normal = 1,
        Repair = 2,
        Fire_return = 3,
        Fire_operation = 4,
        Eps = 5,
        Earthquake = 6,
        Unknown = 7

    }
}
