using Demonstrativo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UsuarioController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ContextIdentity _contextIdentity;

        public UsuarioController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<UsuarioController> logger,
            RoleManager<IdentityRole> roleManager,
            ContextIdentity contextIdentity)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _contextIdentity = contextIdentity;
        }

        public IActionResult Index()
        {
            return View(CarregarUsuario());
        }

        [Authorize(Policy = "roleAdministrador")]
        public IActionResult Register()
        {
            return View(new RegistrarViewModel()
            {
                UserRoles = _roleManager.Roles.ToList()
            });
        }

        [HttpPost]
        [Authorize(Policy = "roleAdministrador")]
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
                    await _userManager.AddToRoleAsync(user, role.Name);
                    return LocalRedirect(viewModel.ReturnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            viewModel.UserRoles = _roleManager.Roles.ToList();

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        [Authorize(Policy = "roleAdministrador")]
        public async Task<IActionResult> Editar(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var nameRole = roles.FirstOrDefault();

            var role = _contextIdentity.Roles.FirstOrDefault(x => x.Name == nameRole);

            return View(new EditarViewModel()
            {
                Id = id,
                Email = user.Email,
                UserRoles = _roleManager.Roles.ToList(),
                UserRole = role?.Id,
            });
        }

        [HttpPost]
        [Authorize(Policy = "roleAdministrador")]
        public async Task<IActionResult> Editar(EditarViewModel viewModel)
        {

            var userRole = await _roleManager.FindByIdAsync(viewModel.UserRole);
            viewModel.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(viewModel.Id);

                user.Email = viewModel.Email;

                var result = await _userManager.UpdateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Any(roleName => roleName != userRole.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user, roles.FirstOrDefault());
                    await _userManager.AddToRoleAsync(user, userRole.Name);
                }

                if (result.Succeeded)
                {
                    return LocalRedirect(viewModel.ReturnUrl);
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

            return View("Usuarios", viewModel);
        }
        [Authorize(Policy = "roleAdministrador")]
        public IActionResult CarregarUsuario()
        {
            var users = _userManager.Users.ToList();

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
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            viewModel.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(viewModel.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(viewModel);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }
        [Authorize(Policy = "roleAdministrador")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return View("Login");
        }

        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

    }
}
