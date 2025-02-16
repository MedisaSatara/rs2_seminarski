﻿using eKarton.Model;
using eKarton.WinFr.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flurl.Http;

namespace eKarton.WinFr
{
    public class ApiService
    {
        private string _route = null;
        private string _resource;
        public string endpoint = $"{Resources.ApiUrl}";

        public static string Username { get; set; }
        public static string Password { get; set; }

        public ApiService(string resource)
        {
            _resource = resource;
        }
        public async Task<T> Get<T>(object searchRequest = null)
        {
            var query = "";
            if (searchRequest != null)
            {
                query = await searchRequest?.ToQueryString();
            }

            var list = await $"{endpoint}{_resource}?{query}".WithBasicAuth(Username,Password).GetJsonAsync<T>();

            return list;
            /* var url = $"{Properties.Settings.Default.ApiUrl}/{_route}";
             var result = await url.GetJsonAsync<T>();
             return result;*/
        }
        public async Task<T> GetById<T>(object Id)
        {

            var url = $"{endpoint}{_resource}/{Id}";
            var result = await url.WithBasicAuth(Username,Password).GetJsonAsync<T>();
            return result;
        }
        public async Task<T> Insert<T>(object request)
        {
            var url = $"{endpoint}{_resource}";

            try
            {
                return await url.WithBasicAuth(Username,Password).PostJsonAsync(request).ReceiveJson<T>();
            }
            catch (FlurlHttpException ex)
            {
                var errors = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();

                var stringBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    stringBuilder.AppendLine($"{error.Key}, ${string.Join(",", error.Value)}");
                }

                MessageBox.Show(stringBuilder.ToString(), "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(T);
            }

        }
        public async Task<T> Update<T>(int id, object request)
        {
            try
            {
                var url = $"{endpoint}{_resource}/{id}";

                //return await url.PutJsonAsync(request).ReceiveJson<T>();
                return await url.WithBasicAuth(Username, Password).PutJsonAsync(request).ReceiveJson<T>();

            }
            catch (FlurlHttpException ex)
            {

                var errors = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();

                var stringBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    stringBuilder.AppendLine($"{error.Key}, ${string.Join(",", error.Value)}");
                }

                MessageBox.Show(stringBuilder.ToString(), "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default(T);
            }
        }

        public async Task<bool> Delete<T>(int id)
        {
            try
            {
                var url = $"{endpoint}{_resource}/{id}";
                return await url.DeleteAsync().ReceiveJson<bool>();

                // return await url.WithBasicAuth(Username, Password).DeleteAsync().ReceiveJson<bool>();
            }
            catch (FlurlHttpException ex)
            {
                var errors = await ex.GetResponseStringAsync();
                //var errors = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();

                var stringBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    stringBuilder.AppendLine($"{error.ToString()}, ${string.Join(",", error.ToString())}");
                }
                MessageBox.Show(stringBuilder.ToString(), "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}
