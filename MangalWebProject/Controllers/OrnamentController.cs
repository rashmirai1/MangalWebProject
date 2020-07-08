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
    public class OrnamentController : BaseController
    {
        DataManager dd = new DataManager();
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult CreateEdit(OrnamentViewModel ornament)
        {
            ornament.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            ornament.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_Ornament tblOrnament = new Mst_Ornament();
            try
            {
                if (ornament.ID <= 0)
                {
                    var data = dd._context.Mst_Ornament.Where(u => u.Orn_Name == ornament.OrnamentName).Select(x => x.Orn_Name).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("OrnamentName", "Ornament Name Already Exists");
                        return Json(ornament);
                    }
                    tblOrnament.Orn_RecordCreated = DateTime.Now;
                    tblOrnament.Orn_RecordCreatedBy = ornament.CreatedBy;
                    dd._context.Mst_Ornament.Add(tblOrnament);
                }
                else
                {
                    tblOrnament = dd._context.Mst_Ornament.Where(x => x.Orn_Id ==ornament.ID).FirstOrDefault();
                }
                tblOrnament.Orn_Name = ornament.OrnamentName;
                tblOrnament.Orn_Product = ornament.Product;
                tblOrnament.Orn_Status = ornament.Status;
                tblOrnament.Orn_RecordUpdated = DateTime.Now;
                tblOrnament.Orn_RecordUpdatedBy = ornament.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(ornament);
        }

        public ActionResult GetOrnamentById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_Ornament tblOrnament = dd._context.Mst_Ornament.Where(x => x.Orn_Id == ID).FirstOrDefault();
            OrnamentViewModel ornament = new OrnamentViewModel();
            ornament.ID = tblOrnament.Orn_Id;
            ornament.OrnamentName = tblOrnament.Orn_Name;
            ornament.Product = (short)tblOrnament.Orn_Product;
            ornament.Status = (short)tblOrnament.Orn_Status;
            ornament.operation = operation;
            return View("Ornament", ornament);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            var deleterecord = dd._context.Mst_Ornament.Where(x => x.Orn_Id == id).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_Ornament.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult doesOrnamentNameExist(string OrnamentName)
        {
            var data = dd._context.Mst_Ornament.Where(u => u.Orn_Name == OrnamentName).Select(x => x.Orn_Name).FirstOrDefault();
            var result = "";
            //Check if document name already exists
            if (data != null)
            {
                if (OrnamentName.ToLower() == data.ToLower().ToString())
                {
                    result = "Ornament Name Already Exists";
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }

        public ActionResult Ornament()
        {
            ButtonVisiblity("Index");
            return View();
        }

        public ActionResult GetOrnamentTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            List<OrnamentViewModel> list = new List<OrnamentViewModel>();
            var model = new OrnamentViewModel();
            var tablelist = dd._context.Mst_Ornament.ToList();
            foreach (var item in tablelist)
            {
                model = new OrnamentViewModel();
                model.ID = item.Orn_Id;
                model.OrnamentName = item.Orn_Name;
                model.ProductStr = item.Orn_Product == 1 ? "Gold Loan" : "Diamond Loan";
                model.StatusStr = GetStatus((short)item.Orn_Status);
                list.Add(model);
            }
            return PartialView("_OrnamentList", list);
        }

        public string GetStatus(short statusid)
        {
            string statusstr = "";
            switch (statusid)
            {
                case 1:
                    statusstr = "Allowed";
                    break;
                case 2:
                    statusstr = "Not allowed";
                    break;
                case 3:
                    statusstr = "Prohibited";
                    break;
                default:
                    break;
            }
            return statusstr;
        }
    }
}