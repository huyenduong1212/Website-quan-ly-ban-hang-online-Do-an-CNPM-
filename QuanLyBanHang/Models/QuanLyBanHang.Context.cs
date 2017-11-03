﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyBanHang.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class QuanLyBanHangEntities : DbContext
    {
        public QuanLyBanHangEntities()
            : base("name=QuanLyBanHangEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<BINHLUAN> BINHLUANs { get; set; }
        public DbSet<CHITIETDONDATHANG> CHITIETDONDATHANGs { get; set; }
        public DbSet<CHITIETGIOHANG> CHITIETGIOHANGs { get; set; }
        public DbSet<CHITIETPHIEUNHAP> CHITIETPHIEUNHAPs { get; set; }
        public DbSet<CHUCNANG> CHUCNANGs { get; set; }
        public DbSet<DONDATHANG> DONDATHANGs { get; set; }
        public DbSet<GIOHANG> GIOHANGs { get; set; }
        public DbSet<LOAINGUOIDUNG> LOAINGUOIDUNGs { get; set; }
        public DbSet<LOAISANPHAM> LOAISANPHAMs { get; set; }
        public DbSet<NGUOIDUNG> NGUOIDUNGs { get; set; }
        public DbSet<NHACUNGCAP> NHACUNGCAPs { get; set; }
        public DbSet<PHIEUNHAP> PHIEUNHAPs { get; set; }
        public DbSet<SANPHAM> SANPHAMs { get; set; }
        public DbSet<SUKIEN> SUKIENs { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
    
        public virtual int DangKy(string hoten, Nullable<bool> gioiTinh, string diaChi, string soDienThoai, string email, string taiKhoan, string matKhau)
        {
            var hotenParameter = hoten != null ?
                new ObjectParameter("Hoten", hoten) :
                new ObjectParameter("Hoten", typeof(string));
    
            var gioiTinhParameter = gioiTinh.HasValue ?
                new ObjectParameter("GioiTinh", gioiTinh) :
                new ObjectParameter("GioiTinh", typeof(bool));
    
            var diaChiParameter = diaChi != null ?
                new ObjectParameter("DiaChi", diaChi) :
                new ObjectParameter("DiaChi", typeof(string));
    
            var soDienThoaiParameter = soDienThoai != null ?
                new ObjectParameter("SoDienThoai", soDienThoai) :
                new ObjectParameter("SoDienThoai", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var taiKhoanParameter = taiKhoan != null ?
                new ObjectParameter("TaiKhoan", taiKhoan) :
                new ObjectParameter("TaiKhoan", typeof(string));
    
            var matKhauParameter = matKhau != null ?
                new ObjectParameter("MatKhau", matKhau) :
                new ObjectParameter("MatKhau", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DangKy", hotenParameter, gioiTinhParameter, diaChiParameter, soDienThoaiParameter, emailParameter, taiKhoanParameter, matKhauParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    }
}
