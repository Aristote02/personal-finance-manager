using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManager.Application.Contracts.Services.Interfaces;
using PersonalFinanceManager.Shared.Constants;
using PersonalFinanceManager.Shared.RequestFeatures;
using PersonalFinanceManager.Shared.Requests.Incomes;

namespace PersonalFinanceManager.Presentation.Controllers;

[Route("api/incomes")]
[ApiController]
[Authorize(Policy = "JwtOrGoogle")]
[Authorize(Roles = $"{Roles.Admin},{Roles.User}")]
public class IncomesController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public IncomesController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateIncome([FromBody] IncomeCreationRequest request)
    {
        var result = await _serviceManager.IncomeService.CreateIncomeAsync(request);
        return Created(nameof(CreateIncome), result);
    }

    [HttpGet("income/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetIncomeById(Guid id)
    {
        var result = await _serviceManager.IncomeService.GetIncomeByIdAsync(id, trackChanges: false);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllIncomes([FromQuery] IncomeParameters parameters)
    {
        var result = await _serviceManager.IncomeService.GetAllIncomesAsync(parameters, trackChanges: false);
        return Ok(result);
    }

    [HttpPut("income")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateIncome([FromBody] IncomeUpdateRequest request)
    {
        await _serviceManager.IncomeService.UpdateIncomeAsync(request, trackChanges: true);
        return NoContent();
    }

    [HttpDelete("income/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteIncome(Guid id)
    {
        await _serviceManager.IncomeService.DeleteIncomeAsync(id, trackChanges: false);
        return NoContent();
    }
}