using BraAuto.Helpers.Extensions;
using BraAuto.Resources;
using BraAuto.ViewModels;
using BraAuto.ViewModels.Helpers;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BraAuto.Controllers
{
    public class UsersController : BaseController
    {
        private Cloudinary _cloudinary;
        private readonly string _googleExternalLoginType = "google";
        private readonly string _facebookExternalLoginType = "facebook";

        public UsersController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public IActionResult Admin(UserAdminSearchModel model)
        {
            this.ExecuteSearch(model);

            Db.Users.LoadEditors(model.Response.Records);

            return this.View(model);
        }

        public IActionResult Register()
        {
            var model = new UserRegisterModel
            {
                UserTypes = Db.UserTypes.GetAll(),
                Locations = Db.Locations.GetAll()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = Db.Users.GetByEmail(model.Email);

                    if (user != null)
                    {
                        this.TempData[Global.AlertKey] = new Alert(Global.EmailAlreadyExists, AlertTypes.Danger).SerializeAlert();

                        return this.View(model);
                    }

                    user = Db.Users.GetByUsername(model.Username);

                    if (user != null)
                    {
                        this.TempData[Global.AlertKey] = new Alert(Global.UsernameAlreadyExists, AlertTypes.Danger).SerializeAlert();

                        return this.View(model);
                    }

                    if (model.Photo != null)
                    {
                        if (!model.Photo.IsValidPhoto())
                        {
                            model.UserTypes = Db.UserTypes.GetAll();
                            model.Locations = Db.Locations.GetAll();

                            this.ModelState.AddModelError(string.Empty, Global.InvalidPhoto);

                            return this.View(model);
                        }

                        user.PhotoUrl = await model.Photo.UploadPhotoAsync();
                    }

                    if (model.UserTypeId == Db.UserTypes.ServiceId &&
                        (model.LocationId == null
                        || model.BookingIntervalHours == null 
                        || model.StartWorkingTime == null 
                        || model.EndWorkingTime == null 
                        || model.MaxBookingsPerDay == null 
                        || model.Photo == null))
                    {
                        model.UserTypes = Db.UserTypes.GetAll();
                        model.Locations = Db.Locations.GetAll();

                        this.ModelState.AddModelError(string.Empty, Global.ServiceRequiredFields);

                        return this.View(model);
                    }

                    user = new User
                    {
                        Username = model.Username,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Birthday = model.Birthday,
                        Mobile = model.Mobile,
                        Description = model.Description,
                        LocationId = model.LocationId,
                        SpecificLocation = model.SpecificLocation,
                        UserTypeId = model.UserTypeId,
                        IsActive = true,
                        BookingIntervalHours = user.BookingIntervalHours,
                        MaxBookingsPerDay = user.MaxBookingsPerDay,
                        StartWorkingTime = user.StartWorkingTime,
                        EndWorkingTime = user.EndWorkingTime
                    };

                    Db.Users.Insert(user);

                    var userInRole = new UserInRole()
                    {
                        UserId = user.Id,
                        UserRoleId = Db.UserRoles.UserId
                    };

                    Db.UserInRoles.Insert(userInRole);

                    return this.RedirectToAction(nameof(Login));
                }
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            model.UserTypes = Db.UserTypes.GetAll();
            model.Locations = Db.Locations.GetAll();

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
                var user = Db.Users.GetByUsernameAndPassword(model.Username, model.Password, isPasswordRequired: true);

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

                return this.RedirectToAction(actionName: "Home", controllerName: "Cars");
            }

            return this.View(model);
        }

        [AllowAnonymous]
        public IActionResult ExternalLogin(string externalLoginType)
        {
            externalLoginType = externalLoginType.ToLower();

            var properties = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("ExternalLoginResponse", "Users")
            };

            if (externalLoginType == _googleExternalLoginType)
            {
                return Challenge(properties, GoogleDefaults.AuthenticationScheme);
            }
            else if(externalLoginType == _facebookExternalLoginType)
            {
                return Challenge(properties, FacebookDefaults.AuthenticationScheme);
            }

            return this.NotFound();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginResponse()
        {
            if(this.LoggedUser != null)
            {
                return this.RedirectToAction(nameof(CarsController.Home), "Cars");
            }

            return this.RedirectToAction(nameof(SetUsername));
        }

        public IActionResult SetUsername()
        {
            var model = new UserSetUsernameModel();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetUsername(UserSetUsernameModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var email = this.User?.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;

                    var user = Db.Users.GetByEmail(email);

                    if (user != null)
                    {
                        this.TempData[Global.AlertKey] = new Alert(Global.EmailAlreadyExists, AlertTypes.Danger).SerializeAlert();

                        return this.View(model);
                    }

                    user = new User
                    {
                        Name = this.User?.FindFirst(x => x.Type == ClaimTypes.Name)?.Value,
                        Username = model.Username,
                        Email = email,
                        UserTypeId = Db.UserTypes.UserId,
                        IsActive = true,
                        IsPasswordRequired = false
                    };
                    
                    Db.Users.Insert(user);

                    var userInRole = new UserInRole()
                    {
                        UserId = user.Id,
                        UserRoleId = Db.UserRoles.UserId
                    };

                    Db.UserInRoles.Insert(userInRole);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return this.RedirectToAction(nameof(CarsController.Home), "Cars");
                }
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            return this.View(model);
        }

        public IActionResult ServiceSearch(UserSearchModel model)
        {
            model.UserTypeId = Db.UserTypes.ServiceId;

            this.ExecuteSearch(model);

            model.Locations = Db.Locations.GetAll();

            return this.View(model);
        }

        [Authorize]
        public IActionResult Edit(uint id)
        {
            var user = Db.Users.GetById(id);

            if (user == null) { return this.NotFound(); }

            if (user.Id != this.LoggedUser.Id && !this.LoggedUser.IsAdmin()) { return this.RedirectToHttpForbidden(); }

            var model = new UserEditModel()
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Birthday = user.Birthday,
                Mobile = user.Mobile,
                Description = user.Description,
                LocationId = user.LocationId,
                Locations = Db.Locations.GetAll(),
                SpecificLocation = user.SpecificLocation,
                IsActive = user.IsActive,
                UserTypeId = user.UserTypeId,
                PhotoUrl = user.PhotoUrl,
                BookingIntervalHours = user.BookingIntervalHours,
                MaxBookingsPerDay = user.MaxBookingsPerDay,
                StartWorkingTime = user.StartWorkingTime,
                EndWorkingTime = user.EndWorkingTime,
                UserTypes = Db.UserTypes.GetAll()
            };

            if (this.LoggedUser.IsAdmin()) { model.UserRoleIds = Db.UserInRoles.GetByUserId(user.Id).Select(uir => uir.UserRoleId); }

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var user = Db.Users.GetById(model.Id);

                    if (user == null) { return this.NotFound(); }

                    if (model.UserTypeId == Db.UserTypes.ServiceId &&
                        (model.LocationId == null
                        || model.BookingIntervalHours == null
                        || model.StartWorkingTime == null
                        || model.EndWorkingTime == null
                        || model.MaxBookingsPerDay == null
                        || model.Photo == null))
                    {
                        if (this.LoggedUser.IsAdmin()) { model.UserRoleIds = Db.UserInRoles.GetByUserId(model.Id).Select(uir => uir.UserRoleId); }

                        model.UserTypes = Db.UserTypes.GetAll();
                        model.Locations = Db.Locations.GetAll();

                        this.ModelState.AddModelError(string.Empty, Global.ServiceRequiredFields);

                        return this.View(model);
                    }

                    if (model.Photo != null)
                    {
                        if (user.PhotoUrl != null)
                        {
                            var publicId = System.IO.Path.ChangeExtension(user.PhotoUrl.Split("/").Last(), null);

                            DeletionParams deletionParams = new DeletionParams(publicId);

                            await this._cloudinary.DestroyAsync(deletionParams);
                        }

                        if (!model.Photo.IsValidPhoto())
                        {
                            model.UserTypes = Db.UserTypes.GetAll();
                            model.Locations = Db.Locations.GetAll();

                            this.ModelState.AddModelError(string.Empty, Global.InvalidPhoto);

                            return this.View(model);
                        }

                        user.PhotoUrl = await model.Photo.UploadPhotoAsync();
                    }
                    // to do check service fields required
                    user.Name = model.Name;
                    user.Birthday = model.Birthday;
                    user.Mobile = model.Mobile;
                    user.Description = model.Description;
                    user.LocationId = model.LocationId;
                    user.SpecificLocation = model.SpecificLocation;
                    user.IsActive = model.IsActive;
                    user.UserTypeId = model.UserTypeId;
                    user.BookingIntervalHours = model.BookingIntervalHours;
                    user.MaxBookingsPerDay = model.MaxBookingsPerDay;
                    user.StartWorkingTime = model.StartWorkingTime;
                    user.EndWorkingTime = model.EndWorkingTime;
                    user.EditorId = this.LoggedUser.Id;

                    Db.Users.Update(user);
                    Db.UserInRoles.DeleteByUserId(user.Id);

                    if (model.UserRoleIds.IsNullOrEmpty()) { model.UserRoleIds = new uint[] { Db.UserRoles.UserId }; }

                    foreach (var userRoleId in model.UserRoleIds)
                    {
                        var userInRole = new UserInRole()
                        {
                            UserId = this.LoggedUser.Id,
                            UserRoleId = userRoleId
                        };

                        Db.UserInRoles.Insert(userInRole);
                    }
                    return this.RedirectToAction("My", "Cars");
                }
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            if (this.LoggedUser.IsAdmin()) { model.UserRoleIds = Db.UserInRoles.GetByUserId(model.Id).Select(uir => uir.UserRoleId); }

            model.UserTypes = Db.UserTypes.GetAll();
            model.Locations = Db.Locations.GetAll();

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
                    var user = Db.Users.GetByUsernameAndPassword(this.LoggedUser.Username, model.OldPassword, true);

                    if (user == null)
                    {
                        this.ModelState.AddModelError(string.Empty, Global.IncorrectPassword);

                        return this.View(model);
                    }

                    Db.Users.SetPassword(user.Id, model.Password);

                    return this.RedirectToAction("My", "Cars");
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

        public IActionResult ServiceDetails(uint id)
        {
            var service = Db.Users.GetById(id);

            if (service == null) { return this.NotFound(); }

            var model = new UserServiceDetailsModel
            {
                Id = service.Id,
                Name = service.Name,
                Email = service.Email,
                Mobile = service.Mobile,
                Description = service.Description,
                Location = Db.Locations.GetById(service.LocationId.Value).Name,
                SpecificLocation = service.SpecificLocation,
                PhotoUrl = service.PhotoUrl
            };

            return this.View("~/Views/Users/Details.cshtml", model);
        }

        protected void ExecuteSearch(UserSearchBaseModel model)
        {
            model.SetDefaultSort("u.created_at", sortDesc: true);

            var request = model.ToSearchRequest();

            request.ReturnTotalRecords = true;

            var response = Db.Users.Search(request);

            model.Response = response;
        }       
    }
}
