using Dapper;
using FilterTags.Model.DbContexts;
using FilterTags.Model.Models;
using FilterTags.Repository.Interface;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FilterTags.Repository.Implements
{
    public class FilterTagRepository : IFilterTagRepository
    {
        private readonly ContextFilterTag _contextFilterTag = new();
        
        private readonly string creatTable = @$"CREATE TABLE IF NOT EXISTS url (
                                                      `Id` BIGINT NOT NULL AUTO_INCREMENT,
                                                      `Url` TEXT NOT NULL,
                                                      PRIMARY KEY (`Id`));

                                                
                                        CREATE TABLE IF NOT EXISTS tag (
                                                      `Id` BIGINT NOT NULL AUTO_INCREMENT,
                                                      `url_id` BIGINT NOT NULL,
                                                      `Name` VARCHAR(255) NULL,
                                                      `Amount` BIGINT NULL DEFAULT 0,
                                                      PRIMARY KEY (`Id`),
                                                      INDEX `user_id_idx` (`url_id` ASC) VISIBLE,
                                                      CONSTRAINT `user_id`
                                                        FOREIGN KEY (`url_id`)
                                                        REFERENCES `url` (`Id`)
                                                        ON DELETE NO ACTION
                                                        ON UPDATE NO ACTION);


                                            ";
        public bool CheckUrl(string url)
        {
            try
            {
                

                string query = $@"SELECT Id
                                         FROM url
                                         WHERE Url = '{url}';";
                _contextFilterTag.GetConnection();
                bool result = _contextFilterTag.Connection.QueryFirstOrDefault<long>(creatTable+query) > 0 ? true : false;

                return result;

            }
            catch (Exception e)
            {
                throw new Exception("Erro método CheckUrl arquivo FilterTagRepository ex: " + e.Message);
            }
            finally
            {
                _contextFilterTag.Close();
            }

        }

        public (List<FilterTag>, long) GetUrls(string url, int? pagina)
        {
            try
            {
                string query = @$"
                                    SELECT * FROM url WHERE url like @url LIMIT 5 OFFSET {pagina};
                                ";
                _contextFilterTag.GetConnection();
                List<FilterTag>  result = _contextFilterTag.Connection.Query<FilterTag>(query , new {url = "%"+url+"%" }).ToList();
                long amount = _contextFilterTag.Connection.QueryFirstOrDefault<long>("SELECT count(1) FROM url WHERE url like @url  ;", new { url = "%" + url + "%" });
              
                return (result,amount);
            }
            catch(Exception e)
            {
                throw new Exception("Erro método GetUrls arquivo FilterTagRepository ex: " + e.Message);
            }
            finally
            {
                _contextFilterTag.Close();
            }
        }

        public List<Tag> GetUrlsTags(long url_id)
        {
            try
            {
                string query = $@"SELECT * FROM tag WHERE url_id={url_id}";
                _contextFilterTag.GetConnection();
                List<Tag> result = _contextFilterTag.Connection.Query<Tag>(query).ToList();
                return result;

            }
            catch(Exception e)
            {
                throw new Exception("Erro método GetUrlsTags arquivo FilterTagRepository ex: " + e.Message);
            }
            finally
            {
                _contextFilterTag.Close();
            }
        }

        public void SaveTags(FilterTag filterTags)
        {
            try
            {
               

                string insertUrl = @$"

                            INSERT INTO url
                                        (Url)
                                        VALUES
                                        ( '{filterTags?.Url}');
                            SELECT LAST_INSERT_ID();

                    ";
                _contextFilterTag.GetConnection();
                long url_id = _contextFilterTag.Connection.QuerySingle<long>(creatTable+insertUrl);

                foreach (KeyValuePair<string, int> filterTag in filterTags.Tags)
                {
                    string insertTag = $@"
                                        INSERT INTO tag
                                                    (url_id,
                                                    Name,
                                                    Amount)
                                                    VALUES
                                                    ({url_id} ,'{filterTag.Key}',{filterTag.Value});
                            ";
                    _contextFilterTag.Connection.Execute(insertTag);
                }


            }
            catch (Exception e)
            {
                throw new Exception("Erro método SaveTags arquivo FilterTagRepository ex: " + e.Message);
            }
            finally
            {
                _contextFilterTag.Close();
            }
        }
    }
}
