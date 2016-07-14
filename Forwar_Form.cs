using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacnet_Test_Final
{
    public partial class Forwar_Form : Form
    {
        private int choice;
       static  int count;
        public Forwar_Form()
        {
            InitializeComponent();
            count = 0;
            Clear();
            show_address();
            if( Forwarder.Get_Status())
            {
                button2.Text = "关闭";
            }
            else
                button2.Text = "启动";
           
           
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Forward_Address address = new Forward_Address(textBox1.Text, textBox2.Text);
            Forwarder.Forwarder_List.Add(address);
            Clear();
            count++;
            show_address();
        }
        private void show_address()
        {
            for (int i = 0; i < Forwarder.Forwarder_List.Count; i++)
            {   
               Forward_Address temp= Forwarder.Forwarder_List[i];

               listView1.Items.Add(new ListViewItem(new string[] {temp.ID, temp.address.ToString()}));
            }
        }
        private void Clear()
        {
            if (count != 0)
            {


                for (int i = 0; i < Forwarder.Forwarder_List.Count; i++)
                    listView1.Items[0].Remove();
            }
        }
      

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {




             Forwarder.Forwarder_List.RemoveAt(choice);
             listView1.Items[choice].Remove();
             Clear();
             show_address();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (Forwarder.Get_Status())
            {
                Forwarder.Set_Status(false);
                button2.Text = "启动";
            }
            else
            {

                Forwarder.Set_Status(true);
                button2.Text = "关闭";
            }
        }

        private void Forwar_Form_MouseClick(object sender, MouseEventArgs e)
        {

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
    }
}
