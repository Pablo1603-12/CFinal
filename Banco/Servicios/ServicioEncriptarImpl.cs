using Banco.Utils;
using System.Security.Cryptography;
using System.Text;

namespace Banco.Servicios
{
    /// <summary>
    /// Clase que implementa la interfaz IServicioEncriptar y detalla la lógica de los metodos que serán necesarios para el encriptado de contraseñas
    /// </summary>
    public class ServicioEncriptarImpl : IServicioEncriptar
    {
        public string Encriptar(string contraseña)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Encriptar() de la clase ServicioEncriptarImpl");

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(contraseña);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método Encriptar() de la clase ServicioEncriptarImpl");
                    return hash;
                }
            }
            catch (ArgumentException e)
            {
                EscribirLog.escribirEnFicheroLog("[Error  ServicioEncriptarImpl - Encriptar()] Error al encriptar: " + e.Message);
                return null;
            }

        }
    }
}
