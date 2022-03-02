using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeManagement.Core.DTOs.v1.Employee;
using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Core.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDTO, Employee>()
                .ForMember(e => e.CreatedAt, o => o.Ignore())
                .ForMember(e => e.UpdatedAt, o => o.Ignore());

            CreateMap<UpdateEmployeeDTO, Employee>()
                .ForMember(e => e.CreatedAt, o => o.Ignore())
                .ForMember(e => e.UpdatedAt, o => o.Ignore());

            CreateMap<Employee, EmployeeDTO>();
        }
    }
}
