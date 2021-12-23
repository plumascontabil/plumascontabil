using Demonstrativo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UsuarioController> _logger;
        public UsuarioController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<UsuarioController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(CarregarUsuario());
        }

        public IActionResult Register()
        {
            return View(new RegistrarViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrarViewModel viewModel)
        {
            viewModel.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = viewModel.Email, Email = viewModel.Email };
                var result = await _userManager.CreateAsync(user, viewModel.Password);                

                if (result.Succeeded)
                {                  
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(viewModel.ReturnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        public async Task<IActionResult> Editar(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return View(new EditarViewModel()
            {
                Id = id,
                Email = user.Email,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EditarViewModel viewModel)
        {
            viewModel.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(viewModel.Id);

                user.Email = viewModel.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return LocalRedirect(viewModel.ReturnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(viewModel);

        }

        public IActionResult CarregarUsuario()
        {
            var users = _userManager.Users;
            var usuarioViewModel = new List<UsuarioViewModel>();
            foreach (var user in users)
            {
                usuarioViewModel.Add(new UsuarioViewModel() {Id = user.Id,Email = user.Email, Usuario = user.UserName });
            }
            return View("Usuarios",usuarioViewModel);
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
    }
}
