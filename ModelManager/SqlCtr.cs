using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelManager
{
    class SqlCtr
    {   //连接数据库
        public MySqlConnection SqlConn()
        {
            string connStr = @"server=localhost;port=3306;user=root;password=123qweasd; database=vs_database;";
            MySqlConnection conn = new MySqlConnection(connStr);
            return conn;
        }

        //增加一条数据
        public void SqlAdd(People people)
        {
            MySqlConnection conn = this.SqlConn();
            try
            {
                //将People添加进数据库
                string sql = "insert into model_info (name, card_id,card_type,gender,pic,address,contact,race,SN) values ('" + people.name + "','" + people.card_id + "','" + people.card_type + "','" + people.gender + "','" + people.pic + "','" + people.address + "','" + people.contact + "','" + people.race + "','" + people.SN + "');";

                MySqlCommand cmd = new MySqlCommand(sql, conn);  // 使conn的连接对象执行sql命令
                conn.Open();
                int count = cmd.ExecuteNonQuery();
            }
            catch (MySqlException exe)
            {
                Console.WriteLine(exe.Message);//有错则报出错误
            }
            finally
            {
                conn.Close();
            }


        }
        /// <summary>
        /// 查询数据库所有信息
        /// </summary>
        /// <returns>返回一个封装好的数据，可用作datagriviews中</returns>
        public DataSet SqlFindAll()
        { MySqlConnection conn = this.SqlConn();
            DataSet ds = new DataSet();
            try
            {

                conn.Open();//建立连接，可能出现异常,使用try catch语句
                string SqlStr = "select id,name,card_type,card_id,gender,address,contact,race,SN from model_info";
                MySqlCommand cmd = new MySqlCommand(SqlStr, conn);
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);//将返回的数据封装进adaper              
                adap.Fill(ds);

            }
            catch (MySqlException exe)
            {
                Console.WriteLine(exe.Message);//有错则报出错误

            }
            finally
            {
                conn.Close();//关闭通道
            }
            return ds;
        }


            //查单个
        public DataSet SqlFindByName(string name)
        {
            MySqlConnection conn = this.SqlConn();
            DataSet ds = new DataSet();
            try
            {

                conn.Open();//建立连接，可能出现异常,使用try catch语句
                string SqlStr = "select id,name,card_type,card_id,gender,address,contact,race,SN from model_info where name = '"+name+"' ";
                MySqlCommand cmd = new MySqlCommand(SqlStr, conn);
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);//将返回的数据封装进adaper              
                adap.Fill(ds);

            }
            catch (MySqlException exe)
            {
                Console.WriteLine(exe.Message);//有错则报出错误

            }
            finally
            {
                conn.Close();//关闭通道
            }
            return ds;
        }

        public void DelById(int id)
        {

        }



    }
}
