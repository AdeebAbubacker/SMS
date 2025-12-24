using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Contracts.IServices;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Queries;
using SchoolManagementSystem.Infrastructure.Constants;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin}")]
    public class DepartmentController(IValidator<CreateDepartmentDTO> createValidator, IDepartmentService departmentService) : ControllerBase
    { 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDTO dto)
        {
            var validationResult = await createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            try
            {
                var department = await departmentService.CreateDepartment(dto);

                return CreatedAtAction(nameof(Create), department);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter, [FromQuery] DepartmentQueryFilter filterQuery, [FromQuery] Sort sort)
        {
            var query = DepartmentQuery.FromFilters(search: filterQuery.Search);
            var department = await departmentService.GetAllDepartments(filter, query, sort);

            return Ok(department);
        }

    }
}
