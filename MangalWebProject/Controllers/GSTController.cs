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
    public class GSTController : BaseController
    {
        DataManager dd = new DataManager();

        [HttpPost]
        public ActionResult Insert(GstViewModel objViewModel)
        {
            ModelState.Remove("Id");
            if (objViewModel.ID == 0)
            {
                //_employeeService.AddEmployee(objViewModel);

                if (InsertData(objViewModel))
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(3, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (InsertData(objViewModel))
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
            }
            return View("Index", objViewModel);
        }

        #region Insert Data

        public bool InsertData(GstViewModel gstvm)
        {
            bool retVal = false;
            gstvm.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            gstvm.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_GstMaster tblGst = new Mst_GstMaster();
            try
            {
                gstvm.ID = dd._context.Mst_GstMaster.Any() ? dd._context.Mst_GstMaster.Max(m => m.Gst_RefId) + 1 : 1;
                if (gstvm.EditID <= 0)
                {
                    tblGst.Gst_RefId = gstvm.ID;
                    tblGst.Gst_RecordCreated = DateTime.Now;
                    tblGst.Gst_RecordCreatedBy = gstvm.CreatedBy;
                    dd._context.Mst_GstMaster.Add(tblGst);
                }
                else
                {
                    tblGst = dd._context.Mst_GstMaster.Where(x => x.Gst_RefId == gstvm.ID).FirstOrDefault();
                }
                tblGst.Gst_EffectiveFrom = gstvm.EffectiveFrom;
                tblGst.Gst_CGST = gstvm.CGST;
                tblGst.Gst_SGST = gstvm.SGST;
                tblGst.Gst_IGST = gstvm.IGST;
                tblGst.Gst_RecordUpdated = DateTime.Now;
                tblGst.Gst_RecordUpdatedBy = gstvm.UpdatedBy;
                dd._context.SaveChanges();
                retVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        #endregion Insert Data

        [HttpPost]
        public JsonResult CreateEdit(GstViewModel gst)
        {
            gst.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            gst.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_GstMaster tblGst = new Mst_GstMaster();
            try
            {
                gst.ID = dd._context.Mst_GstMaster.Any() ? dd._context.Mst_GstMaster.Max(m => m.Gst_RefId) + 1 : 1;
                if (gst.EditID <= 0)
                {
                    tblGst.Gst_RefId = gst.ID;
                    tblGst.Gst_RecordCreated = DateTime.Now;
                    tblGst.Gst_RecordCreatedBy = gst.CreatedBy;
                    dd._context.Mst_GstMaster.Add(tblGst);
                }
                else
                {
                    tblGst = dd._context.Mst_GstMaster.Where(x => x.Gst_RefId == gst.ID).FirstOrDefault();
                }
                tblGst.Gst_EffectiveFrom = gst.EffectiveFrom;
                tblGst.Gst_CGST = gst.CGST;
                tblGst.Gst_SGST = gst.SGST;
                tblGst.Gst_IGST = gst.IGST;
                tblGst.Gst_RecordUpdated = DateTime.Now;
                tblGst.Gst_RecordUpdatedBy = gst.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(gst);
        }

        public ActionResult GetGSTById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_GstMaster tblGst = dd._context.Mst_GstMaster.Where(x => x.Gst_RefId == ID).FirstOrDefault();
            GstViewModel gstvm = new GstViewModel();
            gstvm.ID = tblGst.Gst_RefId;
            gstvm.EditID = tblGst.Gst_RefId;
            gstvm.EffectiveFrom = tblGst.Gst_EffectiveFrom;
            gstvm.CGST = tblGst.Gst_CGST;
            gstvm.SGST = tblGst.Gst_SGST;
            gstvm.IGST = tblGst.Gst_IGST;
            gstvm.operation = operation;
            return View("GST", gstvm);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            var deleterecord = dd._context.Mst_GstMaster.Where(x => x.Gst_RefId == id).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_GstMaster.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult GST()
        {
            ButtonVisiblity("Index");
            var model = new GstViewModel();
            model.EffectiveFrom = DateTime.Now;
            model.ID = dd._context.Mst_GstMaster.Any() ? dd._context.Mst_GstMaster.Max(m => m.Gst_RefId) + 1 : 1;
            return View(model);
        }

        public ActionResult GetGSTTable(string Operation)
        {
            Session["Operation"] = Operation;
            var tablelist = dd._context.Mst_GstMaster.ToList();
            return PartialView("_GSTList", tablelist);
        }
    }
}
