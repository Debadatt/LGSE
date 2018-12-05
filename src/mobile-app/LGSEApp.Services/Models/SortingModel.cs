using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LGSEApp.Services.Models
{
    public class SortingModel
    {
        public int id { get; set; }
        public string SortingText { get; set; }
    }

    public class SortingData
    {
        static ObservableCollection<SortingModel> sortingResults = new ObservableCollection<SortingModel>();
        static SortingData()
        {
            sortingResults.Add(new SortingModel() { id= 0,SortingText= "PostCode Asc" });
            sortingResults.Add(new SortingModel() { id = 1, SortingText = "PostCode Desc" });
            sortingResults.Add(new SortingModel() { id = 2, SortingText = "PostTown Asc" });
            sortingResults.Add(new SortingModel() { id = 3, SortingText = "PostTown Desc" });
        }
        public static async Task<ObservableCollection<SortingModel>> GetSortList()
        {
            return sortingResults;
        }
    }
}
