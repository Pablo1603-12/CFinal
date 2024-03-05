using Banco.DTO;
using Banco.Utils;
using DAL.Entidades;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace Banco.Servicios
{
    /// <summary>
    /// Clase que implementa la interfaz IConvertirAdto y detalla la lógica de los metodos que serán necesarios para el paso de DAO a DTO
    /// </summary>
    public class ConvertirAdtoImpl : IConvertirAdto
    {
        public UsuarioDTO usuarioToDto(Usuario u)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método usuarioToDto() de la clase ConvertirAdtoImpl");

                UsuarioDTO dto = new UsuarioDTO();
                string[] nombreApellidos = u.NombreApellidos.Split(' ');

                if (nombreApellidos.Length > 0)
                {
                    dto.NombreUsuario = nombreApellidos[0];

                    if (nombreApellidos.Length > 1)
                    {
                        StringBuilder apellidos = new StringBuilder();
                        for (int i = 1; i < nombreApellidos.Length; i++)
                        {
                            apellidos.Append(nombreApellidos[i]).Append(" ");
                        }
                        dto.ApellidosUsuario = apellidos.ToString().Trim();
                    }

                    dto.TlfUsuario = u.TlfMovil;
                    dto.EmailUsuario = u.Email;
                    dto.ClaveUsuario = u.Contraseña;
                    dto.Token = u.TokenRecuperacion;
                    dto.ExpiracionToken = u.FchExpiracionToken;
                    dto.Id = u.IdUsuario;
                    dto.FechaRegistro = u.FchRegistro;
                    dto.CuentaConfirmada = u.CuentaConfirmada;
                    dto.Rol = u.Rol;

                    if (u.Foto != null)
                    {
                        dto.Foto = u.Foto;
                    }

                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método usuarioToDto() de la clase ConvertirAdtoImpl");
                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - usuarioToDto()] - Error al convertir usuario DAO a usuarioDTO (return null): {e}");
                return null;
            }
        }

        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listaUsuarioToDto() de la clase ConvertirAdtoImpl");

                List<UsuarioDTO> listaDto = new List<UsuarioDTO>();
                foreach (Usuario u in listaUsuario)
                {
                    listaDto.Add(usuarioToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaUsuarioToDto() de la clase ConvertirAdtoImpl");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - listaUsuarioToDto()] - Error al convertir lista de usuario DAO a lista de usuarioDTO (return null): {e}");
            }
            return null;
        }

        
    }
}
