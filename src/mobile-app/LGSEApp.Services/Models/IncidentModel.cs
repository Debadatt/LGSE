using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Models
{
  public  class IncidentModel
    {
        public string id { get; set; }
        public string incidentId { get; set; }
        public string categoriesMstrId { get; set; }
        public string categoryName { get; set; }
        public string description { get; set; }
        public string notes { get; set; }
        public int? noOfPropsAffected { get; set; }
        public int? noOfZones { get; set; }
        public int? noOfCells { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public int? status { get; set; }
        public object closingNotes { get; set; }
        public int? noOfPropsIsolated { get; set; }
        public int? noOfPropsRestored { get; set; }
        public string createdBy { get; set; }
        public string modifiedBy { get; set; }
    }
}
