﻿using HDNHD.Core.Models;
using HoaDonNuocHaDong.Areas.ThuNgan.Helpers;
using HoaDonNuocHaDong.Areas.ThuNgan.Models;
using HoaDonNuocHaDong.Areas.ThuNgan.Repositories;
using HoaDonNuocHaDong.Areas.ThuNgan.Repositories.Interfaces;
using HoaDonNuocHaDong.Base;
using HoaDonNuocHaDong.Repositories;
using HoaDonNuocHaDong.Repositories.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace HoaDonNuocHaDong.Areas.ThuNgan.Controllers
{
    public class HoaDonController : BaseController
    {
        private IHoaDonRepository hoaDonRepository;
        private IToRepository toRepository;

        public HoaDonController()
        {
            hoaDonRepository = uow.Repository<HoaDonRepository>();
            toRepository = uow.Repository<ToRepository>();
        }

        /// <summary>
        /// view list of HoaDon with filter
        /// </summary>
        public ActionResult Index(HoaDonFilterModel filter, Pager pager, String todo)
        {
            var current = DateTime.Now.AddMonths(-1);

            // default values
            if (filter.Mode == HoaDonFilterModel.FilterByManagementInfo) // not in filter
            {
                if ((filter.Month == null && filter.Year == null) ||
                    filter.TrangThaiThu == HDNHD.Models.Constants.ETrangThaiThu.DaQuaHan)
                {
                    filter.Month = current.Month;
                    filter.Year = current.Year;

                    if (filter.TrangThaiThu == null)
                        filter.TrangThaiThu = HDNHD.Models.Constants.ETrangThaiThu.ChuaNopTien;
                    if (filter.HinhThucThanhToan == null)
                        filter.HinhThucThanhToan = HDNHD.Models.Constants.EHinhThucThanhToan.TienMat;
                }

                // set selected to, quan huyen = nhanVien's to, quan huyen
                if (nhanVien != null && filter.QuanHuyenID == null)
                {
                    filter.NhanVienID = nhanVien.NhanvienID;
                    filter.ToID = nhanVien.ToQuanHuyenID;

                    var to = toRepository.GetByID(nhanVien.ToQuanHuyenID ?? 0);
                    if (to != null)
                    {
                        filter.QuanHuyenID = to.QuanHuyenID;
                    }
                }
            }

            // query items
            var items = hoaDonRepository.GetAllHoaDonModel();
            items = filter.ApplyFilter(items);

            // apply actions
            //if (todo == "DanhDauTatCa")
            //{
            //    foreach (var item in items)
            //    {
            //        HoaDonHelpers.ThanhToan(item, DateTime.Now, uow);
            //    }
            //}

            items = pager.ApplyPager(items);

            #region view data
            title = "Quản lý công nợ khách hàng";

            ViewBag.Filter = filter;
            ViewBag.Pager = pager;
            ViewBag.Current = current;
            #endregion
            return View(items.ToList());
        }
        
        public ActionResult ThemGiaoDich(int hoaDonID, Pager pager)
        {
            IGiaoDichRepository giaoDichRepository = uow.Repository<GiaoDichRepository>();
            var model = hoaDonRepository.GetHoaDonModelByID(hoaDonID);
            
            if (model == null)
                return HttpNotFound();
            
            var giaoDichs = giaoDichRepository.GetAllGiaoDichModelByKHID(model.KhachHang.KhachhangID);
            giaoDichs = pager.ApplyPager(giaoDichs);
            #region view data
            ViewBag.HoaDonModel = model;
            ViewBag.Pager = pager;
            #endregion
            return View(giaoDichs.ToList());
        }
    }
}