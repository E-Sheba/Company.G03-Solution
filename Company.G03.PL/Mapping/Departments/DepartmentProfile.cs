using AutoMapper;
using Company.G03.DAL.Models;
using Company.G03.PL.ViewModels;

namespace Company.G03.PL.Mapping.Departments
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentViewModel>().ReverseMap();
        }
    }
}
