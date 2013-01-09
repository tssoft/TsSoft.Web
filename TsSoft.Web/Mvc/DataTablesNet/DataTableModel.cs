using System.Collections.Generic;

namespace TsSoft.Web.Mvc.DataTablesNet
{
    /// <summary>
    /// <a href="http://datatables.net/usage/server-side">Parameters sent to the server</a>
    /// </summary>
    public class DataTableSettings
    {
        /// <summary>
        /// This parameter will change with each draw (it is basically a draw count).
        /// Note that it strongly recommended for security reasons that you 'cast'
        /// this parameter to an integer in order to prevent Cross Site Scripting (XSS) attacks
        /// </summary>
        public int RequestId { get; set; }

        public SortColumn[] SortColumns { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }
    }

    public class SortColumn
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public SortOrder Order { get; set; }
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }
}