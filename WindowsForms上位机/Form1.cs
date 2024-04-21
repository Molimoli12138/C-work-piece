using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms上位机
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Multiline = true;
            double k = 0.001,m=k;
            for (int i=0;i<10;i++)
            {
                comboBox1.Items.Add("COM" + i.ToString());
            }
            for (int i = 0; i < 7; i++)
            {
                m = m * 10;
                comboBox4.Items.Add("x" + m.ToString());
            }
            for (int i = 0; i < 7; i++)
            {
                k = k * 10;
                comboBox5.Items.Add("x" + k.ToString());
            }
            comboBox1.Text = "COM1";//串口号多额默认值
            comboBox2.Text = "9600";//波特率默认值
            comboBox6.Text = "8";
            comboBox7.Text = "1";
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(SerialPort1_DataReceived); 
        }

        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int m = 3;
            string[] arr = new string[7];
            string s = "010101";
            string v = "123456";
            for (int i=0;i<7;i++)
            {
                arr[i] = (0.01 * Math.Pow(10, i)).ToString();
            }
            

            if (serialPort1.IsOpen)
            {
                try
                {
                    if (comboBox3.Text == "三角波")    //检测选择的波形
                    {
                        s = "01";
                    }
                    else if (comboBox3.Text == "方波")
                    {
                        s = "02";
                    }
                    else if (comboBox3.Text == "正弦波")
                    {
                        s = "03";
                    }

                    for (int i = 0; i < 7; i++)      //检测选择的幅度倍数
                    {
                        if (comboBox4.Text == "x" + arr[i])
                        {
                            s = s + "0" + i.ToString();
                            break;
                        }
                    }

                    for (int i = 0; i < 7; i++)            //检测选择的频率倍数
                    {
                        if (comboBox5.Text == "x" + arr[i])
                        {
                            s = s + "0" + i.ToString();
                            break;
                        }
                    }

                    {
                        for (int i = 0; i < 6; i++)                   //发出前对数据校验，若超出协议则强制关闭串口
                        {
                            if (s[1] == v[i] | s[3] == v[i] | s[5] == v[i])
                            {
                                m = m - 1;
                            }
                        }
                        if (s[0] != '0' | s[2] != '0' | s[4] != '0' | m != 0)
                        {
                            MessageBox.Show("数据错误，请重试", "错误");
                            serialPort1.Close();
                            return;
                        }
                    }

                    {
                        try                                    //写入异常则强制关闭串口
                        {
                            serialPort1.WriteLine(s);
                        }
                        catch
                        {
                            MessageBox.Show("数据写入错误，请重试", "错误");
                            serialPort1.Close();
                            return;
                        }
                    }


                        if (comboBox3.Text == "三角波")      //写入成功后在文本框中显示信息
                        {
                            textBox1.AppendText("三角波 ");
                        }
                        else if (comboBox3.Text == "正弦波")
                        {
                            textBox1.AppendText("正弦波 ");
                        }
                        else if (comboBox3.Text == "方波")
                        {
                            textBox1.AppendText("方波 ");
                        }
                    

                    for (int i = 0; i < 7; i++)
                    {
                        if(comboBox4.Text == "x"+arr[i])
                        {
                            textBox1.AppendText("频率倍数为"+arr[i]+" ");
                        }
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        if (comboBox5.Text == "x" + arr[i])
                        {
                            textBox1.AppendText("幅度倍数为" + arr[i] + " 发送成功\r\n");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("发送出错", "错误");
                }
                }
            else
            {
                MessageBox.Show("发送失败，端口未开启");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)            //打开串口按下并检测
        {
            try                                                           //尝试操作，若出现异常则关闭串口
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text, 10);//波特率设置
                serialPort1.DataBits = Convert.ToInt32(comboBox6.Text, 10);//数据位设置
                if (comboBox6.Text == "1")                                 //停止位设置
                {
                    serialPort1.StopBits = StopBits.One;
                }
                else if (comboBox6.Text =="2")
                {
                    serialPort1.StopBits = StopBits.Two;
                }
                else
                {
                    serialPort1.StopBits = StopBits.OnePointFive;
                }
                serialPort1.Open();
                button3.Enabled = true;//打开串口按钮不可用
                button2.Enabled = false;//关闭串口
            }
            catch
            {
                MessageBox.Show("端口错误,请检查串口", "错误");
            }
    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button2.Enabled = true;
            serialPort1.Close();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
