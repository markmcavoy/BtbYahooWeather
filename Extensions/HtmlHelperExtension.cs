using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNuke.Web.Razor.Helpers;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Web;

namespace BiteTheBullet.DNN.Modules.BTBYahooWeather.Extensions
{
    public static class HtmlHelperExtension
    {
        public static HtmlString DropDownListFor(this HtmlHelper helper, string id, IEnumerable<SelectListItem> items)
        {
            var ddl = new DropDownList();
            ddl.ID = id;

            foreach (var item in items)
            {
                ddl.Items.Add(new ListItem(item.Key, item.Value.ToString()));

                if (item.Selected)
                    ddl.Items[ddl.Items.Count - 1].Selected = true;
            }
            
            return RenderControl(ddl);
        }


        private static HtmlString RenderControl(Control c)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (HtmlTextWriter writer = new HtmlTextWriter(sw))
            {
                c.RenderControl(writer);
            }

            return new HtmlString(sb.ToString());
        }
    }
}
