using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {
        private const string NoDepartmentFoundMessage = "There is no department for the provided Id.";
        private const string NoIdProvidedMessage = "No Id was provided.";

        private readonly SalesWebMvcContext _context;
        private readonly DepartmentService _departmentService;

        public DepartmentsController(DepartmentService departmentService, SalesWebMvcContext context)
        {
            _context = context;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _departmentService.FindAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = NoIdProvidedMessage
                };
                return RedirectToAction(nameof(Error), errorParams);
            }

            var department = await _departmentService.FindByIdAsync(id.Value);
            if (department == null)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = NoDepartmentFoundMessage
                };
                return RedirectToAction(nameof(Error), errorParams);
            }

            return View(department);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.InsertAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = NoIdProvidedMessage
                };
                return RedirectToAction(nameof(Error), errorParams);
            }

            var department = await _departmentService.FindByIdAsync(id.Value);
            if (department == null)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = NoDepartmentFoundMessage
                };
                return RedirectToAction(nameof(Error), errorParams);
            }
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Id mismatch."
                };
                return RedirectToAction(nameof(Error), errorParams);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentService.UpdateAsync(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (NotFoundException e)
                {
                    var errorParams = new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        e.Message
                    };
                    return RedirectToAction(nameof(Error), errorParams);
                }
                catch (DbConcurrencyException e)
                {
                    var errorParams = new
                    {
                        StatusCode = StatusCodes.Status503ServiceUnavailable,
                        e.Message
                    };
                    return RedirectToAction(nameof(Error), errorParams);
                }
            }
            return View(department);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = NoIdProvidedMessage
                };
                return RedirectToAction(nameof(Error), errorParams);
            }

            var department = await _departmentService.FindByIdAsync(id.Value);
            if (department == null)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = NoDepartmentFoundMessage
                };
                return RedirectToAction(nameof(Error), errorParams);
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departmentService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    e.Message
                };
                return RedirectToAction(nameof(Error), errorParams);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode, string message)
        {
            ErrorViewModel evm = new ErrorViewModel
            {
                StatusCode = statusCode ?? StatusCodes.Status500InternalServerError,
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(evm);
        }
    }
}
