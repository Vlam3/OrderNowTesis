//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrderNowDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ingrediente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ingrediente()
        {
            this.ExtraPedido = new HashSet<ExtraPedido>();
            this.IngredienteFactura = new HashSet<IngredienteFactura>();
            this.IngredientesAlimento = new HashSet<IngredientesAlimento>();
            this.DetalleIngrediente = new HashSet<DetalleIngrediente>();
            this.ExtraDisponible = new HashSet<ExtraDisponible>();
        }
    
        public int IdIngrediente { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Stock { get; set; }
        public Nullable<double> ValorNeto { get; set; }
        public Nullable<int> IdTipoAlimento { get; set; }
        public Nullable<int> IdTipoMedicion { get; set; }
        public string Foto { get; set; }
        public Nullable<int> Porción { get; set; }
        public Nullable<int> IdTipoMedicionPorcion { get; set; }
        public Nullable<int> Estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExtraPedido> ExtraPedido { get; set; }
        public virtual TipoAlimento TipoAlimento { get; set; }
        public virtual TipoMedicion TipoMedicion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IngredienteFactura> IngredienteFactura { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IngredientesAlimento> IngredientesAlimento { get; set; }
        public virtual TipoMedicion TipoMedicion1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleIngrediente> DetalleIngrediente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExtraDisponible> ExtraDisponible { get; set; }
    }
}
