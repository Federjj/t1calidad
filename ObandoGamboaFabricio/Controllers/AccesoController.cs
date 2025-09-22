// Importa las bibliotecas necesarias para autenticación, autorización y manejo de datos.
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ObandoGamboaFabricio.Data;
using ObandoGamboaFabricio.Models;
using ObandoGamboaFabricio.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Controllers
{
    // Define el controlador AccesoController para manejar la lógica de registro, login y logout.
    public class AccesoController : Controller
    {
        // Define el contexto de base de datos para interactuar con la base de datos.
        private readonly appDbContext appDbContext;
        // Define un objeto para manejar el hashing de contraseñas.
        private readonly PasswordHasher<Usuarios> _passwordHasher;

        // Constructor que inicializa el contexto de base de datos y el manejador de contraseñas.
        public AccesoController(appDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            _passwordHasher = new PasswordHasher<Usuarios>();
        }

        // Acción para mostrar la vista de registro.
        [HttpGet]
        public IActionResult Registro()
        {
            // Obtiene la lista de roles desde la base de datos y la pasa a la vista.
            ViewBag.Roles = appDbContext.Roles.ToList();
            return View();
        }

        // Acción para manejar el registro de usuarios.
        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioVM userVM)
        {
            // Verifica si el correo ya está registrado (correo único) en la base de datos.
            if (appDbContext.Usuarios.Any(u => u.Correo == userVM.CorreoVM))
            {
                TempData["ErrorMessage"] = "El usuaio con este correo ya existe.";
                ViewBag.Roles = appDbContext.Roles.ToList();
                return View(userVM);
            }

            // Verifica si las contraseñas coinciden.
            if (userVM.PasswordVM != userVM.RepPasswordVM)
            {
                TempData["ErrorMessage"] = "Las contraseñas no coinciden.";
                return View();
            }

            // Busca si existe un usuario administrador en la base de datos.
            Usuarios? usuario_admin = await appDbContext.Usuarios.Where(modelo => modelo.RolID == 1).FirstOrDefaultAsync();

            // Asigna el rol normal si ya existe un administrador.
            if (usuario_admin != null)
            {
                userVM.RolID_VM = 2;
            }

            // Asigna el rol de administrador si no existe ningun adminstrador.
            if (usuario_admin == null)
            {
                userVM.RolID_VM = 1;
            }

            // Crea un nuevo usuario con los datos proporcionados.
            Usuarios usernw = new Usuarios()
            {
                Nombre = userVM.NombreVM,
                Apellido = userVM.ApellidoVM,
                Correo = userVM.CorreoVM,
                RolID = userVM.RolID_VM,
            };

            // Hashea la contraseña del usuario.
            usernw.Password = _passwordHasher.HashPassword(usernw, userVM.PasswordVM);
            // Agrega el usuario a la base de datos.
            appDbContext.Usuarios.Add(usernw);
            // Guarda los cambios en la base de datos.
            appDbContext.SaveChanges();
            // Redirige a la vista de login.
            return RedirectToAction("Login", "Acceso");
        }

        // Acción para mostrar la vista de login.

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Acción para manejar la lógica de login.
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            // Busca el usuario en la base de datos por correo y carga su rol.
            var usuario = await appDbContext.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Correo == loginVM.Correo);

            // Si el usuario no existe, retorna la vista de login.
            if (usuario == null)
            {
                TempData["ErrorMessage"] = "El usuario no existe.";
                return View();
            }

            // Verifica la contraseña del usuario.
            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, loginVM.Password);

            // Si la contraseña es incorrecta, retorna la vista de login.
            if (resultado == PasswordVerificationResult.Failed)
            {
                TempData["ErrorMessage"] = "Email o contraseña incorrecta.";
                return View();
            }

            // Crea una lista de claims para el usuario autenticado.
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
            };

            // Crea una identidad de claims para el usuario.
            var claimsIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Inicia sesión para el usuario.
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);

            // Redirige según el rol del usuario.
            if (usuario.Rol.Nombre == "Admin")
            {
                return RedirectToAction("Index", "Acceso");
            }
            else if (usuario.Rol.Nombre == "Usuario")
            {
                return RedirectToAction("Index", "Articulo");
            }
            else
            {
                return RedirectToAction("Registro", "Acceso");
            }
        }

        // Acción para cerrar sesión.
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Acción para redirigir a la lista de artículos.
        public async Task<IActionResult> ListaArti()
        {
            return RedirectToAction("Index", "Articulo");
        }

        // Acción para mostrar la vista de creación de usuarios.
        [Authorize] //se usa authoize para proteger la vista
        [HttpGet]
        public IActionResult Create()
        {
            var rolUsuario = User.FindFirstValue(ClaimTypes.Role); //guardamos al usuario acual (Su rol)
            if (rolUsuario != "Admin") //validamos que el rol del usuario sea admin para redireccionarlo a la pag. crear, si no, se redirecciona al index
            {
                TempData["ErrorMessage"] = "No tiene permiso para crear usuarios. Solo los administradores pueden realizar esta acción.";
                return RedirectToAction("Index", "Acceso");
            }

            return View();
        }

        // Acción para manejar la creación de usuarios.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(UsuarioVM userVM)
        {
            var rolUsuario = User.FindFirstValue(ClaimTypes.Role); //se guarda el rol del usuario como arriba
            if (rolUsuario != "Admin") //si no tiene el rol de admin no se creará el usuario y se redireccionará al index.
            {
                TempData["ErrorMessage"] = "No tiene permiso para crear usuarios. Solo los administradores pueden realizar esta acción.";
                return RedirectToAction("Index", "Acceso");
            }
            //si si tiene el rol admin se creará el usuario
            else
            {
                // Crea un nuevo usuario con los datos proporcionados.
                Usuarios usernw = new Usuarios()
                {
                    Nombre = userVM.NombreVM,
                    Apellido = userVM.ApellidoVM,
                    Correo = userVM.CorreoVM,
                    RolID = userVM.RolID_VM,
                };

                // Hashea la contraseña del usuario.
                usernw.Password = _passwordHasher.HashPassword(usernw, userVM.PasswordVM);
                // Agrega el usuario a la base de datos.
                appDbContext.Usuarios.Add(usernw);
                // Guarda los cambios en la base de datos.
                appDbContext.SaveChanges();
                
            }

            return RedirectToAction("Index");
        }

        // Acción para mostrar la lista de usuarios.
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //cargamos cada usuario
            var usuarios = await appDbContext.Usuarios.Include(u => u.Rol).ToListAsync();
            return View(usuarios);
        }

        // Acción para mostrar la vista de edición de usuarios.
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //seguimos la misma lógica que en Create para el usuario pero para el edit
            var rolUsuario = User.FindFirstValue(ClaimTypes.Role);
            if (rolUsuario != "Admin")
            {
                TempData["ErrorMessage"] = "No tiene permiso para editar usuarios. Solo los administradores pueden realizar esta acción.";
            }
            //se guadan los cambios del viewmodel para el usuario
            else
            {
                var usuario = await appDbContext.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                UsuarioVM usuarioVM = new UsuarioVM()
                {
                    IdUsuario = usuario.IdUsuario,
                    NombreVM = usuario.Nombre,
                    ApellidoVM = usuario.Apellido,
                    CorreoVM = usuario.Correo,
                    PasswordVM = usuario.Password,
                    RolID_VM = usuario.RolID
                };

                return View(usuarioVM);
            }
            return RedirectToAction("Index", "Acceso");
        }

        // Acción para manejar la edición de usuarios.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(UsuarioVM usuarioVM)
        {
            //seguimos la misma lógica que en Create de usuarios, si no es rol admin no se aplicaraán los cambos.
            var rolUsuario = User.FindFirstValue(ClaimTypes.Role);
            if (rolUsuario != "Admin")
            {
                TempData["ErrorMessage"] = "No tiene permiso para editar usuarios. Solo los administradores pueden realizar esta acción.";
            }
            else
            {
                    Usuarios usuario = await appDbContext.Usuarios.FindAsync(usuarioVM.IdUsuario);

                    if (usuario == null)
                    {
                        return NotFound();
                    }

                    usuario.Nombre = usuarioVM.NombreVM;
                    usuario.Apellido = usuarioVM.ApellidoVM;
                    usuario.Correo = usuarioVM.CorreoVM;
                    usuario.Password = _passwordHasher.HashPassword(usuario, usuarioVM.PasswordVM);
                    usuario.RolID = usuarioVM.RolID_VM;

                    appDbContext.Usuarios.Update(usuario);
                    await appDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                
            }

            return RedirectToAction("Index", "Acceso");
        }

        // Acción para manejar la eliminación de usuarios.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            //sigue la misma lógica que Create, se valida el rol
            var rolUsuario = User.FindFirstValue(ClaimTypes.Role);
            if (rolUsuario != "Admin")
            {
                TempData["ErrorMessage"] = "No tiene permiso para eliminar usuarios. Solo los administradores pueden realizar esta acción.";
            }
            else
            {
                var usuario = await appDbContext.Usuarios.FindAsync(id);
                if (usuario != null)
                {
                    appDbContext.Usuarios.Remove(usuario);
                    await appDbContext.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
