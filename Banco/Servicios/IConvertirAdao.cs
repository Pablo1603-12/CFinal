using Banco.DTO;
using DAL.Entidades;

namespace Banco.Servicios
{
    
    public interface IConvertirAdao
    {
        
        public Usuario usuarioToDao(UsuarioDTO usuarioDTO);

        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO);

    }
}
