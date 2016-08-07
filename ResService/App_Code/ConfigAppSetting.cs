using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ConfigAppSetting 的摘要说明
/// </summary>
public class ConfigAppSetting
{
    //域名upDomain
    public static readonly string imgHandlerDomain = System.Configuration.ConfigurationManager.AppSettings["imgHandlerDomain"].ToString();
    public static readonly string upDomain = System.Configuration.ConfigurationManager.AppSettings["upDomain"].ToString();
    
}