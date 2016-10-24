using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardPF.Models;
namespace DashBoardPF.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Get the chart data from the database.  At this point it is just an array of VentasRestaurant objects.
            var data = GetMarketSalesFromDatabase();

            return View(new VentasChartModel()
            {
                Title = "Total Sales By Sucursal and Anio",
                Subtitle = "Pollo1, Pollo2, Pollo3, and Pollo4",
                DataTable = ConstructDataTable(data)
            });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        private GoogleVisualizationDataTable ConstructDataTable(VentasRestaurant[] data)
        {
            var dataTable = new GoogleVisualizationDataTable();

            // Get distinct markets from the data
            var markets = data.Select(x => x.Sucursal).Distinct().OrderBy(x => x);

            // Get distinct years from the data
            var years = data.Select(x => x.Anio).Distinct().OrderBy(x => x);

            // Specify the columns for the DataTable.
            // In this example, it is Sucursal and then a column for each year.
            dataTable.AddColumn("Sucursal", "string");
            foreach (var year in years)
            {
                dataTable.AddColumn(year.ToString(), "number");
            }

            // Specify the rows for the DataTable.
            // Each Sucursal will be its own row, containing the total sales for each year.
            foreach (var Sucursal in markets)
            {
                var values = new List<object>(new[] { Sucursal });
                foreach (var year in years)
                {
                    var totalSales = data
                        .Where(x => x.Sucursal == Sucursal && x.Anio == year)
                        .Select(x => x.VentasTotales)
                        .SingleOrDefault();
                    values.Add(totalSales);
                }
                dataTable.AddRow(values);
            }

            return dataTable;
        }



        private VentasRestaurant[] GetMarketSalesFromDatabase()
        {
            // Let's pretend this came from a database, via EF, Dapper, or something like that...
            return new VentasRestaurant[]
            {
                new VentasRestaurant() { Sucursal = "Pollo1", Anio = 2013, VentasTotales = 723898 },
                new VentasRestaurant() { Sucursal = "Pollo1", Anio = 2014, VentasTotales = 898132 },
                new VentasRestaurant() { Sucursal = "Pollo1", Anio = 2015, VentasTotales = 941823 },

                new VentasRestaurant() { Sucursal = "Pollo3", Anio = 2013, VentasTotales = 509132 },
                new VentasRestaurant() { Sucursal = "Pollo3", Anio = 2014, VentasTotales = 570913 },
                new VentasRestaurant() { Sucursal = "Pollo3", Anio = 2015, VentasTotales = 460923 },

                new VentasRestaurant() { Sucursal = "Pollo2", Anio = 2013, VentasTotales = 753939 },
                new VentasRestaurant() { Sucursal = "Pollo2", Anio = 2014, VentasTotales = 830923 },
                new VentasRestaurant() { Sucursal = "Pollo2", Anio = 2015, VentasTotales = 910302 },

                new VentasRestaurant() { Sucursal = "Pollo4", Anio = 2013, VentasTotales = 109012 },
                new VentasRestaurant() { Sucursal = "Pollo4", Anio = 2014, VentasTotales = 400302 },
                new VentasRestaurant() { Sucursal = "Pollo4", Anio = 2015, VentasTotales = 492901 }
            };
        }



    }
}