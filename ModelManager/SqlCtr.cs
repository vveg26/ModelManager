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
    {   //连接数据库,存在异常，若连接失败则会导致xxx
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
        //根据SN值来查询
        public bool SqlFindBySN(string SN)
        {
            MySqlConnection conn = this.SqlConn();
            bool flag = false;
            try
            {

                conn.Open();//建立连接，可能出现异常,使用try catch语句
                string SqlStr = "select id,name,card_type,card_id,gender,address,contact,race,SN from model_info where SN = '" + SN + "' ";
                MySqlCommand cmd = new MySqlCommand(SqlStr, conn);
                cmd.ExecuteNonQuery();//有返回结果说明存在SN
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);//将返回的数据封装进adaper
                DataSet ds = new DataSet();
                adap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    flag=false;
                }
                else
                {
                    flag = true;
                }

            }
            catch (MySqlException exe)
            {
                Console.WriteLine(exe.Message);//有错则报出错误

            }
            finally
            {
                
                conn.Close();//关闭通道
            }
            return flag;
        }

        //根据ID删除
        public void DelById(int id)
        {
            MySqlConnection conn = this.SqlConn();
            try
            {

                conn.Open();//建立连接，可能出现异常,使用try catch语句
                string SqlStr = "delete from model_info where id = "+id+" ";
                MySqlCommand cmd = new MySqlCommand(SqlStr, conn);
                cmd.ExecuteNonQuery();

            }
            catch (MySqlException exe)
            {
                Console.WriteLine(exe.Message);//有错则报出错误

            }
            finally
            {
                conn.Close();//关闭通道
            }
        }



    }
}
