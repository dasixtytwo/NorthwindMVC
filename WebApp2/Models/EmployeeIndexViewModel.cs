using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using dasixtytwo.lib;
using Microsoft.AspNetCore.Mvc;

namespace WebApp2.Models
{
    public class EmployeeIndexViewModel
    {
        public IList<Employee> Employees { get; set; }
    }
}