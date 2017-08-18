//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HoaDonNuocHaDong
{
    using System;
    using System.Collections.Generic;
    
    public partial class Khachhang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Khachhang()
        {
            this.Congnoes = new HashSet<Congno>();
            this.DuCoes = new HashSet<DuCo>();
            this.Hoadonnuocs = new HashSet<Hoadonnuoc>();
        }
    
        public int KhachhangID { get; set; }
        public Nullable<int> QuanhuyenID { get; set; }
        public Nullable<int> PhuongxaID { get; set; }
        public Nullable<int> CumdancuID { get; set; }
        public Nullable<int> TuyenKHID { get; set; }
        public Nullable<int> LoaiKHID { get; set; }
        public Nullable<int> LoaiapgiaID { get; set; }
        public Nullable<int> HinhthucttID { get; set; }
        public Nullable<int> TuyenongkythuatID { get; set; }
        public string Sotaikhoan { get; set; }
        public string Masothue { get; set; }
        public Nullable<System.DateTime> Ngaykyhopdong { get; set; }
        public Nullable<int> Tilephimoitruong { get; set; }
        public Nullable<int> Soho { get; set; }
        public Nullable<System.DateTime> Ngayap { get; set; }
        public Nullable<System.DateTime> Ngayhetap { get; set; }
        public Nullable<int> Sonhankhau { get; set; }
        public string Ten { get; set; }
        public string Diachi { get; set; }
        public string Dienthoai { get; set; }
        public string Ghichu { get; set; }
        public Nullable<int> Sokhuvuc { get; set; }
        public string Sohopdong { get; set; }
        public Nullable<int> Tinhtrang { get; set; }
        public string MaKhachHang { get; set; }
        public Nullable<short> Phibaovemoitruong { get; set; }
        public string Diachithutien { get; set; }
        public Nullable<int> TTDoc { get; set; }
        public Nullable<System.DateTime> Ngaythanhly { get; set; }
        public string Lydothanhly { get; set; }
        public Nullable<System.DateTime> Ngayngungcapnuoc { get; set; }
        public string Lydongungcapnuoc { get; set; }
        public Nullable<System.DateTime> Ngaycapnuoclai { get; set; }
        public string Lydocapnuoclai { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> Ngaylapdat { get; set; }
        public Nullable<int> Chisolapdat { get; set; }
        public Nullable<long> CreatedTime { get; set; }
        public Nullable<long> UpdatedTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Congno> Congnoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DuCo> DuCoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hoadonnuoc> Hoadonnuocs { get; set; }
        public virtual Phuongxa Phuongxa { get; set; }
        public virtual Tuyenkhachhang Tuyenkhachhang { get; set; }
    }
}
