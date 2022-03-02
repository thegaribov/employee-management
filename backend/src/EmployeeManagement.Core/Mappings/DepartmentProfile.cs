﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeManagement.Core.DTOs.v1.Department;
using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Core.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDTO, Department>()
                .ForMember(e => e.CreatedAt, o => o.Ignore())
                .ForMember(e => e.UpdatedAt, o => o.Ignore());

            CreateMap<UpdateDepartmentDTO, Department>()
                .ForMember(e => e.CreatedAt, o => o.Ignore())
                .ForMember(e => e.UpdatedAt, o => o.Ignore());

            CreateMap<Department, DepartmentDTO>();
        }
    }
}
