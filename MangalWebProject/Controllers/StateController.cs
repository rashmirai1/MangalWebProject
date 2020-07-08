using MangalWebProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MangalWebProject.Models.EntityManager;
using MangalWebProject.Models.Entity;

namespace MangalWebProject.Controllers
{
    public class StateController : BaseController
    {
        DataManager dd = new DataManager();

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult CreateEdit(StateViewModel state)
        {
            state.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            state.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_State tblstate = new Mst_State();
            try
            {
                if (state.ID <= 0)
                {
                    var data = dd._context.Mst_State.Where(u => u.St_Desc == state.StateName).Select(x => x.St_Desc).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("StateName", "State Name Already Exists");
                        return Json(state);
                    }
                    tblstate.St_RecordCreated = DateTime.Now;
                    tblstate.St_RecordCreatedBy = state.CreatedBy;
                    dd._context.Mst_State.Add(tblstate); 
                }
                else
                {
                    tblstate = dd._context.Mst_State.Where(x => x.St_Id == state.ID).FirstOrDefault();                    
                }
                tblstate.St_Code = state.StateCode;
                tblstate.St_Desc = state.StateName;
                tblstate.St_CountryId = state.CountryId;
                tblstate.St_RecordUpdated = DateTime.Now;
                tblstate.St_RecordUpdatedBy = state.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Json(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult GetStateById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_State tblstate = dd._context.Mst_State.Where(x => x.St_Id == ID).FirstOrDefault();
            StateViewModel state = new StateViewModel();
            state.ID = tblstate.St_Id;
            state.StateCode = tblstate.St_Code;
            state.StateName = tblstate.St_Desc;
            state.CountryId = (int)tblstate.St_CountryId;
            state.operation = operation;
            ViewBag.CountryList = new SelectList(dd._context.Mst_Country.ToList(), "Cn_id", "Cn_Name");
            return View("State", state);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            string data = "";
            if (dd._context.Mst_City.Any(o => o.Ct_StateId == id))
            {
                data = "Record Cannot Be Deleted Already In Use!";

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var deleterecord = dd._context.Mst_State.Where(x => x.St_Id == id).FirstOrDefault();
                if (deleterecord != null)
                {
                    dd._context.Mst_State.Remove(deleterecord);
                    dd._context.SaveChanges();
                }
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult doesStateNameExist(string StateName)
        {
            var data = dd._context.Mst_State.Where(u => u.St_Desc == StateName).Select(x => x.St_Desc).FirstOrDefault();
            var result = "";
            //Check if state name already exists
            if (data != null)
            {
                if (StateName.ToLower() == data.ToLower().ToString())
                {
                    result = "State Name Already Exists";
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }

        public ActionResult State()
        {
            ButtonVisiblity("Index");
            var model = new StateViewModel();
            ViewBag.CountryList = new SelectList(dd._context.Mst_Country.ToList(), "Cn_id", "Cn_Name");
            return View(model);
        }

        public ActionResult GetStateTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            return PartialView("_StateList",dd._context.Mst_State.ToList());
        }
    }
}