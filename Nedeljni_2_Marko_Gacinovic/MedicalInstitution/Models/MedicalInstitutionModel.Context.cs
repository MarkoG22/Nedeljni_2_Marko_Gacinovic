﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MedicalInstitutionEntities : DbContext
    {
        public MedicalInstitutionEntities()
            : base("name=MedicalInstitutionEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblDoctor> tblDoctors { get; set; }
        public virtual DbSet<tblHospital> tblHospitals { get; set; }
        public virtual DbSet<tblMaintance> tblMaintances { get; set; }
        public virtual DbSet<tblManager> tblManagers { get; set; }
        public virtual DbSet<tblPatient> tblPatients { get; set; }
        public virtual DbSet<tblShift> tblShifts { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<vwDoctor> vwDoctors { get; set; }
        public virtual DbSet<vwMaintance> vwMaintances { get; set; }
        public virtual DbSet<vwManager> vwManagers { get; set; }
        public virtual DbSet<vwPatient> vwPatients { get; set; }
    }
}