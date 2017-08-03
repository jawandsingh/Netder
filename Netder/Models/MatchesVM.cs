using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netder.Models
{
    public class MatchesVM
    {
        public class ProcessedFile
        {
            public int width { get; set; }
            public string url { get; set; }
            public int height { get; set; }
        }

        public class Photo
        {
            public string id { get; set; }
            public string url { get; set; }
            public List<ProcessedFile> processedFiles { get; set; }
        }

        public class Teaser
        {
            public string @string { get; set; }
            public string type { get; set; }
        }

        public class Result
        {
            public int distance_mi { get; set; }
            public int connection_count { get; set; }
            public int common_like_count { get; set; }
            public int common_friend_count { get; set; }
            public List<object> common_likes { get; set; }
            public List<object> common_friends { get; set; }
            public string content_hash { get; set; }
            public string _id { get; set; }
            public string birth_date { get; set; }
            public string name { get; set; }
            public string ping_time { get; set; }
            public List<Photo> photos { get; set; }
            public List<object> jobs { get; set; }
            public List<object> schools { get; set; }
            public Teaser teaser { get; set; }
            public List<object> teasers { get; set; }
            public int s_number { get; set; }
            public int gender { get; set; }
            public string birth_date_info { get; set; }
        }

        public class MatchRootObject
        {
            public int status { get; set; }
            public List<Result> results { get; set; }
        }
    }
}
