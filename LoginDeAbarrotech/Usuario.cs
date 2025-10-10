using System;

namespace LoginDeAbarrotech
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public int id_empleado { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string rol_usuario { get; set; }

        public Usuario() { }

        public Usuario(int id_usuario, int id_empleado, string usuario, string contrasena, string rol_usuario)
        {
            this.id_usuario = id_usuario;
            this.id_empleado = id_empleado;
            this.usuario = usuario;
            this.contrasena = contrasena;
            this.rol_usuario = rol_usuario;
        }
    }
}