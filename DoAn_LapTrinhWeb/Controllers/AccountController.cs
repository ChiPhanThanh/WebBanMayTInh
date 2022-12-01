using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;


namespace DoAn_LapTrinhWeb.Controllers
{
    public class AccountController : Controller
    {
        private DbContext db = new DbContext();
        // GET: Account

        //check đăng nhập
        [HttpPost]
        public JsonResult CheckSignIn(Account model)
        {
            string result;
            model.password = Crypto.Hash(model.password);
            var DataItem = db.Accounts.Where(m => m.status == "1" && m.username.ToLower() == model.username && m.password == model.password).SingleOrDefault();
            if (DataItem != null)
            {
                var userData = new LoggedUserData
                {
                    UserId = DataItem.account_id,
                    Name = DataItem.Name,
                    Username = DataItem.username,
                    Email = DataItem.Email,
                    RoleCode = DataItem.Role,

                };
                Session["Role"] = DataItem.Role;
                FormsAuthentication.SetAuthCookie(JsonConvert.SerializeObject(userData), false);
                result = "Success";
            }
            else
            {
                result = "Fail";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //đăng xuất
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        //check điều kiện đăng ký
        //[HttpPost]
        //public JsonResult SaveData(Register register)
        //{
        //    string result;
        //    var checkemail = db.Accounts.Any(m => m.Email == register.Email);
        //    var checkusername = db.Accounts.Any(m => m.username == register.username);
        //    if (checkemail)
        //    {
        //        result = "0";
        //    }

        //    else if (checkusername)
        //    {
        //        result = "1";//check user đã có trong database chưa nếu có thì báo lỗi đã tồn tại
        //    }
        //    else
        //    {
        //        Account a = new Account();
        //        a.Role = Const.ROLE_MEMBER_CODE; //admin quyền là 0: thành viên quyền là 1             

        //        a.status = "1";
        //        a.Email = register.Email;
        //        a.create_by = "1";
        //        a.update_by = "1";
        //        a.Confirmpassword = "";
        //        a.Requestcode = "";
        //        a.update_at = DateTime.Now;
        //        a.password = Crypto.Hash(register.password); //mã hoá password
        //        a.create_at = DateTime.Now; //thời gian tạo tạo khoản
        //        db.Accounts.Add(a);
        //        User u = new User();
        //        u.account_id = a.account_id;
        //        u.Name = register.Name;
        //        u.Phone = register.Phone;
        //        u.Addres_1 = register.Addres_1;
        //        db.Users.Add(u);
        //        db.SaveChanges(); //add dữ liệu vào database

        //        //BuildEmailTemplate(model.account_id);
        //        result = "2";
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet); //gửi thông báo khi đăng ký thành công
        //}
        public ActionResult Register()
        {
            return View();
        }
       [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account model)
        {
            string fail = "";
            string success = "";
            var checkemail = db.Accounts.Any(m => m.Email == model.Email);
            var checkusername = db.Accounts.Any(m => m.username == model.username);
            var checkphone = db.Accounts.Any(m => m.Phone == model.Phone);
            if (checkemail)
            {
                fail = "email đã được sử dụng";
            }
            else if (checkusername)
            {
                fail = "Tên đăng nhập đã được sử dụng";
            }
            else if (checkphone)
            {
                fail = "SĐT đã được sử dụng";
            }
            else
            {
                model.Role = Const.ROLE_MEMBER_CODE; //admin quyền là 0: thành viên quyền là 1             
                model.status = "1";
                model.Email = model.Email;
                model.create_by = "1";
                model.update_by = "1";
                model.Name = model.Name;
                model.Phone = model.Phone;
                model.update_at = DateTime.Now;
                db.Configuration.ValidateOnSaveEnabled = false; //do password có nhiều ràng buộc "validdation nên phải thêm" không thêm sẽ báo lõi "Validation failed for one or more entities" 
                model.Addres_1 = model.Addres_1;
                model.password = Crypto.Hash(model.password); //mã hoá password
                model.create_at = DateTime.Now; //thời gian tạo tạo khoản
                db.Accounts.Add(model);
                //BuildEmailTemplate(model.account_id);
                db.SaveChanges(); //add dữ liệu vào database
                success = "<script>alert('Đăng ký thành công');</script>";
                 return RedirectToAction("Index", "Home");
            }
            //return Json(result, JsonRequestBehavior.AllowGet);
            ViewBag.Success = success;
            ViewBag.Fail = fail;
            return View();
        }
        /*public ActionResult Confirm(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }
        public JsonResult RegisterConfirm(int regId)
        {
            Account Data = db.Accounts.Where(x => x.account_id == regId).FirstOrDefault();
            Data.status = "1";
            db.SaveChanges();
            var msg = "Your Email Is Verified!";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public void BuildEmailTemplate(int regID)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = db.Accounts.Where(x => x.account_id == regID).FirstOrDefault();
            var url = "https://localhost:44336/" + "Account/Confirm?regId=" + regID;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Your Account Is Successfully Created", body, regInfo.Email);
        }

        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "noreply@nvtcomputer.tech.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);
        }

        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smail8971.maychuemail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("noreply@nvtcomputer.tech ", "123456a@");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/
        public ActionResult ForgotPassword()
        {

            return View();

        }
        //check điều kiện quên mật khẩu
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string fail = "";
            string success = "";
            var account = db.Accounts.Where(m => m.Email == EmailID).FirstOrDefault(); // kiểm tra email đã trùng với email đăng ký tài khoản chưa, nếu chưa đăng ký sẽ trả về fail
            if (account != null)
            {
                //Send email for reset password
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword"); // gửi code reset đến mail đã nhập ở form quên mật khẩu , kèm code resetpass,  tên tiêu đề gửi
                string sendmail = account.Email;
                account.Requestcode = resetCode; //request code phải giống reset code         
                db.Configuration.ValidateOnSaveEnabled = false; // tắt validdation
                db.SaveChanges();
                success = "Đường dẫn reset password đã được gửi, vui lòng kiểm tra email";
            }
            else
            {
                fail = "Email chưa tồn tại trong hệ thống"; // tài khoản không có trong hệ thống sẽ báo fail
            }

            ViewBag.Message1 = success;
            ViewBag.Message2 = fail;
            return View();
        }
        public ActionResult Resetpassword(string id)

        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
            var user = db.Accounts.Where(a => a.Requestcode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPassword model = new ResetPassword();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var user = db.Accounts.Where(a => a.Requestcode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.password = Crypto.Hash(model.NewPassword);
                    user.Requestcode = "";
                    user.update_by = "1";
                    user.update_at = DateTime.Now;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "Cập nhật mật khẩu thành công";

                }
            }
            else
            {
                return HttpNotFound();
            }
            ViewBag.Message = message;
            return View(model);
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor)
        {
            var verifyUrl = "/Account/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            ///để dùng gmail gửi email rs cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
            var fromEmail = new MailAddress("noreply@nvtcomputer.tech", "NVT Computer"); // "username email-vd: vn123@gmail.com" ,"tên hiển thị mail khi gửi"

            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "123456a@"; //có thể thay bằng mật khẩu gmail của bạn
            string subject = "";
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml"); //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"

            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset your Password";
                body = body.Replace("@viewBag.Confirmlink", link); //hiển thị nội dung lên form html
                body = body.Replace("@viewBag.Confirmlink", Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl));//hiển thị nội dung lên form html
            }
            var smtp = new SmtpClient
            {
                Host = "mail8971.maychuemail.com", //tên mấy chủ nếu bạn dùng gmail thì đổi  "Host = "smtp.gmail.com"
                Port = 587,
                EnableSsl = true, //bật ssl
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        [Authorize]     // Đăng nhập mới có thể truy cập
        public ActionResult Editprofile()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Accounts.Where(u => u.account_id == userId).FirstOrDefault();
            if (user != null)
            {
                return View(user);
            }
            return View();
        }

        [HttpPost]
        [Authorize]     // Đăng nhập mới có thể truy cập
        public JsonResult Editprofile(Account model)
        {
            string result;
            var userId = User.Identity.GetUserId();
            var account = db.Accounts.Where(m => m.account_id == userId).SingleOrDefault();
            if (account != null)
            {
                account.account_id = userId;
                account.Name = model.Name;
                account.Phone = model.Phone;
                account.Addres_1 = model.Addres_1;
                account.Addres_2 = model.Addres_2;
                account.Addres_3 = model.Addres_3;
                account.status = "1";
                account.update_by = "1";
                account.update_at = DateTime.Now;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                result = "Success";
            }
            else
            {
                result = "Fail";
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult UserAddress()
        {

            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ChangePassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword model)
        {

            var message = "";
            var userId = User.Identity.GetUserId();
            var account = db.Accounts.SingleOrDefault(m => m.account_id == userId);
            if (account != null)
            {
                account.password = Crypto.Hash(model.NewPassword);
                account.update_by = "1";
                account.update_at = DateTime.Now;
                db.SaveChanges();
                message = "Cập nhật mật khẩu thành công";
            }
            else
            {
                message = "Lỗi không xác định";
            }

            return View(model);

        }


        public ActionResult TrackingOrder(int? page)
        {

            if (User.Identity.IsAuthenticated)
            {
             
                ViewBag.Deli = db.Deliveries;
                ViewBag.Payment = db.Payments;
                var list = db.Orders.Where(m => m.account_id == User.Identity.GetUserId());
                ViewBag.itemOrder = db.Oder_Detail.OrderByDescending(m => m.order_id);
                ViewBag.productOrder = db.Products;
                return View("TrackingOrder", GetOrder(page));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //đánh số trang
        private IPagedList GetOrder(int? page)
        {
            var userId = User.Identity.GetUserId();
            int pageSize = 2; //1 trang hiện thỉ tối đa 9 sản phẩm
            int pageNumber = (page ?? 1); //đánh số trang
            var list = db.Orders.Where(m => m.account_id == userId).OrderByDescending(m => m.order_id)
                .ToPagedList(pageNumber, pageSize);
            return list;
        }
        // lấ
        public ActionResult AddAddress()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult OrderDetail(int id)
        {

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var list = db.Orders.Where(m => m.account_id == userId);
                ViewBag.itemOrder1 = db.Oder_Detail.Where(m => m.order_id == id).ToList();
                ViewBag.productOrder1 = db.Products.ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult UserLogged()
        {
            // Được gọi khi nhấn [Thanh toán]
            return Json(User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
        }
    }
}