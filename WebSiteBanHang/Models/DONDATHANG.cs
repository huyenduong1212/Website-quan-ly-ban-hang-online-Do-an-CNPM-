//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebSiteBanHang.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DONDATHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONDATHANG()
        {
            this.CHITIETDONDATHANGs = new HashSet<CHITIETDONDATHANG>();
        }
    
        public int MaDDH { get; set; }
        public Nullable<System.DateTime> ThoiDiemDat { get; set; }
        public Nullable<int> TinhTrangGiaoHang { get; set; }
        public Nullable<System.DateTime> ThoiDiemLap { get; set; }
        public Nullable<System.DateTime> NgayGiaoDuKien { get; set; }
        public Nullable<int> UuDai { get; set; }
        public Nullable<decimal> TongTien { get; set; }
        public Nullable<int> MaNV { get; set; }
        public Nullable<int> MaKH { get; set; }
        public Nullable<int> MaGioHang { get; set; }
        public Nullable<decimal> PhiVanChuyen { get; set; }
        public String DiaChiNhanHang { get; set; }
        public String SoDienThoaiNhanHang { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETDONDATHANG> CHITIETDONDATHANGs { get; set; }
        public virtual GIOHANG GIOHANG { get; set; }
    }
}
