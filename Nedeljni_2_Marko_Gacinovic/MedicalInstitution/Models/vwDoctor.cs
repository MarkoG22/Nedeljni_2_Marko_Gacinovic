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
    
    public partial class vwDoctor
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string IdCard { get; set; }
        public string Gender { get; set; }
        public System.DateTime Birthdate { get; set; }
        public string Citizenship { get; set; }
        public bool Manager { get; set; }
        public string Username { get; set; }
        public string Pasword { get; set; }
        public int DoctorID { get; set; }
        public string AccountNumber { get; set; }
        public string Department { get; set; }
        public Nullable<int> ManagerID { get; set; }
        public Nullable<bool> Reception { get; set; }
        public Nullable<int> ShiftID { get; set; }
        public string UniqueNumber { get; set; }
    }
}