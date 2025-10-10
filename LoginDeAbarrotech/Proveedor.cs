using System;

namespace LoginDeAbarrotech
{
    public class Proveedor
    {
        public int id_proveedor { get; set; }
        public string nombre_proveedor { get; set; }
        public string responsable_proveedor { get; set; }
        public string direccion_proveedor { get; set; }
        public long telefono_proveedor { get; set; }
        public string email_proveedor { get; set; }
        public string estado_proveedor { get; set; }

        public Proveedor() { }

        public Proveedor(int id_proveedor, string nombre_proveedor, string responsable_proveedor,
                         string direccion_proveedor, long telefono_proveedor, string email_proveedor, string estado_proveedor)
        {
            this.id_proveedor = id_proveedor;
            this.nombre_proveedor = nombre_proveedor;
            this.responsable_proveedor = responsable_proveedor;
            this.direccion_proveedor = direccion_proveedor;
            this.telefono_proveedor = telefono_proveedor;
            this.email_proveedor = email_proveedor;
            this.estado_proveedor = estado_proveedor;
        }
    }
}
