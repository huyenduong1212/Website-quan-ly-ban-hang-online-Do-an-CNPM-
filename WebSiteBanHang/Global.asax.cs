using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Security.Principal;
using System.Web.Routing;
using System.Web.Security;

namespace WebSiteBanHang
{
  public class MvcApplication : System.Web.HttpApplication
  {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["SoNguoiTruyCap"] = 0;
            Application["SoNguoiDangOnline"] = 0;
        }
        protected void Session_Start()
        {
            Application.Lock(); // Dùng để đồng bộ hóa 
            Application["SoNguoiTruyCap"] = (int)Application["SoNguoiTruyCap"] + 1;
            Application["SoNguoiDangOnline"] = (int)Application["SoNguoiDangOnline"] + 1;
            //Application["Online"] = (int)Application["Online"] + 1;
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock(); // Dùng để đồng bộ hóa 
            Application["SoNguoiDangOnline"] = (int)Application["SoNguoiDangOnline"] - 1;
            Application.UnLock();

        }
        //protected void Application_AuthenticationRequest(Object sender, EventArgs e)
        //{
        //  var taiKhoan_cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];//lấy dữ liệu (index của cookie) từ request được gửi lên từ client
        //  if (taiKhoan_cookie != null)
        //  {
        //    var authticket = FormsAuthentication.Decrypt(taiKhoan_cookie.Value);
        //    var chucNang = authticket.UserData.Split(new char[] { ',' });//cắt thành một mảng lưu vào chucNang dựa vào ký tự ','
        //    var userPrincipal = new GenericPrincipal(new GenericIdentity(authticket.Name), chucNang);//Quyen là mảng chuỗi biểu diễn các roles đi với cái authTicket.Nam này
        //    Context.User = userPrincipal;//Thiết lập cái thông tin bảo mật cho HTTP request hiện tại
        //  }

        //}
    }
}
