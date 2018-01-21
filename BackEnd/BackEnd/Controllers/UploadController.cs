using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BackEnd.Controllers
{
    public class UploadController : ApiController
    {
        // GET: api/Upload
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Upload/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Upload
        public HttpResponseMessage PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];

                    var path = "";
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    if (extension == ".ico")
                    {
                        path = "~/Content/Icons/icon-";
                    }
                    else
                    {
                        path = "~/Content/Images/img-";
                    }

                    int counter = 0;
                    var filePath = HttpContext.Current.Server.MapPath(path + postedFile.FileName);
                    var responsePath = "http://localhost:13124/Content/Images/img-" + postedFile.FileName;
                    postedFile.SaveAs(filePath);
                    if (extension == ".ico")
                    {
                        dict.Add("path", "icon-" + counter + extension);
                    }
                    else
                    {
                        dict.Add("path", responsePath);
                    }
                    return Request.CreateResponse(HttpStatusCode.Created, dict);
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch
            {
                var res = string.Format("Internal error");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        // PUT: api/Upload/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Upload/5
        public void Delete(int id)
        {
        }
    }
}
