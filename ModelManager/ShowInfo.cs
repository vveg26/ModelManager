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
            this.dataGridView1.DataSource=sql.SqlFindAll().Tables[0].DefaultView;//存在图片流无法显示的问题
        }
    }
}
