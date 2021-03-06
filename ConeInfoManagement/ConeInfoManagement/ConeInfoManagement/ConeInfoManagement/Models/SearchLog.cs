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
    
    public partial class SearchLog
    {
        public int id { get; set; }
        public string query { get; set; }
        public int searchResultCount { get; set; }
        public System.DateTime searchDate { get; set; }
        public Nullable<int> keywordType { get; set; }
        public Nullable<int> searchId { get; set; }
    
        public virtual Keyword Keyword { get; set; }
        public virtual Search Search { get; set; }
    }
}
