using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMaster.Model
{
    public class ResourceFileRequest
    {
        public string Name { get; set; }
        public IFormFile JsonFile { get; set; }
    }
}
