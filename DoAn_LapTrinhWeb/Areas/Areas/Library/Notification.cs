using System.Web;

namespace DoAn_LapTrinhWeb
{
    public class Notification
    {
        public static bool has_flash()
        {
            if (HttpContext.Current.Application["Notification"].Equals("")) return false;

            return true;
        }

        public static void set_flash(string mgs, string mgs_type)
        {
            var tb = new ModelNotification();
            tb.mgs = mgs;
            tb.mgs_type = mgs_type;

            HttpContext.Current.Application["Notification"] = tb;
        }

        public static ModelNotification get_flash()
        {
            var Notifi = (ModelNotification) HttpContext.Current.Application["Notification"];
            HttpContext.Current.Application["Notification"] = "";
            return Notifi;
        }
    }
}