using Company.G03.BLL.Interfaces;
using Company.G03.BLL.Repositories;
using Company.G03.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G03.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDepartmentRepository _DepartmentRepository;
        private IEmployeeRepository _EmployeeRepository;
        public UnitOfWork(AppDbContext context)
        {
            _EmployeeRepository = new EmployeeRepository(context);
            _DepartmentRepository = new DepartmentRepository(context);
             _context = context;
        }
        public IDepartmentRepository Department => _DepartmentRepository;

        public IEmployeeRepository Employee => _EmployeeRepository;

        public async Task<int> CompleteAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
