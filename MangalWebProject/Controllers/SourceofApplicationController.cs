using MangalWebProject.Models;
using MangalWebProject.Models.Entity;
using MangalWebProject.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MangalWebProject.Controllers
{
    public class SourceofApplicationController : BaseController
    {
        DataManager dd = new DataManager();
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult CreateEdit(SourceofApplicationViewModel source)
        {
            source.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            source.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_SourceofApplication tblSource = new Mst_SourceofApplication();
            try
            {
                source.ID = dd._context.Mst_SourceofApplication.Any() ? dd._context.Mst_SourceofApplication.Max(m => m.Soa_Id) + 1 : 1;
                if (source.EditID <= 0)
                {
                    var data = dd._context.Mst_SourceofApplication.Where(u => u.Soa_Name ==source.SourceName && u.Soa_Status == 1).Select(x => x.Soa_Name).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("SourceName", "Source Name Already Exists");
                        return Json(source);
                    }
                    tblSource.Soa_Id = source.ID;
                    tblSource.Soa_RecordCreated = DateTime.Now;
                    tblSource.Soa_RecordCreatedBy = source.CreatedBy;
                    dd._context.Mst_SourceofApplication.Add(tblSource);
                }
                else
                {
                    tblSource = dd._context.Mst_SourceofApplication.Where(x => x.Soa_Id == source.ID).FirstOrDefault();
                }

                tblSource.Soa_Name = source.SourceName;
                tblSource.Soa_Category =source.SourceCategory;
                tblSource.Soa_Status = source.SourceStatus;
                tblSource.Soa_RecordUpdated = DateTime.Now;
                tblSource.Soa_RecordUpdatedBy = source.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(source);
        }

        public ActionResult GetSourceApplicationById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_SourceofApplication tblsource = dd._context.Mst_SourceofApplication.Where(x => x.Soa_Id == ID).FirstOrDefault();
            SourceofApplicationViewModel source = new SourceofApplicationViewModel();
            source.ID = tblsource.Soa_Id;
            source.EditID = tblsource.Soa_Id;
            source.SourceName = tblsource.Soa_Name;
            source.SourceCategory = (short)tblsource.Soa_Category;
            source.SourceStatus = (short)tblsource.Soa_Status;
            source.operation = operation;
            return View("SourceApplication", source);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            var deleterecord = dd._context.Mst_SourceofApplication.Where(x => x.Soa_Id == id).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_SourceofApplication.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult doesSourceNameExist(string SourceName)
        {
            var data = dd._context.Mst_SourceofApplication.Where(u => u.Soa_Name == SourceName && u.Soa_Status==1).Select(x => x.Soa_Name).FirstOrDefault();
            var result = "";
            //Check if city name already exists
            if (data != null)
            {
                if (SourceName.ToLower() == data.ToLower().ToString())
                {
                    result = "Source Name Already Exists";
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }

        public ActionResult SourceApplication()
        {
            ButtonVisiblity("Index");
            var model = new SourceofApplicationViewModel();
            model.ID = dd._context.Mst_SourceofApplication.Any() ? dd._context.Mst_SourceofApplication.Max(m => m.Soa_Id) + 1 : 1;
            return View(model);
        }

        public ActionResult GetSourceApplicationTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            List<SourceofApplicationViewModel> list = new List<SourceofApplicationViewModel>();
            var model = new SourceofApplicationViewModel();
            var tablelist = dd._context.Mst_SourceofApplication.ToList();
            int onlinecategory = 1;
            int status = 1;
            foreach (var item in tablelist)
            {
                model = new SourceofApplicationViewModel();
                model.SourceName = item.Soa_Name;
                model.EditID = item.Soa_Id;
                model.ID = item.Soa_Id;
                model.SourceCategirystr = item.Soa_Category == onlinecategory ? "Online" : "Offline";
                model.SourceStatusstr = item.Soa_Status == status ? "Active" : "Inactive";
                list.Add(model);
            }
            return PartialView("_SourceApplicationList",list);
        }
    }
}