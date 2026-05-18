using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly List<WeatherForecast> Items = new();

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // GET: api/weatherforecast
        [HttpGet]
        public ActionResult<IEnumerable<WeatherForecast>> GetAll()
        {
            return Ok(Items);
        }

        // GET: api/weatherforecast/{id}
        [HttpGet("{id:int}")]
        public ActionResult<WeatherForecast> GetById(int id)
        {
            var item = Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound(new
                {
                    Message = $"WeatherForecast with id {id} was not found."
                });
            }

            return Ok(item);
        }

        // POST: api/weatherforecast
        [HttpPost]
        public ActionResult<WeatherForecast> Create([FromBody] WeatherForecast request)
        {
            var item = new WeatherForecast
            {
                Id = Items.Count == 0 ? 1 : Items.Max(x => x.Id) + 1,
                Date = request.Date,
                TemperatureC = request.TemperatureC,
                Summary = request.Summary
            };

            Items.Add(item);

            _logger.LogInformation("Created WeatherForecast with id {Id}", item.Id);

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        // PUT: api/weatherforecast/{id}
        [HttpPut("{id:int}")]
        public ActionResult<WeatherForecast> Update(int id, [FromBody] WeatherForecast request)
        {
            var item = Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound(new
                {
                    Message = $"WeatherForecast with id {id} was not found."
                });
            }

            item.Date = request.Date;
            item.TemperatureC = request.TemperatureC;
            item.Summary = request.Summary;

            _logger.LogInformation("Updated WeatherForecast with id {Id}", id);

            return Ok(item);
        }

        // DELETE: api/weatherforecast/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var item = Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound(new
                {
                    Message = $"WeatherForecast with id {id} was not found."
                });
            }

            Items.Remove(item);

            _logger.LogInformation("Deleted WeatherForecast with id {Id}", id);

            return NoContent();
        }
    }

    public class WeatherForecast
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public string? Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}