using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Songza.Controllers
{
    public class DefaultController : Controller
    {
        //todo: implement service for audio player control http://stackoverflow.com/questions/15485768/changing-html5s-source-src-attribute-takes-no-effect-wtih-angularjs
        //todo: private string concierge = "http://songza.com/api/1/situation/targeted?site=songza&current_date=1-jan-2013&day=0&period=0&device=iphone";

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Genres()
        {
            var url = "http://songza.com/api/1/gallery/tag/genres";
            var genres = GetApiResult(url);

            return Json(genres, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Stations(int[] stationIds)
        {
            var urlBuilder = new StringBuilder("http://songza.com/api/1/station/multi");

            for (int i = 0; i < stationIds.Length; i++)
			{
			    if (i == 0)
			    {
			        urlBuilder.Append("?id=" + stationIds[i]);
			    }
			    else
			    {
			        urlBuilder.Append("&id=" + stationIds[i]);
			    }
			}

            var url = urlBuilder.ToString();
            var stations = GetApiResult(url);

            return Json(stations, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StationDetails(int stationId)
        {
            var url = "http://songza.com/api/1/station/" + stationId;
            var stationDetails = GetApiResult(url);

            return Json(stationDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StationNext(int stationId)
        {
            var url = "http://songza.com/api/1/station/" + stationId + "/next"; 
            var stationNext = GetApiResult(url);

            return Json(stationNext, JsonRequestBehavior.AllowGet);
        }
        


        private object GetApiResult(string url)
        {
            var client = new MyWebClient();
            client.Encoding = Encoding.UTF8;
            

            var js = new JavaScriptSerializer();

            var stationsString = client.DownloadString(url);
            var stations = js.Deserialize<object>(stationsString);

            return stations;
        }

    }

    class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.95 Safari/537.36";
            return request;
        }
    }
}
