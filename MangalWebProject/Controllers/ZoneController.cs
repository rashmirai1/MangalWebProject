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
    public class ZoneController : BaseController
    {
        DataManager dd = new DataManager();
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult CreateEdit(ZoneViewModel zone)
        {
            zone.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            zone.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_Zone tblzone = new Mst_Zone();

            try
            {
                if (zone.ID <= 0)
                {
                    var data = dd._context.Mst_Zone.Where(u => u.Zne_Desc == zone.ZoneName).Select(x => x.Zne_Desc).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("ZoneName", "Zone Name Already Exists");
                        return Json(zone);
                    }
                    tblzone.Zne_RecordCreated = DateTime.Now;
                    tblzone.Zne_RecordCreatedBy = zone.CreatedBy;
                    dd._context.Mst_Zone.Add(tblzone);
                }
                else
                {
                    tblzone = dd._context.Mst_Zone.Where(x => x.Zne_No == zone.ID).FirstOrDefault();                    
                }
                tblzone.Zne_Desc = zone.ZoneName;
                tblzone.Zne_RecordUpdated = DateTime.Now;
                tblzone.Zne_RecordUpdatedBy = zone.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(zone);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult GetZoneById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_Zone tblzone = dd._context.Mst_Zone.Where(x => x.Zne_No == ID).FirstOrDefault();
            ZoneViewModel zone = new ZoneViewModel();
            zone.ID = tblzone.Zne_No;
            zone.ZoneName = tblzone.Zne_Desc;
            zone.operation = operation;
            return View("Zone", zone);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            string data = "";
            if (dd._context.Mst_PinCode.Any(o => o.Pc_ZoneId == id))
            {
                data = "Record Cannot Be Deleted Already In Use!";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var deleterecord = dd._context.Mst_Zone.Where(x => x.Zne_No == id).FirstOrDefault();
                if (deleterecord != null)
                {
                    dd._context.Mst_Zone.Remove(deleterecord);
                    dd._context.SaveChanges();
                }
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult doesZoneNameExist(string ZoneName)
        {
            var data = dd._context.Mst_Zone.Where(u => u.Zne_Desc == ZoneName).Select(x => x.Zne_Desc).FirstOrDefault();
            var result = "";
            //Check if state name already exists
            if (data != null)
            {
                if (ZoneName.ToLower() == data.ToLower().ToString())
                {
                    result = "Zone Name Already Exists";
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }

        public ActionResult Zone()
        {
            ButtonVisiblity("Index");
            var model = new ZoneViewModel();
            return View(model);
        }

        public ActionResult GetZoneTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            return PartialView("_ZoneList", dd._context.Mst_Zone.ToList());
        }
    }
}