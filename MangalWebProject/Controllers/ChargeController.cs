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
    public class ChargeController : BaseController
    {
        //date time done
        DataManager dd = new DataManager();

        #region Charge

        public ActionResult Charge()
        {
            try
            {
                ButtonVisiblity("Index");
                var chargeviewmodel = new ChargeViewModel();
                var chargetrn = new ChargeDetailsViewModel();
                chargeviewmodel.chargeDetailsCollection = new List<ChargeDetailsViewModel>();
                chargeviewmodel.ReferenceDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                //purchaseviewmodel.PurchaseTrnCollection.Add(purchasetrn);
                return View(chargeviewmodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Purchase

        #region Insert

        [HttpPost]
        public ActionResult Insert(ChargeViewModel objViewModel)
        {
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    if (objViewModel.ID == 0)
                    {
                        if (InsertData(objViewModel))
                        {
                            return Json(1, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(3, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        if (UpdateData(objViewModel))
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                objViewModel.chargeDetailsCollection = new List<ChargeDetailsViewModel>();
                return View("Charge", objViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Insert

        #region Insert Data

        public bool InsertData(ChargeViewModel chargeViewModel)
        {
            bool retVal = false;
            chargeViewModel.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            chargeViewModel.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_Charge tblCharge = new Mst_Charge();
            Mst_ChargeDetails tblChargeDetails = new Mst_ChargeDetails();
            try
            {
                tblCharge.Chg_Name = chargeViewModel.ChargeName;
                tblCharge.Chg_ReferenceDate = chargeViewModel.ReferenceDate;
                tblCharge.Chg_Status = chargeViewModel.Status;
                tblCharge.Chg_RecordCreated = DateTime.Now;
                tblCharge.Chg_RecordCreatedBy = chargeViewModel.CreatedBy;
                tblCharge.Chg_RecordUpdated = DateTime.Now;
                tblCharge.Chg_RecordUpdatedBy = chargeViewModel.UpdatedBy;
                dd._context.Mst_Charge.Add(tblCharge);
                dd._context.SaveChanges();

                int PID = dd._context.Mst_Charge.Max(x => x.Chg_Id);

                //save the data in Charge Details table
                foreach (var p in chargeViewModel.chargeDetailsCollection)
                {
                    var chargetrn = new Mst_ChargeDetails
                    {
                        Chgd_ChgRefId = PID,
                        Chgd_LoanAmountGreater = p.LoanAmountGreaterthan,
                        Chgd_LoanAmountLess = p.LoanAmountLessthan,
                        Chgd_ChargesAmt = p.ChargeAmount,
                        Chgd_ChargeType = p.ChargeType,
                        Chg_RecordCreatedBy = Convert.ToInt32(Session["UserLoginId"]),
                        Chg_RecordCreated = DateTime.Now,
                        Chg_RecordUpdatedBy = Convert.ToInt32(Session["UserLoginId"]),
                        Chg_RecordUpdated = DateTime.Now,
                    };
                    dd._context.Mst_ChargeDetails.Add(chargetrn);
                    dd._context.SaveChanges();
                }
                retVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        #endregion Insert Data

        #region Update Data

        public bool UpdateData(ChargeViewModel chargeViewModel)
        {
            bool retVal = false;
            try
            {
                var chargeObj = dd._context.Mst_Charge.Where(x => x.Chg_Id == chargeViewModel.ID).FirstOrDefault();

                //update the data in charge table
                chargeObj.Chg_Name = chargeViewModel.ChargeName;
                chargeObj.Chg_ReferenceDate = chargeViewModel.ReferenceDate;
                chargeObj.Chg_Status = chargeViewModel.Status;
                chargeObj.Chg_RecordUpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
                chargeObj.Chg_RecordUpdated = DateTime.Now;

                List<Mst_ChargeDetails> NewChargeDetailsList = new List<Mst_ChargeDetails>();

                //update the data in Charge Details table
                foreach (var p in chargeViewModel.chargeDetailsCollection)
                {
                    var FindChargebject = dd._context.Mst_ChargeDetails.Where(x => x.Chgd_Id == p.ID && x.Chgd_ChgRefId== chargeViewModel.ID).FirstOrDefault();
                    if (FindChargebject == null)
                    {
                        var chargetrnnew = new Mst_ChargeDetails
                        {
                            Chgd_ChgRefId = chargeViewModel.ID,
                            Chgd_LoanAmountGreater = p.LoanAmountGreaterthan,
                            Chgd_LoanAmountLess = p.LoanAmountLessthan,
                            Chgd_ChargesAmt = p.ChargeAmount,
                            Chgd_ChargeType = p.ChargeType,
                            Chg_RecordCreatedBy = Convert.ToInt32(Session["UserLoginId"]),
                            Chg_RecordCreated = DateTime.Now,
                            Chg_RecordUpdatedBy = Convert.ToInt32(Session["UserLoginId"]),
                            Chg_RecordUpdated = DateTime.Now
                        };
                        dd._context.Mst_ChargeDetails.Add(chargetrnnew);
                    }
                    else
                    {
                        FindChargebject.Chgd_LoanAmountGreater = p.LoanAmountGreaterthan;
                        FindChargebject.Chgd_LoanAmountLess = p.LoanAmountLessthan;
                        FindChargebject.Chgd_ChargesAmt = p.ChargeAmount;
                        FindChargebject.Chgd_ChargeType = p.ChargeType;
                        FindChargebject.Chg_RecordUpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
                        FindChargebject.Chg_RecordUpdated = DateTime.Now;
                    }
                    NewChargeDetailsList.Add(FindChargebject);
                }
                #region charge details remove
                //take the loop of table and check from list if found in list then not remove else remove from table itself
                var chargetrnobjlist = dd._context.Mst_ChargeDetails.Where(x => x.Chgd_ChgRefId == chargeViewModel.ID).ToList();
                if (chargetrnobjlist != null)
                {
                    foreach (Mst_ChargeDetails item in chargetrnobjlist)
                    {
                        if (NewChargeDetailsList.Contains(item))
                        {
                            continue;
                        }
                        else
                        {
                            dd._context.Mst_ChargeDetails.Remove(item);
                        }
                    }
                    dd._context.SaveChanges();
                }
                #endregion charge trn remove
                retVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        #endregion Update Data


        #region Delete

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            try
            {
                var chargetrndata = dd._context.Mst_ChargeDetails.Where(x => x.Chgd_ChgRefId == id).ToList();
                //Delete the data from Installation Type Data
                if (chargetrndata != null)
                {
                    foreach (var chargetrn in chargetrndata)
                    {
                        dd._context.Mst_ChargeDetails.Remove(chargetrn);
                    }
                    dd._context.SaveChanges();
                }
                var chargedata = dd._context.Mst_Charge.Find(id);
                dd._context.Mst_Charge.Remove(chargedata);
                dd._context.SaveChanges();
                return Json(JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Delete

        #region GetChargeTable

        public ActionResult GetChargeTable(string Operation)
        {
            Session["Operation"] = Operation;
            ButtonVisiblity(Operation);
            List<ChargeViewModel> list = new List<ChargeViewModel>();
            var model = new ChargeViewModel();
            var tablelist = dd._context.Mst_Charge.ToList();
            foreach (var item in tablelist)
            {
                model = new ChargeViewModel();
                model.ID = item.Chg_Id;
                model.ChargeName = item.Chg_Name;
                model.ReferenceDate = item.Chg_ReferenceDate;
                model.StatusStr = item.Chg_Status == 1 ? "Active" : "Inacitve";
                list.Add(model);
            }
            return PartialView("_ChargePartialTable", list);
        }

        #endregion GetChargeTable

        #region GetChargeDetailsById

        public ActionResult GetChargeDetailsById(int ID)
        {
            try
            {
                ChargeViewModel chargeViewModel = new ChargeViewModel();
                string operation = Session["Operation"].ToString();
                //get installation and service table data
                var chargeobj = dd._context.Mst_Charge.Where(x => x.Chg_Id == ID).FirstOrDefault();
                //get charge trn table data
                var purchasetrndatalist = dd._context.Mst_ChargeDetails.Where(x => x.Chgd_ChgRefId == ID).ToList();
                chargeViewModel = ToViewModelPurchase(chargeobj, purchasetrndatalist);
                chargeViewModel.operation = operation;
                return View("Charge", chargeViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion GetChargeDetailsById

        public static ChargeViewModel ToViewModelPurchase(Mst_Charge charge, ICollection<Mst_ChargeDetails> ChargeTrnList)
        {
            var purchaseviewmodel = new ChargeViewModel
            {
                ChargeName = charge.Chg_Name,
                ReferenceDate = charge.Chg_ReferenceDate,
                Status = charge.Chg_Status,
                ID = charge.Chg_Id,
            };

            IList<ChargeDetailsViewModel> ChargeTrnViewModelList = new List<ChargeDetailsViewModel>();
            foreach (var c in ChargeTrnList)
            {
                var ChargeTrnViewModel = new ChargeDetailsViewModel
                {
                    ID = c.Chgd_Id,
                    LoanAmountGreaterthan = c.Chgd_LoanAmountGreater,
                    LoanAmountLessthan = c.Chgd_LoanAmountLess,
                    ChargeAmount = c.Chgd_ChargesAmt,
                    ChargeType = (short)c.Chgd_ChargeType,
                    ChargeTypeStr = c.Chgd_ChargeType == 1 ? "Amount" : "Percentage"
                };
                ChargeTrnViewModelList.Add(ChargeTrnViewModel);
            }
            purchaseviewmodel.chargeDetailsCollection = ChargeTrnViewModelList;
            return purchaseviewmodel;
        }
    }
}