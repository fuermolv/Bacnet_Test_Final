using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
   public  class Device
    {
        protected BACNET_OBJECT_ID Object_Identifier;
        protected String Object_Name;
        protected String Identification_Number;
        protected UInt32 Total_Running_Time;
        protected UInt32 Present_Counter_Value;
        protected List<BACnetMessageCode> Message_Code;
        protected BACNET_TIMESTAMP Time_Stamps;
        protected List<Property_TimeStamps> Property_Time_List;
       

       private void Property_Time_ini()
        {
          /*  Property_TimeStamps id = new Property_TimeStamps();
            id.poperty_id = BACNET_PROPERTY_ID.PROP_LIFT_Device_Identifer;
            Add_Property_Time(id);

            Property_TimeStamps runtime = new Property_TimeStamps();
            runtime.poperty_id = BACNET_PROPERTY_ID.PROP_LIFT_Total_Running_Time;
            Add_Property_Time(runtime);

            Property_TimeStamps cv = new Property_TimeStamps();
            cv.poperty_id = BACNET_PROPERTY_ID.PROP_LIFT_Present_Counter_Value;
            Add_Property_Time(cv);*/

            Property_TimeStamps me = new Property_TimeStamps();
            me.poperty_id = BACNET_PROPERTY_ID.PROP_LIFT_Message_Code;
            Add_Property_Time(me);




        }
        public void Set_OBJECT_ID(BACNET_OBJECT_ID id)
        {
            Object_Identifier.instance = id.instance;
            Object_Identifier.type = id.type;
        }
        public uint Get_OBJECT_ID_Number()
        {
            return Object_Identifier.instance;
        }
       public List<Property_TimeStamps> Get_Property_Time_List()
        {
            return Property_Time_List;
        }
       public void Add_Property_Time(Property_TimeStamps t)
       {
           for(int i=0;i<Property_Time_List.Count;i++)
           {
               if(Property_Time_List[i].poperty_id==t.poperty_id)
               {
                   Property_Time_List.RemoveAt(i);
               }

           }
           Property_Time_List.Add(t);

       }
       
        public Device()
        {
            Message_Code = new List<BACnetMessageCode>();
            Message_Code.Add(BACnetMessageCode.Fault_Free);
            Time_Stamps = new BACNET_TIMESTAMP();
            Object_Identifier = new BACNET_OBJECT_ID();
            Property_Time_List = new List<Property_TimeStamps>();
            Property_Time_ini();
        
           
        }
        public void Set_Identification_Number(String id)
        {
            Identification_Number=id;
        }
       

        public String Get_Identification_Number()
        {
            return Identification_Number;
        }
     
        public void Set_Time_Stamps(BACNET_DATE value)
        {
            this.Time_Stamps.tag = 2;
            this.Time_Stamps.value.date.year = value.year;
            this.Time_Stamps.value.date.month=value.month;
            this.Time_Stamps.value.date.wday=value.wday;
            this.Time_Stamps.value.date.day=value.day;
       
            
        }
        public void Set_Time_Stamps(BACNET_DATE value, ref BACNET_TIMESTAMP Time_Stamps)
        {
            Time_Stamps.tag = 2;
            Time_Stamps.value.date.year = value.year;
            Time_Stamps.value.date.month = value.month;
            Time_Stamps.value.date.wday = value.wday;
            Time_Stamps.value.date.day = value.day;


        }
        public void Set_Time_Stamps(BACNET_TIME value)
        {
            this.Time_Stamps.tag = 2;
           
           
            this.Time_Stamps.value.time.hour = value.hour;
            this.Time_Stamps.value.time.min = value.min;
            this.Time_Stamps.value.time.sec = value.sec;
            this.Time_Stamps.value.time.hundredths = value.hundredths;


        }

        public void Set_Time_Stamps(BACNET_TIME value, ref BACNET_TIMESTAMP Time_Stamps)
        {
            Time_Stamps.tag = 2;


            Time_Stamps.value.time.hour = value.hour;
            Time_Stamps.value.time.min = value.min;
            Time_Stamps.value.time.sec = value.sec;
            Time_Stamps.value.time.hundredths = value.hundredths;


        }


        

        public UInt32 Get_Total_Running_Time()
        {
            return Total_Running_Time;
        }
       public void Set_Total_Running_Time(UInt32 time)
        {
            Total_Running_Time = time;
        }
        public UInt32 Get_Present_Counter_Value()
        {
            return Present_Counter_Value;
        }
        public void Set_Present_Counter_Value(UInt32 times)
        {
            Present_Counter_Value = times;
        }
        public String Get_Time_Stamps()
        {    
            String temp=String.Empty;
            temp +=( Time_Stamps.value.date.year+1900).ToString() + "年";
            temp += Time_Stamps.value.date.month.ToString() + "月";
            temp += Time_Stamps.value.date.day.ToString() + "日";
            temp += Time_Stamps.value.time.hour.ToString() + "时";
            temp += Time_Stamps.value.time.min.ToString() + "分";
            temp += Time_Stamps.value.time.sec.ToString() + "秒";

            return temp;

        }
        public String Get_Time_Stamps(ref BACNET_TIMESTAMP t)
        {
             String temp=String.Empty;
            temp +=( t.value.date.year+1900).ToString() + "年";
            temp += t.value.date.month.ToString() + "月";
            temp += t.value.date.day.ToString() + "日";
            temp += t.value.time.hour.ToString() + "时";
            temp += t.value.time.min.ToString() + "分";
            temp += t.value.time.sec.ToString() + "秒";

            return temp;

        }
        public void Set_Message_Code(List<BACnetMessageCode> code_list)
        {
            Message_Code = code_list;
        }
       public void Remove_Message_Code( BACnetMessageCode code)
       {
           Message_Code.Remove(code);
       }
       public List<BACnetMessageCode> Get_Message_Code()
        {
           List<BACnetMessageCode> result=new List<BACnetMessageCode>();
           
           int count=Message_Code.Count ;
           for(int i=0;i<count;i++)
           {
               result.Add(Message_Code[i]);
           }

           return result;
        }
     
        
    }
  public struct Property_TimeStamps
   {
     internal  BACNET_TIMESTAMP time;
     internal BACNET_PROPERTY_ID poperty_id;
   }
  public  enum BACnetMessageCode
    {

       Fault_Free=0,//电梯无故障
       Safe_Switch_Off=1 , //电梯运行时安全回路断路
       Close_Door_Fault= 2, //关门故障
       Open_Door_Fault = 3, //关门故障
       Acident_Break_Off=4   ,   //轿厢在开锁区域外停止
       Acident_Move = 5, //轿厢意外移动
       Motor_Restrict =6,//电动机运转时间限制器动作
       Car_Position_Miss = 7,//楼层位置丢失
       Prevent_Falut_Again=8, //防止电梯再运行故障
       Auto_Operation = 40,//电梯恢复自动运行模式
       Outage = 41,//主电源断电
       Stop_Pattern=42,//进入停止服务
       Repair_Pattern = 43,//进入检修运行模式
       Fire_Return_Pattern = 44,//进入消防返回模式
       Fire_Pattern = 45,//进入消防员运行模式
       Emergency_Power_Parttern = 46,//进入应急电源运行
       Earthquake_Parttern=47,//地震模式
       Alarm_button = 90, //报警按钮动作
       


    }
}
