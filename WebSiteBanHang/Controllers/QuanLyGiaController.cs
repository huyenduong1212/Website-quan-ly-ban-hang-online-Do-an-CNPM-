using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
namespace WebSiteBanHang.Controllers
{
    public class QuanLyGiaController : Controller
    {
        // GET: QuanLyGia
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {

            return View(db.SANPHAMs.Where(n => n.DaXoa == false).OrderByDescending(n => n.MaSP));

        }
        [HttpPost]
        public ActionResult SuaGia(SANPHAM sp, int[] dssua)
        {

            foreach (int id in dssua)
            {
                if (id == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
                if (sp == null)
                {
                    return HttpNotFound();
                }
                db.Entry(sp).State = EntityState.Modified;

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}