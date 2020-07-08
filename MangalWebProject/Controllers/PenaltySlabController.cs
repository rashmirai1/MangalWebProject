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
    public class PenaltySlabController : BaseController
    {
        DataManager dd = new DataManager();

        [HttpPost]
        public ActionResult Insert(PenaltySlabViewModel objViewModel)
        {
            ModelState.Remove("Id");
            if (objViewModel.ID == 0)
            {
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

        public bool InsertData(PenaltySlabViewModel penalty)
        {
            bool retVal = false;
            penalty.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            penalty.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_PenaltySlab tblPenaltySlab = new Mst_PenaltySlab();
            try
            {
                penalty.ID = dd._context.Mst_PenaltySlab.Any() ? dd._context.Mst_PenaltySlab.Max(m => m.Ps_Id) + 1 : 1;
                if (penalty.EditID <= 0)
                {
                    tblPenaltySlab.Ps_Id = penalty.ID;
                    tblPenaltySlab.Ps_RecordCreated = DateTime.Now;
                    tblPenaltySlab.Ps_RecordCreatedBy = penalty.CreatedBy;
                    dd._context.Mst_PenaltySlab.Add(tblPenaltySlab);
                }
                else
                {
                    tblPenaltySlab = dd._context.Mst_PenaltySlab.Where(x => x.Ps_Id == penalty.ID).FirstOrDefault();
                }
                tblPenaltySlab.Ps_Datewef =Convert.ToDateTime(penalty.Datewef);
                tblPenaltySlab.Ps_Penalty = penalty.PenaltyAmount;
                tblPenaltySlab.Ps_Accounthead = penalty.AccountHead;
                tblPenaltySlab.Ps_RecordUpdated = DateTime.Now;
                tblPenaltySlab.Ps_RecordUpdatedBy = penalty.UpdatedBy;
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

        public ActionResult GetPenaltySlabById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_PenaltySlab tblPenalty = dd._context.Mst_PenaltySlab.Where(x => x.Ps_Id == ID).FirstOrDefault();
            PenaltySlabViewModel penalty = new PenaltySlabViewModel();
            penalty.ID = tblPenalty.Ps_Id;
            penalty.EditID = tblPenalty.Ps_Id;
            penalty.Datewef = tblPenalty.Ps_Datewef.ToString();
            penalty.PenaltyAmount = tblPenalty.Ps_Penalty;
            penalty.AccountHead = tblPenalty.Ps_Accounthead;
            penalty.operation = operation;
            return View("PenaltySlab", penalty);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            var deleterecord = dd._context.Mst_PenaltySlab.Where(x => x.Ps_Id == id).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_PenaltySlab.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult PenaltySlab()
        {
            ButtonVisiblity("Index");
            var model = new PenaltySlabViewModel();
            model.ID = dd._context.Mst_PenaltySlab.Any() ? dd._context.Mst_PenaltySlab.Max(m => m.Ps_Id) + 1 : 1;
            ViewBag.AccountHeadList = new SelectList(dd._context.tblaccountmasters.ToList(), "AccountID", "Name");
            return View(model);
        }

        public ActionResult GetPenaltySlabTable(string Operation)
        {
            Session["Operation"] = Operation;
            List<PenaltySlabViewModel> list = new List<PenaltySlabViewModel>();
            var model = new PenaltySlabViewModel();
            var tablelist = dd._context.Mst_PenaltySlab.ToList();
            foreach (var item in tablelist)
            {
                model = new PenaltySlabViewModel();
                model.Datewef = item.Ps_Datewef.ToString();
                model.EditID = item.Ps_Id;
                model.ID = item.Ps_Id;
                model.PenaltyAmount = item.Ps_Penalty;
                model.AccountHeadStr = dd._context.tblaccountmasters.Where(x => x.AccountID == item.Ps_Accounthead).Select(x => x.Name).FirstOrDefault();
                list.Add(model);
            }
            return PartialView("_PenaltySlabList", list);
        }
    }
}