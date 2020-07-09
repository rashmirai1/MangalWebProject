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
    public class SchemeController : BaseController
    {
        DataManager dd = new DataManager();
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult CreateEdit(SchemeViewModel scheme)
        {
            scheme.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            scheme.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_SchemeMaster tblSchemeMaster = new Mst_SchemeMaster();
            try
            {
                if (scheme.EditID <= 0)
                {
                    scheme.SchemeId = dd._context.Mst_SchemeMaster.Any() ? dd._context.Mst_SchemeMaster.Max(m => m.SchemeId) + 1 : 1;
                    var data = dd._context.Mst_SchemeMaster.Where(u => u.SchemeName == scheme.SchemeName && u.Status == 1).Select(x => x.SchemeName).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("SchemeName", "Scheme Name Already Exists");
                        return Json(scheme);
                    }
                    tblSchemeMaster.SchemeId = scheme.SchemeId;
                    tblSchemeMaster.RecordCreated = DateTime.Now;
                    tblSchemeMaster.RecordCreatedBy = scheme.CreatedBy;
                    dd._context.Mst_SchemeMaster.Add(tblSchemeMaster);
                }
                else
                {
                    tblSchemeMaster = dd._context.Mst_SchemeMaster.Where(x => x.SchemeId == scheme.SchemeId).FirstOrDefault();
                }
                tblSchemeMaster.Product = scheme.Product;
                tblSchemeMaster.SchemeName = scheme.SchemeName;
                //tblSchemeMaster.Purity = scheme.Purity;
                tblSchemeMaster.SchemeType = scheme.SchemeType;
                tblSchemeMaster.Frequency = scheme.Frequency;
                tblSchemeMaster.MinTenure = scheme.MinTenure;
                tblSchemeMaster.MaxTenure = scheme.MaxTenure;
                tblSchemeMaster.MinLoanAmount = scheme.MinLoanAmount;
                tblSchemeMaster.MaxLoanAmount = scheme.MaxLoanAmount;
                tblSchemeMaster.MinLTVPerc = scheme.MinLTVPerc;
                tblSchemeMaster.MaxLTVPerc = scheme.MaxLTVPerc;
                tblSchemeMaster.MinRoiPerc = scheme.MinROIPerc;
                tblSchemeMaster.MaxRoiPerc = scheme.MaxROIPerc;
                tblSchemeMaster.GracePeriod = scheme.GracePeriod;
                tblSchemeMaster.EffectiveRoiPerc = scheme.EffectiveROIPerc;
                tblSchemeMaster.LockInPeriod = scheme.LockInPeriod;
                tblSchemeMaster.ProcessingFeeType = scheme.ProcessingFeeType;
                tblSchemeMaster.ProcessingCharges = scheme.ProcessingCharges;
                tblSchemeMaster.Status = scheme.Status;
                tblSchemeMaster.RecordUpdated = DateTime.Now;
                tblSchemeMaster.RecordUpdatedBy = scheme.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(scheme);
        }

        public ActionResult GetSchemeById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            ViewBag.PurityList = new SelectList(dd._context.Mst_PurityMaster.ToList(), "Id", "PurityName");
            Mst_SchemeMaster tblScheme = dd._context.Mst_SchemeMaster.Where(x => x.SchemeId == ID).FirstOrDefault();
            SchemeViewModel scheme = new SchemeViewModel();
            scheme.Product = tblScheme.Product;
            scheme.SchemeName = tblScheme.SchemeName;
           // scheme.Purity = tblScheme.Purity;
            scheme.SchemeType = tblScheme.SchemeType;
            scheme.Frequency = tblScheme.Frequency;
            scheme.MinTenure = (int)tblScheme.MinTenure;
            scheme.MaxTenure = (int)tblScheme.MaxTenure;
            scheme.MinLoanAmount = tblScheme.MinLoanAmount;
            scheme.MaxLoanAmount = tblScheme.MaxLoanAmount;
            scheme.MinLTVPerc = tblScheme.MinLTVPerc;
            scheme.MaxLTVPerc = tblScheme.MaxLTVPerc;
            scheme.MinROIPerc = tblScheme.MinRoiPerc;
            scheme.MaxROIPerc = tblScheme.MaxRoiPerc;
            scheme.GracePeriod = tblScheme.GracePeriod;
            scheme.EffectiveROIPerc = tblScheme.EffectiveRoiPerc;
            scheme.LockInPeriod = tblScheme.LockInPeriod;
            scheme.ProcessingFeeType = tblScheme.ProcessingFeeType;
            scheme.ProcessingCharges = tblScheme.ProcessingCharges;
            scheme.Status = tblScheme.Status;
            scheme.operation = operation;
            return View("Scheme", scheme);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            var deleterecord = dd._context.Mst_SchemeMaster.Where(x => x.SchemeId == id).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_SchemeMaster.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult doesSourceNameExist(string SchemeName)
        {
            var data = dd._context.Mst_SchemeMaster.Where(u => u.SchemeName == SchemeName && u.Status == 1).Select(x => x.SchemeName).FirstOrDefault();
            var result = "";
            //Check if city name already exists
            if (data != null)
            {
                if (SchemeName.ToLower() == data.ToLower().ToString())
                {
                    result = "Scheme Name Already Exists";
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }

        public ActionResult Scheme()
        {
            ButtonVisiblity("Index");
            var model = new SchemeViewModel();
            ViewBag.PurityList = new SelectList(dd._context.Mst_PurityMaster.ToList(), "Id", "PurityName");
            model.SchemeId = dd._context.Mst_SchemeMaster.Any() ? dd._context.Mst_SchemeMaster.Max(m => m.SchemeId) + 1 : 1;
            return View(model);
        }

        public JsonResult GetPurity(int id)
        {
            var data = new SelectList(dd._context.Mst_PurityMaster.Where(x => x.PurityType == id).ToList(), "Id", "PurityName");
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSchemeTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            List<SchemeViewModel> list = new List<SchemeViewModel>();
            var model = new SchemeViewModel();
            var tablelist = dd._context.Mst_SchemeMaster.ToList();
            foreach (var item in tablelist)
            {
                model = new SchemeViewModel();
                model.SchemeName = item.SchemeName;
                model.ProductStr = item.Product == 1 ? "Gold Loan" : "Diamond Loan";
                model.SchemeId = item.SchemeId;
                model.EditID = item.SchemeId;
                model.SchemeTypeStr = item.SchemeType == 1 ? "Slabwise" : "Slabwise";
                model.FrequencyStr = item.Frequency == 1 ? "Montly" : "Montly";
                model.Statusstr = item.Status == 1 ? "Active" : "Inactive";
                list.Add(model);
            }
            return PartialView("_SchemeList", list);
        }
    }
}