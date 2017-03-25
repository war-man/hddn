﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoaDonNuocHaDong;
using System.Web.Routing;
using System.Data.Entity.Validation;
using HoaDonNuocHaDong.Base;
using HoaDonNuocHaDong.Repositories;

namespace HoaDonNuocHaDong.Controllers
{
    public class TuyenController : BaseController
    {
        private HoaDonHaDongEntities db = new HoaDonHaDongEntities();



        // GET: /Tuyen/
        public ActionResult Index()
        {
            var nhanVien = db.Nhanviens.ToList();
            if(!LoggedInUser.Isadmin.Value){
                int phongBanId = getPhongBanNguoiDung();
                nhanVien = db.Nhanviens.Where(p=>p.PhongbanID == phongBanId).ToList();
            }
            var tuyenkhachhangs = db.Tuyenkhachhangs.Where(p => p.IsDelete == false || p.IsDelete == null).Include(t => t.Cumdancu).Include(t => t.To);
            ViewBag._nhanVien = nhanVien;
            return View(tuyenkhachhangs.OrderByDescending(p=>p.TuyenKHID).ToList());
        }


        public int getPhongBanNguoiDung()
        {
            var phongBanRepository = uow.Repository<PhongBanRepository>();
            if (nhanVien != null)
            {
                var phongBan = phongBanRepository.GetSingle(m => m.PhongbanID == nhanVien.PhongbanID);
                int phongBanID = phongBan.PhongbanID;
                return phongBanID;
            }
            return 0;
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {

            String nhanVien = form["nhanvien"];

            IEnumerable<Tuyenkhachhang> tuyenkhachhangs = null;
            if (String.IsNullOrEmpty(nhanVien))
            {
                //nếu trống thì chọn tất cả
                var nhanVienFilter = db.Nhanviens.ToList();
                tuyenkhachhangs = db.Tuyenkhachhangs.Include(t => t.Cumdancu).Include(t => t.To).OrderByDescending(p => p.TuyenKHID);
                ViewBag._nhanVien = nhanVienFilter;
                ViewBag.chiNhanh = db.Chinhanhs;
                return View(tuyenkhachhangs.ToList());
            }
            //nếu không sẽ tiến hành lọc tuyến theo chi nhánh
            else
            {
                int nhanVienID = Convert.ToInt32(nhanVien);
                var tuyenTheoNhanVien = (from p in db.Tuyentheonhanviens
                                         join r in db.Tuyenkhachhangs on p.TuyenKHID equals r.TuyenKHID
                                         join s in db.Nhanviens on p.NhanVienID equals s.NhanvienID
                                         where p.NhanVienID == nhanVienID
                                         select new
                                         {
                                             TuyenKhachHang = r
                                         });
                tuyenkhachhangs = tuyenTheoNhanVien.Select(p => p.TuyenKhachHang);
                //nếu để trống thì chọn tất cả
                var nhanVienFilter = db.Nhanviens.ToList();
                ViewBag._nhanVien = nhanVienFilter;
                return View(tuyenkhachhangs.ToList());
            }
        }

        // GET: /Tuyen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Tuyenkhachhang tuyenkhachhang = db.Tuyenkhachhangs.Find(id);
            Tuyentheonhanvien tuyenTheoNhanVien = db.Tuyentheonhanviens.FirstOrDefault(p => p.TuyenKHID == id);
            if (tuyenTheoNhanVien != null)
            {
                ViewBag.selectedNhanVien = tuyenTheoNhanVien.NhanVienID;
                ViewBag._nhanVien = db.Nhanviens.Where(p => p.IsDelete == false || p.IsDelete == null).ToList();
            }
            //nếu ko có thì load tất cả ra
            else
            {
                ViewBag._nhanVien = db.Nhanviens.Where(p => p.IsDelete == false || p.IsDelete == null).ToList();
            }
            if (tuyenkhachhang == null)
            {
                return HttpNotFound();
            }
            ViewBag.CumdancuID = new SelectList(db.Cumdancus.Where(p => p.IsDelete == false || p.IsDelete == null), "CumdancuID", "Ten", tuyenkhachhang.CumdancuID);

            ViewBag.ToID = new SelectList(db.Toes, "ToID", "Ten", tuyenkhachhang.ToID);
            return View(tuyenkhachhang);
        }

        // GET: /Tuyen/Create
        public ActionResult Create()
        {
            ViewBag.CumdancuID = new SelectList(db.Cumdancus.Where(p => p.IsDelete == false), "CumdancuID", "Ten");
            ViewData["nhanVien"] = db.Nhanviens.Where(p => p.IsDelete == false || p.IsDelete == null).ToList();
            ViewBag.ToID = new SelectList(db.Toes, "ToID", "Ten");
            return View();
        }

        // POST: /Tuyen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Matuyen,TuyenKHID,ToID,CumdancuID,Ten,Diachi,IsDelete")] Tuyenkhachhang tuyenkhachhang)
        {
            tuyenkhachhang.IsDelete = false;
            if (ModelState.IsValid)
            {
                //kiểm tra trong tuyến trước
                Tuyenkhachhang existingTuyen = db.Tuyenkhachhangs.FirstOrDefault(p => p.Matuyen == tuyenkhachhang.Matuyen && p.Ten == tuyenkhachhang.Ten);
                if (existingTuyen == null)
                {
                    db.Tuyenkhachhangs.Add(tuyenkhachhang);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        Response.Write(e.ToString());
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.hasTuyen = true;
                }
            }
            ViewData["nhanVien"] = db.Nhanviens.Where(p => p.IsDelete == false || p.IsDelete == null).ToList();
            ViewBag.CumdancuID = new SelectList(db.Cumdancus.Where(p => p.IsDelete == false), "CumdancuID", "Ten");
            ViewBag.ToID = new SelectList(db.Toes, "ToID", "Ten", tuyenkhachhang.ToID);
            return View(tuyenkhachhang);
        }

        // GET: /Tuyen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tuyenkhachhang tuyenkhachhang = db.Tuyenkhachhangs.Find(id);
            Tuyentheonhanvien tuyenTheoNhanVien = db.Tuyentheonhanviens.FirstOrDefault(p => p.TuyenKHID == id);
            if (tuyenTheoNhanVien != null)
            {
                ViewBag.selectedNhanVien = tuyenTheoNhanVien.NhanVienID;
                ViewBag._nhanVien = db.Nhanviens.Where(p => p.IsDelete == false || p.IsDelete == null).ToList();
            }
            //nếu ko có thì load tất cả ra
            else
            {
                ViewBag._nhanVien = db.Nhanviens.Where(p => p.IsDelete == false || p.IsDelete == null).ToList();
            }
            if (tuyenkhachhang == null)
            {
                return HttpNotFound();
            }
            ViewBag.CumdancuID = new SelectList(db.Cumdancus.Where(p => p.IsDelete == false || p.IsDelete == null), "CumdancuID", "Ten", tuyenkhachhang.CumdancuID);

            ViewBag.ToID = new SelectList(db.Toes, "ToID", "Ten", tuyenkhachhang.ToID);
            return View(tuyenkhachhang);
        }

        // POST: /Tuyen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Matuyen,TuyenKHID,ToID,CumdancuID,Ten,Diachi,IsDelete")] Tuyenkhachhang tuyenkhachhang)
        {

            if (ModelState.IsValid)
            {

                db.Entry(tuyenkhachhang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.nhanVien = new SelectList(db.Nhanviens.Where(p => p.IsDelete == false || p.IsDelete == null), "NhanvienID", "Ten");
            ViewBag.CumdancuID = new SelectList(db.Cumdancus.Where(p => p.IsDelete == false || p.IsDelete == null), "CumdancuID", "Ten", tuyenkhachhang.CumdancuID);
            ViewBag.ToID = new SelectList(db.Toes, "ToID", "Ten", tuyenkhachhang.ToID);
            return View(tuyenkhachhang);
        }

        // GET: /Tuyen/Delete/5
        public ActionResult Delete(int? id)
        {
            db.Tuyenkhachhangs.Find(id.Value).IsDelete = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AssignEmployee(FormCollection form)
        {
            String selectedTuyen = form["selectedTuyen"];
            String _selectedNhanVien = form["nhanvien"];

            if (_selectedNhanVien != null)
            {
                int selectedNhanVien = Convert.ToInt32(_selectedNhanVien);
                string[] selectedTuyenArray = selectedTuyen.Split(',');

                foreach (var item in selectedTuyenArray)
                {
                    int checkedTuyen = Convert.ToInt32(item);
                    List<Tuyentheonhanvien> tuyenTheoNhanVien = db.Tuyentheonhanviens.Where(p => p.TuyenKHID == checkedTuyen).ToList();
                    db.Tuyentheonhanviens.RemoveRange(tuyenTheoNhanVien);
                    db.SaveChanges();
                    //re-add lại tuyến theo nhân viên
                    Tuyentheonhanvien tuyenTheoNhanVienMoi = new Tuyentheonhanvien();
                    tuyenTheoNhanVienMoi.NhanVienID = selectedNhanVien;
                    tuyenTheoNhanVienMoi.TuyenKHID = checkedTuyen;
                    db.Tuyentheonhanviens.Add(tuyenTheoNhanVienMoi);
                    db.SaveChanges();

                }
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
