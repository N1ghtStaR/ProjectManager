namespace ProjectManager.Filters
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class IsAuthenticated : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            ////////////////////////////////////////////////////////////////////////
            //                                                                    //
            //   При опит за достъп до страница при не регистриран протребител,   // 
            //   потребителя се препраща към логин страница                       //
            //                                                                    //
            ////////////////////////////////////////////////////////////////////////

            if (filterContext.HttpContext.Session["ID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Authentication", action = "LogIn" }));
            }
        }
    }
}
