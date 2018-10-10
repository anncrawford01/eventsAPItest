using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace HttpClientSample
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

    public class Event
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
    public class Organization
    {
        public int id { get; set; }
        public string short_name { get; set; }
        public string long_name { get; set; }
        public DateTime created_at { get; set; }
        public bool is_disabled { get; set; }
        public string description { get; set; }
        public DateTime renewed_at { get; set; }
        public object alternate_id { get; set; }
        public string website_url { get; set; }
        public int member_count { get; set; }
        public string pic_url { get; set; }
        public int umbrella_id { get; set; }
        public string keywords { get; set; }
        public Category category { get; set; }
    }

    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowEvent(IEnumerable<Event> evnts)
        {
            foreach (var evnt in evnts)
            {
                Console.WriteLine($"Name: {evnt.name}\teventi id:{evnt.id}");
            }
            
        }


           static async Task<IEnumerable<Event>> GetEventAsync(string path)
        {
            // Event evnt = null;
            IEnumerable<Event> evnt = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
               evnt = await response.Content.ReadAsAsync<Event[]>();
            }
            return evnt;
        }


        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter api key as the argument.");
                return 1;
            }
            string apikey = args[0];
            RunAsync(apikey).GetAwaiter().GetResult();
            return 0;
        }

        static async Task RunAsync(string apikey)
        {
   
            // orgsync client
            client.BaseAddress = new Uri("https://orgsync.com/api/v2/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string serviceUrlEvent = string.Format("events/?key={0}", apikey);

            try
            {
                IEnumerable<Event> evnt = null;
                // Get events
                evnt = await GetEventAsync(serviceUrlEvent);
                ShowEvent(evnt);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}