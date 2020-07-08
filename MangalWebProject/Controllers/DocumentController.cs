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
    public class DocumentController : BaseController
    {
        DataManager dd = new DataManager();
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult CreateEdit(DocumentViewModel document)
        {
            document.CreatedBy = Convert.ToInt32(Session["UserLoginId"]);
            document.UpdatedBy = Convert.ToInt32(Session["UserLoginId"]);
            Mst_DocumentMaster tblDocument = new Mst_DocumentMaster();
            try
            {
                if (document.ID <= 0)
                {
                    var data = dd._context.Mst_DocumentMaster.Where(u => u.Doc_DocumentName == document.DocumentName && u.Doc_Status == 1).Select(x => x.Doc_DocumentName).FirstOrDefault();
                    if (data != null)
                    {
                        ModelState.AddModelError("DocumentName", "Document Name Already Exists");
                        return Json(document);
                    }
                    tblDocument.Doc_Id = document.ID;
                    tblDocument.Doc_RecordCreated = DateTime.Now;
                    tblDocument.Doc_RecordCreatedBy = document.CreatedBy;
                    dd._context.Mst_DocumentMaster.Add(tblDocument);
                }
                else
                {
                    tblDocument = dd._context.Mst_DocumentMaster.Where(x => x.Doc_Id == document.ID).FirstOrDefault();
                }
                tblDocument.Doc_DocumentName = document.DocumentName;
                tblDocument.Doc_DocumentType = document.DocumentType;
                tblDocument.Doc_ExpiryDateApplicable = false;
                if (document.ExpiryApplicableStr == "Yes")
                    tblDocument.Doc_ExpiryDateApplicable = true;
                tblDocument.Doc_Status = document.DocumentStatus;
                tblDocument.Doc_RecordUpdated = DateTime.Now;
                tblDocument.Doc_RecordUpdatedBy = document.UpdatedBy;
                dd._context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(document);
        }

        public ActionResult GetDocumentById(int ID)
        {
            string operation = Session["Operation"].ToString();
            ButtonVisiblity(operation);
            Mst_DocumentMaster tblDocument = dd._context.Mst_DocumentMaster.Where(x => x.Doc_Id == ID).FirstOrDefault();
            DocumentViewModel document = new DocumentViewModel();
            document.ID = tblDocument.Doc_Id;
            document.DocumentName = tblDocument.Doc_DocumentName;
            document.DocumentType = (short)tblDocument.Doc_DocumentType;
            document.ExpiryDateApplicable = (bool)tblDocument.Doc_ExpiryDateApplicable;
            document.ExpiryApplicableStr = "No";
            if (document.ExpiryDateApplicable==true)
            {
                document.ExpiryApplicableStr = "Yes";
            }
            document.DocumentStatus = (short)tblDocument.Doc_Status;
            document.operation = operation;
            return View("Document", document);
        }

        // GETDelete/5
        public ActionResult Delete(int id)
        {
            var deleterecord = dd._context.Mst_DocumentMaster.Where(x => x.Doc_Id == id).FirstOrDefault();
            if (deleterecord != null)
            {
                dd._context.Mst_DocumentMaster.Remove(deleterecord);
                dd._context.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult doesDocumentNameExist(string DocumentName)
        {
            var data = dd._context.Mst_DocumentMaster.Where(u => u.Doc_DocumentName == DocumentName && u.Doc_Status == 1).Select(x => x.Doc_DocumentName).FirstOrDefault();
            var result = "";
            //Check if document name already exists
            if (data != null)
            {
                if (DocumentName.ToLower() == data.ToLower().ToString())
                {
                    result = "Document Name Already Exists";
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }

        public ActionResult Document()
        {
            ButtonVisiblity("Index");
            return View();
        }

        public ActionResult GetDocumentTable(string Operation)
        {
            Session["Operation"] = Operation;
            //ButtonVisiblity(Operation);
            List<DocumentViewModel> list = new List<DocumentViewModel>();
            var model = new DocumentViewModel();
            var tablelist = dd._context.Mst_DocumentMaster.ToList();
            int status = 1;
            bool expiryapplicable =true;
            foreach (var item in tablelist)
            {
                model = new DocumentViewModel();
                model.ID = item.Doc_Id;
                model.DocumentName = item.Doc_DocumentName;
                model.DocumentTypeStr = GetDocumentType((short)item.Doc_DocumentType);
                model.ExpiryApplicableStr = item.Doc_ExpiryDateApplicable == expiryapplicable ? "Yes" : "No";
                model.DocumentStatusStr = item.Doc_Status == status ? "Active" : "Inactive";
                list.Add(model);
            }
            return PartialView("_DocumentList", list);
        }

        public string GetDocumentType(short documenttypeid)
        {
            string documenttypestr = "";
            switch (documenttypeid)
            {
                case 1:
                    documenttypestr = "ID Proof";
                    break;
                case 2:
                    documenttypestr = "Address Proof";
                    break;
                case 3:
                    documenttypestr = "Legal Documents";
                    break;
                case 4:
                    documenttypestr = "Income Proof";
                    break;
                case 5:
                    documenttypestr = "Others";
                    break;
                default:
                    break;
            }
            return documenttypestr;
        }
    }
}