using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ZPA
{
    public partial class Courses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string todo = this.Request.QueryString["todo"];
            switch (todo)
            {
                case "register":
                    string course = this.Request.QueryString["course"];
                    switch (course)
                    {
                        case "1":
                            ButtonRegisterCourse1.Enabled = false;
                            LabelStatusCourse1.Text = "Angemeldet";
                            break;
                        case "2":
                            ButtonRegisterCourse2.Enabled = false;
                            LabelStatusCourse2.Text = "Angemeldet";
                            break;
                        case "3":
                            ButtonRegisterCourse3.Enabled = false;
                            LabelStatusCourse3.Text = "Angemeldet";
                            break;
                        case "4":
                            ButtonRegisterCourse4.Enabled = false;
                            LabelStatusCourse4.Text = "Angemeldet";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}