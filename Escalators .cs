using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    public class Escalators:Device
    {
        private BACnetEscalatorServiceMode Service_Mode; //当前服务状态 
        private Byte Operation_Status; //运行状态
        private Byte Operation_Direction;//运行方向

        private void Property_Time_ini()
        {
            Property_TimeStamps sm = new Property_TimeStamps();
            sm.poperty_id = BACNET_PROPERTY_ID.PROP_Service_Mode;
            base.Add_Property_Time(sm);

            Property_TimeStamps os = new Property_TimeStamps();
            os.poperty_id = BACNET_PROPERTY_ID.PROP_Operation_Status;
            base.Add_Property_Time(os);

            Property_TimeStamps od = new Property_TimeStamps();
            od.poperty_id = BACNET_PROPERTY_ID.PROP_Operation_Direction;
            base.Add_Property_Time(od);

        }
        public Escalators()
        {
            Property_Time_ini();
        }
        public void Set_Service_Mode(BACnetEscalatorServiceMode mode)
        {
            Service_Mode = mode;
        }
        public BACnetEscalatorServiceMode Get_Service_Mode()
        {
            return Service_Mode;

        }
        public void Set_Operation_Status(Byte status)
        {
            Operation_Status = status;
        }

        public Byte Get_Operation_Status()
        {
            return Operation_Status;
        }
        public void Set_Operation_Direction(Byte status)
        {
            Operation_Direction = status;
        }

        public Byte Get_Operation_Direction()
        {
            return Operation_Direction;
        }
        


    }
   public enum BACnetEscalatorServiceMode
    {
        Stop=0,
        Normal=1,
        Repair=2,
        Unknow=3,

    }
}
