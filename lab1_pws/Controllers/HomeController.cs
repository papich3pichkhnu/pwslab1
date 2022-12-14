using FluentValidation;
using FluentValidation.Results;
using lab1_pws.Models;
using lab1_pws.Services.Interfaces.Services;
using lab1_pws.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace lab1_pws.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IFileService _fileService;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IValidator<Person> _validator;

        public HomeController(ILogger<HomeController> logger, IValidator<Person> validator, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _logger = logger;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            _validator = validator;
        }

        [Route("home")]
        [Route("")]
        public IActionResult Index()
        {

            return View();
        }
        [Route("about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("feedback")]
        public IActionResult FeedBack()
        {
            return View();
        }
        [Route("person-form")]
        public IActionResult PersonForm()        
        {
            return View();
        }

        [HttpPost]
        [Route("send-email")]
        public async Task<IActionResult> SendEmail(MailModel mailModel)
        {
            await _emailSender.SendEmailAsync(mailModel.To, mailModel.ToName, "Feedback message", mailModel.Body);
            return RedirectToAction(nameof(Index));
        }

        [Route("cloud")]
        public IActionResult Cloud()
        {
            var files = _fileService.GetFilesList(Path.Combine(this._webHostEnvironment.WebRootPath, "Files/"));

            return View(files);
        }

        [HttpGet]
        [Route("download-file")]
        public FileResult DownloadFile(string filename)
        {
            string path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files/") + filename;

            var bytes = _fileService.GetFileDownload(path);

            return File(bytes, "application/octet-stream", filename);
        }
        [HttpPost]
        [Route("upload-file")]
        public async Task<IActionResult> UploadFile(IFormFile File)
        {
            var res = await _fileService.UploadFile(File, "wwwroot/Files");

            return RedirectToAction(nameof(Cloud));
        }
        [Route("get-cult")]
        public string GetCulture()
        {
            return $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";
        }

        [Route("change-culture")]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30), IsEssential=true });

            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Person person, string returnUrl)
        {
            ValidationResult result = await _validator.ValidateAsync(person);

            if (!result.IsValid)
            {
                // Copy the validation results into ModelState.
                // ASP.NET uses the ModelState collection to populate 
                // error messages in the View.
                result.AddToModelState(this.ModelState);

                // re-render the view when validation failed.
                return View("PersonForm", person);
            }

           
            TempData["notice"] = "Person successfully created";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
