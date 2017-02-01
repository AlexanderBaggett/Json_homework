using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Json_homework
{
    class Program
    {


        static string pathBase = @"c:\Reports\";


        static void Main(string[] args)
        {
            GenerateReports();
            Console.WriteLine("Reports created in " +pathBase);
            Console.ReadLine();
         
        }

        public static void GenerateReports()
        {
            Parallel.Invoke(
                () => PostsByUserIDReport(),
                () => EmailAddressReport(),
                () => CompletedTodosReport()

            );
        }

        /// <summary>
        /// Grab teh User IDs for the specified users and use that to get their Todos
        /// Sum each one separately and write it to a report
        /// </summary>
        private static void CompletedTodosReport()
        {
 
            string address = @"https://jsonplaceholder.typicode.com/users?username=Maxime_Nienow&username=Leopoldo_Corkery&username=Antonette";
            string response = GetJSONString(address);

            JavaScriptSerializer js = new JavaScriptSerializer();
            UserInfo[] users = js.Deserialize<List<UserInfo>>(response).ToArray();

            StringBuilder newAddress = new StringBuilder();
            newAddress.Append(@"https://jsonplaceholder.typicode.com/todos?");
            Dictionary<string, string> usernameToUserID = new Dictionary<string, string>();

            foreach (UserInfo ui in users)
            {
                newAddress.Append("userId=" + ui.id+"&");
                usernameToUserID.Add(ui.username, ui.id);
            }

             address = newAddress.ToString();
             response = GetJSONString(address);
             Todo [] todos = js.Deserialize<List<Todo>>(response).ToArray();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();

            foreach (KeyValuePair<string,string> kvp in usernameToUserID)
            {

                //var stuff =
                //      from todo in todos
                //      where todo.userId.Equals(kvp.Value)
                //      select todo;
                //var x = stuff.Count();

                var totalTodos =  todos.Where(todo => todo.userId.Equals(kvp.Value)).Count();
                sb.AppendLine(kvp.Key + " : " + totalTodos);
                sb.AppendLine(); //space it out for visual effect
            }

           // System.Threading.Thread.Sleep(500);
            SaveReport("CompletedTodosReport", sb.ToString());

    
        }


        /// <summary>
        /// Grab a list of the posts titled iusto eius quod necessitatibus culpa ea
        /// Use that to get a postID and use that to filter the comments and grab
        /// a list of email addresses associated with that post

        /// </summary>
        private static void EmailAddressReport()
        {
            string address = @"https://jsonplaceholder.typicode.com/posts?title=iusto%20eius%20quod%20necessitatibus%20culpa%20ea";
            string response = GetJSONString(address);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Post[] posts = js.Deserialize<List<Post>>(response).ToArray();

            //there is only 1 post, I would create a foreach loop here if there were more than one
            address = @"https://jsonplaceholder.typicode.com/comments?postId="+posts[0].id; 
            response = GetJSONString(address);
            Comment [] comments =js.Deserialize<List<Comment>>(response).ToArray();
            //  Console.WriteLine();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Email addresses for Post \"iusto eius quod necessitatibus culpa ea\" ");
            sb.AppendLine();//for neatness
            foreach (Comment c in comments)
            {
                sb.AppendLine("Email: " + c.email       +"      Name:" + c.name);
            }
            SaveReport("EmailAddress", sb.ToString());
        }



        /// <summary>
        /// Grab The posts for user 3 serialize it as an array and get it's count
        /// This method could be coded to take a params [] and have that programmatically
        /// aded to the url. I choose not to for this excerise.
        /// </summary>
        private static void PostsByUserIDReport()
        {
            string address = @"https://jsonplaceholder.typicode.com/posts?userId=3";
            string response = GetJSONString(address);
            //Console.WriteLine(response);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Post [] p = js.Deserialize<List<Post>>(response).ToArray();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Total Posts for User 3");
            sb.AppendLine(p.Length.ToString());
            SaveReport("PostsByUserId", sb.ToString());

        }

        /// <summary>
        /// Make an HTTP Get request for JSON data at the specified address
        /// and read the response into a string and return it
        /// </summary>
        /// <param name="address"></param>
        /// <returns>JSON text</returns>
        private static string GetJSONString(string address)
        {
            HttpWebRequest get = WebRequest.Create(address) as HttpWebRequest;
            get.Method = "GET";
            get.ContentType = "application/json; charset=utf-8";
            var JSONResponse = get.GetResponse();

            string JSONText;
            using (var streamReader = new StreamReader(JSONResponse.GetResponseStream()))
            {
                JSONText = streamReader.ReadToEnd();
                return JSONText;
            }
        }
        /// <summary>
        /// Save the report to C:\reports\ and use the report title and the date for the filename
        /// </summary>
        /// <param name="ReportTitle"></param>
        /// <param name="ReportContents"></param>
        /// <returns></returns>
        private static bool SaveReport(string ReportTitle, string ReportContents)
        {
            try
            {
                if (System.IO.Directory.Exists(pathBase))
                { }
                else
                { System.IO.Directory.CreateDirectory(pathBase); }
                string reportString = pathBase + ReportTitle +" "+ DateTime.Now.Date.ToLongDateString() +".txt";
                System.IO.File.WriteAllText(reportString, ReportContents);
                Console.WriteLine(ReportTitle + " created");
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to write " + ReportTitle);
                Console.WriteLine(e.ToString());

                return false;
            }
           
        }


    }
}
