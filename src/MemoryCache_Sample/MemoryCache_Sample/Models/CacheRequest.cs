using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryCache_Sample.Models
{
    public class CacheRequest
    {
        public string Key { get; set; }
        public object Data { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}
