using Banco.DTO;
using Banco.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Security.Cryptography;

namespace Banco.Servicios
{
    
    public class UsuarioServicioImpl : IUsuarioServicio
    {
        private readonly BancoContext _contexto;
        private readonly IServicioEncriptar _servicioEncriptar;
        private readonly IConvertirAdao _convertirAdao;
        private readonly IConvertirAdto _convertirAdto;
        private readonly IServicioEmail _servicioEmail;

        public UsuarioServicioImpl(
            BancoContext contexto,
            IServicioEncriptar servicioEncriptar,
            IConvertirAdao convertirAdao,
            IConvertirAdto convertirAdto,
            IServicioEmail servicioEmail
        )
        {
            _contexto = contexto;
            _servicioEncriptar = servicioEncriptar;
            _convertirAdao = convertirAdao;
            _convertirAdto = convertirAdto;
            _servicioEmail = servicioEmail;
            CrearUsuarioAdmin();
        }
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrarUsuario() de la clase UsuarioServicioImpl");

                var usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.Email == userDTO.EmailUsuario && !u.CuentaConfirmada);

                if (usuarioExistente != null)
                {
                    userDTO.EmailUsuario = "EmailNoConfirmado";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarUsuario() de la clase UsuarioServicioImpl");
                    return userDTO;
                }

                var emailExistente = _contexto.Usuarios.FirstOrDefault(u => u.Email == userDTO.EmailUsuario && u.CuentaConfirmada);

                if (emailExistente != null)
                {
                    userDTO.EmailUsuario = "EmailRepetido";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarUsuario() de la clase UsuarioServicioImpl");
                    return userDTO;
                }

                userDTO.ClaveUsuario = _servicioEncriptar.Encriptar(userDTO.ClaveUsuario);
                Usuario usuarioDao = _convertirAdao.usuarioToDao(userDTO);
                usuarioDao.FchRegistro = DateTime.Now;
                usuarioDao.Rol = "ROLE_USER";
                string token = generarToken();
                usuarioDao.TokenRecuperacion = token;

                _contexto.Usuarios.Add(usuarioDao);
                _contexto.SaveChanges();

                string nombreUsuario = usuarioDao.NombreApellidos;
                _servicioEmail.enviarEmailConfirmacion(userDTO.EmailUsuario, nombreUsuario, token);

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarUsuario() de la clase UsuarioServicioImpl");

                return userDTO;
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - registrarUsuario()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return null;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - registrarUsuario()] Error al registrar un usuario: " + e.Message);
                return null;
            }


        }
        private string generarToken()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método generarToken() de la clase UsuarioServicioImpl");

                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {
                    byte[] tokenBytes = new byte[30];
                    rng.GetBytes(tokenBytes);

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método generarToken() de la clase UsuarioServicioImpl");

                    return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
                }
            }
            catch (ArgumentException ae)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl -  generarToken()] Error al generar un token de usuario " + ae.Message);
                return null;
            }

        }

        public bool confirmarCuenta(string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método confirmarCuenta() de la clase UsuarioServicioImpl");

                Usuario? usuarioExistente = _contexto.Usuarios.Where(u => u.TokenRecuperacion == token).FirstOrDefault();

                if (usuarioExistente != null && !usuarioExistente.CuentaConfirmada)
                {
                    // Entra en esta condición si el usuario existe y su cuenta no se ha confirmado
                    usuarioExistente.CuentaConfirmada = true;
                    usuarioExistente.TokenRecuperacion = null;
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método confirmarCuenta() de la clase UsuarioServicioImpl. Cuenta confirmada OK.");

                    return true;
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método confirmarCuenta() de la clase UsuarioServicioImpl. La cuenta no existe o ya está confirmada");
                    return false;
                }
            }
            catch (ArgumentException ae)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - confirmarCuenta()] Error al confirmar la cuenta " + ae.Message);
                return false;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - confirmarCuenta()] Error al confirmar la cuenta " + e.Message);
                return false;
            }
        }

        public bool iniciarProcesoRecuperacion(string emailUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método iniciarProcesoRecuperacion() de la clase UsuarioServicioImpl");

                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.Email == emailUsuario);

                if (usuarioExistente != null)
                {
                    // Generar el token y establecer la fecha de expiración
                    string token = generarToken();
                    DateTime fechaExpiracion = DateTime.Now.AddMinutes(1);

                    // Actualizar el usuario con el nuevo token y la fecha de expiración
                    usuarioExistente.TokenRecuperacion = token;
                    usuarioExistente.FchExpiracionToken = fechaExpiracion;

                    // Actualizar el usuario en la base de datos
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    // Enviar el correo de recuperación
                    string nombreUsuario = usuarioExistente.NombreApellidos;
                    _servicioEmail.enviarEmailRecuperacion(emailUsuario, nombreUsuario, token);

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método iniciarProcesoRecuperacion() de la clase UsuarioServicioImpl.");

                    return true;
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] El usuario con email " + emailUsuario + " no existe");
                    return false;
                }
            }
            catch (DbUpdateException dbe)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - iniciarProcesoRecuperacion()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - iniciarProcesoRecuperacion()] Error al iniciar el proceso de recuperación: " + ex.Message);
                return false;
            }
        }

        public UsuarioDTO obtenerUsuarioPorToken(string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerUsuarioPorToken() de la clase UsuarioServicioImpl");

                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == token);

                if (usuarioExistente != null)
                {
                    UsuarioDTO usuario = _convertirAdto.usuarioToDto(usuarioExistente);
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerUsuarioPorToken() de la clase UsuarioServicioImpl.");
                    return usuario;
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerUsuarioPorToken() de la clase UsuarioServicioImpl. No existe el usuario con el token " + token);
                    return null;
                }
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - obtenerUsuarioPorToken()] Error al obtener usuario por token " + e.Message);
                return null;
            }
        }

        public bool modificarContraseñaConToken(UsuarioDTO usuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método modificarContraseñaConToken() de la clase UsuarioServicioImpl");

                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == usuario.Token);

                if (usuarioExistente != null)
                {
                    string nuevaContraseña = _servicioEncriptar.Encriptar(usuario.ClaveUsuario);
                    usuarioExistente.Contraseña = nuevaContraseña;
                    usuarioExistente.TokenRecuperacion = null; // Se establece como null para invalidar el token ya consumido al cambiar la contraseña
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método modificarContraseñaConToken() de la clase UsuarioServicioImpl.");
                    return true;
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - modificarContraseñaConToken()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - verificarCredenciales()] Error al modificar contraseña del usuario: " + e.Message);
                return false;
            }
            return false;
        }

        public bool verificarCredenciales(string emailUsuario, string claveUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método verificarCredenciales() de la clase UsuarioServicioImpl");

                string contraseñaEncriptada = _servicioEncriptar.Encriptar(claveUsuario);
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.Email == emailUsuario && u.Contraseña == contraseñaEncriptada);
                if (usuarioExistente == null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método verificarCredenciales() de la clase UsuarioServicioImpl. Username no encontrado");
                    return false;
                }
                if (!usuarioExistente.CuentaConfirmada)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método verificarCredenciales() de la clase UsuarioServicioImpl. El usuario no tiene la cuenta confirmada");
                    return false;
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método verificarCredenciales() de la clase UsuarioServicioImpl");

                return true;
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - verificarCredenciales()] Error al comprobar las credenciales del usuario: " + e.Message);
                return false;
            }

        }

        public UsuarioDTO obtenerUsuarioPorEmail(string email)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerUsuarioPorEmail() de la clase UsuarioServicioImpl");

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                var usuario = _contexto.Usuarios.FirstOrDefault(u => u.Email == email);

                if (usuario != null)
                {
                    usuarioDTO = _convertirAdto.usuarioToDto(usuario);
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerUsuarioPorEmail() de la clase UsuarioServicioImpl");

                return usuarioDTO;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - obtenerUsuarioPorEmail()] Error al obtener el usuario por email: " + e.Message);
                return null;
            }
        }

        public List<UsuarioDTO> obtenerTodosLosUsuarios()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodosLosUsuarios() de la clase UsuarioServicioImpl");

            return _convertirAdto.listaUsuarioToDto(_contexto.Usuarios.ToList());
        }

        public UsuarioDTO buscarPorId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorId() de la clase UsuarioServicioImpl");

                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                if (usuario != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método buscarPorId() de la clase UsuarioServicioImpl");

                    return _convertirAdto.usuarioToDto(usuario);
                }
            }
            catch (ArgumentException iae)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - buscarPorId()] Al buscar el usuario por su id " + iae.Message);
            }
            return null;
        }

        public void eliminar(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método eliminar() de la clase UsuarioServicioImpl");

                Usuario? usuario = _contexto.Usuarios.Find(id);
                if (usuario != null)
                {
                    _contexto.Usuarios.Remove(usuario);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminar() de la clase UsuarioServicioImpl. Usuario eliminado OK");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - eliminar()] Error de persistencia al eliminar el usuario por su id " + dbe.Message);
            }
        }

        public void actualizarUsuario(UsuarioDTO usuarioModificado)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método actualizarUsuario() de la clase UsuarioServicioImpl");

                Usuario? usuarioActual = _contexto.Usuarios.Find(usuarioModificado.Id);

                if (usuarioActual != null)
                {
                    usuarioActual.NombreApellidos = usuarioModificado.NombreUsuario + " " + usuarioModificado.ApellidosUsuario;
                    usuarioActual.TlfMovil = usuarioModificado.TlfUsuario;
                    usuarioActual.Rol = usuarioModificado.Rol;
                    usuarioActual.Foto = usuarioModificado.Foto;

                    _contexto.Usuarios.Update(usuarioActual);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método actualizarUsuario() de la clase UsuarioServicioImpl. Usuario actualizado OK");
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método actualizarUsuario() de la clase UsuarioServicioImpl. Usuario no encontrado");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - actualizarUsuario()] Error de persistencia al modificar el usuario " + dbe.Message);
            }
        }

        public int contarUsuariosPorRol(string rol)
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método contarUsuariosPorRol() de la clase UsuarioServicioImpl");
            return _contexto.Usuarios.Count(u => u.Rol == rol);
        }

        public List<UsuarioDTO> buscarPorCoincidenciaEnEmail(string palabra)
        {
            try
            {
                List<Usuario> usuarios = _contexto.Usuarios.Where(u => u.Email.Contains(palabra)).ToList();
                if (usuarios != null)
                {
                    return _convertirAdto.listaUsuarioToDto(usuarios);
                }
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - buscarPorCoincidenciaEnEmail()] Al buscar el usuario por su email " + e.Message);
            }
            return null;
        }


        public void CrearUsuarioAdmin()
        {
            try
            {
                // Verifica si ya existe un usuario con el correo electrónico "admin@admin.es"
                var usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.Email == "admin@admin.es");

                // Si no existe, crea un nuevo usuario administrador
                if (usuarioExistente == null)
                {
                    // Crea un nuevo objeto de Usuario con los datos del administrador
                    var usuarioAdmin = new Usuario
                    {
                        NombreApellidos = "Administrador",
                        Email = "admin@admin.es",
                        Contraseña = _servicioEncriptar.Encriptar("admin"), // Encripta la contraseña "admin"
                        CuentaConfirmada = true, // Puedes establecer esto según tus requerimientos
                        Rol = "ROLE_ADMIN",
                        FchRegistro = DateTime.Now, // Fecha de registro actual
                        Foto = null, // Opcional: puedes establecer la foto del administrador si es necesario
                        TlfMovil = "", // Opcional: establece el teléfono móvil del administrador si es necesario
                        TokenRecuperacion = null, // Opcional: no hay token de recuperación al crear el administrador
                        FchExpiracionToken = null // Opcional: no hay fecha de expiración del token al crear el administrador
                    };

                    // Agrega el usuario administrador al contexto y guarda los cambios en la base de datos
                    _contexto.Usuarios.Add(usuarioAdmin);
                    _contexto.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante la creación del usuario administrador
                Console.WriteLine("Error al crear el usuario administrador: " + ex.Message);
            }
        }

    }


}
