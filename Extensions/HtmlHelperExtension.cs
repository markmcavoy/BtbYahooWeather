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
        public static HtmlString DropDownListFor(this HtmlHelper helper, string id, IDictionary<string, int> data, string selectedItem)
        {
            var ddl = new DropDownList();
            ddl.ID = id;

            foreach (var item in data)
            {
                ddl.Items.Add(new ListItem(item.Key, item.Value.ToString()));
            }

            if (!string.IsNullOrEmpty(selectedItem))
            {
                var selected = ddl.Items.FindByText(selectedItem);
                if (selected != null)
                    selected.Selected = true; 
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
