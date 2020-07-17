//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MangalWebProject.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mst_Branch
    {
        public int ID { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public short BranchType { get; set; }
        public System.DateTime InceptionDate { get; set; }
        public Nullable<System.DateTime> RentPeriodAgreed { get; set; }
        public string Address { get; set; }
        public int Pincode { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public Nullable<System.DateTime> DateWEF { get; set; }
        public Nullable<short> Status { get; set; }
        public Nullable<System.DateTime> RecordCreated { get; set; }
        public Nullable<System.DateTime> RecordUpdated { get; set; }
        public Nullable<int> RecordCreatedBy { get; set; }
        public Nullable<int> RecordUpdatedBy { get; set; }
    
        public virtual Mst_PinCode Mst_PinCode { get; set; }
    }
}
