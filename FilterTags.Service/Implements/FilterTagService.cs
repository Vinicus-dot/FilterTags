using FilterTags.Model.Models;
using FilterTags.Repository.Implements;
using FilterTags.Repository.Interface;
using FilterTags.Service.Interface;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace FilterTags.Service.Implements
{
    public class FilterTagService : IFilterTagService
    {
        private readonly IFilterTagRepository _filterTagRepository;

        public FilterTagService(IFilterTagRepository filterTagRepository)
        {
            _filterTagRepository = filterTagRepository;
        }
        async Task<Model.Models.FilterTag>  IFilterTagService.GetTags(string url)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    throw new Exception("URL inválida ou incompleta.");

                HttpClient httpClient = new();
                string htmlContent = await httpClient.GetStringAsync(url);
  
                HtmlDocument htmlDocument = new();
                htmlDocument.LoadHtml(htmlContent);

                Model.Models.FilterTag filter = new()
                {
                   Url = url,
                   Tags = new()
                };

                CountTagsRecursive(htmlDocument.DocumentNode, filter.Tags);
             
                return filter;
            }
            catch(Exception e)
            {
                throw new Exception("Erro método GetTags arquivo FilterService ex: "+e.Message);
            }
        }

        public void SaveTags(string data)
        {
            try
            {
                FilterTag? filterTags = JsonConvert.DeserializeObject<FilterTag>(data);

                if (string.IsNullOrEmpty(filterTags?.Url))
                    throw new Exception("Url Vazia ou nula");

                if (filterTags.Tags == null || filterTags.Tags.Count <= 0)
                    throw new Exception("Tags Vazia ou nula");

                if (_filterTagRepository.CheckUrl(filterTags.Url))
                        throw new Exception("Url já existente!");

                if (filterTags != null)
                    _filterTagRepository.SaveTags(filterTags);
                else
                    throw new Exception("Objeto filterTags Null");
            }
            catch(Exception e)
            {
                throw new Exception("Erro método SaveTags arquivo FilterService ex: " + e.Message);
            }
        }

        public (List<FilterTag>, long) GetUrls(string url, int? pagina)
        {
            try
            {
                var result = _filterTagRepository.GetUrls(url, pagina);
                if ((result.Item2 % 5) == 0)
                    result.Item2 /= 5;
                else
                {
                    result.Item2 /= 5;
                    result.Item2 += 1;
                }
                if (result.Item2 == 0)
                    result.Item2 = 1;
                return result;
            }
            catch(Exception e)
            {
                throw new Exception("Erro método GetUrls arquivo FilterService ex: " + e.Message);
            }
        }

        private void CountTagsRecursive(HtmlNode node, Dictionary<string, int>? tagCountDict)
        {
            if (node == null)
                return;

            if (!node.Name.Contains("#"))
            {
                string tagType = node.Name;
                if (tagCountDict != null && tagCountDict.ContainsKey(tagType))
                    tagCountDict[tagType]++;
                else if (tagCountDict != null)
                    tagCountDict[tagType] = 1;
            }

            foreach (HtmlNode childNode in node.ChildNodes)
                CountTagsRecursive(childNode, tagCountDict);
        }

        public List<Tag> GetUrlsTags(long url_id)
        {
            try
            {
                if (url_id <= 0)
                    throw new Exception("Id da url infomado não é valido!");

               return _filterTagRepository.GetUrlsTags(url_id);
            }
            catch(Exception e)
            {
                throw new Exception("Erro método GetUrlsTags arquivo FilterService ex: " + e.Message);
            }
                
        }
    }
}
