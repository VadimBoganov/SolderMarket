using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace solder.HtmlHelpers
{
    public static class FormatListSoldersHelper
    {
        public static HtmlString FormatListSolders(this IHtmlHelper html, List<solder.Models.Solder> list, int countObjectsInRow)
        {
            int count = 0;
            string result = string.Empty;
            while (count < list.Count)
            {
                result += "<tr>";
                result += Add(list.Skip(count).Take(countObjectsInRow).ToList(), 0);
                result += "</tr>";
                count += countObjectsInRow;
            }
            return new HtmlString(result);
        } 

        static string Add(List<solder.Models.Solder> list, int count)
        {
            string result = string.Empty;
            if(count < list.Count)
            {
                var item = list.Skip(count).FirstOrDefault();
                 if(item.PictureName != null)
                    result += string.Format("<td>{0} {1} <img style='width:80px; height:60px;' src={2} /></td>", 
                    item.Name, item.SolderProduct.Name,
                    "/images/solders/" + item.PictureName.Replace(" ", "%20"));
                 else
                    result += string.Format("<td>{0} {1}</td>", item.Name, item.SolderProduct.Name);

                result += Add(list, count + 1);
            }
            return result;
        }
    }
}