//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WcfServiceConeInfo
{
    using System;
    using System.Collections.Generic;
    
    public partial class Province
    {
        public Province()
        {
            this.People = new HashSet<Person>();
        }
    
        public string code { get; set; }
        public string countryCode { get; set; }
        public string name { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual ICollection<Person> People { get; set; }
    }
}
