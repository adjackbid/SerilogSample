using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

namespace SerilogDemo
{
    public partial class Contact : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            log.Information("Button1 Click");
            Thread.Sleep(2 * 1000);
        }
    }
}