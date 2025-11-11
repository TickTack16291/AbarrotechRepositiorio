using System;

namespace LoginDeAbarrotech
{
    public class Venta
    {
        public int id_venta { get; set; }
        public int id_usuario { get; set; }
        public DateTime fecha_venta { get; set; }
        public float total_venta { get; set; }
        public string forma_pago_venta { get; set; }
        public int caja { get; set; }

        public Venta() { }

        public Venta(int id_venta, int id_usuario, DateTime fecha_venta, float total_venta, string forma_pago_venta, int caja)
        {
            this.id_venta = id_venta;
            this.id_usuario = id_usuario;
            this.fecha_venta = fecha_venta;
            this.total_venta = total_venta;
            this.forma_pago_venta = forma_pago_venta;
            this.caja = caja;
        }
    }
}