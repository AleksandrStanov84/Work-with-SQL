using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace ConsoleApplication15
{
    class Program
    {
        SqlConnection conn;
        public Program()
        {
            conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

            //conn = null;
            //conn = new SqlConnection();
            //conn.ConnectionString = @"Data Source=(localdb)\v11.0;
            //Initial Catalog=Library; Integrated Security=SSPI;";
            //или
            //SqlConnection conn = null;
            //conn = new SqlConnection(@"Data Source=(localdb)\v11.0;
            //Initial Catalog=Library; Integrated Security=SSPI;");
        }
        static void Main(string[] args)
        {
            Program pr = new Program();
            //pr.InsertQuery();
            pr.CleverRead();
        }
        public void InsertQuery()
        {
            try
            {
                //открыть соединение
                conn.Open();
                //подготовить запрос insert
                //в переменной типа string
                string insertString = @"insert into
                Authors (FirstName, LastName)
                values ('Roger', 'Zelazny')";
                //создать объект command,
                //инициализировав оба свойства
                SqlCommand cmd =
                new SqlCommand(insertString, conn);

                //выполнить запрос, занесенный
                //в объект command
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void CleverRead()
        {
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@p1"; //сопоставление с параметром
                                              //в запросе
                param1.SqlDbType = System.Data.SqlDbType.NVarChar;
                //тип параметра
                param1.Value = "Roger"; //значение параметра
                string sql = @"select * from Authors where FirstName = @p1";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(param1);
                //cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = "Roger";
                rdr = cmd.ExecuteReader();
                int line = 0; //счетчик строк
                //извлечь полученные строки
                while (rdr.Read())
                {
                    if (line == 0)
                    {
                        //цикл по числу прочитанных полей
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            //вывести в консольное окно имена полей
                            Console.Write(rdr.GetName(i).
                            ToString() + " ");
                        }
                    }
                    Console.WriteLine();
                    line++;
                    Console.WriteLine(rdr[1] + " " + rdr[2]);

                }
                Console.WriteLine("Обработано записей: " +
                   line.ToString());
            }
            finally
            {
                //закрыть reader
                if (rdr != null)
                {
                    rdr.Close();
                }
                //закрыть соединение
                if (conn != null)
                {
                    conn.Close();
                }
            }

        }
        public void ReadData()
        {
            SqlDataReader rdr = null;
            try
            {
                //открыть соединение
                conn.Open();
                //создать новый объект command с запросом select
                SqlCommand cmd = new SqlCommand("select * from Authors", conn);
                //выполнить запрос select, сохранив
                //возвращенный результат
                rdr = cmd.ExecuteReader();
                int line = 0; //счетчик строк
                //извлечь полученные строки
                while (rdr.Read())
                {
                    if (line == 0)
                    {
                        //цикл по числу прочитанных полей
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            //вывести в консольное окно имена полей
                            Console.Write(rdr.GetName(i).
                            ToString() + " ");
                        }
                    }
                    Console.WriteLine();
                    line++;
                    Console.WriteLine(rdr[1] + " " + rdr[2]);

                }
                Console.WriteLine("Обработано записей: " +
                   line.ToString());
            }
            finally
            {
                //закрыть reader
                if (rdr != null)
                {
                    rdr.Close();
                }
                //закрыть соединение
                if (conn != null)
                {
                    conn.Close();
                }
            }

        }
        public void ReadData2()
        {
            SqlDataReader rdr = null;
            try
            {
                //Open the connection
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Authors; select* from Books", conn);
                rdr = cmd.ExecuteReader();
                int line = 0;
                //извлечь полученные строки
                do
                {
                    while (rdr.Read())
                    {
                        if (line == 0) //формируем шапку
                                       //таблицы перед выводом первой строки
                        {
                            //цикл по числу прочитанных полей
                            for (int i = 0; i < rdr.
                             FieldCount; i++)
                            {
                                //вывести в консольное окно
                                //имена полей
                                Console.Write(rdr.GetName(i).
                                ToString() + "\t");
                            }
                            Console.WriteLine();
                        }
                        line++;
                        Console.WriteLine(rdr[0] + "\t" +
                        rdr[1] + "\t" + rdr[2]);
                    }

                    Console.WriteLine("Total records processed: " + line.ToString());
                } while (rdr.NextResult());
            }
            finally
            {
                //close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void ExecStoredProcedure()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("getBooksNumber",
            conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AuthorId", System.Data.
            SqlDbType.Int).Value = 1;
            SqlParameter outputParam = new SqlParameter("@BookCount", System.Data.SqlDbType.Int);

            outputParam.Direction = ParameterDirection.
            Output;
            //outputParam.Value = 0; //заполнять Value не надо!
            cmd.Parameters.Add(outputParam);
            cmd.ExecuteNonQuery();
            Console.WriteLine(cmd.Parameters["@BookCount"].
            Value.ToString());
        }
    }
}
