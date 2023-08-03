using FilterTags.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterTags.Repository.Interface
{
    public interface IFilterTagRepository
    {
       void SaveTags(FilterTag filterTags);
       bool CheckUrl(string url);
       (List<FilterTag>, long) GetUrls(string url, int? pagina);
        List<Tag> GetUrlsTags(long url_id);
    }
}
