using BraAuto.ViewModels;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BraAuto.Resources;
using BraAuto.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace BraAuto.Controllers
{
    public class UsersController : BaseController
    {
        public IActionResult Register()
        {
            var model = new UserRegisterModel
            {
                UserTypes = Db.UserTypes.GetAll()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Register(UserRegisterModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = Db.Users.GetByEmail(model.Email);
                    user = Db.Users.GetByUsername(model.Username);

                    if (user != null) { return this.View(model); }

                    user = new User
                    {
                        Username = model.Username,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Birthday = model.Birthday,
                        Mobile = model.Mobile,
                        Descripton = model.Description,
                        Location = model.Location,
                        UserTypeId = model.UserTypeId,
                        IsActive = true,
                    };

                    Db.Users.Insert(user);

                    var userInRole = new UserInRole()
                    {
                        UserId = user.Id,
                        UserRoleId = Db.UserRoles.UserRoleId
                    };

                    Db.UserInRoles.Insert(userInRole);

                    return this.RedirectToAction(nameof(Login));
                }
            }
            catch (Exception)
            {

                throw;
            }

            model.UserTypes = Db.UserTypes.GetAll();

            return this.View(model);
        }

        public IActionResult Login()
        {
            var model = new LoginUserModel();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            if (this.LoggedUser != null)
            {
                return this.RedirectToAction(string.Empty, string.Empty);
            }

            if (this.ModelState.IsValid)
            {
                var user = Db.Users.GetByUsernameAndPassword(model.Username, model.Password);

                if (user == null) 
                { 
                    this.ModelState.AddModelError(string.Empty, Global.IncorrectCredentials);

                    return this.View(model);
                }

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                };

                await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return this.RedirectToAction(actionName: "Home",controllerName: "Cars");
            }

            return this.View(model);
        }

        [Authorize]
        public IActionResult Edit(uint userId)
        {
            var user = Db.Users.GetById(userId);

            if (user == null) { return this.NotFound(); }

            if (user.Id != this.LoggedUser.Id || !this.LoggedUser.IsAdmin()) { return this.RedirectToHttpForbidden(); }

            var model = new UserEditModel()
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Birthday = user.Birthday,
                Mobile = user.Mobile,
                Description = user.Descripton,
                Location = user.Location,
                IsActive = user.IsActive,
                UserTypeId = user.UserTypeId,
                UserTypes = Db.UserTypes.GetAll()
            };

            if(this.LoggedUser.IsAdmin()) { model.UserRoleIds = Db.UserInRoles.GetByUserId(user.Id).Select(uir => uir.UserRoleId); }

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(UserEditModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = Db.Users.GetById(model.Id);

                    if (user == null) { return this.NotFound(); }

                    user.Name = model.Name;
                    user.Birthday = model.Birthday;
                    user.Mobile = model.Mobile;
                    user.Descripton = model.Description;
                    user.Location = model.Location;
                    user.IsActive = model.IsActive;
                    user.UserTypeId = model.UserTypeId;
                    user.EditorId = this.LoggedUser.Id;

                    Db.Users.Update(user);
                    Db.UserInRoles.DeleteByUserId(user.Id);

                    if (model.UserRoleIds.IsNullOrEmpty()) { model.UserRoleIds = new uint[] { Db.UserRoles.UserRoleId }; }

                    foreach (var userRoleId in model.UserRoleIds)
                    {
                        var userInRole = new UserInRole()
                        {
                            UserId = this.LoggedUser.Id,
                            UserRoleId = userRoleId
                        };

                        Db.UserInRoles.Insert(userInRole);
                    }
                    return this.RedirectToAction(actionName: "My", controllerName: "Cars");
                }
            }
            catch (Exception)
            {

                throw;
            }

            if (this.LoggedUser.IsAdmin()) { model.UserRoleIds = Db.UserInRoles.GetByUserId(model.Id).Select(uir => uir.UserRoleId); }

            model.UserTypes = Db.UserTypes.GetAll();

            return this.View(model);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            var model = new ChangePasswordUserModel();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordUserModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = Db.Users.GetByUsernameAndPassword(this.LoggedUser.Username, model.OldPassword);

                    if(user == null)
                    {
                        this.ModelState.AddModelError(string.Empty, Global.IncorrectPassword);

                        return this.View(model);
                    }

                    Db.Users.SetPassword(user.Id, model.Password);

                    return this.RedirectToAction(actionName: "My", controllerName: "Cars");
                }
            }
            catch (Exception ex)
            {

                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            return this.View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LogOff()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(String.Empty, String.Empty);
        }
    }
}
