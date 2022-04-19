using AutoMapper;
using EmployeeManagement.Core.DTOs.v1.Department;
using EmployeeManagement.Core.DTOs.v1.Employee;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Extensions.ModelState;
using EmployeeManagement.Core.Filters.Base;
using EmployeeManagement.Service.Business.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EmployeeManagement.API.Controllers.v1
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/v1/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        #region Fields

        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        public EmployeeController(
            IEmployeeService employeeService,
            IDepartmentService departmentService,
            IMapper mapper)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _mapper = mapper;
        }

        #endregion

        #region List

        #region Documentation

        /// <summary>
        /// Returns avaiable employees. You can filter returned result by providing query string
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     /api/v1/employees
        ///     /api/v1/employees?query=Mahmud Qaribov
        ///     /api/v1/employees?page=2&amp;pageSize=15
        ///     /api/v1/employees?sort=name_asc,surname_desc
        ///     /api/v1/employees?query=Mahmud Qaribov&amp;page=2&amp;pageSize=15&amp;sort=name_asc,surname_desc
        ///
        /// </remarks>
        /// <response code="200">Employees retrieved</response>
        /// <response code="400">Paging, Searching, Sorting query string is not valid</response>
        /// <response code="500">Server error</response>
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<EmployeeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #endregion

        [HttpGet(Name = "employee-list")]
        public async Task<IActionResult> List([FromQuery] QueryParams queryParams)
        {
            var employees = await _employeeService.GetAllSearchedPaginatedSortedAsync(queryParams.Query, queryParams.Sort, queryParams.Page, queryParams.PageSize);

            Response.Headers.Add("X-Pagination", employees.ToJson());

            var model = _mapper.Map<IEnumerable<Employee>, List<EmployeeDTO>>(employees.Data);

            return Ok(model);
        }

        #endregion

        #region Details

        #region Documentation

        /// <summary>
        /// Get details about specific employee
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <response code="200">Related employee info</response>
        /// <response code="404">Requested resource not found</response>
        /// <response code="500">Server error</response>
        [ProducesResponseType(typeof(EmployeeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #endregion

        [HttpGet("{id:int}", Name = "employee-details")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var employee = await _employeeService.GetAsync(id);
            if (employee == null) return NotFound();

            var model = _mapper.Map<Employee, EmployeeDTO>(employee);

            return Ok(model);
        }

        #endregion

        #region Create

        #region Documentation

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <response code="201">Employee created</response>
        /// <response code="400">DTO is not valid</response>
        /// <response code="404">Related department is not found</response>
        /// <response code="500">Server error</response>
        [ProducesResponseType(typeof(EmployeeDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #endregion

        [HttpPost(Name = "employee-create")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDTO model)
        {
            var department = await _departmentService.GetAsync(model.DepartmentId);
            if (department == null) return NotFound();

            var newEmployee = _mapper.Map<CreateEmployeeDTO, Employee>(model);
            await _employeeService.CreateAsync(newEmployee);

            var employeeDTO = _mapper.Map<Employee, EmployeeDTO>(newEmployee);

            return CreatedAtRoute("employee-details", new { Id = employeeDTO.Id }, employeeDTO);
        }

        #endregion

        #region Update

        #region Documentation

        /// <summary>
        /// Update an employee
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <response code="204">Employee updated</response>
        /// <response code="400">DTO is not valid</response>
        /// <response code="404">Related department or employee is not found</response>
        /// <response code="500">Server error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #endregion

        [HttpPut("{id:int}", Name = "employee-update")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEmployeeDTO model)
        {
            var department = await _departmentService.GetAsync(model.DepartmentId);
            if (department == null) return NotFound();

            var employee = await _employeeService.GetAsync(id);
            if (employee == null) return NotFound();

            _mapper.Map<UpdateEmployeeDTO, Employee>(model, employee);

            await _employeeService.UpdateAsync(employee);

            return NoContent();
        }

        #endregion

        #region Delete

        #region Documentation

        /// <summary>
        /// Delete employee
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <response code="200">Employee deleted</response>
        /// <response code="404">Employee is not found with specified id</response>
        /// <response code="500">Server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #endregion

        [HttpDelete("{id:int}", Name = "employee-delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var employee = await _employeeService.GetAsync(id);
            if (employee == null) return NotFound();

            await _employeeService.DeleteAsync(employee);

            return Ok();
        }

        #endregion

    }
}
