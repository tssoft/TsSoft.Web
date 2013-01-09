using System.IO;
using System.Web.Mvc;

namespace TsSoft.Web.Mvc.Helper
{
    public class ViewHelper
    {
        public static string ViewToString(string viewName, object model, ControllerContext controllerContext)
        {
            using (StringWriter sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewData = new ViewDataDictionary(model);
                var tempData = new TempDataDictionary();
                var viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}