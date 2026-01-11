using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JapaneseFood.Common
{
    public static class ExtensionClass
    {
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveUnicode().Trim();
            str = str.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 250 ? str.Length : 250).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("utf-8").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }
        public static string RemoveUnicode(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                        "đ",
                                        "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                        "í","ì","ỉ","ĩ","ị",
                                        "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                        "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                        "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                        "d",
                                        "e","e","e","e","e","e","e","e","e","e","e",
                                        "i","i","i","i","i",
                                        "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                        "u","u","u","u","u","u","u","u","u","u","u",
                                        "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        public static SelectList ToSelectList<TEnum>(this TEnum obj)
                    where TEnum : struct, IComparable, IFormattable, IConvertible
        {

            return new SelectList(System.Enum.GetValues(typeof(TEnum)).OfType<System.Enum>()
                .Select(x =>
                    new SelectListItem
                    {
                        Text = System.Enum.GetName(typeof(TEnum), x),
                        Value = (Convert.ToInt32(x)).ToString()
                    }), "Value", "Text");
        }
        public static SelectList ToSelectListByDes<TEnum>(this TEnum obj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {

            return new SelectList(System.Enum.GetValues(typeof(TEnum)).OfType<System.Enum>()
                .Select(x =>
                    new SelectListItem
                    {
                        Text = GetEnumDescription(x),
                        Value = (Convert.ToInt32(x)).ToString()
                    }), "Value", "Text");
        }
        public static string GetEnumDescription(System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}
