using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Filters
{
    public class IsLogin : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            var _userId = context.HttpContext.Session.GetString("Login");
            if (string.IsNullOrEmpty(_userId))
            {
                context.Result = new RedirectResult("/Login/Login");
            }

        }
    }
}
