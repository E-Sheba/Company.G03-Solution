using Company.G03.DAL.Models;
using Company.G03.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Company.G03.PL.Controllers
{
    [Authorize(Roles="Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string searchInput)
        {

            var users = Enumerable.Empty<UserViewModel>();

            if (string.IsNullOrEmpty(searchInput))
            {
                users = await _userManager.Users.Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Roles = _userManager.GetRolesAsync(u).GetAwaiter().GetResult()
                }).ToListAsync();
            }
            else
            {
                users = await _userManager.Users.Where(u => u.Email
                                     .ToLower()
                                     .Contains(searchInput.ToLower()))
                                     .Select(u => new UserViewModel()
                                     {
                                         Id = u.Id,
                                         FirstName = u.FirstName,
                                         LastName = u.LastName,
                                         Email = u.Email,
                                         Roles = _userManager.GetRolesAsync(u).GetAwaiter().GetResult()
                                     }).ToListAsync();
            }
            return View(users);
        }


        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            try
            {

                if (id is null)
                {
                    return BadRequest();
                }
                var userFromDb = await _userManager.FindByIdAsync(id);
                if (userFromDb is null)
                {
                    return NotFound();
                }
                var user = new UserViewModel()
                {
                    Id = userFromDb.Id,
                    FirstName = userFromDb.FirstName,
                    LastName = userFromDb.LastName,
                    Email = userFromDb.Email,
                    Roles = _userManager.GetRolesAsync(userFromDb).GetAwaiter().GetResult()
                };
                return View(ViewName, user);
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
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            try
            {

                if (ModelState.IsValid)
                {
                    var userFromDb = await _userManager.FindByIdAsync(id);
                    if (userFromDb is null)
                    {
                        return NotFound();
                    }
                    userFromDb.FirstName = model.FirstName;
                    userFromDb.LastName = model.LastName;
                    userFromDb.Email = model.Email;

                    var result = await _userManager.UpdateAsync(userFromDb);
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
        public async Task<IActionResult> Delete(string? id) => await Details(id, "Delete");
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByIdAsync(id);
                if (userFromDb is null) return NotFound();
                else
                {
                    await _userManager.DeleteAsync(userFromDb);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }


    }
}
