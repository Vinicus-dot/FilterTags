using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterTags.Model.Models
{
    public class Tag
    {
        public long Id { get; set; }
        public long url_id { get; set; }
        public string Name { get; set; }
        public long Amount { get; set; }
    }
}
