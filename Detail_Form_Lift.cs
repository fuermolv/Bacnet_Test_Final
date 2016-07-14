using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Bacnet_Test_Final
{
    public partial class Detail_Form_Lift : Form
    {
        public UInt32 device_id;
        private Bacnet_Lift this_lift;
        
        public Detail_Form_Lift()
        {
            InitializeComponent();
        }

        private void Detail_Form_Load(object sender, EventArgs e)
        {
            timer1.Start();
            
        }
          private void listview_init()
        {
           
            Device_Manager manager = Device_Manager.Get_Device_Manager();

            this_lift = manager.Get_Lift(device_id);
             
              
              

                 listView1.Items.Add(new ListViewItem(new string[] { "设备标识符", "","" ,}));
                 listView1.Items.Add(new ListViewItem(new string[] { "对象标识符", "", "" ,}));
                 listView1.Items.Add(new ListViewItem(new string[] { "设备编号", "", "" ,}));
                 listView1.Items.Add(new ListViewItem(new string[] { "服务状态", "", "", }));
                 listView1.Items.Add(new ListViewItem(new string[] { "运行状态", "", "", }));
                 listView1.Items.Add(new ListViewItem(new string[] { "运行方向", "", "", }));
                 listView1.Items.Add(new ListViewItem(new string[] { "门区", "", "" ,}));
                 listView1.Items.Add(new ListViewItem(new string[] { "当前楼层", "", "" ,}));
                 listView1.Items.Add(new ListViewItem(new string[] { "关门到位", "", "", }));
                 listView1.Items.Add(new ListViewItem(new string[] { "轿内人员", "", "" ,}));
                 listView1.Items.Add(new ListViewItem(new string[] { "运行时间", "", "", }));
                 listView1.Items.Add(new ListViewItem(new string[] { "运行次数", "", "", }));
                 listView1.Items.Add(new ListViewItem(new string[] { "信息代码", "", "", }));
                 listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize); 
       
        }

          private void Detail_Form_Lift_Shown(object sender, EventArgs e)
          {
              listview_init();
          }

          private void Detail_Form_Lift_FormClosed(object sender, FormClosedEventArgs e)
          {
              timer1.Stop();
          }

          private void timer1_Tick(object sender, EventArgs e)
          {
              Lift_Update();
          }
          private void Lift_Update()
          {

              if (this_lift != null)
              {
                  if (this_lift.monitored)
                  {
                      textBox1.Text = "正在监控";
                      button2.Text = "取消监控";
                  }
                  else
                  {
                      textBox1.Text = "未监控";
                      button2.Text = "启动监控";
                  }
                  listView1.Items[0].SubItems[1].Text = this_lift.device_object.Get_OBJECT_ID_Number().ToString();
                  listView1.Items[1].SubItems[1].Text=this_lift.lift.Get_OBJECT_ID_Number().ToString();
                  listView1.Items[2].SubItems[1].Text = this_lift.lift.Get_Identification_Number();
                  switch (this_lift.lift.Get_Service_Mode())
                  {
                      case BACnetLiftServiceMode.Stop:
                          {
                              listView1.Items[3].SubItems[1].Text = "停止";
                              break;
                          }
                      case BACnetLiftServiceMode.Normal:
                          {
                              listView1.Items[3].SubItems[1].Text = "正常";
                              break;
                          }
                      case BACnetLiftServiceMode.Repair:
                          {
                              listView1.Items[3].SubItems[1].Text = "修理";
                              break;
                          }
                      case BACnetLiftServiceMode.Eps:
                          {
                              listView1.Items[3].SubItems[1].Text = "应急电源";
                              break;
                          }
                      case BACnetLiftServiceMode.Fire_operation:
                          {
                              listView1.Items[3].SubItems[1].Text = "消防运行";
                              break;
                          }
                      case BACnetLiftServiceMode.Fire_return:
                          {
                              listView1.Items[3].SubItems[1].Text = "消防返回";
                              break;
                          }
                      case BACnetLiftServiceMode.Earthquake:
                          {
                              listView1.Items[3].SubItems[1].Text = "地震模式";
                              break;
                          }
                      case BACnetLiftServiceMode.Unknown:
                          {
                              listView1.Items[3].SubItems[1].Text = "未知";
                              break;
                          }
                  }
                  switch (this_lift.lift.Get_Car_Status())
                  {
                      case 0:
                          {
                              listView1.Items[4].SubItems[1].Text = "停止";
                              break;
                          }
                      case 1:
                          {
                              listView1.Items[4].SubItems[1].Text = "运行";
                              break;
                          }
                  }

                  switch (this_lift.lift.Get_Car_Direction())
                  {
                      case 0:
                          {
                              listView1.Items[5].SubItems[1].Text = "无方向";
                              break;
                          }
                      case 1:
                          {
                              listView1.Items[5].SubItems[1].Text = "上行";
                              break;
                          }
                      case 2:
                          {
                              listView1.Items[5].SubItems[1].Text = "下行";
                              break;
                          }
                  }
                  if (this_lift.lift.Get_Door_Zone())
                  {
                      listView1.Items[6].SubItems[1].Text = "在门区";

                  }
                  else
                  {
                      listView1.Items[6].SubItems[1].Text = "非门区";

                  }


                  listView1.Items[7].SubItems[1].Text = this_lift.lift.Get_Car_Position().ToString();

                  if (this_lift.lift.Get_Door_Status())
                  {
                      listView1.Items[8].SubItems[1].Text = "到位";

                  }
                  else
                  {
                      listView1.Items[8].SubItems[1].Text = "不到位";

                  }


                  if (this_lift.lift.Get_Passenger_Status())
                  {
                      listView1.Items[9].SubItems[1].Text = "有人";

                  }
                  else
                  {
                      listView1.Items[9].SubItems[1].Text = "没人";

                  }

                  listView1.Items[10].SubItems[1].Text = this_lift.lift.Get_Total_Running_Time().ToString() + "小时";
                  listView1.Items[11].SubItems[1].Text = this_lift.lift.Get_Present_Counter_Value().ToString() + "次";

                  textBox2.Text = this_lift.lift.Get_Time_Stamps();
                  String MessageCode = String.Empty;
                  List<BACnetMessageCode> message_list = new List<BACnetMessageCode>();
                  message_list = this_lift.lift.Get_Message_Code();
                  for(int i=0;i<message_list.Count;i++)
                  {
                      MessageCode += message_list[i].ToString() + ",";
                  }
                  textBox3.Text = MessageCode;
                  if (this_lift.lift.GetAlaram_Status())
                  {
                     
                      textBox4.Text = "正在报警";
                      textBox4.BackColor = System.Drawing.Color.Red;

                  }
                  else
                  {

                      textBox4.Text = "未报警"; 
                      textBox4.BackColor = System.Drawing.Color.Gray;
                      
                  }
                  List<Property_TimeStamps> timelist=this_lift.lift.Get_Property_Time_List();

                  for(int i=0;i<timelist.Count;i++)
                  {
                      Property_TimeStamps temp = timelist[i];
                      switch(temp.poperty_id)
                      {
                          case BACNET_PROPERTY_ID.PROP_Identification_Number:
                              {
                                  listView1.Items[2].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                          case BACNET_PROPERTY_ID.PROP_Service_Mode:
                              {
                                  listView1.Items[3].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                          case BACNET_PROPERTY_ID.PROP_Car_Status:
                              {
                                  listView1.Items[4].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                          case BACNET_PROPERTY_ID.PROP_Car_Direction:
                              {
                                  listView1.Items[5].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                          case BACNET_PROPERTY_ID.PROP_Door_Zone:
                              {
                                  listView1.Items[6].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                          case BACNET_PROPERTY_ID.PROP_Car_Position:
                              {
                                  listView1.Items[7].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                          case BACNET_PROPERTY_ID.PROP_DOOR_STATUS:
                              {
                                  listView1.Items[8].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                          case BACNET_PROPERTY_ID.PROP_Passenger_Status:
                              {
                                  listView1.Items[9].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                          case BACNET_PROPERTY_ID.PROP_Total_Running_Time:
                              {
                                  listView1.Items[10].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                          case BACNET_PROPERTY_ID.PROP_Present_Counter_Value:
                              {
                                  listView1.Items[11].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                          case BACNET_PROPERTY_ID.PROP_LIFT_Message_Code:
                              {
                                  listView1.Items[12].SubItems[2].Text = this_lift.lift.Get_Time_Stamps(ref temp.time);
                                  break;
                              }
                      }
                  }
             
                                }
          }

          private void button1_Click(object sender, EventArgs e)
          {
              UdpSender udpsend = new UdpSender(1476);
              udpsend.Send_ReadProperty_Identifer(device_id);
              Device_Manager manager=Device_Manager.Get_Device_Manager();
              Thread.Sleep(300);
              foreach (Bacnet_Device temp in manager.device_list)
              {
                 
                  if (temp.device_object.Get_OBJECT_ID_Number() == device_id)
                      Form1.listview.Items[manager.device_list.IndexOf(temp)].SubItems[6].Text = manager.Get_Lift(device_id).lift.Get_Identification_Number();

              }

          }

          private void textBox1_TextChanged(object sender, EventArgs e)
          {

          }

          private void button2_Click(object sender, EventArgs e)
          {

              if (this_lift.monitored == false)
              {
                  UdpSender udpsend = new UdpSender(1476);
                  udpsend.Send_CovSubscribeService(device_id);
                
              }
                if (this_lift.monitored == true)
                {
                      UdpSender udpsend = new UdpSender(1476);
                      udpsend.Send_CovSubscribeService(device_id, true);
                    
                  }
          
              
            

          }

        

          private void button4_Click(object sender, EventArgs e)
          {
              UdpSender udpsend = new UdpSender(1476);
              udpsend.Send_Read_Property_Multi_Service(device_id);
          }

          private void textBox2_TextChanged(object sender, EventArgs e)
          {

          }

          private void label2_Click(object sender, EventArgs e)
          {

          }

          private void button5_Click(object sender, EventArgs e)
          {
              UdpSender sen = new UdpSender(1476);
              sen.Send_Alarm_Ack(this_lift.device_object.Get_OBJECT_ID_Number());
          }

          private void listView1_SelectedIndexChanged(object sender, EventArgs e)
          {

          }

      
    }
}
