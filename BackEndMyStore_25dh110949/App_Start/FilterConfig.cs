using System.Web;
using System.Web.Mvc;

namespace BackEndMyStore_25dh110949
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
