using System;
using System.Web.UI;

namespace SerilogDemo
{
    public class BasePage : Page
    {

        public LogHelper log;

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                log = new LogHelper(this.GetType().Name, this); //this.GetType().Name 取得程式名稱
                Session["log"] = log; //紀錄至Session
            }
            else
            {
                log = (LogHelper)Session["log"];
            }

            log.InitRequestTraceID(); //取得Request TraceID
            log.Information("OnLoad"); //PageLoad時寫入Log

            base.OnLoad(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            log.Information("OnLoadComplete"); //PageLoad完成時寫入Log
            base.OnLoadComplete(e);
        }

    }
}