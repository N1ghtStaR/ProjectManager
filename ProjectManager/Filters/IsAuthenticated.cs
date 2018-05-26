namespace ProjectManager.Filters
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class IsAuthenticated : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {   
            if (filterContext.HttpContext.Session["ID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Authentication", action = "LogIn" }));
            }
        }
    }
}
