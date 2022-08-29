using Demonstrativo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Demonstrativo.Controllers
{
    public class UsuarioController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UsuarioController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ContextIdentity _contextIdentity;
        private readonly Context _context;

        public UsuarioController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<UsuarioController> logger,
            RoleManager<IdentityRole> roleManager,
        ContextIdentity contextIdentity, Context context) : base(context, roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _contextIdentity = contextIdentity;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(CarregarUsuario());
        }

        //[Authorize(Policy = "roleAdministrador")]
        public IActionResult Register()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(new RegistrarViewModel()
            {
                Email = null,
                Password = null,
                ConfirmPassword = null,
                UserRoles = _roleManager.Roles.ToList(),
                EmpresasId = new List<int>()
            });
        }

        [HttpPost]
        //[Authorize(Policy = "roleAdministrador")]
        public async Task<IActionResult> Register(RegistrarViewModel viewModel)
        {
            var role = await _roleManager.FindByIdAsync(viewModel.UserRole);
            viewModel.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = viewModel.Email, Email = viewModel.Email };
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation(((int)EEventLog.Post), "User {email} created.", viewModel.Email);

                    if (viewModel.EmpresasId != null)
                    {
                        foreach (var id in viewModel.EmpresasId)
                        {
                            var usuarioEmpresa = new UsuarioEmpresa();
                            usuarioEmpresa.EmpresaId = id;
                            usuarioEmpresa.UsuarioId = user.Id;
                            _context.UsuarioEmpresa.Add(usuarioEmpresa);
                            _context.SaveChanges();
                        }
                    }

                    await _userManager.AddToRoleAsync(user, role.Name);
                    TempData["criado"] = "criado";
                    return RedirectToAction("CarregarUsuario");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            viewModel.UserRoles = _roleManager.Roles.ToList();
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        //[Authorize(Policy = "roleAdministrador")]
        public async Task<IActionResult> Editar(string id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var nameRole = roles.FirstOrDefault();
            var empresas = _context.UsuarioEmpresa.Where(f => f.UsuarioId == id).ToList();

            var role = _contextIdentity.Roles.FirstOrDefault(x => x.Name == nameRole);
            return View(new EditarViewModel()
            {
                Id = id,
                Email = user.Email,
                UserRoles = _roleManager.Roles.ToList(),
                UserRole = role?.Id,
                EmpresasId = empresas.Select(f => f.EmpresaId).ToList()
            });
        }

        [HttpPost]
        //[Authorize(Policy = "roleAdministrador")]
        public async Task<IActionResult> Editar(EditarViewModel viewModel)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var userRole = await _roleManager.FindByIdAsync(viewModel.UserRole);
            viewModel.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(viewModel.Id);

                user.Email = viewModel.Email;
                var role = _userManager.GetRolesAsync(user).Result; ;
                await _userManager.RemoveFromRolesAsync(user, role);

                await _userManager.AddToRoleAsync(user, userRole.Name);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation(((int)EEventLog.Put), "User {email} edited.", user.Email);

                    var empresas = _context.UsuarioEmpresa.ToList().Where(e => e.UsuarioId == user.Id);
                    foreach (var empresa in empresas)
                    {
                        _context.UsuarioEmpresa.Remove(empresa);
                        _context.SaveChanges();
                    }

                    if (viewModel.EmpresasId != null)
                    {
                        foreach (var id in viewModel.EmpresasId)
                        {
                            var usuarioEmpresa = new UsuarioEmpresa();
                            usuarioEmpresa.EmpresaId = id;
                            usuarioEmpresa.UsuarioId = user.Id;
                            _context.UsuarioEmpresa.Add(usuarioEmpresa);
                            _context.SaveChanges();
                        }
                    }

                }

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Any(roleName => roleName != userRole.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user, roles.FirstOrDefault());
                    await _userManager.AddToRoleAsync(user, userRole.Name);
                }

                if (result.Succeeded)
                {
                    return RedirectToAction("CarregarUsuario");
                    //return LocalRedirect(viewModel.ReturnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            var editarViewModel = new EditarViewModel
            {
                UserRole = userRole.Id
            };
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return RedirectToAction("CarregarUsuario");
        }
        //[Authorize(Policy = "roleAdministrador")]
        public IActionResult CarregarUsuario()
        {
            var users = _userManager.Users.ToList();
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var usuarioViewModel = new List<UsuarioViewModel>();
            foreach (var user in users)
            {
                usuarioViewModel.Add(new UsuarioViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Usuario = user.UserName,
                    UserRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
                });
            }
            return View("Usuarios", usuarioViewModel);
        }
        public IActionResult Login()

        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            viewModel.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    this.MetodClaim(viewModel);
                    _logger.LogInformation(((int)EEventLog.Post), "User {email} logged in.", viewModel.Email);

                    return LocalRedirect(viewModel.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Verifique os dados de acesso.");
                    return View(viewModel);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        private void MetodClaim(LoginViewModel viewModel)
        {
            var users = _userManager.Users.Where(x => x.Email == viewModel.Email).FirstOrDefault();

            var role = _userManager.GetRolesAsync(users).Result.FirstOrDefault();

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, users.Id));
            claims.Add(new Claim(ClaimTypes.Name, users.UserName));
            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                var apx = _roleManager.Roles.Where(f => f.Name == role).FirstOrDefault().Id;
                claims.Add(new Claim(ClaimTypes.Rsa, apx));
            }

            var minhaIdentity = new ClaimsIdentity(claims, "Usuario");
            var userPrincipal = new ClaimsPrincipal(new[] { minhaIdentity });
            HttpContext.SignInAsync(userPrincipal);
        }

        //[Authorize(Policy = "roleAdministrador")]
        public async Task<IActionResult> Logout()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            await _signInManager.SignOutAsync();
            _logger.LogInformation(((int)EEventLog.Get), "User logged out.");

            HttpContext.Session.Remove("empresaId");
            HttpContext.Session.Remove("competenciasId");
            await HttpContext.SignOutAsync();
            return View("Login");
        }

        public IActionResult AccessDenied()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View("AccessDenied");
        }

    }
}
