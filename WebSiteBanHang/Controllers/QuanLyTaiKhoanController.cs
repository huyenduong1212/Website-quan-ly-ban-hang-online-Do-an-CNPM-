using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class QuanLyTaiKhoanController : Controller
    {
        // GET: QuanLyTaiKhoan
         QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyNhanVien
        public ActionResult Index()
        {
            var user = from nd in db.NGUOIDUNGs
                       join lnd in db.LOAINGUOIDUNGs on nd.MaLoaiNguoiDung equals lnd.MaLoaiNguoiDung
                       select new NguoiDung_LoaiNguoiDungModel { NGUOIDUNG = nd, LOAINGUOIDUNG = lnd };
            return View(user);
        }
        [HttpGet]
        public ActionResult ThemTaiKhoan()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemTaiKhoan(NGUOIDUNG nd)
        {
            db.NGUOIDUNGs.Add(nd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SuaTaiKhoan(int MaNguoiDung)
        {
            var nd = db.NGUOIDUNGs.First(n => n.MaNguoiDung == MaNguoiDung);
            return View(nd);
        }
        [HttpPost]
        public ActionResult SuaTaiKhoan(NGUOIDUNG nd)
        {
            var entry = db.Entry(nd);
            entry.State = EntityState.Modified;
            entry.Property(e => e.Ho).IsModified = false;
            entry.Property(e => e.TenLot).IsModified = false;
            entry.Property(e => e.Ten).IsModified = false;
            entry.Property(e => e.GioiTinh).IsModified = false;
            entry.Property(e => e.DiaChi).IsModified = false;
            entry.Property(e => e.SoDienThoai).IsModified = false;
            entry.Property(e => e.Email).IsModified = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult XoaTaiKhoan(int MaNguoiDung)
        {
            NGUOIDUNG nd = db.NGUOIDUNGs.SingleOrDefault(n => n.MaNguoiDung == MaNguoiDung);
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NGUOIDUNGs.Remove(nd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}