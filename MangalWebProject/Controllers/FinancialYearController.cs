using MangalWebProject.Models;
using MangalWebProject.Models.Entity;
using MangalWebProject.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MangalWebProject.Controllers
{
    public class FinancialYearController : BaseController
    {
        DataManager dd = new DataManager();
        static int FId;
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //swamini123456
        public JsonResult CreateEdit()
        {
            tblfinancialyear tblfinancial = new tblfinancialyear();
            var financial = new FinancialViewModel();
            try
            {
                //if (financial.ID <= 0)
                //{
                //    dd._context.tblfinancialyears.Add(tblfinancial);
                //}
                //else
                //{
                //    tblfinancial = dd._context.tblfinancialyears.Where(x => x.FinancialyearID == financial.ID).FirstOrDefault();
                //}
                FId = 1;
                tblfinancial = dd._context.tblfinancialyears.Where(x => x.FinancialyearID == FId).FirstOrDefault();
                if (tblfinancial != null)
                {
                    string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["c1"].ConnectionString;
                    SqlConnection cnn = new SqlConnection(cnnString);
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "generateFinancialYear";
                    //add any parameters the stored procedure might require
                    cnn.Open();
                    object o = cmd.ExecuteNonQuery();
                    cnn.Close();
                }
                else
                {
                    DateTime thisDate = DateTime.Now;
                    string currentyear = DateTime.Now.ToString("yyyy");
                    DateTime nextyear = thisDate.AddYears(1);
                    financial.FinancialYearFrom = "01/04/" + currentyear;
                    financial.FinancialYearTo = "31/03/" + nextyear.Year;

                    tblfinancial.Financialyearfrom = Convert.ToDateTime(financial.FinancialYearFrom);
                    tblfinancial.Financialyearto = Convert.ToDateTime(financial.FinancialYearTo);
                    tblfinancial.StartDate = Convert.ToDateTime(financial.FinancialYearFrom);
                    tblfinancial.EndDate = Convert.ToDateTime(financial.FinancialYearTo);
                    tblfinancial.CompID = 1;
                    tblfinancial.Financialyear = "April" + tblfinancial.Financialyearfrom.Year + "-" + "March" + tblfinancial.Financialyearto.Year;
                    dd._context.tblfinancialyears.Add(tblfinancial);
                    dd._context.SaveChanges();
                }
                FId = dd._context.tblfinancialyears.Max(x => x.FinancialyearID);
                financial.ID = FId;
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
            tblfinancialyear tblyear = dd._context.tblfinancialyears.Where(x => x.FinancialyearID == 1).FirstOrDefault();
            if (tblyear != null)
            {
                model.ID = tblyear.FinancialyearID;
                model.FinancialYearFrom = tblyear.Financialyearfrom.ToShortDateString();
                model.FinancialYearTo = tblyear.Financialyearto.ToShortDateString();
            }
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