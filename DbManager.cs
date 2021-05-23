using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using Mono.Data.Sqlite;

namespace PhoneDirectory
{
    public static class DbManager
    {
        public static SqliteConnection start()
        {
            /*
             * create a database connection to the SQLite database.
             * :param dbname: a Sqlite dbname to connect.
             * :return: void
             */

            if (!File.Exists("PhoneBook.db"))
            {
                SqliteConnection.CreateFile("PhoneBook.db");
            }
            SqliteConnection conn = new SqliteConnection("Data Source=PhoneBook.db; Version=3;");
            conn.Open();
            string sql = "CREATE TABLE IF NOT EXISTS 'MAINTABLE'(" +
                "cid INTEGER PRIMARY KEY AUTOINCREMENT," +
                "fullname TEXT NOT NULL," +
                "nickname TEXT," +
                "phoneticName TEXT," +
                "gender VARCHAR(1) NOT NULL," +
                "address TEXT NOT NULL," +
                "company TEXT," +
                "post TEXT," +
                "photo TEXT," +
                "phones TEXT," +
                "mails TEXT," +
                "lastupdateon TIMESTAMP DEFAULT CURRENT_TIMESTAMP" +
                ")";
            SqliteCommand comm = new SqliteCommand(sql, conn);
            comm.ExecuteNonQuery();
            conn.Close();
            return conn;
        }
        public static DataTable sqli(SqliteConnection conn, string sql)
        {
            /*
             * runs regular sql statement
             * :param conn: a Sqlite connection object
             * :param sql: a SQL statement
             * :return: DataTable with Selected rows IF sql is 'SELECT' statement, Empty DataTable.
             */

            bool isSelect = false;
            char[] s = { ';' };
            string[] qry = sql.Split(s, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in qry)
            {
                if (item.Trim().StartsWith("select", StringComparison.OrdinalIgnoreCase))
                {
                    isSelect = true;
                    break;
                }
            }
            conn.Open();
            SqliteCommand comm = new SqliteCommand(sql, conn);
            DataTable dt = new DataTable();
            if (!isSelect)
            {
                comm.ExecuteNonQuery();
                conn.Close();
                return dt;
            }
            SqliteDataAdapter adapter = new SqliteDataAdapter(comm);
            adapter.Fill(dt);
            conn.Close();
            return dt;
        }
        public static DataTable psqli(SqliteConnection conn, string sql, ArrayList param)
        {
            /*
             * runs prepared sql statement
             * :param conn: a Sqlite connection object
             * :param sql: a SQL statement
             * :param param: collection of parameters to prepare
             * :return: DataTable with Selected rows IF sql is 'SELECT' statement, Empty DataTable.
             */

            bool isSelect = false;
            char[] s = { ';' };
            string[] qry = sql.Split(s, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in qry)
            {
                if (item.Trim().StartsWith("select", StringComparison.OrdinalIgnoreCase))
                {
                    isSelect = true;
                    break;
                }
            }
            conn.Open();
            SqliteCommand comm = new SqliteCommand(sql, conn);
            foreach (var item in param)
            {
                SqliteParameter prm = new SqliteParameter();
                prm.Value = item;
                comm.Parameters.Add(prm);
            }
            SqliteParameter pr = new SqliteParameter();
            comm.Prepare();
            DataTable dt = new DataTable();
            if (!isSelect)
            {
                comm.ExecuteNonQuery();
                conn.Close();
                return dt;
            }
            SqliteDataAdapter adapter = new SqliteDataAdapter(comm);
            adapter.Fill(dt);
            conn.Close();
            return dt;
        }
        public static void Add_update_contact(string name, string nickname, string phoneticname, char gen, string address, string company, string post, string photo, string phones, string mails, int id=-1)
        {
            SqliteConnection conn = start();
            string sql = "CREATE TABLE IF NOT EXISTS 'MAINTABLE'(" +
            	"cid INTEGER PRIMARY KEY AUTOINCREMENT," +
                "fullname TEXT NOT NULL," +
                "nickname TEXT," +
                "phoneticName TEXT," +
                "gender VARCHAR(1) NOT NULL," +
                "address TEXT NOT NULL," +
                "company TEXT," +
                "post TEXT," +
                "photo TEXT," +
                "phones TEXT," +
                "mails TEXT," +
                "lastupdateon TIMESTAMP DEFAULT CURRENT_TIMESTAMP" +
            	")";
            sqli(conn, sql);
            if (id == -1)
            {
                sql = "INSERT INTO MAINTABLE(fullname, nickname, phoneticName, gender, address, company, post, photo, phones, mails) VALUES(?,?,?,?,?,?,?,?,?,?)";
                ArrayList param = new ArrayList();
                string gender = gen.ToString();
                param.Add(name);
                param.Add(nickname);
                param.Add(phoneticname);
                param.Add(gender);
                param.Add(address);
                param.Add(company);
                param.Add(post);
                param.Add(photo);
                param.Add(phones);
                param.Add(mails);
                psqli(conn, sql, param);
            }
            else
            {
                sql = "UPDATE MAINTABLE SET fullname=?, nickname=?, phoneticName=?, gender=?, address=?, company=?, post=?, photo=?, phones=?, mails=?, lastupdateon=CURRENT_TIMESTAMP WHERE cid=?";
                ArrayList param = new ArrayList();
                string gender = gen.ToString();
                param.Add(name);
                param.Add(nickname);
                param.Add(phoneticname);
                param.Add(gender);
                param.Add(address);
                param.Add(company);
                param.Add(post);
                param.Add(photo);
                param.Add(phones);
                param.Add(mails);
                param.Add(id);
                psqli(conn, sql, param);
            }
        }
        public static ArrayList Get_all_contacts()
        {
            SqliteConnection conn = start();
            string sql = "SELECT cid,fullname,gender,photo FROM MAINTABLE ORDER BY cid ASC";
            DataTable dt = sqli(conn, sql);
            ArrayList al = new ArrayList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TableCell sn = new TableCell();
                TableCell cid = new TableCell();
                TableCell fullname = new TableCell();
                TableCell gender = new TableCell();
                TableCell photo = new TableCell();
                TableRow tr = new TableRow();
                sn.Text = (i+1).ToString();
                tr.Cells.Add(sn);
                cid.Text = dt.Rows[i]["cid"].ToString();
                tr.Cells.Add(cid);
                string ps = dt.Rows[i]["photo"].ToString();
                char gs = char.Parse(dt.Rows[i]["gender"].ToString());
                if (ps == null || ps == "")
                    if (gs.Equals('F'))
                        photo.Text = "<img id='imgpreview' runat='server' src='/Statics/photos/female.png' alt='Photo' style='width: 40px; height: 40px; border-radius: 20px;'/>";
                    else
                        photo.Text = "<img id='imgpreview' runat='server' src='/Statics/photos/male.png' alt='Photo' style='width: 40px; height: 40px; border-radius: 20px;'/>";
                else
                    photo.Text = "<img id='imgpreview' runat='server' src='" + ps + "' alt='Photo' style='width: 40px; height: 40px; border-radius: 20px;'/>";
                tr.Cells.Add(photo);
                fullname.Text = dt.Rows[i]["fullname"].ToString();
                tr.Cells.Add(fullname);
                al.Add(tr);
            }
            return al;
        }
        public static Dictionary<string, string> selectContact(string id)
        {
            SqliteConnection conn = start();
            string sql = "SELECT * FROM MAINTABLE WHERE cid='" + id + "'";
            DataTable dt = sqli(conn, sql);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if(dt.Rows.Count == 0)
            {
                return dict;
            }
            DataRow dr = dt.Rows[0];
            dict.Add("id", dr["cid"].ToString());
            string ps = dr["photo"].ToString();
            char gs = char.Parse(dr["gender"].ToString());
            if (ps == null || ps == "")
                if (gs.Equals('F'))
                    dict.Add("imglink", "/Statics/photos/female.png");
                else
                    dict.Add("imglink", "/Statics/photos/male.png");
            else
                dict.Add("imglink", ps);
            if (gs.Equals('F'))
                dict.Add("gender", "Female");
            else if (gs.Equals('M'))
                dict.Add("gender", "Male");
            else
                dict.Add("gender", "Others");
            dict.Add("fullname", dr["fullname"].ToString());
            dict.Add("nickname", dr["nickname"].ToString());
            dict.Add("phoneticName", dr["phoneticName"].ToString());
            dict.Add("address", dr["address"].ToString());
            dict.Add("company", dr["company"].ToString());
            dict.Add("post", dr["post"].ToString());
            dict.Add("phones", dr["phones"].ToString());
            dict.Add("mails", dr["mails"].ToString());
            dict.Add("lastupdateon", dr["lastupdateon"].ToString());
            return dict;
        }
    }
}