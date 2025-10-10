using System;

namespace LoginDeAbarrotech
{
    public class Empleado
    {
        public long id_empleado { get; set; }
        public string nombre_empleado { get; set; }
        public string direccion_empleado { get; set; }
        public long telefono_empleado { get; set; }
        public string correo_electronico_empleado { get; set; }
        public string rol_empleado { get; set; }
        public float salario_empleado { get; set; }
        public DateTime fecha_inicio_contrato { get; set; }
        public TimeSpan hora_entrada_empleado { get; set; }
        public TimeSpan hora_salida_empleado { get; set; }
        public string estado_empleado { get; set; }
        public DateTime fecha_fin_contrato { get; set; }

        public Empleado() { }

        public Empleado(long id_empleado, string nombre_empleado, string direccion_empleado, long telefono_empleado,
                        string correo_electronico_empleado, string rol_empleado, float salario_empleado,
                        DateTime fecha_inicio_contrato, TimeSpan hora_entrada_empleado, TimeSpan hora_salida_empleado,
                        string estado_empleado, DateTime fecha_fin_contrato)
        {
            this.id_empleado = id_empleado;
            this.nombre_empleado = nombre_empleado;
            this.direccion_empleado = direccion_empleado;
            this.telefono_empleado = telefono_empleado;
            this.correo_electronico_empleado = correo_electronico_empleado;
            this.rol_empleado = rol_empleado;
            this.salario_empleado = salario_empleado;
            this.fecha_inicio_contrato = fecha_inicio_contrato;
            this.hora_entrada_empleado = hora_entrada_empleado;
            this.hora_salida_empleado = hora_salida_empleado;
            this.estado_empleado = estado_empleado;
            this.fecha_fin_contrato = fecha_fin_contrato;
        }
    }
}
