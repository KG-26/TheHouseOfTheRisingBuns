using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheHouseOfTheRisingBuns.Data;
using TheHouseOfTheRisingBuns.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace TheHouseOfTheRisingBuns.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public RecipesController(
            ApplicationDbContext context,
            IHostingEnvironment environment)

        {
            _context = context;
            _environment = environment;

        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Recipe.Include(r => r.CategoryName);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .Include(r => r.CategoryName)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }
        

        // GET: Recipes/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CategoryID,Title,Time,Servings,Ingredients,Method,RecipeThumb,CategoryName")] ICollection<IFormFile> files, Recipe recipe, Category category)
        {


            if (ModelState.IsValid)
            {
                //Get current logged in User.
                var recipeID = recipe.ID;
                //Go the recipe table and find the current recipe via there ID.
                var recipeCurrent = _context.Recipe;

                var recipeThumb = recipe.RecipeThumb;

                var date = DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss_");

                var uploads = Path.Combine(_environment.WebRootPath, "RecipeImages");

                foreach (var file in files)
                {
                    var filename = file.FileName;

                    if (file.Length > 0)
                    {
                        var name = Path.Combine(date + file.FileName);
                        var yolk = Path.Combine(uploads, name);
                        using (var fileStream = new FileStream(yolk, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        recipe.RecipeThumb = name;
                    }
                }


                var image = files.ToArray();
                var imageWithName = image[0].FileName;
                //Add validation if file is not present and fail model
                if (imageWithName == null)
                {
                    ModelState.AddModelError("FileURL", "Please upload file");
                }

                if (ModelState.IsValid)
                {
      
                    //User the returned user from 'appUser' set the profile image to equal 'Date' +  'imagewithname' 
                    recipeThumb = date + imageWithName;
                }

                if (recipe == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{recipeID}'.");
                }
                _context.Add(recipe);
                await _context.SaveChangesAsync();
               
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", category.CategoryName);
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        [Authorize(Roles = "Admins")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.SingleOrDefaultAsync(m => m.ID == id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", recipe.CategoryID);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CategoryID,Title,Time,Servings,Ingredients,Method,RecipeThumb,CategoryName")] ICollection<IFormFile> files, Recipe recipe, Category category)
        {
            if (id != recipe.ID)
            {
                return NotFound();
            }
         


            if (ModelState.IsValid)
            {
                try
                {
                    

                    if (ModelState.IsValid)
                    {
                        //Get current recipe.
                        var recipeID = recipe.ID;
                        //Go the recipe table and find the current recipe via there ID.
                        var recipeCurrent = _context.Recipe;

                        var recipeThumb = recipe.RecipeThumb;

                        var date = DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss_");

                        var uploads = Path.Combine(_environment.WebRootPath, "RecipeImages");

                        foreach (var file in files)
                        {
                            var filename = file.FileName;

                            //try {

                                if (file.Length > 0)
                            {
                                var name = Path.Combine(date + file.FileName);
                                var yolk = Path.Combine(uploads, name);
                                using (var fileStream = new FileStream(yolk, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }
                                recipe.RecipeThumb = name;
                            }

                            
                            //    catch (ApplicationException)
                            //{
                            //    if (!RecipeExists(recipe.ID))
                            //    {
                            //        return NotFound();
                            //    }
                            //    else
                            //    {
                            //        throw;
                            //    }
                            //}
                        }
                           
                        

                        var image = files.ToArray();
                        var imageWithName = image[0].FileName;


                        if (ModelState.IsValid)
                        {

                            //User the returned user from 'appUser' set the profile image to equal 'Date' +  'imagewithname' 
                            recipeThumb = date + imageWithName;
                            _context.Update(recipe);
                            await _context.SaveChangesAsync();
                        }
                      
                        if (recipe == null)
                        {
                            throw new ApplicationException($"Unable to load user with ID '{recipeID}'.");
                        }

                    }

                    
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.ID))
                    {
                        return NotFound();
                    }

                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", recipe.CategoryID);
            return View(recipe);
        }

//        {
//            if (id != category.ID)
//            {
//                return NotFound();
//    }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(category);
//                    await _context.SaveChangesAsync();
//}
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!CategoryExists(category.ID))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(category);
//        }
   

        // GET: Recipes/Delete/5
        [Authorize(Roles = "Admins")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .Include(r => r.CategoryName)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipe.SingleOrDefaultAsync(m => m.ID == id);
            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.ID == id);
        }

    }
}