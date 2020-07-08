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
    public class AuditCheckListController : BaseController
    {
        DataManager dd = new DataManager();
        [HttpPost]
        public ActionResult Insert(AuditCheckListViewModel objViewModel)
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

        public bool InsertData(AuditCheckListViewModel audit)
        {
            bool retVal = false;
            audit.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            audit.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_AuditCheckList tblAudit = new Mst_AuditCheckList();
            try
            {
                if (audit.ID <= 0)
                {
                    tblAudit.Acl_RecordCreated = DateTime.Now;
                    tblAudit.Acl_RecordCreatedBy = audit.CreatedBy;
                    dd._context.Mst_AuditCheckList.Add(tblAudit);
                }
                else
                {
                    tblAudit = dd._context.Mst_AuditCheckList.Where(x => x.Acl_Id == audit.ID).FirstOrDefault();
                }
                tblAudit.Acl_EffectiveDate = audit.EffectiveDate;
                tblAudit.Acl_Categoryofaudit = audit.CategoryAudit;
                tblAudit.Acl_CheckPoint = audit.AuditCheckPoint;
                tblAudit.Acl_Status = audit.Status;
                tblAudit.Acl_RecordUpdated = DateTime.Now;
                tblAudit.Acl_RecordUpdatedBy = audit.UpdatedBy;
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
        //[ValidateAntiForgeryToken]
        public JsonResult CreateEdit(AuditCheckListViewModel audit)
        {
            audit.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            audit.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_AuditCheckList tblAudit = new Mst_AuditCheckList();
            try
            {
                if (audit.ID <= 0)
                {
                    tblAudit.Acl_RecordCreated = DateTime.Now;
                    tblAudit.Acl_RecordCreatedBy = audit.CreatedBy;
                    dd._context.Mst_AuditCheckList.Add(tblAudit);
                }
                else
                {
                    tblAudit = dd._context.Mst_AuditCheckList.Where(x => x.Acl_Id == audit.ID).FirstOrDefault();
                }
                tblAudit.Acl_EffectiveDate = audit.EffectiveDate;
                tblAudit.Acl_Categoryofaudit = audit.CategoryAudit;
                tblAudit.Acl_CheckPoint = audit.AuditCheckPoint;
                tblAudit.Acl_Status = audit.Status;
                tblAudit.Acl_RecordUpdated = DateTime.Now;
                tblAudit.Acl_RecordUpdatedBy = audit.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(audit);
        }

        public ActionResult GetAuditCheckListById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_AuditCheckList tblAudit = dd._context.Mst_AuditCheckList.Where(x => x.Acl_Id == ID).FirstOrDefault();
            var audit = new AuditCheckListViewModel();
            audit.ID = tblAudit.Acl_Id;
            audit.EffectiveDate = tblAudit.Acl_EffectiveDate;
            audit.CategoryAudit = tblAudit.Acl_Categoryofaudit;
            audit.AuditCheckPoint = tblAudit.Acl_CheckPoint;
            audit.Status = tblAudit.Acl_Status;
            audit.operation = operation;
            return View("AuditCheckList", audit);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            var deleterecord = dd._context.Mst_AuditCheckList.Where(x => x.Acl_Id == id).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_AuditCheckList.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult AuditCheckList()
        {
            ButtonVisiblity("Index");
            return View();
        }

        public ActionResult GetAuditCheckListTable(string Operation)
        {
            Session["Operation"] = Operation;
            List<AuditCheckListViewModel> list = new List<AuditCheckListViewModel>();
            var model = new AuditCheckListViewModel();
            var tablelist = dd._context.Mst_AuditCheckList.ToList();
            foreach (var item in tablelist)
            {
                model = new AuditCheckListViewModel();
                model.ID = item.Acl_Id;
                model.EffectiveDate = item.Acl_EffectiveDate;
                model.CategoryAuditStr = GetCategoryAudit(item.Acl_Categoryofaudit);
                model.AuditCheckPoint = item.Acl_CheckPoint;
                model.StatusStr = item.Acl_Status == 1 ? "Active" : "Inacitve";
                list.Add(model);
            }
            return PartialView("_AuditCheckList", list);
        }

        public string GetCategoryAudit(short auditid)
        {
            string categorystr = "";
            switch (auditid)
            {
                case 1:
                    categorystr = "Branch Status";
                    break;
                case 2:
                    categorystr = "Gold Status";
                    break;
                case 3:
                    categorystr = "Cash Status";
                    break;
                case 4:
                    categorystr = "ADMIN RELATED/COMPLAINCE";
                    break;
                case 5:
                    categorystr = "REGISTERS";
                    break;
                case 6:
                    categorystr = "RISKS";
                    break;
                case 7:
                    categorystr = "Legal";
                    break;
                case 8:
                    categorystr = "Process";
                    break;
                case 9:
                    categorystr = "Security";
                    break;
                case 10:
                    categorystr = "Grading";
                    break;
                default:
                    break;
            }
            return categorystr;
        }
    }
}