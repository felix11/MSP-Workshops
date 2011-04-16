using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ZPA
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string todo = this.Request.QueryString["todo"];
            switch (todo)
            {
                case "login":
                    LabelLoginStatus.Text = "Angemeldet";
                    break;
                case "logout":
                    LabelLoginStatus.Text = "Nicht Angemeldet";
                    break;
                default:
                    break;
            }
        }
    }
}
