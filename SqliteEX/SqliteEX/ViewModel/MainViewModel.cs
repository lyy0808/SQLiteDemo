using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace SqliteEX.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        //数据库连接
        SQLiteConnection m_dbConnection;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            MyClick = new RelayCommand<string>(CreateDB);
            CreateTable = new RelayCommand(UseSqliteHelperCreateTable);
            InsertTable = new RelayCommand(UseSqliteHelperInsertTable);
            SelectTable = new RelayCommand(UseSqliteHelperSelectTable);
            UpdataTable = new RelayCommand(UseSqliteHelperUpdataTable);
        }
        public RelayCommand<string> MyClick { get; set; }
        public RelayCommand CreateTable { get; set; }

        public RelayCommand InsertTable { get; set; }
        public RelayCommand SelectTable { get; set; }

        public RelayCommand UpdataTable { get; set; }

        private void UseSqliteHelperUpdataTable()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    Dictionary<string, object> dicData = new Dictionary<string, object>();
                    dicData["ID"] = "1002";
                    dicData["TimeAT"] = DateTime.Now.AddDays(1);

                    byte[] cha = BitConverter.GetBytes(257414.2);
                    dicData["CHA"] = cha;
                    sh.Update("MyData", dicData, "ID", 1001);
                    conn.Close();
                }
            }

        }
        private void UseSqliteHelperSelectTable()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    string strSelct = "Select * from MyData";
                    DataTable dt = sh.Select(strSelct);
                    foreach (DataRow row in dt.Rows)
                    {
                        Console.WriteLine("{0},{1},{2}",row["ID"],row["TimeAt"],BitConverter.ToDouble((byte[])row["CHA"],0));                       
                    }
                    conn.Close();
                }
            }

        }
        private void UseSqliteHelperInsertTable()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    var dic = new Dictionary<string, object>();
                    dic["ID"] = "1001";
                    dic["TimeAT"] = DateTime.Now;

                    byte[] cha = BitConverter.GetBytes(250.1);
                    dic["CHA"] = cha;
                    sh.Insert("MyData", dic);

                    conn.Close();
                }
            }
        }

        private void UseSqliteHelperCreateTable()
        {
            string SqlitePath = AppDomain.CurrentDomain.BaseDirectory + "MyDatabase.sqlite";
            if (!File.Exists(SqlitePath))
            {
                SQLiteConnection.CreateFile("MyDatabase.sqlite");

            }

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;"))
            {
                SQLiteTable sqTable = new SQLiteTable("MyData");
                sqTable.Columns.Add(new SQLiteColumn("ID", true));
                sqTable.Columns.Add(new SQLiteColumn("TimeAT", ColType.DateTime));
                sqTable.Columns.Add(new SQLiteColumn("CHA", ColType.BLOB));
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    sh.CreateTable(sqTable);

                    conn.Close();
                }
            }

        }

        #region OrgionFunction


        private void CreateDB(string str)
        {
            createNewDatabase();
            connectToDatabase();
            createTable();
            fillTable();
            printHighscores();

        }
        //创建一个空的数据库
        void createNewDatabase()
        {
            SQLiteConnection.CreateFile("MyDatabase.sqlite");
        }
        //创建一个连接到指定数据库
        void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
        }
        //在指定数据库中创建一个table
        void createTable()
        {
            string sql = "create table highscores (name varchar(20), score int)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }
        //插入一些数据
        void fillTable()
        {
            string sql = "insert into highscores (name, score) values ('Me', 3000)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into highscores (name, score) values ('Myself', 6000)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into highscores (name, score) values ('And I', 9001)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        DataTable dt = new DataTable();
        //使用sql查询语句，并显示结果
        void printHighscores()
        {
            string sql = "select * from highscores order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
            Console.ReadLine();


            SQLiteDataAdapter slda = new SQLiteDataAdapter(sql, m_dbConnection);
            DataSet ds = new DataSet();
            slda.Fill(ds);
            dt = ds.Tables[0];

        }
        #endregion

    }
}