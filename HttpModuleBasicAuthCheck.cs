using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IIS_HTTP_MODULE
{
    public class HttpModuleBasicAuthCheck : System.Web.IHttpModule
    {
        public void Dispose()
        {
            //Nothing
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;            
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            //Log operation goes here    
            HttpContext context = ((HttpApplication)sender).Context;

            foreach (string key in context.Request.Headers.Keys)
            {
                if (key == "Authorization")
                {
                    string basichVal = context.Request.Headers["Authorization"];
                    if (basichVal != null)
                    {
                        if (!IsValidBasichHash(basichVal))
                        {
                            context.Request.Abort();
                        }
                    }
                }
            }
        }

        private bool IsValidBasichHash(string hash)
        {
            bool bContinue = true;

            try
            {                
                string cleanHash = hash.Remove(0, 5);
                string decodedHash = Base64Decode(cleanHash);
                string[] hashParts = decodedHash.Split(':');
                if (!string.IsNullOrEmpty(hashParts[1]))
                {
                    //There is a value here. If you want to add more interesting processing based on 
                    //a specific scenario; add here
                    Console.WriteLine(hashParts[1]);
                }
                else
                {
                    bContinue = false;
                }

            }
            catch (Exception e)
            {
                bContinue = false;  
            }

            return bContinue;
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
