﻿using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UnityEngine;

namespace NearClientUnity.Utilities
{
    public static class Web
    {
        public static async Task<dynamic> FetchJsonAsync(string url, string json = "")
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                if (!string.IsNullOrEmpty(json))
                {
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(url, content);
                }
                else
                {
                    response = await client.GetAsync(url);
                }

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();

                    dynamic rawResult = JObject.Parse(jsonString);

                    if (rawResult.error != null && rawResult.error.data != null)
                    {
                        throw new Exception($"[{rawResult.error.code}]: {rawResult.error.data}: {rawResult.error.message}");
                    }

                    if (rawResult.result != null)
                    {
                        return rawResult.result;
                    }
                    return rawResult;
                }
                else
                {
                    throw new Exception(response.StatusCode + response.Content.ToString());
                }
            }
        }

        public static async Task<dynamic> FetchJsonAsync(ConnectionInfo connection, string json = "")
        {
            var url = connection.Url;
            var result = await FetchJsonAsync(url, json);
            return result;
        }
    }
}