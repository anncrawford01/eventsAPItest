using System;
using System.IO; // stream class
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net; // needed for HttpWebRequest
using System.Threading.Tasks;
using System.Security.Cryptography;      //for encoding-add system.security.dll to references
using System.Web.Script.Serialization;  // need for Jason; add system.Web.Extensions in references
using System.Net.Http;                  // httpclient class
using System.Net.Http.Headers;

namespace eventsAPItest
{
    class Program
    {


        public class Category
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class UmbrellaCategory
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Occurrence
        {
            public bool is_all_day { get; set; }
            public object starts_at { get; set; }
            public object ends_at { get; set; }
        }

        public class RootObject
        {
            public int id { get; set; }
            public bool is_public { get; set; }
            public string name { get; set; }
            public string location { get; set; }
            public bool is_approved { get; set; }
            public Category category { get; set; }
            public UmbrellaCategory umbrella_category { get; set; }
            public string description { get; set; }
            public string html_description { get; set; }
            public int rsvps { get; set; }
            public int org_id { get; set; }
            public List<Occurrence> occurrences { get; set; }
        }
        static void Main(string[] args)
        {
            // references 
            //  https://msdn.microsoft.com/en-us/library/system.net.http.httpclient(v=vs.118).aspx
            //** http://www.tutorialsteacher.com/webapi/consuming-web-api-in-dotnet-using-httpclient
            // https://forums.asp.net/t/2081327.aspx?Get+HttpClient+with+parameters
            // https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client

            try
            {
                string HostUri = "https://orgsync.com/api/v2/";
                string apiKey = "XdUBCwxeA-doeAGhrw1zpg";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(HostUri);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //string serviceUrl = string.Format(OrgInfoServiceUrl, ConfigurationCommon.OrgSyncApiKey, id);

                    // HTTP Get
                     var responseTask = client.GetAsync("events/?key=XdUBCwxeA-doeAGhrw1zpg");

                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<RootObject[]>();
                        readTask.Wait();
                   
                        var rootobjects = readTask.Result;
                        foreach (var rootobject in rootobjects)
                        {
                            Console.WriteLine(rootobject.name);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //An error occurred.  
                if (ex.Source != null)
                    Console.WriteLine("IOException source: {0} \n Error: {1}", ex.Source, ex.Message);
            }
           
            Console.WriteLine("Press any key to end");
            Console.ReadLine();

        }
    }
    
}
