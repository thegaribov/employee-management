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
using System.Threading.Tasks;

namespace EmployeeManagement.API.Controllers.v1
{
    [Produces("application/json")]
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

        [HttpGet(Name = "employee-list")]
        public async Task<IActionResult> List([FromQuery] QueryParams queryParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.SerializeErrors() });
            }

            var sortedEmployees = await _employeeService.GetAllSortedAsync(queryParams.SortQuery);

            //Response.Headers.Add("X-Pagination", paginatedEmployees.ToJson());

            var model = _mapper.Map<IEnumerable<Employee>, List<EmployeeDTO>>(sortedEmployees);

            return Ok(model);
        }

        #endregion

        #region Details

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

        [HttpPost(Name = "employee-create")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.SerializeErrors() });
            }

            var department = await _departmentService.GetAsync(model.DepartmentId);
            if (department == null) return NotFound();

            var newEmployee = _mapper.Map<CreateEmployeeDTO, Employee>(model);
            await _employeeService.CreateAsync(newEmployee);

            return CreatedAtRoute("employee-details", new { Id = newEmployee.Id });
        }

        #endregion

        #region Update

        [HttpPut("{id:int}", Name = "employee-update")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEmployeeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.SerializeErrors() });
            }

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
