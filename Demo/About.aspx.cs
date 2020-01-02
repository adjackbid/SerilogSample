using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace SerilogDemo
{
    public partial class About : BasePage
    {

        //private LogHelper log;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    log = new LogHelper("About", this); //宣告LogHelper物件(程式名稱，page物件)
            //    Session["log"] = log; //紀錄至Session
            //}
            //else
            //{
            //    log = (LogHelper)Session["log"];
            //}
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            log.Init(); //初始化：取定TraceID
            try
            {
                log.Information("Serilog Demo");

                throw new Exception("Test Error"); //手動拋出例外
            }
            catch (Exception ex)
            {
                log.Error("Something wrong", ex); //記錄例外log
            }
            log.CompleteLog(); //結束
        }
    }
}