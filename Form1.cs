using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace Bacnet_Test_Final
{
    public partial class Form1 : Form
    {
        Device_Manager manager;
        
        private int choice;
        public static List<Detail_Form_Lift> detail_list_Lift;
        public static List<Detail_Form_Escalators> detail_list_Escalators;
        public static ListView listview;
        public Form1()
        {
            InitializeComponent();
            manager = Device_Manager.Get_Device_Manager();
            detail_list_Lift = new List<Detail_Form_Lift>();
            detail_list_Escalators = new List<Detail_Form_Escalators>();
            listview = listView1;
            manager.Event_Handler = new Client_Event_Handler();
            Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();


        }
    


        private void button3_Click(object sender, EventArgs e)
        {

            Bacnet_Server server = new Bacnet_Server(47808,1476);
            Bacnet_Server.Set_Broadcast_Port(47809);
            button3.Enabled = false;
            Thread t1 = new Thread(new ThreadStart(server.Set));
            t1.Start();



        }

        private void button1_Click(object sender, EventArgs e)
        {
            clear_items();
            manager.device_list.Clear();
            UdpSender Udpsender = new UdpSender(1476);
            Udpsender.Send_WhoIsService();
            Thread.Sleep(1000);
            Show_Device_List();
        }
        private void Show_Device_List()
        {
        
        
            for (int i = 0; i < manager.device_list.Count; i++)
            {


                Bacnet_Device temp = manager.device_list[i];
                listView1.Items.Add(new ListViewItem(new string[] { temp.device_object.Get_OBJECT_ID_Number().ToString(), "", "", "", "","","","","" }));
                listView1.Items[i].SubItems[1].Text = temp.address.ToString();
                listView1.Items[i].SubItems[2].Text = temp.device_object.Max_APDU_Length_Accepted.ToString();
                listView1.Items[i].SubItems[3].Text = temp.device_object.Vendor_Identifier.ToString();
                listView1.Items[i].SubItems[4].Text = temp.device_object.Segmentation_Supported.ToString();
                listView1.Items[i].SubItems[7].Text = "否";


            }
        }

     
        private void clear_items()
        {
            for (int i = 0; i < manager.device_list.Count; i++)
                listView1.Items[0].Remove();
            detail_list_Lift.Clear();
            detail_list_Escalators.Clear();
        }
        private void clear()
        {
            clear_items();
            manager.Clear() ;
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {


                if (listView1.SelectedItems.Count > 0)
                {
                    choice = listView1.SelectedItems[0].Index;

                }
                Point point = this.PointToClient(listView1.PointToScreen(new Point(e.X, e.Y)));
                contextMenuStrip1.Show(this, point);
            }
        }

        private void 查看详情ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bacnet_Device detail_device = manager.device_list[choice];
            UInt32 id = detail_device.device_object.Object_Identifier.instance;

            if (!manager.Is_Exist(id))
            {

                UdpSender udp_send = new UdpSender(1476);
                udp_send.Send_ReadProperty_Object_List(manager.device_list[choice].device_object.Get_OBJECT_ID_Number());
                Thread.Sleep(300);
            }
           
            if (manager.Is_Lift(id))
            {
                Detail_Form_Lift form;
               


                form = new Detail_Form_Lift();
                form.device_id = id;
                
               for(int i=0;i<detail_list_Lift.Count;i++)
                  
               {
                   if (detail_list_Lift[i].device_id == id)
                       detail_list_Lift.RemoveAt(i);
               }

               
                detail_list_Lift.Add(form);
                form.Show();
                foreach(Bacnet_Device temp in manager.device_list)
                {
                    if (temp.device_object.Get_OBJECT_ID_Number() == id)
                        
                    {                      
                        Form1.listview.Items[manager.device_list.IndexOf(temp)].SubItems[5].Text = "电梯";
                              
                     };

                }
             
                   
   

            }
            else if (manager.Is_Escalators(id))
            {
                Detail_Form_Escalators form;


                form = new Detail_Form_Escalators();
                form.device_id = id;
                detail_list_Escalators.Add(form);
                foreach (Bacnet_Device temp in manager.Escalators_list)
                {
                    if (temp.device_object.Get_OBJECT_ID_Number() == id)
                        Form1.listview.Items[manager.device_list.IndexOf(temp)].SubItems[5].Text = "扶梯";

                }

                form.Show();
            }
            else
                MessageBox.Show("无法查看该设备详细");
            
            

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Detail_Form_Lift lift in detail_list_Lift)
            {
                if (lift.device_id == manager.device_list[choice].device_object.Object_Identifier.instance)
                    detail_list_Lift.Remove(lift);

            };

            foreach (Detail_Form_Escalators Escalators in detail_list_Escalators)
            {
                if (Escalators.device_id == manager.device_list[choice].device_object.Object_Identifier.instance)
                    detail_list_Escalators.Remove(Escalators);

            };
            manager.device_list.RemoveAt(choice);
            listView1.Items[choice].Remove();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Forwar_Form f = new Forwar_Form();
            f.Show();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Record_Update();
        }

        private void Record_Update()
        {
            String Record = String.Empty;
            for(int i=0;i<BacnetRecord.inforamtion_list.Count;i++)
            {
                String temp=String.Empty;
                if(BacnetRecord.inforamtion_list[i].send==true)
                {
                    temp += "发送 ";
                }
                else
                    temp += "收到 ";
               if(BacnetRecord.inforamtion_list[i].pdu_type==BACNET_PDU_TYPE.PDU_TYPE_UNCONFIRMED_SERVICE_REQUEST)
               {
                  temp += BacnetRecord.inforamtion_list[i].uncon_type.ToString();
                }
                 if(BacnetRecord.inforamtion_list[i].pdu_type==BACNET_PDU_TYPE.PDU_TYPE_COMPLEX_ACK)
                 {
                     temp += BacnetRecord.inforamtion_list[i].cnf_type.ToString();
                 }
                 if (BacnetRecord.inforamtion_list[i].pdu_type == BACNET_PDU_TYPE.PDU_TYPE_CONFIRMED_SERVICE_REQUEST)
                 {
                     temp += BacnetRecord.inforamtion_list[i].cnf_type.ToString();
                 }

                 temp += "   时间: ";
                 temp += BacnetRecord.inforamtion_list[i].time.ToString();
                 if (BacnetRecord.inforamtion_list[i].device_id!=UInt32.MaxValue)
                 {
                     temp += " 设备标识符：" + BacnetRecord.inforamtion_list[i].device_id.ToString();
                 }
                 temp += "\r\n";
                 Record += temp;

            }
            textBox1.Text = Record;
        

        }













    }
}
