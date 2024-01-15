using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

public class SimpleHttpServer
{
    public static void Main(string[] args)
    {
        // SocketProgram.SocketMain();
        // DatabaseProgram.DatabaseMain();
        // DatabaseProgram.Top10();
        string ip = "127.0.0.1";
        int port = 8080;

        HttpListener listener = new HttpListener();
        listener.Prefixes.Add($"http://{ip}:{port}/");

        listener.Start();
        Console.WriteLine($"服务器已启动，监听地址：http://{ip}:{port}/");
        string path = Assembly.GetEntryAssembly().Location;
        var dir = Path.GetDirectoryName(path);
        Console.WriteLine("当前程序路径：" + dir);
//测试链接http://127.0.0.1:8080/?name=Marko&score=5555

        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            var url = context.Request.Url.ToString();
            if (url.EndsWith("favicon.ico"))
            {
                continue;
            }

            Console.WriteLine(context.Request.Url.ToString());
            var name = GetQueryStringValue(context.Request.QueryString, "name");
            var scoreStr = GetQueryStringValue(context.Request.QueryString, "score");
            name = Uri.UnescapeDataString(name);

            int.TryParse(scoreStr, out int score);
            DatabaseProgram.WriteData(name, score);
            JObject top10 = DatabaseProgram.Top10();
            // Console.WriteLine("前十：" + top10.ToString());
            string responseString = "Top10:\n" + top10.ToString();
            Console.WriteLine(responseString);
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
            // Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            // {
            //     {"name", name},
            //     {"score", score}
            // };
            //
            // string json = JsonConvert.SerializeObject(keyValuePairs);
            // File.WriteAllText(Path.Combine(dir, "http.txt"), json);
        }
    }

    private static string GetQueryStringValue(System.Collections.Specialized.NameValueCollection queryString, string key)
    {
        if (queryString.HasKeys() && queryString.Get(key) != null)
        {
            return queryString.Get(key);
        }

        return "";
    }
}