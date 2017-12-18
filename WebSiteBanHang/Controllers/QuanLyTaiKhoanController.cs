using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using WebSiteBanHang.ViewModels;

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
                       where nd.MaLoaiNguoiDung != 1 && nd.MaLoaiNguoiDung != 2
                       join lnd in db.LOAINGUOIDUNGs on nd.MaLoaiNguoiDung equals lnd.MaLoaiNguoiDung
                       select new NguoiDung_LoaiNguoiDungModel { NGUOIDUNG = nd, LOAINGUOIDUNG = lnd };
            return View(user);
        }
        [HttpGet]
        public ActionResult ThemTaiKhoan()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        public ActionResult ThemTaiKhoan(NGUOIDUNG nd, FormCollection f)
        {
            string TaiKhoan = f["TaiKhoan"].ToString();
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(n => n.TaiKhoan == TaiKhoan);
            if (nguoidung != null)
            {
                return Content("Tài Khoản Đã Tồn Tại");
            }
            else
            {
                db.NGUOIDUNGs.Add(nd);
                db.SaveChanges();
                SetViewBag();
                return JavaScript("window.location = '" + Url.Action("Index", "QuanLyTaiKhoan") + "'");
            }
        }
        public void SetViewBag(int? selectedId = null)
        {           
            ViewBag.MaLoaiNguoiDung = new SelectList(ListALL(), "MaLoaiNguoiDung", "TenLoaiNguoiDung", selectedId);
        }
        public List<LOAINGUOIDUNG> ListALL()
        {
            return db.LOAINGUOIDUNGs.ToList();
        }
        public ActionResult SuaTaiKhoan(int MaNguoiDung)
        {
            var nd = db.NGUOIDUNGs.First(n => n.MaNguoiDung == MaNguoiDung);
            SetViewBag(nd.MaNguoiDung);
            return View(nd);
        }
        [HttpPost]
        public ActionResult SuaTaiKhoan(NGUOIDUNG nd)
        {
            var nguoidung = db.NGUOIDUNGs.Find(nd.MaNguoiDung);
            nguoidung.TaiKhoan = nd.TaiKhoan;
            nguoidung.MatKhau = nd.MatKhau;
            nguoidung.MaLoaiNguoiDung = nd.MaLoaiNguoiDung;
            nguoidung.TrangThai = nd.TrangThai;
            db.SaveChanges();
            SetViewBag(nd.MaNguoiDung);
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