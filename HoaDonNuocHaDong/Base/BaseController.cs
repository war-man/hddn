﻿using HDNHD.Core.Constants;
using HDNHD.Core.Controllers;
using HDNHD.Core.Models;
using HDNHD.Models.Constants;
using HoaDonNuocHaDong.Models.KhachHang;
using HoaDonNuocHaDong.Repositories;
using HoaDonNuocHaDong.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoaDonNuocHaDong.Base
{
    public class BaseController : SecuredController
    {
        protected HDNHDUnitOfWork uow;
        protected HoaDonHaDongEntities db = new HoaDonHaDongEntities();
        protected HDNHD.Models.DataContexts.Nhanvien nhanVien;
        protected HDNHD.Models.DataContexts.Phongban phongBan;

        // view data
        protected string title = "hdn";

        public BaseController()
        {
            uow = new HDNHDUnitOfWork();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            INhanVienRepository nhanVienRepository = uow.Repository<NhanVienRepository>();
            IPhongBanRepository phongBanRepository = uow.Repository<PhongBanRepository>();

            var role = EUserRole.Admin;
            // load nhanVien for LoggedInUser
            nhanVien = nhanVienRepository.GetByID(LoggedInUser.NhanvienID ?? 0);

            if (nhanVien != null)
            {
                phongBan = phongBanRepository.GetSingle(m => m.PhongbanID == nhanVien.PhongbanID);

                if (phongBan != null)
                {
                    var tenPhongBan = phongBan.Ten.ToLower();

                    if (tenPhongBan.Contains("kinh"))
                    {
                        role = EUserRole.KinhDoanh;
                    }
                    else if (tenPhongBan.Contains("in"))
                    {
                        role = EUserRole.InHoaDon;
                    }
                    else // add more role here
                    {
                        role = EUserRole.ThuNgan;
                    }
                }
            }
            // cache role
            RequestScope.UserRole = role;
            //kiểm tra ngày tháng hiện tại, nếu ngày tháng hiện tại mà lớn hơn ngày hết áp định thì cho số hộ(số định mức) = 1
            var currentDate = DateTime.Now;
            ModelKhachHang modelKH = new ModelKhachHang();
            modelKH.updateKHHetHanApGia(currentDate);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            // view data
            ViewBag.Title = title;
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int chucNangID = getChucNangIDFromUrl(controllerName, actionName);
            appendToLogTable(chucNangID);
        }

        private void appendToLogTable(int chucNangID)
        {
            Lichsusudungct lichSu = new Lichsusudungct();
            lichSu.ChucnangID = chucNangID;
            lichSu.NguoidungID = LoggedInUser.NguoidungID;
            lichSu.Thoigian = DateTime.Now;
            db.Lichsusudungcts.Add(lichSu);
            db.SaveChanges();
        }

        private int getChucNangIDFromUrl(string controllerName, string actionName)
        {
            if (String.IsNullOrEmpty(actionName))
            {
                actionName = "Index";
            }

            int nhomChucNangID = getNhomChucNangIDFromUrl(controllerName);

            Chucnangchuongtrinh chucNang = db.Chucnangchuongtrinhs.FirstOrDefault(p => p.NhomchucnangID == nhomChucNangID && p.TenAction == actionName);
            if (chucNang != null)
            {
                return chucNang.ChucnangID;
            }
            return 0;
        }

        private int getNhomChucNangIDFromUrl(string controllerName)
        {
            String controllerNameToLower = controllerName + "Controller".ToLower();
            Nhomchucnang nhomChucNang = db.Nhomchucnangs.FirstOrDefault(p => p.TenController.ToLower() == controllerNameToLower);
            if (nhomChucNang != null)
            {
                return nhomChucNang.NhomchucnangID;
            }
            return 0;
        }

        protected override ActionResult RedirectToLoginPage(string prevUrl)
        {
            return RedirectToAction("Login", "Secure", new { area = "", prevUrl = prevUrl });
        }

        protected override void Dispose(bool disposing)
        {
            uow.Dispose();
            base.Dispose(disposing);
        }

        public ViewResult ExcelResult(String view, object data)
        {
            Response.Clear();
            //Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "utf-8";
            Response.AddHeader("Content-Disposition",
                String.Format(@"attachment; filename={0}.xls", HDNHD.Core.Helpers.StringHelpers.GenerateSlug(title)));

            return View(view, data);
        }
    }
}