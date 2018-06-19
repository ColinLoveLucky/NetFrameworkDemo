using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SqlServerDemo.SqlClient
{
    public class SqlClientStreamDemo
    {
        //SqlDataReader
        //DbDataReader
        //SqlParameter
        private const string _windowsConnection = "Server=172.16.37.67;integrated security=false;database=DbBooks;user Id=sa;password=Password@1;Asynchronous Processing=true";
        public async Task CopyBinaryToFile()
        {
            string filePath = "D:\\binary.bin";
            using (SqlConnection connection = new SqlConnection(_windowsConnection))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT [bindata] FROM [Streams] WHERE [id]=@id", connection))
                {
                    command.Parameters.AddWithValue("id", 2);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(System.Data.CommandBehavior.SequentialAccess))
                    {
                        if (await reader.ReadAsync())
                        {
                            if (!(await reader.IsDBNullAsync(0)))
                            {
                                using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    using (Stream data = reader.GetStream(0))
                                    {
                                        await data.CopyToAsync(file);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public async Task PrintTextValues()
        {
            using (SqlConnection connection = new SqlConnection(_windowsConnection))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT [id], [textdata] FROM [Streams]", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync(System.Data.CommandBehavior.SequentialAccess))
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine("{0}:", reader.GetInt32(0));
                            if (await reader.IsDBNullAsync(1))
                            {
                                Console.WriteLine("(NULL)");
                            }
                            else
                            {
                                char[] buffer = new char[4096];
                                int charsRead = 0;
                                using (TextReader data = reader.GetTextReader(1))
                                {
                                    do
                                    {
                                        charsRead = await data.ReadAsync(buffer, 0, buffer.Length);
                                        Console.WriteLine(buffer, 0, charsRead);
                                    } while (charsRead > 0);
                                }
                            }
                        }
                    }
                }
            }
        }
        public async Task PrintXmlValues()
        {
            using (SqlConnection connection = new SqlConnection(_windowsConnection))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT [id], [xmldata] FROM [Streams]", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync(System.Data.CommandBehavior.SequentialAccess))
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine("{0}:", reader.GetInt32(0));
                            if (await reader.IsDBNullAsync(1))
                                Console.WriteLine("(NUll)");
                            else
                            {
                                using (XmlReader xmlReader = reader.GetXmlReader(1))
                                {
                                    int depth = 1;
                                    while (xmlReader.Read())
                                    {
                                        switch (xmlReader.NodeType)
                                        {
                                            case XmlNodeType.Element:
                                                Console.WriteLine("{0}<{1}>", new string('\t', depth), xmlReader.Name);
                                                depth++;
                                                break;
                                            case XmlNodeType.Text:
                                                Console.WriteLine("{0}{1}", new string('\t', depth), xmlReader.Value);
                                                break;
                                            case XmlNodeType.EndElement:
                                                depth--;
                                                Console.WriteLine("{0}</{1}>", new string('\t', depth), xmlReader.Name);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public  void CreateDemoFiles()
        {
            Random rand = new Random();
            byte[] data = new byte[1024];
            rand.NextBytes(data);
            using (FileStream file = File.Open("binarydata.bin", FileMode.OpenOrCreate))
            {
                file.Write(data, 0, data.Length);
            }
            using (StreamWriter writer = new StreamWriter(File.Open("textdata.txt", FileMode.OpenOrCreate)))
            {
                writer.Write(Convert.ToBase64String(data));
            }
        }
        public async Task StreamBLOBToServer()
        {
            using (SqlConnection connection = new SqlConnection(_windowsConnection))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("INSERT INTO [BinaryStreams] (bindata) VALUES (@bindata)", connection))
                {
                    using (FileStream file = File.Open("binarydata.bin", FileMode.Open))
                    {
                        command.Parameters.Add("bindata", SqlDbType.Binary).Value = file;

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }
        public async Task StreamTextToServer()
        {
            using (SqlConnection connection = new SqlConnection(_windowsConnection))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("INSERT INTO [TextStreams] (textdata) VALUES (@textdata)", connection))
                {
                    {
                        using (StreamReader file = File.OpenText("textdata.txt"))
                        {
                            command.Parameters.Add("textdata", SqlDbType.NVarChar).Value = file;

                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }
    }
}
