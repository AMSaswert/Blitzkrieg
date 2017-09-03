using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BackEnd.Controllers
{
    [RoutePrefix("api")]
    public class UploadController : ApiController
    {
        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpPost]
        [Route("Upload")]
        [ResponseType(typeof(string))]
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
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png", ".ico" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.jpeg,.gif,.png.,.ico");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 5 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var path = "";
                            if (extension == ".ico")
                            {
                                path = "~/Content/Icons/icon-";
                            }
                            else
                            {
                                path = "~/Content/Images/img-";
                            }
                            int counter = 0;
                            while (true)
                            {
                                bool found = false;
                                foreach(var ex in AllowedFileExtensions)
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath(path + counter + ex)))
                                    {
                                        found = true;
                                        break;
                                    }
                                    
                                }
                                if (!found)
                                {
                                    break;
                                }
                                counter++;
                            }

                            var filePath = HttpContext.Current.Server.MapPath(path + counter + extension);

                            postedFile.SaveAs(filePath);
                            if(extension == ".ico")
                            {
                                dict.Add("path", "icon-" + counter + extension);
                            }
                            else
                            {
                                dict.Add("path", "img-" + counter + extension);
                            }
                            return Request.CreateResponse(HttpStatusCode.Created, dict);

                        }
                    }

                    //var message1 = string.Format("Image Updated Successfully.");
                    //return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
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



        // DELETE api/<controller>/5
        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpDelete]
        [Route("ImgRemove/{id}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult Delete(int id)
        {
            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png" };
            bool found = false;
            string ext = "";
            foreach (var ex in AllowedFileExtensions)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/Content/Images/img-" + id + ex)))
                {
                    found = true;
                    ext = ex;
                    break;
                }
                

            }
            if (!found)
            {
                return NotFound();
            }
            string path = HttpContext.Current.Server.MapPath("~/Content/Images/img-" + id + ext);

            File.Delete(path);

            return Ok(path);
        }

        // DELETE api/<controller>/5
        [Authorize(Roles = "AppUser, Moderator, Admin")]
        [HttpDelete]
        [Route("IconRemove/{id}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult DeleteIcon(int id)
        {
            IList<string> AllowedFileExtensions = new List<string> { ".ico" };
            bool found = false;
            string ext = "";
            foreach (var ex in AllowedFileExtensions)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/Content/Icons/icon-" + id + ex)))
                {
                    found = true;
                    ext = ex;
                    break;
                }
                

            }
            if (!found)
            {
                return NotFound();
            }
            string path = HttpContext.Current.Server.MapPath("~/Content/Icons/icon-" + id + ext);

            File.Delete(path);

            return Ok(path);
        }
    }

    

}
