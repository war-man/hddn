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
    
    public partial class Quyencuanguoidung
    {
        public Nullable<int> NguoidungID { get; set; }
        public Nullable<int> QuyenID { get; set; }
        public int QuyencuanguoidungID { get; set; }
    
        public virtual Nguoidung Nguoidung { get; set; }
        public virtual Quyen Quyen { get; set; }
    }
}
