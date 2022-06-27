using System;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;
using log4net;
using log4net.Config;
using System.Collections;

public static class Logger
{
    private static readonly ILog log = LogManager.GetLogger("FileAppender");
    private static readonly ILog activityLog = LogManager.GetLogger("File1Appender");

    public static void Error(Exception ex)
    {
        log4net.Config.XmlConfigurator.Configure();
        log.Error("***--------ErrorLogger--------***");
        log.Error("Source		: " + ex.Source.ToString().Trim());
        log.Error("Method		: " + ex.TargetSite.Name.ToString());
        log.Error("Date : " + DateTime.Now.ToShortDateString() + "(MM/DD/YYYY)");
        log.Error("Time : " + DateTime.Now.ToLongTimeString());
        log.Error("Computer : " + Dns.GetHostName().ToString());
        log.Error("Error		: " + ex.Message.ToString().Trim());
        log.Error("Stack Trace	: " + ex.StackTrace.ToString().Trim());
        log.Error("***-----------------------------------------------------------------------------------------------------------------------***");
    }
    public static void Activity(object Method)
    {
        log4net.Config.XmlConfigurator.Configure();
        activityLog.Info("***--------ActivityLogger--------***");
        activityLog.Info("Date: " + DateTime.Now.ToShortDateString() + "(MM/DD/YYYY)");
        activityLog.Info("Time: " + DateTime.Now.ToLongTimeString());
        activityLog.Info("Computer: " + Dns.GetHostName().ToString());
        activityLog.Info("Method : " + Method);
        activityLog.Info("***-----------------------------------------------------------------------------------------------------------------------***");
    }
}