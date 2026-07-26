using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Warehouse.Mvc.Models.Product;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos.Product;

namespace Warehouse.Mvc.Controllers.Product
{
    public class ProductsController(IProductService productService,ICategoryService categoryService,ISubCategoryService subCategoryService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var products = await productService.GetAllProductsAsync(ct);
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id,CancellationToken ct)
        {
            string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out int userId))
            {
                return Unauthorized();
            }

            var result = await productService.SoftDeleteProductAsync(id, userId, ct);

            TempData[result.IsSuccess? "SuccessMessage": "ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken ct)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var product = await productService
                .GetProductDetailAsync(id, ct);

            var viewModel = new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                Unit = product.Unit,
                IsConsumable = product.IsConsumable
            };

            await PopulateDropdownsAsync(viewModel, ct);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel viewModel,CancellationToken ct)
        {
            if (viewModel.Id <= 0)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(viewModel, ct);
                return View(viewModel);
            }

            var updateDto = new UpdateProductDto
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                CategoryId = viewModel.CategoryId,
                SubCategoryId = viewModel.SubCategoryId,
                Unit = viewModel.Unit,
                IsConsumable = viewModel.IsConsumable
            };

            var result = await productService
                .UpdateProductAsync(updateDto, ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message ?? "خطا در ویرایش کالا.");

                await PopulateDropdownsAsync(viewModel, ct);

                return View(viewModel);
            }

            TempData["SuccessMessage"] =
                result.Message ?? "کالا با موفقیت ویرایش شد.";

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(EditProductViewModel viewModel,CancellationToken ct)
        {
            var categories = await categoryService.GetCategoriesAsync(ct);

            var subCategories = await subCategoryService.GetAllSubCategoriesByCategoryId(viewModel.CategoryId,ct);

            viewModel.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            viewModel.SubCategories = subCategories
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
                .ToList();
        }
        private async Task PopulateCreateDropdownsAsync(CreateProductViewModel viewModel, CancellationToken ct)
        {
            var categories = await categoryService.GetCategoriesAsync(ct);

            var subCategories = await subCategoryService.GetAllSubCategoriesByCategoryId(viewModel.CategoryId, ct);

            viewModel.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            viewModel.SubCategories = subCategories
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
                .ToList();
        }

        [HttpGet]
        public async Task<IActionResult> GetSubCategories(int categoryId,CancellationToken ct)
        {
            if (categoryId <= 0)
            {
                return Json(Array.Empty<object>());
            }

            var subCategories = await subCategoryService
                .GetAllSubCategoriesByCategoryId(categoryId, ct);

            var result = subCategories
                .Select(subCategory => new
                {
                    id = subCategory.Id,
                    name = subCategory.Name
                })
                .ToList();

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReloadSubCategories(EditProductViewModel viewModel,CancellationToken ct)
        {
            if (viewModel.Id <= 0)
            {
                return BadRequest();
            }

            // The old subcategory may not belong to the newly selected category.
            viewModel.SubCategoryId = null;

            await PopulateDropdownsAsync(viewModel, ct);

            // Prevent old ModelState values from overriding the updated ViewModel.
            ModelState.Clear();

            return View("Edit", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var viewModel = new CreateProductViewModel();

            var categories = await categoryService.GetCategoriesAsync(ct);

            viewModel.Categories= categories
                .Select(category=>new SelectListItem
                {
                    Value=category.Id.ToString(),
                    Text=category.Name
                }
                ).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel viewModel,CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCreateDropdownsAsync(viewModel, ct);
                return View(viewModel);
            }
            var product = new CreateProductDto
            {
                ProductName = viewModel.ProductName,
                CategoryId = viewModel.CategoryId,
                SubCategoryId = viewModel.SubCategoryId,
                Unit = viewModel.Unit,
                IsConsumable = viewModel.IsConsumable,
            };

            var result = await productService.CreateProductAsync(product,ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "خطا در ایجاد کالا");
                await PopulateCreateDropdownsAsync(viewModel, ct);
                return View(viewModel);
            }
            TempData["SuccessMessage"] =result.Message ?? "کالا با موفقیت ایجاد شد.";

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReloadCreateSubCategories(CreateProductViewModel viewModel,CancellationToken ct)
        {
            viewModel.SubCategoryId = null;

            await PopulateCreateDropdownsAsync(viewModel, ct);

            ModelState.Clear();

            return View("Create", viewModel);
        }

    }
}
