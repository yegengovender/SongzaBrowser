using System.Text;
using System.Web.Mvc;
using Songza.Infrastructure;
using Songza.Models;

namespace Songza.Controllers
{
    public class DefaultController : Controller
    {
        //todo: implement service for audio player control http://stackoverflow.com/questions/15485768/changing-html5s-source-src-attribute-takes-no-effect-wtih-angularjs
        //todo: private string concierge = "http://songza.com/api/1/situation/targeted?site=songza&current_date=1-jan-2013&day=0&period=0&device=iphone";

        private const string StationDetailsUrl = "http://songza.com/api/1/station/{0}";
        private const string StationNextUrl = "http://songza.com/api/1/station/{0}/next";
        private const string StationsUrl = "http://songza.com/api/1/station/multi";
        private const string GenresUrl = "http://songza.com/api/1/gallery/tag/genres";
        private const string MoodsUrl = "http://songza.com/api/1/gallery/tag/moods";
        private const string DecadesUrl = "http://songza.com/api/1/gallery/tag/decades";
        private const string CultureUrl = "http://songza.com/api/1/gallery/tag/culture";
        private const string CategoryUrl = "http://songza.com/api/1/gallery/tag/{0}";
        
        private readonly WebApiHelper _webApiHelper;
        private readonly NameList[] CategoryList = new NameList[] { 
                                                     new NameList("genres"             ),
                                                     new NameList("activities"         ),
                                                     new NameList("moods"              ),
                                                     new NameList("decades"            ),
                                                     new NameList("culture"            ),
                                                     new NameList("record_store_clerk" )
                                                 };

        public DefaultController(WebApiHelper webApiHelper)
        {
            
            _webApiHelper = webApiHelper;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public JsonResult Categories()
        {
            return Json(CategoryList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CategoryFilter(string category)
        {
            var url = string.Format(CategoryUrl, category);
            var genres = _webApiHelper.GetApiResult(url);

            return Json(genres, JsonRequestBehavior.AllowGet);            
        }

        public JsonResult Genres()
        {
            var genres = _webApiHelper.GetApiResult(GenresUrl);

            return Json(genres, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Stations(int[] stationIds)
        {
            var url = MultiStationsUrl(stationIds);

            var stations = _webApiHelper.GetApiResult(url);

            return Json(stations, JsonRequestBehavior.AllowGet);
        }

        private string MultiStationsUrl(int[] stationIds)
        {
            var urlBuilder = new StringBuilder(StationsUrl);

            for (var i = 0; i < stationIds.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        urlBuilder.Append("?id=" + stationIds[i]);
                        break;
                    default:
                        urlBuilder.Append("&id=" + stationIds[i]);
                        break;
                }
            }

            var url = urlBuilder.ToString();
            return url;
        }

        [HttpPost]
        public JsonResult StationDetails(int stationId)
        {
            var url = string.Format(StationDetailsUrl, stationId);

            var stationDetails = _webApiHelper.GetApiResult(url);

            return Json(stationDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StationNext(int stationId)
        {
            var url = string.Format(StationNextUrl, stationId);

            var stationNext = _webApiHelper.GetApiResult(url);

            return Json(stationNext, JsonRequestBehavior.AllowGet);
        }
    }
}
