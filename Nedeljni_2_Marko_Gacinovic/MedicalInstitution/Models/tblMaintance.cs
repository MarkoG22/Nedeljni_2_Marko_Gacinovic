//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedicalInstitution.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblMaintance
    {
        public int MaintanceID { get; set; }
        public int UserID { get; set; }
        public bool GrowPermision { get; set; }
        public bool InvalidDuty { get; set; }
        public bool AmbulanceDuty { get; set; }
    
        public virtual tblUser tblUser { get; set; }
    }
}
