using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RelatoX.Application.DTOs;
using RelatoX.Application.DTOs.Queries;
using RelatoX.Application.Interfaces;
using RelatoX.Domain.Entities;
using RelatoX.Infra.Utils;

namespace RelatoX.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumptionsController : ControllerBase
    {
        private readonly IConsumptionService _consumptionService;
        private readonly IValidator<ConsumptionPostDto> _validator;

        public ConsumptionsController(IConsumptionService consumptionService, IValidator<ConsumptionPostDto> validator)
        {
            _consumptionService = consumptionService;
            _validator = validator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ConsumptionEntry>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult<ApiResponse<ConsumptionEntry>>> PostConsumption([FromBody] ConsumptionPostDto consumptionDto)
        {
            var validationResult = await _validator.ValidateAsync(consumptionDto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => e.ErrorMessage).ToArray();

                return BadRequest(ApiResponse<string>.Fail(errors));
            }

            var result = await _consumptionService.AddConsumptionAsync(consumptionDto);
            return Created(string.Empty, ApiResponse<ConsumptionEntry>.Ok(result));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ConsumptionEntry>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult<ApiResponse<List<ConsumptionEntry>>>> GetAllConsumption([FromQuery] ConsumptionQuery query)
        {
            var result = await _consumptionService.GetAllConsumptionAsync(
                query.Page,
                query.PageSize,
                query.Month,
                query.Year,
                query.Type);

            return Ok(ApiResponse<List<ConsumptionEntry>>.Ok(result));
        }

        [HttpGet("reports")]
        [ProducesResponseType(typeof(ApiResponse<List<ReportDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status500InternalServerError)]
        [Produces("application/json", "application/pdf", "text/csv")]
        public async Task<ActionResult<List<ReportDto>>> GetConsumptionReport([FromQuery] ReportQuery query)
        {
            var reports = await _consumptionService.GetConsumptionReportAsync(
                query.Year,
                query.Month,
                query.Type);

            return query.Format.ToLower() switch
            {
                "json" => Ok(ApiResponse<List<ReportDto>>.Ok(reports)),
                "csv" => Ok(File(CSVGenerator.Generate(reports), "text/csv", "report.csv")),
                "pdf" => Ok(File(PDFGenerator.Generate(reports), "application/pdf", "report.pdf")),
                _ => BadRequest(ApiResponse<string>.Fail("Formato inválido. Use 'json', 'pdf' ou 'csv'."))
            };
        }

        [HttpGet("reports/user/{Id}")]
        [ProducesResponseType(typeof(ApiResponse<MonthlyUserReportDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status500InternalServerError)]
        [Produces("application/json", "application/pdf", "text/csv")]
        public async Task<ActionResult<MonthlyUserReportDto>> GetReportsByUser([FromRoute] string Id, [FromQuery] ReportQuery query)
        {
            var reportByUser = await _consumptionService.GetConsumptionReportByUserAsync(Id);

            if (reportByUser.Summary.Count == 0)
                return NoContent();

            return query.Format.ToLower() switch
            {
                "json" => Ok(ApiResponse<MonthlyUserReportDto>.Ok(reportByUser)),
                "csv" => Ok(File(CSVGenerator.GenerateByUser(reportByUser), "text/csv", "report.csv")),
                "pdf" => Ok(File(PDFGenerator.GenerateByUser(reportByUser), "application/pdf", "report.pdf")),
                _ => BadRequest(ApiResponse<string>.Fail("Formato inválido. Use 'json', 'pdf' ou 'csv'."))
            };
        }
    }
}