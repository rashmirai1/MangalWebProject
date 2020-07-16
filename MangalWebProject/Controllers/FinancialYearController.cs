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
    public class FinancialYearController : BaseController
    {
        DataManager dd = new DataManager();

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //swamini123456
        public JsonResult CreateEdit(FinancialViewModel financial)
        {
            tblfinancialyear tblfinancial = new tblfinancialyear();
            try
            {
                if (financial.ID <= 0)
                {
                    var data = dd._context.tblfinancialyears.Where(u => u.Financialyear == financial.FinancialYearFrom).Select(x => x.Financialyearfrom).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("FinancialYearFrom", "Record Already Exists");
                        return Json(financial);
                    }
                    dd._context.tblfinancialyears.Add(tblfinancial);
                }
                else
                {
                    tblfinancial = dd._context.tblfinancialyears.Where(x => x.FinancialyearID == financial.ID).FirstOrDefault();
                }
                tblfinancial.Financialyearfrom = Convert.ToDateTime(financial.FinancialYearFrom);
                tblfinancial.Financialyearto = Convert.ToDateTime(financial.FinancialYearTo);
                tblfinancial.Financialyear = "";
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(financial);
        }

        public ActionResult GetFinancialYearById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            tblfinancialyear tblyear = dd._context.tblfinancialyears.Where(x => x.FinancialyearID == ID).FirstOrDefault();
            FinancialViewModel year = new FinancialViewModel();
            year.ID = tblyear.FinancialyearID;
            year.FinancialYearFrom = tblyear.Financialyearfrom.ToShortDateString();
            year.FinancialYearTo = tblyear.Financialyearto.ToShortDateString();
            year.operation = operation;
            return View("FinancialYear", year);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            var deleterecord = dd._context.Mst_City.Where(x => x.Ct_Id == id).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_City.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinancialYear()
        {
            ButtonVisiblity("Index");
            var model = new FinancialViewModel();
            DateTime thisDate = DateTime.Today;
            string currentyear = DateTime.Now.ToString("yyyy");
            DateTime nextyear = thisDate.AddYears(1); 
            model.FinancialYearFrom = "01/04/" + currentyear;
            model.FinancialYearTo = "31/03/" + nextyear;
            return View(model);
        }

        public ActionResult GetFinancialYearTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            return PartialView("_FinancialYearList", dd._context.tblfinancialyears.ToList());
        }

    }
}