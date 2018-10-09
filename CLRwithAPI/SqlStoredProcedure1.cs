//using System;
using System.IO; // stream class
using System.Collections.Generic;
using System.Text;
//using System.Net; // needed for HttpWebRequest
using System.Threading.Tasks;
using System.Security.Cryptography;      //for encoding-add system.security.dll to references
using EventsApidll;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Data;


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
    public partial class StoredProcedures
    {
        // sqlpipe https://docs.microsoft.com/en-us/dotnet/api/microsoft.sqlserver.server.sqlpipe.sendresultsend?view=netframework-4.7.2
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void SqlStoredProcedure1()
        {
        
            // Create the record and specify the metadata for the columns.
            SqlDataRecord record = new SqlDataRecord(
            new SqlMetaData("eventname", SqlDbType.NVarChar, 500));


            // Mark the begining of the result-set.
            SqlContext.Pipe.SendResultsStart(record);

            OrgsynEvents xxx = new OrgsynEvents();

            var rootobjects = xxx.GetEvents();

            foreach (var rootobject in rootobjects)
            {
                SqlContext.Pipe.Send(rootobject.name);
            }

            SqlContext.Pipe.SendResultsEnd();

        }

    }
}

