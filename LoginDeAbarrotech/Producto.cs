using System;

namespace LoginDeAbarrotech
{
    public class Producto
    {
        public int id_producto { get; set; }
        public string nombre_producto { get; set; }
        public string marca_producto { get; set; }
        public int presentacion_producto { get; set; }
        public string unidad_medida_producto { get; set; }
        public float precio_venta_producto { get; set; }
        public float precio_compra_producto { get; set; }
        public string estado_producto { get; set; }
        public string categoria_producto { get; set; }
        public int id_proveedor_producto { get; set; }

        public Producto() { }

        public Producto(int id_producto, string nombre_producto, string marca_producto, int presentacion_producto, string unidad_medida_producto, float precio_venta_producto, float precio_compra_producto, string estado_producto, string categoria_producto, int id_proveedor_producto)
        {
            this.id_producto = id_producto;
            this.nombre_producto = nombre_producto;
            this.marca_producto = marca_producto;
            this.presentacion_producto = presentacion_producto;
            this.unidad_medida_producto = unidad_medida_producto;
            this.precio_venta_producto = precio_venta_producto;
            this.precio_compra_producto = precio_compra_producto;
            this.estado_producto = estado_producto;
            this.categoria_producto = categoria_producto;
            this.id_proveedor_producto = id_proveedor_producto;
        }
    }
}