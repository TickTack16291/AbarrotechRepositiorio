using System;

namespace LoginDeAbarrotech
{
    public class Compra
    {
        public long id_compra { get; set; }
        public long id_usuario { get; set; }
        public DateTime fecha_compra { get; set; }
        public float total_compra { get; set; }
        public string forma_pago_compra { get; set; }
        public int caja { get; set; }

        public Compra() { }

        public Compra(long id_compra, long id_usuario, DateTime fecha_compra, float total_compra, string forma_pago_compra, int caja)
        {
            this.id_compra = id_compra;
            this.id_usuario = id_usuario;
            this.fecha_compra = fecha_compra;
            this.total_compra = total_compra;
            this.forma_pago_compra = forma_pago_compra;
            this.caja = caja;
        }
    }
}
