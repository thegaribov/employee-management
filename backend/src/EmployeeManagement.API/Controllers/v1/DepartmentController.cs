using AutoMapper;
using EmployeeManagement.Core.DTOs.v1.Department;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Extensions.ModelState;
using EmployeeManagement.Service.Business.Abstracts;
using Mainwave.MimeTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net.Http.Headers;
using EmployeeManagement.Core.Common;
using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Core.Filters;

namespace EmployeeManagement.API.Controllers.v1
{
    [Produces(MimeType.Application.Json)]
    [Consumes(MimeType.Application.Json)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("api/v1/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        #region Documentation

        /// <summary>
        /// Returns avaiable departments. You can filter returned result by providing query string
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     /api/v1/departments
        ///     /api/v1/departments?query=Mahmud Qaribov
        ///     /api/v1/departments?page=2&amp;pageSize=15
        ///     /api/v1/departments?sort=name_asc,surname_desc
        ///     /api/v1/departments?query=Mahmud Qaribov&amp;page=2&amp;pageSize=15&amp;sort=name_asc,surname_desc
        ///
        /// </remarks>
        /// <response code="200">Departments retrieved</response>
        /// <response code="400">
        /// 1) Paging, searching or sorting query string is not valid
        /// </response>

        [ProducesResponseType(typeof(List<DepartmentDetailsDTO>), StatusCodes.Status200OK)]

        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DepartmentsQueryParams queryParams)
        {
            return Ok(await _departmentService.GetAllAsync(queryParams));
        }

        #region Documentation

        /// <summary>
        /// Get department details by Id
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <response code="200">Department info</response>
        [ProducesResponseType(typeof(DepartmentDetailsDTO), StatusCodes.Status200OK)]

        #endregion

        [HttpGet("{id:int}", Name = "department-details")]
        public async Task<IActionResult> GetDetails([FromRoute] int id)
        {
            return Ok(await _departmentService.GetDetailsAsync(id));
        }

        #region Documentation

        /// <summary>
        /// Create a new department
        /// </summary>
        /// <response code="201">Department created successfully</response>
        /// <response code="400">DTO is not valid</response>
        [ProducesResponseType(typeof(CreateDepartmentResponseDTO), StatusCodes.Status201Created)]

        #endregion

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequestDTO model)
        {
            var departmentDTO = await _departmentService.CreateAsync(model);

            return CreatedAtRoute("department-details", new { Id = departmentDTO.Id }, departmentDTO);
        }

        #region Documentation

        /// <summary>
        /// Update department by Id
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <response code="204">Department successfully updated</response>
        /// <response code="400">DTO is not valid</response>
        /// <response code="404">
        /// 1) Department is not found with specified Id
        /// </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]

        #endregion

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDepartmentDTO departmentDTO)
        {
            await _departmentService.UpdateAsync(id, departmentDTO);

            return NoContent();
        }

        #region Documentation

        /// <summary>
        /// Delete department by Id
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <response code="200">Department deleted</response>
        /// <response code="404">
        /// 1) Department is not found with specified Id
        /// </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        #endregion

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _departmentService.DeleteAsync(id);

            return Ok();
        }

        #region Department employees

        [HttpGet("{departmentId:int}/employees")]
        public async Task<IActionResult> GetEmployees([FromRoute] int departmentId, [FromQuery] QueryParams queryParams)
        {
            return Ok(await _departmentService.GetEmployeesAsync(departmentId, queryParams));
        }

        [HttpGet("{departmentId:int}/employees/{employeeId}")]
        public async Task<IActionResult> GetEmployeeDetails([FromRoute] int departmentId, [FromRoute] int employeeId)
        {
            return Ok(await _departmentService.GetEmployeeDetailsAsync(departmentId, employeeId));
        }

        #endregion
    }
}
