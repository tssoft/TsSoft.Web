using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TsSoft.Web.Mvc.DataTablesNet
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <author>Eugene Yaroslavov</author>
    public class DataTableModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var model = (DataTableSettings)base.BindModel(controllerContext, bindingContext);
            model.RequestId = Convert.ToInt32(request["sEcho"]);
            model.Take = Convert.ToInt32(request["iDisplayLength"]);
            model.Skip = Convert.ToInt32(request["iDisplayStart"]);

            var columns = new List<SortColumn>();
            int sortColumnCount = Convert.ToInt32(request["iSortingCols"]);
            for (int i = 0; i < sortColumnCount; i++)
            {
                var column = new SortColumn();
                column.Index = Convert.ToInt32(request["iSortCol_" + i]);
                column.Name = request["mDataProp_" + column.Index];
                var sortOrder = request["sSortDir_" + i];
                column.Order = !string.IsNullOrWhiteSpace(sortOrder) && sortOrder.ToUpper() == "DESC"
                    ? SortOrder.Descending : SortOrder.Ascending;
                columns.Add(column);
            }
            model.SortColumns = columns.ToArray();
            return model;
        }
    }
}