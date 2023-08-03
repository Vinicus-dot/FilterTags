using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterTags.Model.DbContexts
{
    public class ContextFilterTag : ContextBase 
    {
        public ContextFilterTag()
        {
            this.ConnectionString = Environment.GetEnvironmentVariable("connectionStringFilterTag") ?? "";
        }
    }
}
