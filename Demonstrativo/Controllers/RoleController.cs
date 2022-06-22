using Demonstrativo.Models;
using DomainService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Context _context;
        private readonly ILogger<RoleManager<IdentityRole>> _logger;

        //private readonly RoleDomainService _roleDomainService;


        public RoleController(RoleManager<IdentityRole> roleManager, Context context,
               ILogger<RoleManager<IdentityRole>> logger) : base(context, roleManager)
        {
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
            //_roleDomainService = roleDomainService;
        }

        public IActionResult Index()
        {
            Init();
            var roles = _roleManager.Roles.ToList();
            CarregarRoles();
            return View(roles);
        }
        [Authorize(Policy = "roleAdministrador")]
        public IActionResult Create()
        {
            Init();
            return View(new RegistrarRoleTelaViewModel());
        }

        private void Init()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            ViewBag.TelaId = _context.Telas.ToList();
        }

        [HttpPost]
        [Authorize(Policy = "roleAdministrador")]
        public async Task<IActionResult> Create(RegistrarRoleTelaViewModel data)
        {

            var newRole = new IdentityRole();
            newRole.Name = data.Role;
            await _roleManager.CreateAsync(newRole);
            _context.SaveChanges();
            var role = _roleManager.GetRoleIdAsync(newRole);
            foreach (var id in data.TelaId)
            {
                var roleTela = new RoleTela();
                roleTela.TelaId = id;
                roleTela.RoleId = role.Result;
                _context.RoleTelas.Add(roleTela);
            }
            _logger.LogInformation(((int)EEventLog.Post), "Role Id: {role} created.", role.Result);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [Authorize(Policy = "roleAdministrador")]
        public void CarregarRoles()
        {

            List<IdentityRole> roles = new();

            var registrarViewModel = new RegistrarViewModel();
            foreach (var role in roles)
            {
                registrarViewModel.UserRoles.Add(new IdentityRole()
                {
                    Id = role.Id,
                    Name = role.Name
                });
            }
        }

        [HttpPost]
        [Authorize(Policy = "roleAdministrador")]
        public async Task<IActionResult> DeletarRole(string id)
        {
            try
            {


                var role = await _roleManager.FindByIdAsync(id);
                var roleTela = _context.RoleTelas.ToList().Where(r => r.RoleId == id);
                if (role != null)
                {
                    foreach (var rt in roleTela)
                    {
                        _context.RoleTelas.Remove(rt);
                        _context.SaveChanges();
                    }
                    await _roleManager.DeleteAsync(role);
                    _logger.LogInformation(((int)EEventLog.Delete), "Role Id: {role} created.", id);
                    _context.SaveChanges();
                    ViewBag.Excluido = true;
                    var roles = _roleManager.Roles.ToList();
                    CarregarRoles();
                    AdicionarCompetenciaMesAtual();
                    CarregarEmpresasCompetencias();
                    return View("Index", roles);
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw e.GetBaseException();
            }

        }


    }
}
