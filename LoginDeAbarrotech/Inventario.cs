using System;

namespace LoginDeAbarrotech
{
    public class Inventario
    {
        public long id_inventario { get; set; }
        public long id_producto { get; set; }
        public long codigo_barras_inventario { get; set; }
        public int cantidad_inventario { get; set; }
        public string ubicacion_inventario { get; set; }
        public DateTime fecha_elaboracion { get; set; }
        public DateTime fecha_caducidad { get; set; }

        public Inventario() { }

        public Inventario(long id_inventario, long id_producto, long codigo_barras_inventario,
                         int cantidad_inventario, string ubicacion_inventario,
                         DateTime fecha_elaboracion, DateTime fecha_caducidad)
        {
            this.id_inventario = id_inventario;
            this.id_producto = id_producto;
            this.codigo_barras_inventario = codigo_barras_inventario;
            this.cantidad_inventario = cantidad_inventario;
            this.ubicacion_inventario = ubicacion_inventario;
            this.fecha_elaboracion = fecha_elaboracion;
            this.fecha_caducidad = fecha_caducidad;
        }
    }
}