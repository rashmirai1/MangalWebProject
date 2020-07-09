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
    public class CityController : BaseController
    {
        DataManager dd = new DataManager();

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //swamini123456
        public JsonResult CreateEdit(CityViewModel city)
        {
            city.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            city.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_City tblcity = new Mst_City();
            try
            {
                if (city.ID <= 0)
                {
                    var data = dd._context.Mst_City.Where(u => u.Ct_Desc == city.CityName).Select(x => x.Ct_Desc).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("CityName", "City Name Already Exists");
                        return Json(city);
                    }
                    tblcity.Ct_RecordCreated = DateTime.Now;
                    tblcity.Ct_RecordCreatedBy = city.CreatedBy;
                    dd._context.Mst_City.Add(tblcity);
                }
                else
                {
                    tblcity = dd._context.Mst_City.Where(x => x.Ct_Id == city.ID).FirstOrDefault();                    
                }
                tblcity.Ct_Desc = city.CityName;
                tblcity.Ct_StateId = city.StateId;
                tblcity.Ct_RecordUpdated = DateTime.Now;
                tblcity.Ct_RecordUpdatedBy = city.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(city);
        }

        public ActionResult GetCityById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_City tblcity = dd._context.Mst_City.Where(x => x.Ct_Id == ID).FirstOrDefault();
            CityViewModel city = new CityViewModel();
            city.ID = tblcity.Ct_Id;
            city.CityName = tblcity.Ct_Desc;
            city.StateId = (int)tblcity.Ct_StateId;
            city.operation = operation;
            ViewBag.StateList = new SelectList(dd._context.Mst_State.ToList(), "St_Id", "St_Desc");
            var countryid = dd._context.Mst_State.Where(x => x.St_Id == city.StateId).Select(x => x.St_CountryId).FirstOrDefault();
            var country = dd._context.Mst_Country.Where(x => x.Cn_Id == countryid).Select(x => x.Cn_Name).FirstOrDefault();
            city.CountryName = country;
            return View("City", city);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            string data = "";
            if (dd._context.Mst_PinCode.Any(o => o.Pc_CityId == id))
            {
                data = "Record Cannot Be Deleted Already In Use!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var deleterecord = dd._context.Mst_City.Where(x => x.Ct_Id == id).FirstOrDefault();
                if (deleterecord != null)
                {
                    dd._context.Mst_City.Remove(deleterecord);
                    dd._context.SaveChanges();
                }
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult doesCityNameExist(string CityName)
        {
            var data = dd._context.Mst_City.Where(u => u.Ct_Desc == CityName).Select(x => x.Ct_Desc).FirstOrDefault();
            var result = "";
            //Check if city name already exists
            if (data != null)
            {
                if (CityName.ToLower() == data.ToLower().ToString())
                {
                    result = "City Name Already Exists";
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }

        public ActionResult City()
        {
            ButtonVisiblity("Index");
            var model = new CityViewModel();
            ViewBag.StateList = new SelectList(dd._context.Mst_State.ToList(), "St_Id", "St_Desc");
            return View(model);
        }

        public ActionResult GetCityTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            return PartialView("_CityList", dd._context.Mst_City.ToList());
        }

        public JsonResult GetCountry(int id)
        {
            var countryid = dd._context.Mst_State.Where(x => x.St_Id == id).Select(x => x.St_CountryId).FirstOrDefault();
            var country = dd._context.Mst_Country.Where(x => x.Cn_Id == countryid).Select(x=>x.Cn_Name).FirstOrDefault();
            return Json(country, JsonRequestBehavior.AllowGet);
        }
    }
}
