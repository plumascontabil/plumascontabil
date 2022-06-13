﻿using Demonstrativo.Models;
using DomainService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        //private readonly RoleDomainService _roleDomainService;


        public RoleController(RoleManager<IdentityRole> roleManager, Context context
            //RoleDomainService roleDomainService
            ) : base(context)
        {
            _roleManager = roleManager;
            //_roleDomainService = roleDomainService;
        }

        public IActionResult Index()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var roles = _roleManager.Roles.ToList();
            CarregarRoles();
            return View(roles);
        }
        [Authorize(Policy = "roleAdministrador")]
        public IActionResult Create()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(new IdentityRole());
        }

        [HttpPost]
        [Authorize(Policy = "roleAdministrador")]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }
        [Authorize(Policy = "roleAdministrador")]
        public void CarregarRoles()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
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
                if (role != null)
                {
                    await _roleManager.DeleteAsync(role);
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
