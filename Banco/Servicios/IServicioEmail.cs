namespace Banco.Servicios
{
    
    public interface IServicioEmail
    {
       
        public void enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token);

       
        void enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token);
    }
}
