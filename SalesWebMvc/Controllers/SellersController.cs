using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private const string NoSellerFoundMessage = "There is no seller for the provided Id.";
        private const string NoIdProvidedMessage = "No Id was provided.";

        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            List<Seller> sellers = await _sellerService.FindAllAsync();
            return View(sellers);
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

            Seller seller = await _sellerService.FindByIdAsync(id.Value);

            if (seller == null)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = NoSellerFoundMessage
                };
                return RedirectToAction(nameof(Error), errorParams);
            }

            return View(seller);
        }

        public async Task<IActionResult> Create()
        {
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                List<Department> departments = await _departmentService.FindAllAsync();
                SellerFormViewModel viewModel = new SellerFormViewModel
                {
                    Seller = seller,
                    Departments = departments
                };
                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
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

            Seller seller = await _sellerService.FindByIdAsync(id.Value);
            if (seller == null)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = NoSellerFoundMessage
                };
                return RedirectToAction(nameof(Error), errorParams);
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel
            {
                Seller = seller,
                Departments = departments
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                List<Department> departments = await _departmentService.FindAllAsync();
                SellerFormViewModel viewModel = new SellerFormViewModel
                {
                    Seller = seller,
                    Departments = departments
                };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Id mismatch."
                };
                return RedirectToAction(nameof(Error), errorParams);
            }

            try
            {
                await _sellerService.UpdateAsync(seller);
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

            Seller seller = await _sellerService.FindByIdAsync(id.Value);
            if (seller == null)
            {
                var errorParams = new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = NoSellerFoundMessage
                };
                return RedirectToAction(nameof(Error), errorParams);
            }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
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

        public IActionResult Error(int statusCode, string message)
        {
            ErrorViewModel evm = new ErrorViewModel
            {
                StatusCode = statusCode,
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(evm);
        }
    }
}
