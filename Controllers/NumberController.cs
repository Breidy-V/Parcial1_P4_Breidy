using Microsoft.AspNetCore.Mvc;
using Parcial1_P4_Breidy.Models;
using Parcial1_P4_Breidy.Services;

namespace Parcial1_P4_Breidy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NumberController(NumberService numberService) : ControllerBase
{
    [HttpGet("{number:int}")]
    public IActionResult Calculate(int number)
    {
        return Ok(number + number);
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var numbers = await numberService.GetListAsync();

        return Ok(numbers);
    }

    [HttpGet("id/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var number = await numberService.GetByIdAsync(id);

        if (number == null)
        {
            return NotFound(new
            {
                mensaje = "Registro no encontrado."
            });
        }

        return Ok(number);
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] NumberRecordSet number)
    {
        bool saved = await numberService.SaveAsync(number);

        if (!saved)
        {
            return StatusCode(500, new
            {
                mensaje = "No se pudo guardar el registro."
            });
        }

        return Ok(new
        {
            Numero = number.Numero,
            Resultado = number.Resultado
        });
    }
}