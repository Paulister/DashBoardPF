﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardPF.Models;

namespace DashBoardPF.Controllers
{
    public class HomeController : Controller
    {

        private PolloCentralEntities1 dbPF = new PolloCentralEntities1();

        public ActionResult Index()
        {


            // Get the chart data from the database.  At this point it is just an array of VentasRestaurant objects.
            var data = VentasPorSucursalMesEnCurso();
            var data2 = VentasPorSucursalAño();

            return View(new VentasChartModel()
            {
                Title = "Total Sales By Sucursal and Anio",
                Subtitle = "Pollo1, Pollo2, Pollo3, and Pollo4",
                DataTable = ConstructDataTableArea(data),
                DataTable2= ConstructDataTableArea2(data2)
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

            };
        }


        protected override void Dispose (bool disposing)
        {
            dbPF.Dispose();
            base.Dispose(disposing);
        }


        private List<VentaSucMesVsAnioAnterior> VentasPorSucursalMesEnCurso()
        {
            List<VentaSucMesVsAnioAnterior> VentasTotalesDia = new List<VentaSucMesVsAnioAnterior>();
            DateTime Fecha1, Fecha2, FechaAux, FechaAux1, FechaAux2, FechaAux3 ;



            
            Fecha1 = Convert.ToDateTime("01/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString());
            Fecha2 = DateTime.Today;


            for (DateTime i = Fecha1; i < Fecha2; i = i.AddDays(1))
            {

                FechaAux = i.AddDays(1);
                var VentaDiaTotal = dbPF.IngresosConc
                    .Where(a => a.Fecha >= i && a.Fecha <= FechaAux)
                    .Sum(b => b.Total);


               VentasTotalesDia.Add(new VentaSucMesVsAnioAnterior{ Anio = Convert.ToInt32(DateTime.Today.Year),
                                                                   Dia = Convert.ToInt32(i.Day),
                                                                   VentasTotal = Convert.ToDecimal(VentaDiaTotal) });
            }

            int AnioAnterior = Fecha2.Year-1;

            FechaAux1 = Convert.ToDateTime("01/" + DateTime.Today.Month.ToString() + "/" + AnioAnterior.ToString());
            FechaAux2 = Convert.ToDateTime(DateTime.Today.Day.ToString() + "/" + DateTime.Today.Month.ToString() + "/" + AnioAnterior.ToString());


            for (DateTime i = FechaAux1; i < FechaAux2; i = i.AddDays(1))
            {

                FechaAux3 = i.AddDays(1);
                var VentaDiaTotal = dbPF.IngresosConc
                    .Where(a => a.Fecha >= i && a.Fecha <= FechaAux3)
                    .Sum(b => b.Total);

                VentasTotalesDia.Add(new VentaSucMesVsAnioAnterior { Anio = Convert.ToInt32(FechaAux1.Year),
                                                                     Dia = Convert.ToInt32(i.Day),
                                                                     VentasTotal = Convert.ToDecimal(VentaDiaTotal) });
            }
            return VentasTotalesDia;

        }


        private GoogleVisualizationDataTable ConstructDataTableArea(List<VentaSucMesVsAnioAnterior> data)
        {
            var dataTable = new GoogleVisualizationDataTable();

           
            var anios= data.Select(x => x.Anio).Distinct().OrderBy(x => x);

            // Obtiene los dias del mes a mostrar
            var dias = data.Select(x => x.Dia).Distinct().OrderBy(x => x);

            // Define Columna y Tipo de variable
            dataTable.AddColumn("Año", "number");
            foreach (var anio in anios)
            {
                dataTable.AddColumn(anio.ToString(), "number");
            }

            foreach (var dia in dias)
            {
                var values = new List<object>(new[] { dia.ToString() });
                foreach (var anio in anios)
                {
                    var ventat = data
                        .Where(x => x.Anio == anio && x.Dia == dia)
                        .Select(x => x.VentasTotal)
                        .SingleOrDefault();
                    values.Add(ventat);
                }
                dataTable.AddRow(values);
            }

            return dataTable;
        }

        private GoogleVisualizationDataTable ConstructDataTableArea2(List<VentasSucAnioVsAnioAnterior> data)
        {
            var dataTable = new GoogleVisualizationDataTable();


            var anios = data.Select(x => x.Anio).Distinct().OrderBy(x => x);

            // Obtiene los dias del mes a mostrar
            var meses = data.Select(x => x.Mes).Distinct().OrderBy(x => x);

            // Define Columna y Tipo de variable
            dataTable.AddColumn("Año", "number");
            foreach (var anio in anios)
            {
                dataTable.AddColumn(anio.ToString(), "number");
            }

            foreach (var mes in meses)
            {
                var values = new List<object>(new[] { mes.ToString() });
                foreach (var anio in anios)
                {
                    var ventat = data
                        .Where(x => x.Anio == anio && x.Mes == mes)
                        .Select(x => x.VentasTotal)
                        .SingleOrDefault();
                    values.Add(ventat);
                }
                dataTable.AddRow(values);
            }

            return dataTable;
        }

        //Venta Por Mes de Año Vigente contra Año Anterior

        private List<VentasSucAnioVsAnioAnterior> VentasPorSucursalAño()
        {
            List<VentasSucAnioVsAnioAnterior> VentasTotalesDia = new List<VentasSucAnioVsAnioAnterior>();
            DateTime Fecha1, Fecha2, FechaAux, FechaAux1, FechaAux2, FechaAux3;



            Fecha1 = Convert.ToDateTime("01/01/" + DateTime.Today.Year.ToString());
            Fecha2 = DateTime.Today;

            for (DateTime i = Fecha1; i < Fecha2; i = i.AddMonths(1))
            {

                
                FechaAux = new DateTime(i.Year, i.Month + 1, 1).AddDays(-1);
                var VentaDiaTotal = dbPF.IngresosConc
                    .Where(a => a.Fecha >= i && a.Fecha <= FechaAux)
                    .Sum(b => b.Total);

                VentasTotalesDia.Add(new VentasSucAnioVsAnioAnterior { Anio = Convert.ToInt32(DateTime.Today.Year),
                                                                       Mes = Convert.ToInt32(i.Month),
                                                                       VentasTotal = Convert.ToDecimal(VentaDiaTotal) });
            }

            int AnioAnterior = Fecha2.Year - 1;


        FechaAux1 = Convert.ToDateTime("01/01/" + AnioAnterior.ToString());
        FechaAux2 = Convert.ToDateTime(DateTime.Today.Day.ToString() + "/" + DateTime.Today.Month.ToString() + "/" + AnioAnterior.ToString());


        for (DateTime i = FechaAux1; i < FechaAux2; i = i.AddMonths(1))
        {

            FechaAux3 = new DateTime(i.Year, i.Month + 1, 1).AddDays(-1);

           
                var VentaDiaTotal = dbPF.IngresosConc
                    .Where(a => a.Fecha >= i && a.Fecha <= FechaAux3)
                    .Sum(b => b.Total);

                VentasTotalesDia.Add(new VentasSucAnioVsAnioAnterior { Anio = Convert.ToInt32(FechaAux1.Year),
                                                                       Mes = Convert.ToInt32(i.Month),
                                                                       VentasTotal = Convert.ToDecimal(VentaDiaTotal) });
            }
            return VentasTotalesDia;
        }


        public ActionResult ReportePorSucursal()
        {
            ViewBag.Sucursales = new SelectList(dbPF.Sucursales.Where(x => x.CeDis == false), "ID", "Identificador");

            return View();
        }

     }
}