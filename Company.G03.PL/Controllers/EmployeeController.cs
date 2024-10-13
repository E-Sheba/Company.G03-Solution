using AutoMapper;
using Company.G03.BLL.Interfaces;
using Company.G03.BLL.Repositories;
using Company.G03.DAL.Models;
using Company.G03.PL.Helpers;
using Company.G03.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.G03.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        #region Injection

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Index
        #region ViewDictionaries
        //View's Dictionary: one way transfer data from action to the view

        // 3 properties inherited from the controller class
        /* 1. ViewData // Dictionary treated like object  ..... requires casting
           2. ViewBag // Dynamic Dictionary doesn't require Casting
           3. TempData // Transfers data from action to view , between two connected requests
            private ITempDataDictionary? _tempData;
            private DynamicViewData? _viewBag;
            private ViewDataDictionary? _viewData;

        */
        //1.
        //ViewData["Data01"] = "Hello World"; //Set

        //2.
        //ViewBag.Data02 = "Hello form view bag";

        //3. 
        //TempData["Data03"] = "Temp Data Hello";

        #endregion
        public async Task<IActionResult> Index(string InputSearch)
        {
            //Empty(); is a Linq operator which is not an extinsion method but it's a static method for the Enumerable class that returns an empty collection
            var employee = Enumerable.Empty<Employee>();


            if (string.IsNullOrEmpty(InputSearch))
            {
                employee = await _unitOfWork.Employee.GetAllAsync();
            }
            else
            {
                employee = await _unitOfWork.Employee.GetByNameAsync(InputSearch);
            }

            var Result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employee);

            return View(Result);
        }

        #endregion

        #region Create

        #region Manual Mapping

        //Casting a viewModel to a Model is called mapping 
        //there are two types of mapping 
        //Manual Mapping

        //Employee employee = new Employee()
        //{
        //    Id = model.Id,
        //    Name = model.Name,
        //    Address = model.Address,
        //    Salary = model.Salary,
        //    Age = model.Age,
        //    Email = model.Email,
        //    HiringDate = model.HiringDate,
        //    IsActive = model.IsActive,
        //    PhoneNumber = model.PhoneNumber,
        //    WorkFor = model.WorkFor,
        //    WorkForId = model.WorkForId,

        //}; 
        #endregion

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Departments"] = await _unitOfWork.Department.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {

            if (ModelState.IsValid)
            {


                try
                {
                    if (model.Image is not null)
                    {

                        model.ImageName = DocumentSettings.Upload(model.Image, "images");
                    }




                    var employee = _mapper.Map<Employee>(model);

                    await _unitOfWork.Employee.AddAsync(employee);
                    var Count = await _unitOfWork.CompleteAsync();

                    if (Count > 0)
                    {
                        TempData["Message01"] = "Employee is Created";
                    }
                    else
                    {
                        TempData["Message02"] = "Failed to create Employee";
                    }
                    return RedirectToAction("Index");
                }

                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);

        }

        #endregion


        #region Details

        #region Manual mapping
        //EmployeeViewModel employeeViewModel = new EmployeeViewModel()
        //{
        //    Id = employee.Id,
        //    Name = employee.Name,
        //    Address = employee.Address,
        //    Salary = employee.Salary,
        //    Age = employee.Age,
        //    Email = employee.Email,
        //    HiringDate = employee.HiringDate,
        //    IsActive = employee.IsActive,
        //    PhoneNumber = employee.PhoneNumber,
        //    WorkFor = employee.WorkFor,
        //    WorkForId = employee.WorkForId,

        //}; 
        #endregion


        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }


                var employee = await _unitOfWork.Employee.GetAsync(id.Value);

                if (employee == null)
                {
                    return NotFound();
                }

               

                var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);
                return View(ViewName, employeeViewModel);
            }
            catch (Exception ex)
            {
                //Log Exception and show a friendly message
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Departments"] = await _unitOfWork.Department.GetAllAsync();
            //if(id == null)
            //{
            //    return BadRequest(); //400
            //}

            return await Details(id, "Edit");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ImageName is not null)
                    {

                        DocumentSettings.Delete(model.ImageName, "images");
                    }
                    if (model.Image is not null)
                    {
                        model.ImageName = DocumentSettings.Upload(model.Image, "images");
                    }


                    #region Manual-Mapping

                    //Employee employee = new Employee()
                    //{
                    //    Id = model.Id,
                    //    Name = model.Name,
                    //    Address = model.Address,
                    //    Salary = model.Salary,
                    //    Age = model.Age,
                    //    Email = model.Email,
                    //    HiringDate = model.HiringDate,
                    //    IsActive = model.IsActive,
                    //    PhoneNumber = model.PhoneNumber,
                    //    WorkFor = model.WorkFor,
                    //    WorkForId = model.WorkForId,

                    //}; 
                    #endregion
                    //AutoMapping
                    var employee = _mapper.Map<Employee>(model);


                    _unitOfWork.Employee.Update(employee);
                    var Count =await _unitOfWork.CompleteAsync();
                    if (Count > 0)
                    {
                        TempData["Message01"] = "Employee is edited successfully";
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id) => await Details(id, "Delete");

        #region RefactoredCode
        //if (id == null)//
        //{
        //    return BadRequest();
        //}
        //var employee = _employeeRepository.Get(id.Value);
        //if (employee == null)
        //{
        //    return NotFound();
        //}  
        #endregion




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Delete([FromRoute] int? id, EmployeeViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                #region MANUAL MAPPING
                //Employee employee = new Employee()
                //{
                //    Id = model.Id,
                //    Name = model.Name,
                //    Address = model.Address,
                //    Salary = model.Salary,
                //    Age = model.Age,
                //    Email = model.Email,
                //    HiringDate = model.HiringDate,
                //    IsActive = model.IsActive,
                //    PhoneNumber = model.PhoneNumber,
                //    WorkFor = model.WorkFor,
                //    WorkForId = model.WorkForId,

                //}; 
                #endregion
                //AUTO MAPPING
                var employee = _mapper.Map<Employee>(model);

                _unitOfWork.Employee.Delete(employee);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    if (model.ImageName is not null)
                    {
                        DocumentSettings.Delete(model.ImageName, "images");
                    }
                    return RedirectToAction("Index");
                }

            }
            return View(model);
        }

        #endregion

    }
}
