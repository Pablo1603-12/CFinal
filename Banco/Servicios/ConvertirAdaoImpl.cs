using Banco.DTO;
using Banco.Utils;
using DAL.Entidades;

namespace Banco.Servicios
{
    /// <summary>
    /// Clase que implementa la interfaz IConvertirAdao y detalla la lógica de los metodos que serán necesarios para el paso de DTO a DAO
    /// </summary>
    public class ConvertirAdaoImpl : IConvertirAdao
    {

        public Usuario usuarioToDao(UsuarioDTO usuarioDTO)
        {

            try
            {
                Usuario usuarioDao = new Usuario();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método usuarioToDao() de la clase ConvertirAdaoImpl");

                usuarioDao.IdUsuario = usuarioDTO.Id;
                usuarioDao.NombreApellidos = $"{usuarioDTO.NombreUsuario} {usuarioDTO.ApellidosUsuario}";
                usuarioDao.Email = usuarioDTO.EmailUsuario;
                usuarioDao.Contraseña = usuarioDTO.ClaveUsuario;
                usuarioDao.TlfMovil = usuarioDTO.TlfUsuario;
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método usuarioToDao() de la clase ConvertirAdaoImpl");

                return usuarioDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ConvertirAdaoImpl - UsuarioToDao()] - Al convertir usuarioDTO a usuarioDAO (return null): {e}");
                return null;
            }
        }

        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO)
        {

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listUsuarioToDao() de la clase ConvertirAdaoImpl");

                List<Usuario> listaUsuarioDao = new List<Usuario>();
                foreach (UsuarioDTO usuarioDTO in listaUsuarioDTO)
                {
                    listaUsuarioDao.Add(usuarioToDao(usuarioDTO));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listUsuarioToDao() de la clase ConvertirAdaoImpl");

                return listaUsuarioDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ConvertirAdaoImpl - listUsuarioToDao()] - Al convertir lista de usuarioDTO a lista de usuarioDAO (return null): {e}");
            }
            return null;
        }

    }
}
