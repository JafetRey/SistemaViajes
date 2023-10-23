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
            if (!_context.Colaboradores.Any() || !_context.Sucursales.Any())
            {
                TempData["Mensaje"] = "Asegúrate de que haya registros de colaboradores y sucursales en la base de datos antes de crear una relación.";
                return RedirectToAction("Index", "Home");
            }

            // Obtener la lista de colaboradores y sucursales para mostrar en los dropdowns
            ViewBag.Colaboradores = _context.Colaboradores.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            }).ToList();

            ViewBag.Sucursales = _context.Sucursales.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Nombre
            }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult CrearColaboradorSucursal(ColaboradorSucursal modelo)
        {
            if (ModelState.IsValid)
            {
                _context.ColaboradorSucursals.Add(modelo);
                _context.SaveChanges();

                ViewData["Mensaje"] = "Relación creada con éxito";

                // Limpiar los campos del modelo para una nueva entrada
                modelo.DistanciaKilometros = 0; // O establecer el valor predeterminado que desees

                return View(modelo);
            }

            // Obtener la lista de colaboradores y sucursales para mostrar en los dropdowns
            ViewBag.Colaboradores = _context.Colaboradores.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            }).ToList();

            ViewBag.Sucursales = _context.Sucursales.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Nombre
            }).ToList();

            ViewData["Mensaje"] = "No se pudo crear la relación. Por favor, verifica los datos.";
            return View(modelo);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("IniciarSesion", "Inicio");
        }


    }
}
