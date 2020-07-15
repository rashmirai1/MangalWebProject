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
    public class BranchController : BaseController
    {
        DataManager dd = new DataManager();

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult CreateEdit(BranchViewModel branch)
        {
            branch.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            branch.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_Branch tblBranch = new Mst_Branch();
            try
            {
                if (branch.ID <= 0)
                {
                    var data = dd._context.Mst_Branch.Where(u => u.BranchName == branch.BranchName && u.Status == 1).Select(x => x.BranchName).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("BranchName", "Branch Name Already Exists");
                        return Json(branch);
                    }
                    tblBranch.RecordCreated = DateTime.Now;
                    tblBranch.RecordCreatedBy = branch.CreatedBy;
                    dd._context.Mst_Branch.Add(tblBranch);
                }
                else
                {
                    tblBranch = dd._context.Mst_Branch.Where(x => x.ID == branch.ID).FirstOrDefault();
                }
                tblBranch.BranchName = branch.BranchName;
                tblBranch.BranchCode = branch.BranchCode;
                tblBranch.BranchType = branch.BranchType;
                tblBranch.InceptionDate = Convert.ToDateTime(branch.DateInception);
                tblBranch.RentPeriodAgreed = Convert.ToDateTime(branch.RentPeriodAgreed);
                tblBranch.Address = branch.Address;
                tblBranch.Pincode = branch.Pincode;
                tblBranch.ContactPerson = branch.ContactPerson;
                tblBranch.MobileNo = branch.MobileNo;
                tblBranch.InTime = branch.InTime;
                tblBranch.OutTime = branch.OutTime;
                //tblBranch.DateWEF = Convert.ToDateTime(branch.DateWEF);
                tblBranch.Status = branch.Status;
                tblBranch.DateWEF = Convert.ToDateTime(branch.RentPeriodAgreed);
                tblBranch.RecordUpdated = DateTime.Now;
                tblBranch.RecordUpdatedBy = branch.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(branch);
        }

        public ActionResult GetBranchById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_Branch tblBranch = dd._context.Mst_Branch.Where(x => x.ID == ID).FirstOrDefault();
            BranchViewModel branch = new BranchViewModel();
            branch.BranchName = tblBranch.BranchName;
            branch.BranchCode = tblBranch.BranchCode;
            branch.BranchType = tblBranch.BranchType;
            branch.DateInception = tblBranch.InceptionDate.ToShortDateString();
            branch.RentPeriodAgreed = Convert.ToDateTime(tblBranch.RentPeriodAgreed).ToShortDateString();
            branch.Address = tblBranch.Address;
            branch.Pincode = (int)tblBranch.Pincode;
            branch.ContactPerson = tblBranch.ContactPerson;
            branch.MobileNo = (int)tblBranch.MobileNo;
            branch.InTime = tblBranch.InTime;
            branch.OutTime = tblBranch.OutTime;
            branch.DateWEF = Convert.ToDateTime(branch.DateWEF).ToShortDateString();
            branch.operation = operation;
            ViewBag.PincodeList = new SelectList(dd._context.Mst_PinCode.ToList(), "Pc_Id", "Pc_Desc");
            var pincodemodel = dd._context.Mst_PinCode.Where(x => x.Pc_Id == branch.Pincode).Select(x => new PincodeViewModel { CityId = x.Pc_CityId, ZoneId = x.Pc_ZoneId, AreaName = x.Pc_AreaName }).FirstOrDefault();
            var ZoneName = dd._context.Mst_Zone.Where(x => x.Zne_No == pincodemodel.ZoneId).Select(x => x.Zne_Desc).FirstOrDefault();
            var cityname = dd._context.Mst_City.Where(x => x.Ct_Id == pincodemodel.CityId).Select(x => new CityViewModel { CityName = x.Ct_Desc, StateId = (int)x.Ct_StateId }).FirstOrDefault();
            var statename = dd._context.Mst_State.Where(x => x.St_Id == cityname.StateId).Select(x => x.St_Desc).FirstOrDefault();
            branch.AreaName = pincodemodel.AreaName;
            branch.ZoneName = ZoneName;
            branch.CityName = cityname.CityName;
            branch.StateName = statename;
            return View("Branch", branch);
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
                var deleterecord = dd._context.Mst_Branch.Where(x => x.ID == id && x.Status == 1).FirstOrDefault();
                if (deleterecord != null)
                {
                    dd._context.Mst_Branch.Remove(deleterecord);
                    dd._context.SaveChanges();
                }
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult doesBranchNameExist(string BranchName)
        {
            var data = dd._context.Mst_Branch.Where(u => u.BranchName == BranchName && u.Status == 1).Select(x => x.BranchName).FirstOrDefault();
            var result = "";
            //Check if branch name already exists
            if (data != null)
            {
                if (BranchName.ToLower() == data.ToLower().ToString())
                {
                    result = "Branch Name Already Exists";
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }

        public ActionResult Branch()
        {
            ButtonVisiblity("Index");
            var model = new BranchViewModel();
            ViewBag.PincodeList = new SelectList(dd._context.Mst_PinCode.ToList(), "Pc_Id", "Pc_Desc");
            return View(model);
        }

        public ActionResult GetBranchTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            var tablelist = dd._context.Mst_Branch.ToList();
            List<BranchViewModel> list = new List<BranchViewModel>();
            var model = new BranchViewModel();
            foreach (var item in tablelist)
            {
                model = new BranchViewModel();
                model.ID = item.ID;
                model.BranchName = item.BranchName;
                model.BranchCode = item.BranchCode;
                model.StatusStr = item.Status == 1 ? "Active" : "Inactive";
                list.Add(model);
            }
            return PartialView("_BranchList",list);
        }

        public JsonResult GetPincodeDetails(int id)
        {
            var branch = new BranchViewModel();
            var pincodemodel = dd._context.Mst_PinCode.Where(x => x.Pc_Id == id).Select(x => new PincodeViewModel { CityId = x.Pc_CityId, ZoneId = x.Pc_ZoneId, AreaName = x.Pc_AreaName }).FirstOrDefault();
            var ZoneName = dd._context.Mst_Zone.Where(x => x.Zne_No == pincodemodel.ZoneId).Select(x => x.Zne_Desc).FirstOrDefault();
            var cityname = dd._context.Mst_City.Where(x => x.Ct_Id == pincodemodel.CityId).Select(x => new CityViewModel { CityName = x.Ct_Desc, StateId = (int)x.Ct_StateId }).FirstOrDefault();
            var statename = dd._context.Mst_State.Where(x => x.St_Id == cityname.StateId).Select(x => x.St_Desc).FirstOrDefault();
            branch.AreaName = pincodemodel.AreaName;
            branch.ZoneName = ZoneName;
            branch.CityName = cityname.CityName;
            branch.StateName = statename;
            return Json(branch, JsonRequestBehavior.AllowGet);
        }
    }
}