using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace TsSoft.Web.Mvc.DataTablesNet
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <author>Eugene Yaroslavov</author>
    /// <author>Alexey Trofimov</author>
    public class DataTableModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var model = (DataTableSettings)base.BindModel(controllerContext, bindingContext);
            model.RequestId = Convert.ToInt32(request["draw"]);
            model.Take = Convert.ToInt32(request["length"]);
            model.Skip = Convert.ToInt32(request["start"]);

            const string columnIndexGroupName = "columnIndex";
            var columnOrderPattern = new Regex(string.Format(@"order\[(?<{0}>\d+)\]\[column\]", columnIndexGroupName));
            var sortColumns = request.Form.AllKeys.Where(k => columnOrderPattern.IsMatch(k))
                .ToDictionary(k => int.Parse(columnOrderPattern.Match(k).Groups[columnIndexGroupName].Value), k => k);
            var columns = new List<SortColumn>();
            foreach (var sortColumn in sortColumns.OrderBy(k => k.Key))
            {
                var isOrderable = bool.Parse(request.Form[string.Format("columns[{0}][orderable]", sortColumn.Key)]);
                if (!isOrderable)
                {
                    continue;
                }

                var columnName = request.Form[string.Format("columns[{0}][name]", sortColumn.Key)];
                var direction = request.Form[string.Format("order[{0}][dir]", sortColumn.Key)];
                columns.Add(new SortColumn
                {
                    Index = sortColumn.Key,
                    Name = columnName,
                    Order = direction == "desc" ? SortOrder.Descending : SortOrder.Ascending,
                });
            }

            model.SortColumns = columns.ToArray();
            return model;
        }
    }
}