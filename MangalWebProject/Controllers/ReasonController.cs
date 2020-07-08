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
    public class ReasonController : BaseController
    {
        DataManager dd = new DataManager();
        [HttpPost]
        public JsonResult CreateEdit(ReasonViewModel reason)
        {
            reason.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            reason.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_Reason tblReason = new Mst_Reason();
            try
            {
                if (reason.ID <= 0)
                {
                    var data = dd._context.Mst_Reason.Where(u => u.Re_Desc == reason.ReasonName).Select(x => x.Re_Desc).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("Reason", "Reason Already Exists");
                        return Json(reason);
                    }
                    tblReason.Re_RecordCreated = DateTime.Now;
                    tblReason.Re_RecordCreatedBy = reason.CreatedBy;
                    dd._context.Mst_Reason.Add(tblReason);
                }
                else
                {
                    tblReason = dd._context.Mst_Reason.Where(x => x.Re_No == reason.ID).FirstOrDefault();
                }
                tblReason.Re_Desc = reason.ReasonName;
                tblReason.Re_Status = reason.Status;
                tblReason.Re_RecordUpdated = DateTime.Now;
                tblReason.Re_RecordUpdatedBy = reason.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(reason);
        }

        public ActionResult GetReasonById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_Reason tblReason = dd._context.Mst_Reason.Where(x => x.Re_No == ID).FirstOrDefault();
            ReasonViewModel reason = new ReasonViewModel();
            reason.ID = tblReason.Re_No;
            reason.ReasonName = tblReason.Re_Desc;
            reason.Status = (short)tblReason.Re_Status;
            reason.operation = operation;
            return View("Reason", reason);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            var deleterecord = dd._context.Mst_Reason.Where(x => x.Re_No == id).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_Reason.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult doesReasonExist(string Reason)
        {
            var data = dd._context.Mst_Reason.Where(u => u.Re_Desc == Reason).Select(x => x.Re_Desc).FirstOrDefault();
            var result = "";
            //Check if record already exists
            if (data != null)
            {
                if (Reason.ToLower() == data.ToLower().ToString())
                {
                    result = "Reason Already Exists";
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }

        public ActionResult Reason()
        {
            ButtonVisiblity("Index");
            return View();
        }

        public ActionResult GetReasonTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            List<ReasonViewModel> list = new List<ReasonViewModel>();
            var model = new ReasonViewModel();
            var tablelist = dd._context.Mst_Reason.ToList();
            foreach (var item in tablelist)
            {
                model = new ReasonViewModel();
                model.ID = item.Re_No;
                model.ReasonName = item.Re_Desc;
                model.StatusStr = item.Re_Status == 1 ? "Active" : "Inactive";
                list.Add(model);
            }
            return PartialView("_ReasonList", list);
        }

    }
}