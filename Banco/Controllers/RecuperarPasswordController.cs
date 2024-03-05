using Banco.DTO;
using Banco.Servicios;
using Banco.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Banco.Controllers
{
   
    public class RecuperarPasswordController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;

        public RecuperarPasswordController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

       
        [HttpGet]
        [Route("/auth/recuperar")]
        public IActionResult MostrarVistaRecuperar([FromQuery(Name = "token")] string token)
        {

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarVistaRecuperar() de la clase RecuperarPasswordController");

                UsuarioDTO usuario = _usuarioServicio.obtenerUsuarioPorToken(token);

                if (usuario != null)
                {
                    ViewData["UsuarioDTO"] = usuario;
                }
                else
                {
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido o el usuario no se ha encontrado";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarVistaRecuperar() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenValidez"]);
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarVistaRecuperar() de la clase RecuperarPasswordController");
                return View("~/Views/Home/recuperar.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarVistaRecuperar() de la clase RecuperarPasswordController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        
        [HttpPost]
        [Route("/auth/recuperar")]
        public IActionResult ProcesarRecuperacionContraseña(UsuarioDTO usuarioDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController");

                UsuarioDTO usuarioExistente = _usuarioServicio.obtenerUsuarioPorToken(usuarioDTO.Token);

                if (usuarioExistente == null)
                {
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenValidez"]);
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }

                if (usuarioExistente.ExpiracionToken.HasValue && usuarioExistente.ExpiracionToken.Value < DateTime.Now)
                {
                    ViewData["MensajeErrorTokenExpirado"] = "El enlace de recuperación ha expirado";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenExpirado"]);
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }

                bool modificadaPassword = _usuarioServicio.modificarContraseñaConToken(usuarioDTO);

                if (modificadaPassword)
                {
                    ViewData["ContraseñaModificadaExito"] = "Contraseña modificada OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["ContraseñaModificadaExito"]);
                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["ContraseñaModificadaError"] = "Error al cambiar de contraseña";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["ContraseñaModificadaError"]);
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }


    }
}
