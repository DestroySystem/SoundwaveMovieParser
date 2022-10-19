using Microsoft.AspNetCore.Mvc;

namespace MovieCatalog.Components
{
    public class MainHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
