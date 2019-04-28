using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager
{
    public class Job
    {
        public string Description { get; set; }
        public string Id { get; set; }

        public Job(string _description, string _id)
        {
            Description = _description;
            Id = _id;
        }
    }
}
