using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Data.SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DatabaseProgram
{
    private static string databasePath =  "D:\\QcUsers\\Administrator\\mydb.db";
    public static void DatabaseMain()
    {
        string databasePath =  "Data Source=D:\\QcUsers\\Administrator\\mydb.db";
        using (var connection = new SQLiteConnection(databasePath))
        {
            connection.Open();
            var command = new SQLiteCommand("SELECT * FROM COMPANY", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    var name = reader.GetString(1);
                    var age = reader.GetInt32(2);
                    var address = reader.GetString(3);
                    var salary = reader.GetFloat(4);
                    Console.WriteLine(id);
                    Console.WriteLine(name);
                    Console.WriteLine(age);
                    Console.WriteLine(address);
                    Console.WriteLine(salary);
                }
            }
        }
    }
    
    public static void WriteData(string name,int score)
    {
        using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;Charset=utf8;"))
        {
            connection.Open();

            // 创建并执行插入数据的 SQL 命令
            
            // string insertSql = "INSERT INTO COMPANY (Name, AGE) VALUES (@name, @age)";
            string insertSql = "INSERT OR REPLACE INTO SCORE (NAME, SCORE) VALUES (@name, @score)";
            using (var command = new SQLiteCommand(insertSql, connection))
            {
                
                // 添加参数
                // var nameBytes = Encoding.ASCII.GetBytes(name);
                // var nameUTF8 = Encoding.UTF8.GetString(nameBytes);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@score", score);
                Console.WriteLine(name);
                // Console.WriteLine(nameUTF8);
                // 执行插入命令
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} rows inserted.");
            }
        }
    }
    
    public static JObject Top10()
    {
        string databasePath = "D:\\QcUsers\\Administrator\\mydb.db";

        using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;Charset=utf8;"))
        {
            connection.Open();

            // 创建并执行按年龄从大到小排序的 SQL 命令
            // string selectSql = "SELECT Name, Age FROM MyTable ORDER BY Age DESC LIMIT 10";
            string selectSql = "SELECT * FROM SCORE ORDER BY SCORE DESC LIMIT 10";
            // Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            // {
            //     {"name", name},
            //     {"score", score}
            // };
            //
            // string json = JsonConvert.SerializeObject(keyValuePairs);
            JObject jsonObject = new JObject();
            int count = 1;
            using (var command = new SQLiteCommand(selectSql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int score = reader.GetInt32(2);
                        JObject p = new JObject {["name"] = name, ["score"] = score};
                        jsonObject[count.ToString()] = p;
                        count++;
                        // var json=JsonConvert.SerializeObject(p);
                        // Console.WriteLine($"Id: {id},name: {name}, score: {score}");
                    }
                }
            }

            return jsonObject;
        }
    }
}