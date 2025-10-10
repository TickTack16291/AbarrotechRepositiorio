using System;

namespace LoginDeAbarrotech
{
    internal class Proveedor
    {
        public long id_proveedor { get; set; }
        public string nombre_proveedor { get; set; }
        public string responsable_proveedor { get; set; }
        public string direccion_proveedor { get; set; }
        public string telefono_proveedor { get; set; }
        public string email_proveedor { get; set; }
        public int estado_proveedor { get; set; }

        public Proveedor() { }

        public Proveedor(long id_proveedor, string nombre_proveedor, string responsable_proveedor,
                         string direccion_proveedor, string telefono_proveedor, string email_proveedor, int estado_proveedor)
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
