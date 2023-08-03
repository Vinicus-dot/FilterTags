using FilterTags.Model.Models;
using FilterTags.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace FilterTags.Controllers
{
    public class FilterTagController : Controller
    {
        private readonly IFilterTagService _filterTagService;

        public FilterTagController(IFilterTagService filterTagService)
        {
            _filterTagService = filterTagService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetTags(string url)
        {
            try
            {
                FilterTag filterTags = _filterTagService.GetTags(url).Result;
                return PartialView("_ResultGetTags", filterTags);
            }
            catch(Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
        [HttpPost]
        public IActionResult SaveTags(string data)
        {
            try
            {
                _filterTagService.SaveTags(data);
                return Json(new { success = true, message= "Url com tags salva!" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
        [HttpGet]
        public IActionResult Urls()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetUrls(string url, int? pagina)
        {
            try
            {
                ViewBag.StopPaginationLast =  (pagina == 0 || pagina == null) ? true : false ;
                (List<FilterTag>,long) filterTags = _filterTagService.GetUrls(url, pagina);               
                ViewBag.StopPaginationNext = filterTags.Item1.Count < 5 ? true : false;
                ViewBag.Amount = filterTags.Item2;
                return PartialView("_ResultGetUrls", filterTags.Item1);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpGet]
        public IActionResult GetUrlsTags(long url_id,string url)
        {
            try
            {
                List<Tag> tags = _filterTagService.GetUrlsTags(url_id);
                ViewBag.UrlTag = url;
                return PartialView("_ResultGetUrlsTags", tags);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
    }
}
