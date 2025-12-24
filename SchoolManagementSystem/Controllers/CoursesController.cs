using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using SchoolManagementSystem.Application.Contracts.IServices;
using SchoolManagementSystem.Application.DTOs.Course;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Queries;
using SchoolManagementSystem.Infrastructure.Constants;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin}")]
    public class CoursesController(IValidator<CreateCourseDto> createValidator, ICourseService courseService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            var validationResult = await createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            try
            {
                var course = await courseService.CreateCourse(dto);

                return CreatedAtAction(nameof(Create), course);
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
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter, [FromQuery] CourseQueryFilter filterQuery, [FromQuery] Sort sort)
        {
            var query = CourseQuery.FromFilters(search: filterQuery.Search);
            var course = await courseService.GetAllCourses(filter, query, sort);

            return Ok(course);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid CourseId)
        {
            try
            {
                return Ok(await courseService.DeleteAsync(CourseId));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}