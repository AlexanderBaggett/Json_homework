using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json_homework
{
    class Model
    {
    }

  public  class Post
    {
        public string userId {get;set;}
        public string id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

    public class Todo
    {
        public string userId { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

    public class Comment
    {
        public string postId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string body { get; set; }

    }


    public class Geo
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class Address
    {
        public string street { get; set; }
        public string suite { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public Geo geo { get; set; }
    }

    public class Company
    {
        public string name { get; set; }
        public string catchPhrase { get; set; }
        public string bs { get; set; }
    }

    public class UserInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public Company company { get; set; }
    }



}
