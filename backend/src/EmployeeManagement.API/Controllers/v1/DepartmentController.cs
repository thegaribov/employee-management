using AutoMapper;
using EmployeeManagement.Core.DTOs.v1.Department;
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
    [Route("api/v1/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;

        #endregion

        #region Ctor

        public DepartmentController(IDepartmentService departmentService, IMapper mapper)
        {
            _mapper = mapper;
            _departmentService = departmentService;
        }

        #endregion

        #region List

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
        /// <response code="400">Paging, Searching, Sorting query string is not valid</response>
        /// <response code="500">Server error</response>
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<DepartmentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #endregion

        [HttpGet(Name = "department-list")]
        public async Task<IActionResult> List([FromQuery] QueryParams queryParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.SerializeErrors() });
            }

            var departments = await _departmentService.GetAllSearchedPaginatedSortedAsync(queryParams.Query, queryParams.Sort, queryParams.Page, queryParams.PageSize);

            Response.Headers.Add("X-Pagination", departments.ToJson());

            var model = _mapper.Map<IEnumerable<Department>, List<DepartmentDTO>>(departments.Data);

            return Ok(model);
        }

        #endregion

        #region Details

        #region Documentation

        /// <summary>
        /// Get details about specific department
        /// </summary>
        /// <param name="id">Detail id</param>
        /// <response code="200">Related department info</response>
        /// <response code="404">Requested resource not found</response>
        /// <response code="500">Server error</response>
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #endregion

        [HttpGet("{id:int}", Name = "department-details")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var department = await _departmentService.GetAsync(id);
            if (department == null) return NotFound();

            var model = _mapper.Map<Department, DepartmentDTO>(department);

            return Ok(model);
        }

        #endregion

        #region Create

        #region Documentation

        /// <summary>
        /// Create a new department
        /// </summary>
        /// <response code="201">Department created</response>
        /// <response code="400">DTO is not valid</response>
        /// <response code="500">Server error</response>
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #endregion

        [HttpPost(Name = "department-create")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.SerializeErrors() });
            }

            var newDepartment = _mapper.Map<CreateDepartmentDTO, Department>(model);
            await _departmentService.CreateAsync(newDepartment);

            var departmentDTO = _mapper.Map<Department, DepartmentDTO>(newDepartment);

            return CreatedAtRoute("department-details", new { Id = departmentDTO.Id }, departmentDTO);
        }

        #endregion

        #region Update

        #region Documentation

        /// <summary>
        /// Update department
        /// </summary>
        /// <param name="id">Department id</param>
        /// <response code="204">Department updated</response>
        /// <response code="400">DTO is not valid</response>
        /// <response code="404">Department is not found with specified id</response>
        /// <response code="500">Server error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #endregion

        [HttpPut("{id:int}", Name = "department-update")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDepartmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.SerializeErrors() });
            }

            var department = await _departmentService.GetAsync(id);
            if (department == null) return NotFound();

            _mapper.Map<UpdateDepartmentDTO, Department>(model, department);

            await _departmentService.UpdateAsync(department);

            return NoContent();
        }

        #endregion

        #region Delete

        #region Documentation

        /// <summary>
        /// Delete department
        /// </summary>
        /// <param name="id">Department id</param>
        /// <response code="200">Department deleted</response>
        /// <response code="404">Department is not found with specified id</response>
        /// <response code="500">Server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #endregion

        [HttpDelete("{id:int}", Name = "department-delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var department = await _departmentService.GetAsync(id);
            if (department == null) return NotFound();

            await _departmentService.DeleteAsync(department);

            return Ok();
        }

        #endregion
    }
}
