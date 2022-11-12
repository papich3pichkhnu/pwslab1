using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections;
using System.Collections.Generic;

namespace lab1_pws.Components
{    
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IEnumerable<string> _menuElements;
        public NavbarViewComponent() => _menuElements = new List<string>() { "Index", "AboutUs", "Feedback", "Cloud", "PersonForm"};
        public IViewComponentResult Invoke()
        {
            return View(_menuElements);
        }
    }
}
