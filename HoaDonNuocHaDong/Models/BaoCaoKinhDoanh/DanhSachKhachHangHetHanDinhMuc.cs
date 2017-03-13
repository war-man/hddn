﻿using HvitFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoaDonNuocHaDong.Models.BaoCaoKinhDoanh
{
    public class DanhSachKhachHangHetHanDinhMuc:ModelBase
    {
        public int TT { get { return GetINT(0); } set { SetINT(0, value); } }
        public string Tuyen { get { return GetSTR(1); } set { SetSTR(1, value); } }
        public int ThuTuDoc { get { return GetINT(2); } set { SetINT(2, value); } }
        public string MaKhachHang { get { return GetSTR(3); } set { SetSTR(3, value); } }
        public string Ten { get { return GetSTR(4); } set { SetSTR(4, value); } }
        public string Diachi { get { return GetSTR(5); } set { SetSTR(5, value); } }
        public int SoDinhMuc { get { return GetINT(6); } set { SetINT(6, value); } }
        public DateTime NgayBatDau { get { return GetDT(7); } set { SetDT(7, value); } }
        public DateTime NgayKetThuc { get { return GetDT(8); } set { SetDT(8, value); } }
        public string GhiChu { get { return GetSTR(9); } set { SetSTR(9, value); } }

        public DanhSachKhachHangHetHanDinhMuc()
        {
            MaxPosModelField = 9;
        }
        protected override Type TransferType()
        {
            return this.GetType();    
        }
    }
}