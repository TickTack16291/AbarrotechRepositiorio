using System;

namespace LoginDeAbarrotech
{
    public class Transaccion
    {
        public long id_transaccion { get; set; }
        public long id_inventario { get; set; }
        public long id_inicio_sesion { get; set; }
        public string tipo_movimiento_transaccion { get; set; }
        public int cantidad_modificada_transaccion { get; set; }
        public DateTime fecha_registro_salida_transaccion { get; set; }
        public long? id_venta { get; set; }
        public long? id_compra { get; set; }

        public Transaccion() { }

        public Transaccion(long id_transaccion, long id_inventario, long id_inicio_sesion,
                           string tipo_movimiento_transaccion, int cantidad_modificada_transaccion,
                           DateTime fecha_registro_salida_transaccion, long? id_venta, long? id_compra)
        {
            this.id_transaccion = id_transaccion;
            this.id_inventario = id_inventario;
            this.id_inicio_sesion = id_inicio_sesion;
            this.tipo_movimiento_transaccion = tipo_movimiento_transaccion;
            this.cantidad_modificada_transaccion = cantidad_modificada_transaccion;
            this.fecha_registro_salida_transaccion = fecha_registro_salida_transaccion;
            this.id_venta = id_venta;
            this.id_compra = id_compra;
        }
    }
}