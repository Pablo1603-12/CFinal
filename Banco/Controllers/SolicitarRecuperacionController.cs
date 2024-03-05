using Banco.DTO;
using Banco.Servicios;
using Banco.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Banco.Controllers
{
    
    public class SolicitarRecuperacionController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;

        public SolicitarRecuperacionController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

       
        [HttpGet]
        [Route("/auth/solicitar-recuperacion")]
        public IActionResult MostrarVistaIniciarRecuperacion()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarVistaIniciarRecuperacion() de la clase SolicitarRecuperacionController");

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarVistaIniciarRecuperacion() de la clase SolicitarRecuperacionController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        
        [HttpPost]
        [Route("/auth/iniciar-recuperacion")]
        public IActionResult ProcesarInicioRecuperacion([Bind("EmailUsuario")] UsuarioDTO usuarioDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController");

                bool envioConExito = _usuarioServicio.iniciarProcesoRecuperacion(usuarioDTO.EmailUsuario);

                if (envioConExito)
                {
                    ViewData["MensajeExitoMail"] = "Proceso de recuperación OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController. " + ViewData["MensajeExitoMail"]);
                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["MensajeErrorMail"] = "No se inició el proceso de recuperación, cuenta de correo electrónico no encontrada.";
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController. " + ViewData["MensajeErrorMail"]);
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }
    }
}
