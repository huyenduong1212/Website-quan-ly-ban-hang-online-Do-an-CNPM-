using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
  //[Authorize(Roles = "1")], Đây là công việc của lập trình viên, không phải của người dùng (admin), người dùng chỉ tương tác trên trang mà hệ thống đã đưa ra sẵn, cho lên giao diện sẽ tiện hơn, ít tương tác với csdl thì sẽ hay hơn
  [Authorize(Roles = "31")]
  public class QuyenController : Controller
  {
    //
    // GET: /CHUCNANG/
    QuanLyBanHangEntities db = new QuanLyBanHangEntities();
    public ActionResult Index()
    {
      return View(db.CHUCNANGs);
    }
    [HttpGet]
    public ActionResult ThemChucNang()
    {
      return View();
    }
    [HttpPost]
    public ActionResult ThemChucNang(CHUCNANG CHUCNANG)
    {
      if (ModelState.IsValid)//Phải có những thuộc tính yêu cầu (ở đây là tham số CHUCNANG) thì sẽ là true
      {
        db.CHUCNANGs.Add(CHUCNANG);
        db.SaveChanges();
      }

      return RedirectToAction("Index");
    }
    //[HttpGet]
    //public ActionResult ChinhSua(CHUCNANG chucNang)
    //{
    //  ViewBag.DefaultValue = chucNang.TenChucNang;
    //  return View(chucNang);
    //}
    //[HttpPost]
    //public ActionResult ChinhSua(CHUCNANG chucNang)
    //{
    //  if (ModelState.IsValid)//Phải có cái để xóa thì nó sẽ Valid, còn không là Invalid
    //  {
    //    db.CHUCNANGs.Remove(chucNang);
    //    db.SaveChanges();
    //  }
    //  return RedirectToAction("Index");
    //}
    //public ActionResult XoaChucNang(int id)
    //{
    //  if(ModelState.IsValid)//Phải có cái để xóa thì nó sẽ Valid, còn không là Invalid
    //  {
    //    db.CHUCNANGs.Remove(db.CHUCNANGs.Where(n => n.MaChucNang == id).SingleOrDefault());
    //    db.SaveChanges();
    //  }
    //  return RedirectToAction("Index");
    //}
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