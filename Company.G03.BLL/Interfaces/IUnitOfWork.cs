using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G03.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        public IDepartmentRepository Department { get;}
        public IEmployeeRepository Employee { get; }
        public Task<int> CompleteAsync();
    }
}
