using SchoolTemplate.Models;

[Route("contact")]
[HttpPost]
public IActionResult Contact( PersonModel model)
{
    return View(model);
}

