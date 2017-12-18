using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
  public class SuKienController : Controller
  {
    // GET: SuKien
    QuanLyBanHangEntities db = new QuanLyBanHangEntities();
    public ActionResult Index()
    {
      return View();
    }
    public ActionResult SanPhamSuKien(int maSuKien, int? page)
    {
      if (maSuKien == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var lstSP = db.SANPHAMs.Where(n => n.MaSuKien == maSuKien);
      if (lstSP.Count() == 0)
      {
        return HttpNotFound();
      }
      //Thực hiện chức năng phân trang
      //Tạo biến số sản phẩm trên trang
      int PageSize = 5;
      //Tạo biến thứ 2: Số trang hiện tại
      int PageNumber = (page ?? 1);
      ViewBag.MaSuKien = maSuKien;
      return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize));
    }
  }
}