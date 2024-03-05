using Banco.DTO;
using DAL.Entidades;

namespace Banco.Servicios
{

    public interface IConvertirAdto
    {
     
        public UsuarioDTO usuarioToDto(Usuario u);

        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario);

    }
}
