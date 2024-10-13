using Company.G03.DAL.Models;
using Company.G03.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.G03.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string searchInput)
        {

            var roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(searchInput))
            {
                roles = await _roleManager.Roles.Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    RoleName = r.Name
                }).ToListAsync();
            }
            else
            {
                roles = await _roleManager.Roles.Where(r => r.Name
                                     .ToLower()
                                     .Contains(searchInput.ToLower()))
                                     .Select(r => new RoleViewModel()
                                     {
                                         Id = r.Id,
                                         RoleName = r.Name
                                     }).ToListAsync();
            }
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var RoleById = await _roleManager.FindByIdAsync(model.Id);
                if (RoleById is null)
                {
                    var Role = new IdentityRole()
                    {
                        Name = model.RoleName
                    };
                    var Result = await _roleManager.CreateAsync(Role);
                    if (Result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid model");
            return View(model);
        }


        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            try
            {

                if (id is null)
                {
                    return BadRequest();
                }
                var roleFromDb = await _roleManager.FindByIdAsync(id);
                if (roleFromDb is null)
                {
                    return NotFound();
                }
                var role = new RoleViewModel()
                {
                    Id = roleFromDb.Id,
                    RoleName = roleFromDb.Name
                };
                return View(ViewName, role);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id) => await Details(id, "Edit");


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            try
            {

                if (ModelState.IsValid)
                {
                    var roleFromDb = await _roleManager.FindByIdAsync(id);
                    if (roleFromDb is null)
                    {
                        return NotFound();
                    }
                    //applying and mapping edits
                    roleFromDb.Name = model.RoleName;

                    //the Update method
                    var result = await _roleManager.UpdateAsync(roleFromDb);
                    if (result.Succeeded)
                    {

                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Operation");
                }

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);

            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(string? id) => await Details(id, "Delete");

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                //finding the role
                var roleFromDb = await _roleManager.FindByIdAsync(id);

                if (roleFromDb is null)
                    return NotFound();
                else
                {
                    //Deleting the role
                    await _roleManager.DeleteAsync(roleFromDb);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
                return NotFound();

            ViewData["roleId"] = roleId;

            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;

                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);
            }

            return View(usersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
                return NotFound();


            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if(appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Edit), new {id = roleId});
            }
            return View(users);
        }

    }
}
