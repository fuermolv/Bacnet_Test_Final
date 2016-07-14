using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacnet_Test_Final
{
    class CovProcessor
    {
        
        static node[] array;
        static byte pos;
  
         static CovProcessor()
       {
           array = new node[255];
            for(int i=0;i<255;i++)
            {
                array[i].id=0;

            }
           pos = 0;
       }
        internal static byte next_free_id(UInt32 temp)
         {
             byte now = pos;
             pos++;
            while(pos!=now)
            {
                if (array[pos].id == 0)
                {
                    array[pos].id = 1;
                    array[pos].device_id = temp;
                    return pos;

                }
                else 
                    pos++;
                if(pos==100)
                {
                    pos = 0;
                }
            }
            return 255; //代表错误
        }
        internal static void free_invoke_id(Byte id)
        {
            array[pos].id = 0;

        }
        internal static UInt32 Get_Device_Id(Byte process_id)
        {
            if (array[process_id].id == 1)
                return array[process_id].device_id;
            else
                return 0;
        }
        internal static void Free_SubscriberProcess(UInt32 device_id)
        {
            for(int i=0;i<255;i++)
            {  
                if(array[i].id==1&&array[i].device_id==device_id)
                array[pos].id = 0;
            }
        }
      struct node
      {
          public int id;
          public UInt32 device_id;
      }
    }
    }

