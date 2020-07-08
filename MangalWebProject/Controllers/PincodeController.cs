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
    public class PincodeController : BaseController
    {
        DataManager dd = new DataManager();

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult CreateEdit(PincodeViewModel pincodeViewModel)
        {
            pincodeViewModel.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            pincodeViewModel.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_PinCode tblpincode = new Mst_PinCode();
            try
            {
                if (pincodeViewModel.ID <= 0)
                {
                    var data = dd._context.Mst_PinCode.Where(u => u.Pc_AreaName == pincodeViewModel.AreaName).Select(x => x.Pc_AreaName).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("AreaName", "Area Name Already Exists");
                        return Json(pincodeViewModel);
                    }
                    tblpincode.Pc_RecordCreated = DateTime.Now;
                    tblpincode.Pc_RecordCreatedBy = pincodeViewModel.CreatedBy;
                    dd._context.Mst_PinCode.Add(tblpincode);
                }
                else
                {
                    tblpincode = dd._context.Mst_PinCode.Where(x => x.Pc_Id == pincodeViewModel.ID).FirstOrDefault();
                }
                tblpincode.Pc_Desc = pincodeViewModel.Pincode;
                tblpincode.Pc_AreaName = pincodeViewModel.AreaName;
                tblpincode.Pc_CityId = pincodeViewModel.CityId;
                tblpincode.Pc_ZoneId = pincodeViewModel.ZoneId;
                tblpincode.Pc_RecordUpdated = DateTime.Now;
                tblpincode.Pc_RecordUpdatedBy = pincodeViewModel.UpdatedBy;

                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(pincodeViewModel);
        }

        public ActionResult GetPincodeById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_PinCode tblpincode = dd._context.Mst_PinCode.Where(x => x.Pc_Id == ID).FirstOrDefault();
            PincodeViewModel pincode = new PincodeViewModel();
            pincode.ID = tblpincode.Pc_Id;
            pincode.Pincode = tblpincode.Pc_Desc;
            pincode.AreaName = tblpincode.Pc_AreaName;
            pincode.CityId = tblpincode.Pc_CityId;
            pincode.ZoneId = tblpincode.Pc_ZoneId;
            pincode.operation = operation;

            ViewBag.CityList = new SelectList(dd._context.Mst_City.ToList(), "Ct_Id", "Ct_Desc");
            ViewBag.ZoneList = new SelectList(dd._context.Mst_Zone.ToList(), "Zne_No", "Zne_Desc");

            var stateid = dd._context.Mst_City.Where(x => x.Ct_Id == pincode.CityId).Select(x => x.Ct_StateId).FirstOrDefault();
            var statename = dd._context.Mst_State.Where(x => x.St_Id == stateid).Select(x => x.St_Desc).FirstOrDefault();
            pincode.StateName = statename;
            return View("Pincode", pincode);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            var deleterecord = dd._context.Mst_PinCode.Where(x => x.Pc_Id == id).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_PinCode.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult doesAreaNameExist(string AreaName)
        {
            var data = dd._context.Mst_PinCode.Where(u => u.Pc_AreaName == AreaName).Select(x => x.Pc_AreaName).FirstOrDefault();
            var result = "";
            //Check if city name already exists
            if (data != null)
            {
                if (AreaName.ToLower() == data.ToLower().ToString())
                {
                    result = "Area Already Exists";
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }

        public ActionResult Pincode()
        {
            ButtonVisiblity("Index");
            var model = new PincodeViewModel();
            ViewBag.CityList = new SelectList(dd._context.Mst_City.ToList(), "Ct_Id", "Ct_Desc");
            ViewBag.ZoneList = new SelectList(dd._context.Mst_Zone.ToList(), "Zne_No", "Zne_Desc");
            return View(model);
        }

        public ActionResult GetPincodeTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            return PartialView("_PincodeList", dd._context.Mst_PinCode.ToList());
        }

        public JsonResult GetState(int id)
        {
            var stateid = dd._context.Mst_City.Where(x => x.Ct_Id == id).Select(x => x.Ct_StateId).FirstOrDefault();
            var statename = dd._context.Mst_State.Where(x => x.St_Id == stateid).Select(x => x.St_Desc).FirstOrDefault();
            return Json(statename, JsonRequestBehavior.AllowGet);
        }
    }
}