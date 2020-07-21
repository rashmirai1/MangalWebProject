using MangalWebProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MangalWebProject.Controllers
{
    public class DocumentUploadController : Controller
    {
        // GET: DocumentUpload
        public ActionResult DocumentUpload()
        {
            var model = new DocumentUploadViewModel();
            model.DocumentUploadList = new List<DocumentUploadViewModel>();
            return View(model);
        }
    }
}