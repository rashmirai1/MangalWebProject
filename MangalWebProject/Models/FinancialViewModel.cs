﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MangalWebProject.Models
{
    public class FinancialViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "FinancialYear From is Required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public string FinancialYearFrom { get; set; }

        [Required(ErrorMessage = "FinancialYear To is Required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public string FinancialYearTo { get; set; }

        public string FinacialYear { get; set; }

        public string operation { get; set; }
    }
}