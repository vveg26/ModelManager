using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelManager
{
    public partial class Form1 : Form
    {

        People people = new People();//定义一个模特类

        string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+"\\"+"Model"+"\\"+"info.txt";//保存模特信息的地址,需要存在
        string SNpath = "";//定义一个标识码地址  ，这个地址也要真实存在     


        public Form1()
        {
            InitializeComponent();

        }
        //保存模特信息
        private void btn_save_Click(object sender, EventArgs e)
        {   
            //TODO 查询模特的SN码，若存在，则获取SN码，若不存在，就保存到数据库中


            bool flag=TextBoxIsNull(this.panel_info,this.pic_pic);//判断是否为空

            if (flag)
            {
                SaveInfo(path);//保存至txt
                SqlCtr sqlCtr = new SqlCtr();
                sqlCtr.SqlAdd(people);//保存至mysql
                MessageBox.Show("successful");
            }
            else
            {
                MessageBox.Show("请完善所有信息");
            }
        }
        /// <summary>
        /// 判断TextBox和图片都填了
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="pictureBox"></param>
        /// <returns>true为非空</returns>
        private bool TextBoxIsNull(Panel panel,PictureBox pictureBox)
        {
            //判断所有txt都填写了
            bool flag = true;
            foreach (Control c in panel.Controls)
            {
                if (c is TextBox)
                {
                    if (string.IsNullOrEmpty((c as TextBox).Text))
                    {

                        flag = false;
                        break;
                    }
                }
            }
            if(pictureBox.Image == null) {
                flag = false;
            }

            return flag;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Init_UI();
        }
        //初始化界面UI
        private void Init_UI()
        {
            //下拉框
            comb_cardtype.Items.Add("身份证");
            comb_cardtype.Items.Add("护照");
            comb_cardtype.Items.Add("驾照");
            comb_cardtype.SelectedIndex = 0;
            //使能控制
            btn_getSN.Enabled = false;
            rabtn_wm.Checked = true;

        }

        /// <summary>
        /// 保存模特信息到指定txt文件中
        /// </summary>
        /// <param name="path">指定txt文件</param>
        private void SaveInfo(String path)
        {
            //封装进模特类
            people.card_id = txt_cardid.Text;
            people.name = txt_name.Text;
            people.card_type = comb_cardtype.SelectedIndex;
            if (rabtn_m.Checked) { people.gender = 1; } else { people.gender = 0; }   //判断是否为女性
            people.pic = GetImageData(pic_pic.Image);
            people.address = txt_address.Text;
            people.contact = txt_contact.Text;
            people.race = txt_race.Text;
            people.SN = people.card_id + people.gender;//随机设置的，改动,作为模特的总地址

            //将个人信息保存到txt文件
            string str = "姓名:" + people.name + "证件类型:" + people.card_type + "证件号:" + people.card_id + "证件图片长度:" + people.pic.Length + "地址:" + people.address + "联系方式:" + people.contact + "种族:" + people.race + "标识码:" + people.SN + "\r\n";//将信息组合成字符串            
            //如果文件不存在，就保存
            if (!File.Exists(path)) { using (System.IO.FileStream fs = System.IO.File.Create(path)) ; }//创建文件
            StreamWriter sw = File.AppendText(path);
            sw.Write(str);
            sw.Close();
            btn_getSN.Enabled = true;
        }

        //生成标识码路径
        private void btn_getSN_Click(object sender, EventArgs e)
        {
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+"\\"+"Model";//文件保存目录

            SNpath = dirPath + "\\" + people.SN;//标识码文件夹路径
            if (!Directory.Exists(SNpath))
                Directory.CreateDirectory(SNpath);
            txt_getSN.Text = SNpath;
        }
        //选择图片按钮
        private void btn_choosepic_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.pic_pic.Image = Image.FromFile(fileDialog.FileName);
            }
        }

        //处理图片的方法
        public byte[] GetImageData(Image imgPhoto)
        {
            //将Image转换成数据流，并保存为bety[]
            MemoryStream mStreanm = new MemoryStream();
            imgPhoto.Save(mStreanm, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] byteData = new Byte[mStreanm.Length];
            mStreanm.Position = 0;
            mStreanm.Read(byteData, 0, byteData.Length);
            mStreanm.Close();
            return byteData;
        }
        public Image ByteToImage(byte[] streamByte)
        {
            MemoryStream mStream = new MemoryStream(streamByte);
            return Image.FromStream(mStream);
        }
        //保存到json的按钮
        private void btn_save_json_Click(object sender, EventArgs e)
        {
            //组合成json

            string json = ToJson(panel_collection);

            //如果有模特标识码，写入到json文件中
            if (!(SNpath == ""))
            {
                string jsonPath = SNpath + "\\" + "json.json";
                if (!File.Exists(jsonPath)) { using (System.IO.FileStream fs = System.IO.File.Create(jsonPath)) ; }//createfile
                StreamWriter sw = File.AppendText(jsonPath);
                sw.Write(json);
                sw.Close();
            }
            else
            {
                MessageBox.Show("没有模特");
            }
        }
        /// <summary>
        /// 遍历panel中的groupbox中的radiobtn的值和对应的label的值，转换成json格式
        /// </summary>
        /// <param name="panel">板子，里面有groupbox和label</param>
        private string ToJson(Panel panel)
        {
            ArrayList arrys = new ArrayList();
            string json = "{";
            //遍历panel种的每一个组件
            foreach (Control groupbox in panel.Controls)
            {

                //如果是groupbox，则遍历groupbox的组件
                if (groupbox is GroupBox)
                {
                    string str_radbtn = " ";//radbtn值
                    string str_lbl = " ";//label值

                    // GroupBox grbox = groupbox as GroupBox;
                    foreach (Control con in groupbox.Controls)
                    {
                        //如果控件为lbl，则取出lbl的值
                        if (con is Label)
                        {
                            Label lbl = con as Label;
                            str_lbl = lbl.Text;
                        }
                        else if (con is RadioButton)
                        //如果控件为radbtn。取出选中的radbtn的值

                        {
                            RadioButton radbtn = con as RadioButton;
                            if (radbtn.Checked)
                            {
                                str_radbtn = radbtn.Text;
                                //组合字符串到数组中,key----value
                                string str = "\"" + str_lbl + "\"" + "" + ":" + "\"" + str_radbtn + "\"";
                                arrys.Add(str);
                            }
                        }




                    }
                }
            }
            //将字符串取出组合成json格式
            int count = 1;
            foreach (string arry in arrys)
            {
                if (count == arrys.Count)
                {
                    json += arry;
                }
                else
                {
                    json += arry + ",";
                    count++;
                }

            }
            json +=  "}";
            return json;


        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        private void ShowFrom(ShowInfo f2)
        {
            f2.Visible = false;//将当前窗口设置为不可视；如果不这样处理则系统报错。
            f2.ShowDialog();//打开窗口
        }

        private void btn_find_Click(object sender, EventArgs e)
        {
            ShowInfo f2 = new ShowInfo();
            ShowFrom(f2);
        }

        private void btn_update_Click(object sender, EventArgs e)
        {

        }
        //创建一个lbl
        //private void btn_create_line_Click(object sender, EventArgs e)
        //{
        //    Label lbl = new Label();//声明一个label
        //    lbl.Location = new System.Drawing.Point(5, 80);//设置位置
        //    lbl.Size = new Size(40, 20);//设置大小
        //    lbl.Text = txt_create_lal.Text;//设置Text值
        //    this.panel_collection.Controls.Add(lbl);//在当前窗体上添加这个label控件
        //}
    }
}
