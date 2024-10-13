using AutoMapper;
using Company.G03.BLL.Interfaces;
using Company.G03.DAL.Data.Contexts;
using Company.G03.DAL.Models;
using Company.G03.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.G03.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        #region Get a Repository object
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper) // Ask Clr to create object from departmentRepository
        {
            //_departmentRepository = departmentRepository;
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var department = await _unitOfWork.Department.GetAllAsync();

          //var departmentViewModel =  _mapper.Map<DepartmentViewModel>(department);
            return View(department);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {


            return View();
        }
        //-----------------------------------------------------------*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid)
            {

                 await _unitOfWork.Department.AddAsync(model);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        #endregion


        #region Details

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id == null)
            {
                return BadRequest(); //400
            }
            var department = await _unitOfWork.Department.GetAsync(id.Value);

            if (department == null)
            {
                return NotFound(); //404
            }
            return View(ViewName, department);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id) => await Details(id, "Edit");


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, Department model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }
                if (ModelState.IsValid)
                {
                    _unitOfWork.Department.Update(model);
                    var Count = await _unitOfWork.CompleteAsync();
                    if (Count > 0)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }
        #endregion

        //----------------------------------------------------------

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id) => await Details(id, "Delete");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, Department entity)
        {
            try
            {
                if (id != entity.Id)
                {
                    return BadRequest();
                }
                if (ModelState.IsValid)
                {
                    _unitOfWork.Department.Delete(entity); //Delegate but not now
                    var Count = await _unitOfWork.CompleteAsync();
                    if (Count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(entity);
        }
        #endregion

    }
}
