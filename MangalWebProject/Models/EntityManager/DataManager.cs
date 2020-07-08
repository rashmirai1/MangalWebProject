using MangalWebProject.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MangalWebProject.Models.EntityManager
{
    public class DataManager
    {
        public MangalDBEntities _context { get; set; }
        public DataManager()
        {
            _context = new MangalDBEntities();
        }
    }
}