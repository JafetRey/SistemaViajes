using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using SistemaViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SistemaViajes.Recursos;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaViajes.Servicios.Implementacion;
using Microsoft.AspNetCore.Identity;

namespace SistemaViajes.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SistemaViajesContext _context;


        public HomeController(ILogger<HomeController> logger, SistemaViajesContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            ViewData["nombreUsuario"] = nombreUsuario;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Colaboradore modelo)
        {
            if (ModelState.IsValid)
            {
                _context.Colaboradores.Add(modelo);
                _context.SaveChanges();

                ViewData["Mensaje"] = "Colaborador creado con éxito";

                return View();
            }

            ViewData["Mensaje"] = "No se pudo crear el colaborador. Por favor, verifica los datos.";
            return View(modelo);
        }

        public IActionResult CrearSucursal()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearSucursal(Sucursale modelo)
        {
            if (ModelState.IsValid)
            {
                _context.Sucursales.Add(modelo);
                _context.SaveChanges();

                ViewData["Mensaje"] = "Sucursal creada con éxito";

                // Limpiar los campos del modelo para una nueva entrada
                modelo.Nombre = string.Empty;
                modelo.Ciudad = string.Empty;
                modelo.BarrioColonia = string.Empty;

                return View(modelo);
            }

            ViewData["Mensaje"] = "No se pudo crear la sucursal. Por favor, verifica los datos.";
            return View(modelo);
        }

        public IActionResult CrearTransportista()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearTransportista(Transportista modelo)
        {
            if (ModelState.IsValid)
            {
                _context.Transportistas.Add(modelo);
                _context.SaveChanges();

                ViewData["Mensaje"] = "Transportista creado con éxito";

                // Limpiar los campos del modelo para una nueva entrada
                modelo.Nombre = string.Empty;
                modelo.TarifaPorKilometro = 0; // Puedes establecer el valor predeterminado que desees

                return View(modelo);
            }

            ViewData["Mensaje"] = "No se pudo crear el transportista. Por favor, verifica los datos.";
            return View(modelo);
        }

        public IActionResult CrearColaboradorSucursal()
        {
            // Verificar si hay registros de colaboradores y sucursales en la base de datos
            var colaboradores = _context.Colaboradores.ToList();
            var sucursales = _context.Sucursales.ToList();

            if (colaboradores.Count == 0 || sucursales.Count == 0)
            {
                TempData["Mensaje"] = "Asegúrate de que haya registros de colaboradores y sucursales en la base de datos antes de crear una relación.";
                return RedirectToAction("Index");
            }

            // Obtener la lista de colaboradores y sucursales para mostrar en los dropdowns
            ViewBag.Colaboradores = new SelectList(colaboradores, "Id", "Nombre");
            ViewBag.Sucursales = new SelectList(sucursales, "Id", "Nombre");

            return View(new ColaboradorSucursal());
        }

        [HttpPost]
        public IActionResult CrearColaboradorSucursal(ColaboradorSucursal modelo)
        {
            // Validar el campo de distancia utilizando el atributo [Range]
            if (ModelState.IsValid)
            {
                // Validar que el ID del colaborador y la sucursal sean números enteros
                if (!int.TryParse(modelo.ColaboradorId.ToString(), out int colaboradorId) || !int.TryParse(modelo.SucursalId.ToString(), out int sucursalId))
                {
                    ModelState.AddModelError("ColaboradorId", "El ID del colaborador debe ser un número entero.");
                    ModelState.AddModelError("SucursalId", "El ID de la sucursal debe ser un número entero.");
                    return View(modelo);
                }

                // Guardar la relación en la base de datos
                _context.ColaboradorSucursals.Add(modelo);
                _context.SaveChanges();

                ViewData["Mensaje"] = "Relación Colaborador-Sucursal creada con éxito";

                // Limpiar los campos del modelo para una nueva entrada
                modelo = new ColaboradorSucursal();
                return View(modelo);
            }

            // Obtener la lista de colaboradores y sucursales para mostrar en los dropdowns
            ViewBag.Colaboradores = new SelectList(_context.Colaboradores, "Id", "Nombre");
            ViewBag.Sucursales = new SelectList(_context.Sucursales, "Id", "Nombre");

            ViewData["Mensaje"] = "No se pudo crear la relación. Por favor, verifica los datos.";
            return View(modelo);
        }

        public IActionResult RegistroViaje()
        {

            // Cargar la lista de sucursales
            var sucursales = _context.Sucursales.ToList();
            ViewBag.Sucursales = new SelectList(sucursales, "Id", "Nombre");

            // Cargar la lista de transportistas
            var transportistas = _context.Transportistas.ToList();
            ViewBag.Transportistas = new SelectList(transportistas, "Id", "Nombre");

            // Cargar la lista de colaboradores
            var colaboradores = _context.Colaboradores.ToList();
            ViewBag.Colaboradores = new SelectList(colaboradores, "Id", "Nombre");

            // Obtener el usuario actual (gerente de tienda)
            var usuarioActual = "UsuarioGerente"; // Reemplaza con la lógica para obtener el usuario actual

            // Pasar el usuario al formulario
            ViewBag.UsuarioRegistrador = usuarioActual;

            return View();
        }

        [HttpPost]
        public IActionResult RegistroViaje(Viaje modelo)
        {
            if (ModelState.IsValid)
            {
                // Realiza las validaciones y el registro del viaje en la base de datos
                var colaboradoresSeleccionados = _context.ColaboradorSucursals.Where(cs => modelo.ColaboradorId.HasValue && cs.ColaboradorId == modelo.ColaboradorId.Value).ToList();
                var sumaDistancias = colaboradoresSeleccionados.Sum(cs => cs.DistanciaKilometros);

                if (sumaDistancias > 100)
                {
                    ModelState.AddModelError("ColaboradorIds", "La suma de las distancias no debe superar los 100 km.");
                    // Recarga la lista de sucursales y transportistas
                    var sucursal = _context.Sucursales.ToList();
                    ViewBag.Sucursales = new SelectList(sucursal, "Id", "Nombre");
                    var transportista = _context.Transportistas.ToList();
                    ViewBag.Transportistas = new SelectList(transportista, "Id", "Nombre");
                    return View(modelo);
                }

                // Verificar si el colaborador ya viajó hoy (puedes implementar esta lógica según tus necesidades)

                // Realizar el registro del viaje en la base de datos
                // Obtener el nombre de usuario del usuario autenticado
                var usuarioActualNombre = User.Identity.Name;
                // Obtener el usuario actual
                var usuarioActual = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuarioActualNombre);

                if (usuarioActual != null)
                {
                    // Asignar el ID del usuario registrado al modelo
                    modelo.UsuarioRegistradorId = usuarioActual.Id;
                }
                else
                {
                    // Manejo de error si no se encuentra el usuario
                    ModelState.AddModelError(string.Empty, "No se pudo encontrar el usuario registrado.");
                    return View(modelo);
                }

                _context.Viajes.Add(modelo);
                _context.SaveChanges();

                // Redirige a una página de éxito o muestra un mensaje de éxito
                return RedirectToAction("RegistroExitoso");
            }

            // Si hay errores de validación, vuelve a mostrar el formulario
            // Recarga la lista de sucursales y transportistas
            var sucursales = _context.Sucursales.ToList();
            ViewBag.Sucursales = new SelectList(sucursales, "Id", "Nombre");
            var transportistas = _context.Transportistas.ToList();
            ViewBag.Transportistas = new SelectList(transportistas, "Id", "Nombre");
            return View(modelo);
        }


        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("IniciarSesion", "Inicio");
        }


    }
}
