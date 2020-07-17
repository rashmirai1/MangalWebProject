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
                    tblBranch.BranchName = branch.BranchName;
                    tblBranch.BranchCode = branch.BranchCode;
                    tblBranch.BranchType = branch.BranchType;
                    tblBranch.InceptionDate = Convert.ToDateTime(branch.DateInception);
                    if (tblBranch.BranchType == 2)
                    {
                        tblBranch.RentPeriodAgreed = null;
                    }
                    else
                    {
                        tblBranch.RentPeriodAgreed = Convert.ToDateTime(branch.RentPeriodAgreed);
                    }
                    tblBranch.DateWEF = Convert.ToDateTime(branch.DateWEF);
                    tblBranch.Address = branch.Address;
                    tblBranch.Pincode = branch.Pincode;
                    tblBranch.ContactPerson = branch.ContactPerson;
                    tblBranch.MobileNo = branch.MobileNo;
                    tblBranch.InTime = branch.InTime;
                    tblBranch.OutTime = branch.OutTime;
                    tblBranch.Status = branch.Status;
                    tblBranch.RecordCreated = DateTime.Now;
                    tblBranch.RecordCreatedBy = branch.CreatedBy;
                    tblBranch.RecordUpdated = DateTime.Now;
                    tblBranch.RecordUpdatedBy = branch.UpdatedBy;
                    dd._context.Mst_Branch.Add(tblBranch);
                }
                else
                {
                    tblBranch = dd._context.Mst_Branch.Where(x => x.ID == branch.ID && x.Status == 1).FirstOrDefault();
                    // first inactive record then insert active record
                    if (tblBranch.InceptionDate != Convert.ToDateTime(branch.DateInception))
                    {
                        tblBranch.Status = 2;
                        dd._context.SaveChanges();
                        tblBranch = new Mst_Branch();
                        tblBranch.BranchName = branch.BranchName;
                        tblBranch.BranchCode = branch.BranchCode;
                        tblBranch.BranchType = branch.BranchType;
                        tblBranch.InceptionDate = Convert.ToDateTime(branch.DateInception);
                        if (tblBranch.BranchType == 2)
                        {
                            tblBranch.RentPeriodAgreed = null;
                        }
                        else
                        {
                            tblBranch.RentPeriodAgreed = Convert.ToDateTime(branch.RentPeriodAgreed);
                        }
                        //tblBranch.DateWEF = Convert.ToDateTime(branch.DateWEF);
                        tblBranch.Address = branch.Address;
                        tblBranch.Pincode = branch.Pincode;
                        tblBranch.ContactPerson = branch.ContactPerson;
                        tblBranch.MobileNo = branch.MobileNo;
                        tblBranch.InTime = branch.InTime;
                        tblBranch.OutTime = branch.OutTime;
                        tblBranch.Status = branch.Status;
                        tblBranch.RecordUpdated = DateTime.Now;
                        tblBranch.RecordUpdatedBy = branch.UpdatedBy;
                        tblBranch.RecordCreated = DateTime.Now;
                        tblBranch.RecordCreatedBy = branch.CreatedBy;
                        dd._context.Mst_Branch.Add(tblBranch);
                    }
                    else
                    {
                        tblBranch.BranchName = branch.BranchName;
                        tblBranch.BranchCode = branch.BranchCode;
                        tblBranch.BranchType = branch.BranchType;
                        tblBranch.InceptionDate = Convert.ToDateTime(branch.DateInception);
                        if (tblBranch.BranchType == 2)
                        {
                            tblBranch.RentPeriodAgreed = null;
                        }
                        else
                        {
                            tblBranch.RentPeriodAgreed = Convert.ToDateTime(branch.RentPeriodAgreed);
                        }
                        //tblBranch.DateWEF = Convert.ToDateTime(branch.DateWEF);
                        tblBranch.Address = branch.Address;
                        tblBranch.Pincode = branch.Pincode;
                        tblBranch.ContactPerson = branch.ContactPerson;
                        tblBranch.MobileNo = branch.MobileNo;
                        tblBranch.InTime = branch.InTime;
                        tblBranch.OutTime = branch.OutTime;
                        tblBranch.Status = branch.Status;
                        tblBranch.RecordUpdated = DateTime.Now;
                        tblBranch.RecordUpdatedBy = branch.UpdatedBy;
                    }
                }
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
            Mst_Branch tblBranch = dd._context.Mst_Branch.Where(x => x.ID == ID && x.Status == 1).FirstOrDefault();
            BranchViewModel branch = new BranchViewModel();
            branch.BranchName = tblBranch.BranchName;
            branch.BranchCode = tblBranch.BranchCode;
            branch.BranchType = tblBranch.BranchType;
            branch.DateInception = tblBranch.InceptionDate.ToShortDateString();
            branch.RentPeriodAgreed = Convert.ToDateTime(tblBranch.RentPeriodAgreed).ToShortDateString();
            branch.DateWEF = Convert.ToDateTime(tblBranch.DateWEF).ToShortDateString();
            branch.Address = tblBranch.Address;
            branch.Pincode = (int)tblBranch.Pincode;
            branch.ContactPerson = tblBranch.ContactPerson;
            branch.MobileNo = tblBranch.MobileNo;
            branch.InTime = tblBranch.InTime;
            branch.OutTime = tblBranch.OutTime;
            branch.operation = operation;
            branch.Status = (short)tblBranch.Status;
            //ViewBag.PincodeList = new SelectList(dd._context.Mst_PinCode.ToList(), "Pc_Id", "Pc_Desc");
            var pincodelist = dd._context.Mst_PinCode.Select(x => new
            {
                PcId = x.Pc_Id,
                PincodeWithArea = x.Pc_Desc + "(" + x.Pc_AreaName + ")"
            }).ToList();
            ViewBag.PincodeList = new SelectList(pincodelist, "PcId", "PincodeWithArea");
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
            //if (dd._context.Mst_PinCode.Any(o => o.Pc_Id == id))
            //{
            //    data = "Record Cannot Be Deleted Already In Use!";

            //    return Json(data, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            var deleterecord = dd._context.Mst_Branch.Where(x => x.ID == id && x.Status == 1).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_Branch.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
            //}
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
            var pincodelist = dd._context.Mst_PinCode.Select(x => new
            {
                PcId = x.Pc_Id,
                PincodeWithArea = x.Pc_Desc + "(" + x.Pc_AreaName + ")"
            }).ToList();

            ViewBag.PincodeList = new SelectList(pincodelist, "PcId", "PincodeWithArea");
            return View(model);
        }

        public ActionResult GetBranchTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            var tablelist = dd._context.Mst_Branch.Where(x => x.Status == 1).ToList();
            List<BranchViewModel> list = new List<BranchViewModel>();
            var model = new BranchViewModel();
            foreach (var item in tablelist)
            {
                model = new BranchViewModel();
                model.ID = item.ID;
                model.BranchName = item.BranchName;
                model.BranchCode = item.BranchCode;
                model.DateInception = item.InceptionDate.ToShortDateString();
                model.DateWEF = Convert.ToDateTime(item.DateWEF).ToShortDateString();
                model.Address = item.Address;
                model.StatusStr = item.Status == 1 ? "Active" : "Inactive";
                list.Add(model);
            }
            return PartialView("_BranchList", list);
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