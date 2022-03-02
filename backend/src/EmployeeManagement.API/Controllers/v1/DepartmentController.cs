using AutoMapper;
using EmployeeManagement.Core.DTOs.v1.Department;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Extensions.ModelState;
using EmployeeManagement.Service.Business.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.API.Controllers.v1
{
    [Produces("application/json")]
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

        [HttpGet(Name = "department-list")]
        public async Task<IActionResult> List()
        {
            var departments = await _departmentService.GetAllAsync();
            var model = _mapper.Map<List<Department>, List<DepartmentDTO>>(departments);

            return Ok(model);
        }

        #endregion

        #region Details

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


        [HttpPost(Name = "department-create")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.SerializeErrors() });
            }

            var newDepartment = _mapper.Map<CreateDepartmentDTO, Department>(model);
            await _departmentService.CreateAsync(newDepartment);

            return CreatedAtRoute("department-details", new { Id = newDepartment.Id }, newDepartment);
        }

        #endregion

        #region Update


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
