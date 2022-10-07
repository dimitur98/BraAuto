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
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using BraAuto.ViewModels.Common;
using System.Linq;
using BraAuto.ViewModels.Helpers;

namespace BraAuto.Controllers
{
    public class UsersController : BaseController
    {
        private Cloudinary _cloudinary;

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
                    user = Db.Users.GetByUsername(model.Username);

                    if (user != null) { return this.View(model); }

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
                        UserRoleId = Db.UserRoles.UserRoleId
                    };

                    Db.UserInRoles.Insert(userInRole);

                    return this.RedirectToAction(nameof(Login));
                }
            }
            catch (Exception ex)
            {

                ex.SaveToLog();
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

            if(this.LoggedUser.IsAdmin()) { model.UserRoleIds = Db.UserInRoles.GetByUserId(user.Id).Select(uir => uir.UserRoleId); }

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

        public IActionResult Search(UserServiceSearchModel model)
        {
            this.ExecuteServiceSearch(model);

            model.Locations = Db.Locations.GetAll();

            return this.View("~/Views/Users/Services/Search.cshtml", model);
        }

        public IActionResult Details(uint id)
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
                Location = Db.Locations.GetById(service.LocationId).Name,
                SpecificLocation = service.SpecificLocation,
                PhotoUrl = service.PhotoUrl
            };

            return this.View("~/Views/Users/Services/Details.cshtml", model);
        }

        public IActionResult BookAppointment(uint? carId)
        {
            var model = new UserServiceBookAppointmentModel();

            this.LoadBookAppoitmentModel(model);

            model.Date = DateTime.Now;

            if(carId != null) { model.CarId = carId.Value; }

            return this.View("~/Views/Users/Services/BookAppointment.cshtml", model);
        }

        [HttpPost]
        public IActionResult BookAppointment(UserServiceBookAppointmentModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var service = Db.Users.GetById(model.ServiceId);
                    var serviceTotalBookings = Db.UserCars.Get(new uint[] { Db.UserCarTypes.ServiceAppointmentId, Db.UserCarTypes.ServiceAppointmentApprovedId }, carId: model.CarId, date: model.Date).Count;

                    if (service.MaxBookingsPerDay <= serviceTotalBookings)
                    {
                        this.LoadBookAppoitmentModel(model);
                        this.TempData[Global.AlertKey] = new Alert(Global.MaxBookingAppointmentsReached, AlertTypes.Warning).SerializeAlert();

                        return this.View(model);
                    }

                    var freeHours = service.GetFreeHours(model.Date);

                    if (!freeHours.Contains(model.Hour))
                    {
                        this.LoadBookAppoitmentModel(model);
                        this.TempData[Global.AlertKey] = new Alert(Global.SelectedTimeNotFree, AlertTypes.Warning).SerializeAlert();

                        return this.View(model);
                    }

                    var time = new TimeSpan((int)model.Hour, 0,0);

                    var appointment = new UserCar
                    {
                        UserId = service.Id,
                        CarId = model.CarId,
                        UserCarTypeId = Db.UserCarTypes.ServiceAppointmentId,
                        Date = model.Date.Add(time)
                    };

                    Db.UserCars.Insert(appointment);

                    //TODO Replace with search page of my booking appointmnets services 
                    return this.View("~/Views/Users/Services/BookAppointment.cshtml",model);
                }
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            this.LoadBookAppoitmentModel(model);

            return this.View(model);
        }

        protected void ExecuteSearch(UserAdminSearchModel model)
        {
            model.SetDefaultSort("u.created_at", sortDesc: true);

            var request = model.ToSearchRequest();

            request.ReturnTotalRecords = true;

            var response = Db.Users.Search(request);

            model.Response = response;
        }

        protected void ExecuteServiceSearch(UserServiceSearchModel model)
        {
            model.SetDefaultSort("u.created_at", sortDesc: true);

            var request = model.ToSearchRequest();

            request.ReturnTotalRecords = true;
            request.UserTypeId = Db.UserTypes.ServiceId;

            var response = Db.Users.Search(request);

            model.Response = response;
        }
    
    
        protected void LoadBookAppoitmentModel(UserServiceBookAppointmentModel model)
        {
            var cars = Db.Cars.GetByUserId(this.LoggedUser.Id);

            Db.Cars.LoadModels(cars);
            Db.Models.LoadMakes(cars.Select(c => c.Model));

            model.Cars = cars.Select(c => new SimpleModel(c.Id, $"{c.Model.Make.Name} {c.Model.Name} {c.Variant}"));
            model.Services = Db.Users.GetByTypeId(Db.UserTypes.ServiceId).Select(u => new SimpleModel(u.Id, u.Name));
        }
    }
}
