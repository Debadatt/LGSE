using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class IncidentRespPub
    {
        public string Id { get; set; }
        public string IncidentId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int? NoOfPropsAffected { get; set; }
        public int? NoOfPropsCompleted { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<int> Status { get; set; }
        public string ClosingNotes { get; set; }
        public string CreatedBy { get; set; }
        public List<PropertyRespPub> MPRNS { get; set; }
    }
}