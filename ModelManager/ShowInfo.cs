using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelManager
{
    public partial class ShowInfo : Form
    {
        public ShowInfo()
        {
            InitializeComponent();
        }

        private void ShowInfo_Load(object sender, EventArgs e)
        {
            SqlCtr sql = new SqlCtr();       
            //this.dataGridView1.ReadOnly = true;
            dataGridView1.AutoGenerateColumns = true;
            this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter; //单击
            this.dataGridView1.DataSource = sql.SqlFindAll().Tables[0].DefaultView;//存在图片流无法显示的问题
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            
        }
        //修改单元格就修改数据库
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            SqlCtr sql = new SqlCtr();
            MySqlConnection conn = sql.SqlConn();
            try
            {
                string strcolumn = dataGridView1.Columns[e.ColumnIndex].HeaderText;//获取列标题
                string strrow = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();//获取焦点触发行的第一个值
                string value = dataGridView1.CurrentCell.Value.ToString();//获取当前点击的活动单元格的值
                string strcomm = "update model_info set " + strcolumn + "='" + value + "'where id = " + strrow;
                //update FilTer set 列名 = value where id = 3
                conn.Open();
                this.label1.Text = strcomm;
                MySqlCommand comm = new MySqlCommand(strcomm, conn);
                comm.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
