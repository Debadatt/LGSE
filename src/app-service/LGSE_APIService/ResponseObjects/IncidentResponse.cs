using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class IncidentResponse
    {
        public string Id { get; set; }
        public string IncidentId { get; set; }
        public string CategoriesMstrId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public Nullable<int> NoOfPropsAffected { get; set; }
        public Nullable<int> NoOfZones { get; set; }
        public Nullable<int> NoOfCells { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<int> Status { get; set; }
        public string ClosingNotes { get; set; }
        public Nullable<int> NoOfPropsIsolated { get; set; }
        public Nullable<int> NoOfPropsRestored { get; set; }
        public List<PropsStatusCountsResp1> PropsStatusCounts { get; set; }
        //public List<IncidentPropsStatusCountsResp> PropsStatusCounts { get; set; }
        public List<string> Cells { get; set; }
        public List<string> Zones { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class PropsStatusCountsResp
    {
        public string Status { get; set; }
        public string ShortText { get; set; }
        public int Count { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class PropsStatusCountsResp1
    {
        public string StatusId { get; set; }

        public int Count { get; set; }

    }
    public class IncidentPropsStatusCountsResp
    {
        public string IncidentId { get; set; }
        public int NS { get; set; }
        public int NA { get; set; }
        public int NC { get; set; }
        public int IS { get; set; }
        public int RS { get; set; }
    }
}