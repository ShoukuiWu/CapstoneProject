//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConeInfoManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Person
    {
        public Person()
        {
            this.Accounts = new HashSet<Account>();
            this.Employees = new HashSet<Employee>();
        }
    
        public int number { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string displayName { get; set; }
        public string phoneHome { get; set; }
        public string phoneOffice { get; set; }
        public string phoneMobile { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string provinceCode { get; set; }
        public string countryCode { get; set; }
        public string postalCode { get; set; }
        public string collegeEmail { get; set; }
        public string personalEmail { get; set; }
    
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual Province Province { get; set; }
    }
}
