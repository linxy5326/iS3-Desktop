﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IS3.Core.Service
{
    public static class WebApiCaller
    {
        public static string HttpPost(string url, string body)
        {
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Accept = "application/json, text/javascript, */*"; //"text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json; charset=utf-8";

                byte[] buffer = encoding.GetBytes(body);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                var res = (HttpWebResponse)ex.Response;
                StringBuilder sb = new StringBuilder();
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                sb.Append(sr.ReadToEnd());
                //string ssb = sb.ToString();
                throw new Exception(sb.ToString());
            }
        }
        

        /// <summary>  
        /// GET Method  
        /// </summary>  
        /// <returns></returns>  
        public static string HttpGet(string url)
        {
            //缓存判断
            if (iS3Cache.checkIfExist(url))
            {
                if (iS3Cache.CheckIfLastet(url))
                {
                    return iS3Cache.GetFromCache(url);
                }
            }
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            myRequest.Timeout = 4000;
            HttpWebResponse myResponse = null;

            myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();

            //缓存保存
            iS3Cache.SaveToCache(url, content);

            return content;

            //异常请求  
            //catch (WebException e)
            //{
            //    myResponse = (HttpWebResponse)e.Response;
            //    using (Stream errData = myResponse.GetResponseStream())
            //    {
            //        using (StreamReader reader = new StreamReader(errData))
            //        {
            //            string text = reader.ReadToEnd();

            //            return text;
            //        }
            //    }
            //}
        }
        static void auth()
        {
            string result = "";

            string serviceAddress = ServiceConfig.TokenURL;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string strContent = "grant_type=password&password=lxd&username=linxiaodong";
            byte[] data = Encoding.UTF8.GetBytes(strContent);
            request.ContentLength = data.Length;

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }

            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            Stream stream = resp.GetResponseStream();

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
        }

        static void post()
        {
            string result = "";

            string serviceAddress = "http://47.98.187.240:8011/api/project/info/LXD";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);

            request.Method = "PUT";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer ty0LpueVaDdsRv_GpZ9zkLaEr9oMGCenZN63I_xKlo6FBjbyYiPSN8WkIJKYePXOAMrd3Y-CLUlJb-jDOEcKYGhYQlDQuGJ1vtYCXZ_cos_aUhlFI3vzJYaikT71fBMxrPdWj5O1oUhos7g2KAkU4uV5uBXty-TEIm6iQkgrvnX2lsaUyj_qih8u_aDd7nNxMADzTrR_3x7Xzlrw8IQrGg");

            string strContent = @"{""ProjectName"": ""test""}";

            using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
            {
                dataStream.Write(strContent);
                dataStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
        }
    }
}