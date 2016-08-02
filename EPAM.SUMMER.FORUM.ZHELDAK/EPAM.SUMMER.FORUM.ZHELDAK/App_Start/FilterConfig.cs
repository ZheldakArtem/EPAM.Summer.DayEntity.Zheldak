using System.Web;
using System.Web.Mvc;

namespace EPAM.SUMMER.FORUM.ZHELDAK
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
