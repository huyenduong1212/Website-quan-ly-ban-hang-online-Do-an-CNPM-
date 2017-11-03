﻿using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;

namespace WebSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySANPHAM")]
    public class QuanLySANPHAMController : Controller
    {
   
        // GET: /QuanLySANPHAM/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index() 
        {

            return View(db.SANPHAMs.Where(n => n.DaXoa == false).OrderByDescending(n=>n.MaSP));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            //ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SANPHAM sp, HttpPostedFileBase[] HinhAnh)
        {
            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            //ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            int loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                { 
                   //Kiểm tra nội dung hình ảnh
                if ( HinhAnh[i].ContentLength > 0) 
                {
                    //Kiểm tra định dạng hình ảnh
                    if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")
                    {
                         ViewBag.upload += "Hình ảnh"+i+" không hợp lệ <br />";
                         loi++;
                    }
                    else
                    {
                        //Kiểm tra hình ảnh tồn tại

                        //Lấy tên hình ảnh
                        var fileName = Path.GetFileName(HinhAnh[0].FileName);
                        //Lấy hình ảnh chuyển vào thư mục hình ảnh 
                        var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                        //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.upload1 = "Hình " + i + "đã tồn tại <br />";
                            loi++;
                            
                        }
                    }
                }
                }

            }
            if (loi > 0)
            {
                return View(sp);
            }
            sp.HinhAnh = HinhAnh[0].FileName;
            sp.HinhAnh = HinhAnh[1].FileName;
            sp.HinhAnh = HinhAnh[2].FileName;
            sp.HinhAnh = HinhAnh[3].FileName;
            sp.HinhAnh = HinhAnh[4].FileName;


                ////Kiểm tra hình tổn tại trong csdl chưa
                //if (HinhAnh[0].ContentLength > 0)
                //{
                //    //Lấy tên hình ảnh
                //    var fileName = Path.GetFileName(HinhAnh[0].FileName);
                //    //Lấy hình ảnh chuyển vào thư mục hình ảnh 
                //    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                //    //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                //    if (System.IO.File.Exists(path))
                //    {
                //        ViewBag.upload = "Hình đã tồn tại";
                //        return View();
                //    }
                //    else
                //    {
                //        //Lấy hình ảnh đưa vào thư mục HinhAnhSP
                //        HinhAnh[0].SaveAs(path);
                //        sp.HinhAnh = fileName;

                //    }

                //}
            db.SANPHAMs.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sản xuất
            //ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC",sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai",sp.MaLoaiSP);
            //ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX",sp.MaNSX);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SANPHAM model)
        {

            //ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
           // ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            //Nếu dữ liệu đầu vào chắn chắn ok 
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {

            //Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sản xuất
            //ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            //ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }

        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM model = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.SANPHAMs.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}