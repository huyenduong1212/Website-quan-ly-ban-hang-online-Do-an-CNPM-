using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class QuanLySANPHAMController : Controller
    {
        //
        // GET: /QuanLySANPHAM/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {

            return View(db.SANPHAMs.Where(n => n.DaXoa == false).OrderByDescending(n=>n.MaSP));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            //Load dropdownlist Mã Sự kiện và dropdownlist loại sp
            ViewBag.MaSuKien = new SelectList(db.SUKIENs.OrderBy(n => n.MaSuKien), "MaSuKien", "TenSuKien");
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SANPHAM sp, HttpPostedFileBase[] HinhAnh)
        {
            //Load dropdownlist Mã Sự kiện và dropdownlist loại sp
            ViewBag.MaSuKien = new SelectList(db.SUKIENs.OrderBy(n => n.TenSuKien), "MaSuKien", "TenSuKien");
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            int loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    //Kiểm tra nội dung hình ảnh
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        //Kiểm tra định dạng hình ảnh
                        if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")
                        {
                            ViewBag.upload += "Hình ảnh" + i + " không hợp lệ <br />";
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
                else
                {
                    db.SANPHAMs.Add(sp);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            if (loi > 0)
            {
                return View(sp);
            }
            sp.HinhAnh = HinhAnh[0].FileName;



            //Kiểm tra hình tổn tại trong csdl chưa
            if (HinhAnh[0].ContentLength > 0)
            {
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh[0].FileName);
                //Lấy hình ảnh chuyển vào thư mục hình ảnh 
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình đã tồn tại";
                    return View();
                }
                else
                {
                    //Lấy hình ảnh đưa vào thư mục HinhAnhSP
                    HinhAnh[0].SaveAs(path);
                    sp.HinhAnh = fileName;
                }
            }
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
            //Load dropdownlist Mã Sự kiện và dropdownlist loại sp
            ViewBag.MaSuKien = new SelectList(db.SUKIENs.OrderBy(n => n.MaSuKien), "MaSuKien", "TenSuKien", sp.MaSuKien);
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SANPHAM model)
        {

            ViewBag.MaSuKien = new SelectList(db.SUKIENs.OrderBy(n => n.MaSuKien), "MaSuKien", "TenSuKien", model.MaSuKien);
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            //Nếu dữ liệu đầu vào chắn chắn ok 
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {

            //Lấy sản phẩm cần chỉnh sửa dựa vào id System.Data.Entity.EntityState.Modified;
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

            //Load dropdownlist Mã Sự kiện và dropdownlist loại sp
            ViewBag.MaSuKien = new SelectList(db.SUKIENs.OrderBy(n => n.TenSuKien), "MaSuKien", "TenSuKien", sp.MaSuKien);
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
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
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            sp.DaXoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}