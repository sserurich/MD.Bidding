﻿using System.Web;
using System.Web.Mvc;

namespace MD.Bidding.Web.Umbraco
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
