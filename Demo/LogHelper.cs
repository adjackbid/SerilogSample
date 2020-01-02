using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

using System.Web.UI;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Parsing;

namespace SerilogDemo
{
    public class LogHelper
    {
        private ILogger _logger;

        private string _EventTraceID = "";
        private string _RequestTraceID = "";

        //Log Start / End
        private long start;
        private long end;

        /// <summary>
        /// 產生TraceID
        /// </summary>
        private void InitEventTraceID()
        {
            _EventTraceID = Guid.NewGuid().ToString();
        }

        public void InitRequestTraceID()
        {
            _RequestTraceID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 初始化Log
        /// </summary>
        public void Init()
        {
            start = Stopwatch.GetTimestamp(); //開始時間
            InitEventTraceID();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programName">程式名稱</param>
        /// <param name="page">page物件(為了取得網址)</param>
        public LogHelper(string programName, Page page)
        {
            _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.WithProperty("網址", page.Request.Url.OriginalString)
            .Enrich.WithProperty("程式名稱", programName)
            .Enrich.WithProperty("使用者名稱", HttpContext.Current.User.Identity.Name)
            .Enrich.FromLogContext()
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();
        }

        private void AddLogProperties(decimal cost = -1)
        {
            //取得事件或方法(C# Reflection)
            StackTrace stackTrace = new StackTrace();
            string sRequestMethod = stackTrace.GetFrame(2).GetMethod().Name; //往上兩層

            //給定參數
            LogContext.PushProperty("方法名稱", sRequestMethod);
            LogContext.PushProperty("RequestTraceID", _RequestTraceID);
            LogContext.PushProperty("EventTraceID", _EventTraceID);
            
            //紀錄時間
            if (cost > 0)
            {
                LogContext.PushProperty("花費時間", cost);
            }
        }

        public void Information(string sMessage)
        {
            AddLogProperties();
            _logger.Information(sMessage);
        }

        public void Error(string sMessage, Exception ex = null) //error通常有Exception
        {
            AddLogProperties();
            _logger.Error(sMessage, ex);
        }

        public void Warning(string sMessage)
        {
            AddLogProperties();
            _logger.Warning(sMessage);
        }

        /// <summary>
        /// 計算花費時間並寫入Log
        /// </summary>
        public void CompleteLog()
        {
            end = Stopwatch.GetTimestamp();
            decimal cost = GetElapsedMilliseconds(start, end);
            AddLogProperties(cost);
            _logger.Debug($"Total Cost：{cost}");
            _EventTraceID = "";
        }

        private decimal GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (decimal)Stopwatch.Frequency; //計算時間(ms)
        }
    }
}