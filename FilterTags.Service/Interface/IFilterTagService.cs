using FilterTags.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterTags.Service.Interface
{
    public interface IFilterTagService
    {
        Task<FilterTag> GetTags(string url);
        void SaveTags(string data);

        (List<FilterTag>, long) GetUrls(string url, int? pagina);
        List<Tag> GetUrlsTags(long url_id);
    }
}
