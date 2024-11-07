using Microsoft.AspNetCore.Mvc;

namespace BestActivityAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BestActivityController : Controller
    {
        [HttpGet]
        public IActionResult GetBestActivities()
        {
            var discounts = new List<BestActivity>
            {
                new BestActivity { Name = "Stickers maken", Description = "Tijdens de Stickers Maken workshop op Hogeschool Zuyd leren studenten creatieve en praktische ontwerpvaardigheden. Ze ontwerpen hun eigen stickers met grafische software, ontdekken de basisprincipes van kleur, vorm en compositie, en maken vervolgens hun ontwerpen werkelijkheid met printtechnieken en een snijplotter. Deze workshop biedt een leuke, hands-on ervaring waarbij studenten hun creativiteit kunnen uiten en unieke, zelfgemaakte stickers mee naar huis nemen." },
                
            };
            return Ok(discounts);
        }
    }

    public class BestActivity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
