using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardPF.Models
{
    public class VentasChartModel
    {

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public GoogleVisualizationDataTable DataTable { get; set; }
        public GoogleVisualizationDataTable DataTable2 { get; set; }


    }
}