//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DashBoardPF.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Platillos
    {
        public int Id_Platillo { get; set; }
        public string Nombre { get; set; }
        public Nullable<bool> Contable { get; set; }
        public int Destino { get; set; }
        public string Tipo { get; set; }
        public Nullable<bool> TasaCero { get; set; }
        public Nullable<bool> Promocion { get; set; }
        public Nullable<bool> PromoTemp { get; set; }
    }
}
