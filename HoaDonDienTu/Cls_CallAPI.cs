

// EFFECTV105, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// EFFECTV105.Cls_CallAPI
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using Bkav.Crypto;
//using BSECUS;
//using EFFECTV105;
//using EFFECTV105.vn.ehoadon.ws;
//using EFFECTV105.vn.ehoadon.wsdemo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HoaDonDienTu;


namespace HoaDonDienTu.Class
{
    public class Cls_CallAPI
    {

        public const int GetInvoiceXMLToSign = 805;

        public const int UploadFileXMLSigned = 501;

        public const int ConfirmAutoSign = 205;

        public const int ConfirmAutoSignList = 206;

        public Cls_CallAPI()
        {
            ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
        }

        //public string POST_Json(string url, string jsonContent)
        //{
        //    try
        //    {
        //        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "POST";
        //        httpWebRequest.PreAuthenticate = false;
        //        StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
        //        streamWriter.Write(jsonContent);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
        //        string text = streamReader.ReadToEnd();
        //        return "OK:" + text;
        //    }
        //    catch (Exception ex)
        //    {
        //        Class.Functions.WriteTextLog(ex.Message + "::error call api");
        //        return "FAIL:" + ex.Message;
        //    }
        //}

        public string POST_Json(string url, string jsonContent)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.PreAuthenticate = false;

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonContent);
                }

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    return "OK:" + result;
                }
            }
            catch (WebException webEx)
            {
                string responseText = "";

                if (webEx.Response != null)
                {
                    using (var stream = webEx.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }

                string logMessage = webEx.Message + "::error call api. Response: " + responseText;
                Class.Functions.WriteTextLog(logMessage);
                return "FAIL:" + logMessage;
            }
            catch (Exception ex)
            {
                string logMessage = ex.Message + "::error call api";
                Class.Functions.WriteTextLog(logMessage);
                return "FAIL:" + logMessage;
            }
        }



        public string POST_Json_Viettel(string url, string jsonContent, string user, string pass)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.PreAuthenticate = false;
                string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + pass));
                httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Basic " + text;
                StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(jsonContent);
                streamWriter.Flush();
                streamWriter.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string text2 = streamReader.ReadToEnd();
                return "OK:" + text2;
            }
            catch (Exception ex)
            {
                Class.Functions.WriteTextLog(ex.Message + "::error call api");
                return "FAIL:" + ex.Message;
            }
        }

        private string EscapeLikeValue(string contents)
        {
            contents = contents.Replace(" ", "+");
            contents = contents.Replace("/", "%2F");
            return contents;
        }

        public string POST_Form_Viettel(string url, string jsonContent, string user, string pass)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "POST";
                httpWebRequest.PreAuthenticate = false;
                string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + pass));
                httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Basic " + text;
                StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                string value = EscapeLikeValue(jsonContent);
                streamWriter.Write(value);
                streamWriter.Flush();
                streamWriter.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string text2 = streamReader.ReadToEnd();
                return "OK:" + text2;
            }
            catch (Exception ex)
            {
                Class.Functions.WriteTextLog(ex.Message + "::error call api");
                return "FAIL:" + ex.Message;
            }
        }




        public string POST_Form_Viettel2(string url, string jsonContent, string access_token)
        {
            string str3;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.Expect100Continue = true;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.PreAuthenticate = false;
                request.Headers[HttpRequestHeader.Cookie] = "access_token=" + access_token;
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(this.b(jsonContent));
                    writer.Flush();
                    writer.Close();
                    using (StreamReader reader = new StreamReader(((HttpWebResponse)request.GetResponse()).GetResponseStream()))
                    {
                        string str2 = reader.ReadToEnd();
                        str3 = "OK:" + str2;
                    }
                }
            }
            catch (Exception exception)
            {
                Class.Functions.WriteTextLog(exception.Message + "::error call api");
                str3 = "FAIL:" + exception.Message;
            }
            return str3;
        }

        public string POST_Json_Viettel2(string url, string jsonContent, string access_token)
        {

            try
            {

                System.Net.ServicePointManager.Expect100Continue = true;
                //  System.Net.ServicePointManager.SecurityProtocol = //(System.Net.SecurityProtocolType)3072;
                ////(System.Net.SecurityProtocolType)12288|
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072 | (System.Net.SecurityProtocolType)768 | System.Net.SecurityProtocolType.Tls;
                //
                var httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                //httpWebRequest.
                httpWebRequest.PreAuthenticate = false;
                //httpWebRequest.Headers[authe]

                httpWebRequest.Headers[System.Net.HttpRequestHeader.Cookie] = "access_token=" + access_token;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = jsonContent;
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                    // httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;
                    var httpResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {

                        var result = streamReader.ReadToEnd();
                        return "OK:" + result;
                    }
                }
            }
            catch (System.Net.WebException e)
            {
                using (System.Net.WebResponse response = e.Response)
                {
                    System.Net.HttpWebResponse httpResponse = (System.Net.HttpWebResponse)response;
                    //  Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        return "FAIL:" + text;
                    }
                }
            }
            catch (Exception ex)
            {
                Class.Functions.WriteTextLog(ex.Message + "::error call api");
                return "FAIL:" + ex.Message;
            }
        }

        private string b(string A_0)
        {
            A_0 = A_0.Replace(" ", "+");
            A_0 = A_0.Replace("/", "%2F");
            return A_0;
        }


        public string POST_XmlFile_TCT(string url, string filename, string user, string pass)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "text/xml";
                httpWebRequest.Accept = "text/html;charset=UTF-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.PreAuthenticate = false;
                string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + pass));
                httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Basic " + text;
                StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                string value = File.ReadAllText(filename);
                streamWriter.Write(value);
                streamWriter.Flush();
                streamWriter.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string text2 = streamReader.ReadToEnd();
                return "OK:" + text2;
            }
            catch (Exception ex)
            {
                Class.Functions.WriteTextLog(ex.Message + "::error call api");
                return "FAIL:" + ex.Message;
            }
        }

        public string UrlEncode(string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < bytes.Length; i++)
            {
                stringBuilder.Append(IntToHex(bytes[i]));
            }
            return stringBuilder.ToString();
        }

        private string IntToHex(int n)
        {
            switch (n)
            {
                case 13:
                    return "%0D";
                case 33:
                    return "!";
                case 42:
                    return "*";
                case 40:
                    return "(";
                case 41:
                    return ")";
                case 95:
                    return "_";
                case 45:
                    return "-";
                case 32:
                    return "+";
                case 46:
                    return ".";
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                    return ((char)n).ToString();
                default:
                    {
                        if (n >= 97 && n <= 122)
                        {
                            return ((char)n).ToString();
                        }
                        if (n >= 65 && n <= 90)
                        {
                            return ((char)n).ToString();
                        }
                        string text = "%";
                        int num = n / 16;
                        if (num < 10)
                        {
                            text += num;
                        }
                        else
                        {
                            switch (num)
                            {
                                case 10:
                                    text += "A";
                                    break;
                                case 11:
                                    text += "B";
                                    break;
                                case 12:
                                    text += "C";
                                    break;
                                case 13:
                                    text += "D";
                                    break;
                                case 14:
                                    text += "E";
                                    break;
                                case 15:
                                    text += "F";
                                    break;
                            }
                        }
                        int num2 = n % 16;
                        if (num2 < 10)
                        {
                            text += num2;
                        }
                        else
                        {
                            switch (num2)
                            {
                                case 10:
                                    text += "A";
                                    break;
                                case 11:
                                    text += "B";
                                    break;
                                case 12:
                                    text += "C";
                                    break;
                                case 13:
                                    text += "D";
                                    break;
                                case 14:
                                    text += "E";
                                    break;
                                case 15:
                                    text += "F";
                                    break;
                            }
                        }
                        return text;
                    }
            }
        }

        public string GET_API2(string url, Dictionary<string, string> headers)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.PreAuthenticate = false;
                foreach (string key in headers.Keys)
                {
                    httpWebRequest.Headers.Add(key, headers[key]);
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string text = streamReader.ReadToEnd();
                return "OK:" + text;
            }
            catch (Exception ex)
            {
            Class.Functions.WriteTextLog(ex.Message + "::error call api");
                return "FAIL:" + ex.Message;
            }
        }

        public string GET_API(string url, string tokenkey)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.PreAuthenticate = false;
                if (!string.IsNullOrEmpty(tokenkey))
                {
                    httpWebRequest.Headers[HttpRequestHeader.Authorization] = "bearer " + tokenkey;
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string text = streamReader.ReadToEnd();
                return "OK:" + text;
            }
            catch (Exception ex)
            {
                Class.Functions.WriteTextLog(ex.Message + "::error call api");
                return "FAIL:" + ex.Message;
            }
        }

        public string ConvertJsonObjectToString(object js)
        {
            return JsonConvert.SerializeObject(js);
        }


        //public string POST_Json_Viettel2(string url, string jsonContent, string access_token)
        //{
        //    try
        //    {
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        ServicePointManager.Expect100Continue = true;
        //        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "POST";
        //        httpWebRequest.PreAuthenticate = false;
        //        httpWebRequest.Headers[HttpRequestHeader.Cookie] = "access_token=" + access_token;
        //        StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
        //        streamWriter.Write(jsonContent);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
        //        string text = streamReader.ReadToEnd();
        //        return "OK:" + text;
        //    }
        //    catch (Exception ex)
        //    {
        //        Class.Functions.WriteTextLog(ex.Message + "::error call api");
        //        return "FAIL:" + ex.Message;
        //    }
        //}

        public static DataSet ConvertJsonToDataSetLinq(string jsonString)
        {
            //IL_004f: Unknown result type (might be due to invalid IL or missing references)
            //IL_0056: Expected O, but got Unknown
            //IL_0057: Unknown result type (might be due to invalid IL or missing references)
            //IL_005c: Unknown result type (might be due to invalid IL or missing references)
            //IL_0072: Unknown result type (might be due to invalid IL or missing references)
            //IL_0079: Expected O, but got Unknown
            JObject val = JObject.Parse(jsonString);
            DataSet dataSet = new DataSet();
            int num = 0;
            foreach (JToken item in from x in ((JContainer)val).Descendants()
                                    where x is JArray
                                    select x)
            {
                num++;
                JArray val2 = new JArray();
                foreach (JObject item2 in item.Children<JObject>())
                {
                    JObject val3 = new JObject();
                    foreach (JProperty item3 in item2.Properties())
                    {
                        if (item3.Value is JValue)
                        {
                            val3.Add(item3.Name, item3.Value);
                        }
                    }
                    val2.Add((JToken)(object)val3);
                }
                DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(((object)val2).ToString());
                dataTable.TableName = "B" + num.ToString("000");
                dataSet.Tables.Add(dataTable);
            }
            return dataSet;
        }

        public static DataTable ConvertJsonToDatatableLinq(string jsonString)
        {
            //IL_0035: Unknown result type (might be due to invalid IL or missing references)
            //IL_003b: Expected O, but got Unknown
            //IL_003c: Unknown result type (might be due to invalid IL or missing references)
            //IL_0041: Unknown result type (might be due to invalid IL or missing references)
            //IL_0056: Unknown result type (might be due to invalid IL or missing references)
            //IL_005d: Expected O, but got Unknown
            JObject val = JObject.Parse(jsonString);
            JToken val2 = (from x in ((JContainer)val).Descendants()
                           where x is JArray
                           select x).First();
            JArray val3 = new JArray();
            foreach (JObject item in val2.Children<JObject>())
            {
                JObject val4 = new JObject();
                foreach (JProperty item2 in item.Properties())
                {
                    if (item2.Value is JValue)
                    {
                        val4.Add(item2.Name, item2.Value);
                    }
                }
                val3.Add((JToken)(object)val4);
            }
            return JsonConvert.DeserializeObject<DataTable>(((object)val3).ToString());
        }

        public DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dataTable = new DataTable();
            string[] array = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> list = new List<string>();
            string[] array2 = array;
            int num = 0;
            if (num < array2.Length)
            {
                string text = array2[num];
                string[] array3 = Regex.Split(text.Replace("{", "").Replace("}", ""), ",\"");
                string[] array4 = array3;
                foreach (string text2 in array4)
                {
                    try
                    {
                        int num2 = text2.IndexOf(":");
                        string item = text2.Substring(0, num2 - 1).Replace("\"", "");
                        if (!list.Contains(item))
                        {
                            list.Add(item);
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception($"Error Parsing Column Name : {text2}");
                    }
                }
            }
            foreach (string item2 in list)
            {
                dataTable.Columns.Add(item2);
            }
            string[] array5 = array;
            foreach (string text3 in array5)
            {
                string[] array6 = Regex.Split(text3.Replace("{", "").Replace("}", ""), ",\"");
                DataRow dataRow = dataTable.NewRow();
                string[] array7 = array6;
                foreach (string text4 in array7)
                {
                    try
                    {
                        int num3 = text4.IndexOf(":");
                        string columnName = text4.Substring(0, num3 - 1).Replace("\"", "");
                        string value = text4.Substring(num3 + 1).Replace("\"", "");
                        dataRow[columnName] = value;
                    }
                    catch (Exception)
                    {
                    }
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
    }
}
