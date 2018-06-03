namespace ProjectManager.Filters
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class Authenticated : FilterAttribute, IAuthorizationFilter
    {
        //////////////////////////////////////////////////////////////////////////
        //                                                                      //
        //   При опит за достъп до страница от вече логнат потребител           // 
        //   изискваща не логнат потребителя се препраща към начална страница   //
        //                                                                      //
        //////////////////////////////////////////////////////////////////////////

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["ID"] != null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
        }
    }
}