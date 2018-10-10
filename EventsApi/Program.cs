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
        public DateTime? renewed_at { get; set; }   // add ? because this can be null
        public object alternate_id { get; set; }
        public string website_url { get; set; }
        public int member_count { get; set; }
        public string pic_url { get; set; }
        public int? umbrella_id { get; set; }
        public string keywords { get; set; }
        public Category category { get; set; }
    }
    // timesheet
    public class AccountTimesheet
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class EventTimesheet
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class OrgTimesheet
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Timesheet
    {
        public int id { get; set; }
        public string description { get; set; }
        public string start_date { get; set; }
        public object end_date { get; set; }
        public double? hours { get; set; }
        public string status { get; set; }
        public string attendance_status { get; set; }
        public object alternate_org_name { get; set; }
        public bool requirements_met { get; set; }
        public bool service { get; set; }
        public AccountTimesheet account { get; set; }
        public EventTimesheet @event { get; set; }
        public OrgTimesheet org { get; set; }
    }

    class Program
    {
        static HttpClient client = new HttpClient();


        // --- Gets 
        //Event
        static async Task<ICollection<Event>> GetEventAsync(string path)
        {
            // Event evnt = null;
            ICollection<Event> evnt = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
               evnt = await response.Content.ReadAsAsync<Event[]>();
            }
            return evnt;
        }

        //Organization
        static async Task<ICollection<Organization>> GetOrganizationAsync(string path)
        {

            ICollection<Organization> orgs = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                orgs = await response.Content.ReadAsAsync<Organization[]>();
            }
            return orgs;
        }

        //Timesheet
        static async Task<ICollection<Timesheet>> GetTimesheetAsync(string path)
        {
            ICollection<Timesheet> timesheets = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                timesheets = await response.Content.ReadAsAsync<Timesheet[]>();
            }
            return timesheets;
        }

        // --- Show ---
        // Event
        static void Show(ICollection<Event> evnts)
        {
            foreach (var evnt in evnts)
            {
                Console.WriteLine($"Name: {evnt.name}\tevent id:{evnt.id}");
                foreach (Occurrence occur in evnt.occurrences)
                {
                    Console.WriteLine(occur.is_all_day ? "All Day" : $"Starts: {occur.starts_at}\tEnds at: {occur.ends_at}");
                }
            }
        }
        // Organization
        static void Show(ICollection<Organization> orgs)
        {
            foreach (var org in orgs)
            {
                Console.WriteLine($"Org NameL: {org.long_name}\torig id:{org.id}"+
                    $"\tOrg NameS: { org.short_name}\tOrg Cat:{ org.category.name}");
            }
        }
        // Timesheet
        static void Show(ICollection<Timesheet> timesheets)
        {
            string cnt = timesheets.Count.ToString();
            Console.WriteLine("Number of timesheets: {0}", cnt);
            foreach (var timesheet in timesheets)
            {
                Console.WriteLine($"Timesheet Org: {timesheet.org.name}\tTimesheet Event: {timesheet.@event}" +
                    $"\tTimesheet Account id: {timesheet.account.id}\tHours:{ timesheet.hours}");
            }
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
            string serviceUrlOrg = string.Format("orgs/?key={0}", apikey);
            string serviceUrlTimesheet = string.Format("timesheets/?key={0}", apikey);

            try
            {
                ICollection<Event> evnt = null;
                // Get events
                evnt = await GetEventAsync(serviceUrlEvent);
                //   Show(evnt);

                // organizations
                ICollection<Organization> org = null;
                // Get orgs
                org = await GetOrganizationAsync(serviceUrlOrg);
                //  Show(org);

                ICollection<Timesheet> timesheet = null;
                timesheet = await GetTimesheetAsync(serviceUrlTimesheet);
                Show(timesheet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}