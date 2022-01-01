using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DoAn_LapTrinhWeb
{
    public static class MyString
    {
        public static string ToBase64(this string s)
        {
            if (s != null)
            {
                var bytes = Encoding.UTF8.GetBytes(s);
                return Convert.ToBase64String(bytes);
            }

            return s;
        }

        public static string FromBase64(this string s)
        {
            if (s != null)
            {
                var bytes = Convert.FromBase64String(s);
                return Encoding.UTF8.GetString(bytes);
            }

            return s;
        }

        public static string ToMD5(this string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            var hash = MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static string ToAscii(this string s)
        {
            string[][] symbols =
            {
                new[] {"[áàảãạăắằẳẵặâấầẩẫậ]", "a"},
                new[] {"[đ]", "d"},
                new[] {"[éèẻẽẹêếềểễệ]", "e"},
                new[] {"[íìỉĩị]", "i"},
                new[] {"[óòỏõọôốồổỗộơớờởỡợ]", "o"},
                new[] {"[úùủũụưứừửữự]", "u"},
                new[] {"[ýỳỷỹỵ]", "y"},
                new[] {"[\\s'\";,]", "-"}
                 
            };
            s = s.ToLower();
            foreach (var ss in symbols) s = Regex.Replace(s, ss[0], ss[1]);
            return s;
        }

        public static string str_slug(string s)
        {
            string[][] symbols =
            {
                new[] {"[áàảãạăắằẳẵặâấầẩẫậ] ", "a"},
                new[] {"[đ]", "d"},
                new[] {"[éèẻẽẹêếềểễệ]", "e"},
                new[] {"[íìỉĩị]", "i"},
                new[] {"[óòỏõọôốồổỗộơớờởỡợ]", "o"},
                new[] {"[úùủũụưứừửữự]", "u"},
                new[] {"[ýỳỷỹỵ]", "y"},
                new[] {"[\\s'\";,]", "-"}
            };
            s = s.ToLower();
            foreach (var ss in symbols) s = Regex.Replace(s, ss[0], ss[1]);
            return s;
        }
    }
}