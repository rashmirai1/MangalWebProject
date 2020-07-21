using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MangalWebProject.Models
{
    public class DocumentUploadViewModel
    {
        public int ID { get; set; }

        public int TransactionId { get; set; }
        public string TransactionNumber { get; set; }
        public string CustomerId { get; set; }
        public string ApplicationNo { get; set; }
        public string LoanAccountNo { get; set; }

        public int DocumentTypeId { get; set; }
        public int DocumentId { get; set; }
        public string ExpiryDate { get; set; }
        public string UploadDocName { get; set; }
        public string Comments { get; set; }
        public int BranchId { get; set; }
        public int FinancialYearId { get; set; }
        public int CompanyId { get; set; }
        public int VerifiedBy { get; set; }
        public string Status { get; set; }
        public  string ReasonForRejection { get; set; }
        public string VerifyComment { get; set; }

        public string operation { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public List<DocumentUploadViewModel> DocumentUploadList { get; set; }
    }
}