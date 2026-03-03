#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TipOfTheDay.Models;
using TipOfTheDay.Data;

namespace TipOfTheDay.Controllers
{
    public class TipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tips
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tip.ToListAsync());
        }

        // GET: Tips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = await _context.Tip
                .FirstOrDefaultAsync(m => m.TipId == id);
            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        // GET: Tips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipId,TipText")] Tip tip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tip);
        }

        // GET: Tips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = await _context.Tip.Include(t => t.Tags)
                                        .Where(t => t.TipId == id)
                                        .FirstOrDefaultAsync<Tip>();
            if (tip == null)
            {
                return NotFound();
            }

            // Put all the tags in vm.TagSelections so that the user can select from them.
            var tags = await _context.Tag.ToListAsync();
            var vm = new TipTagVM { Tip = tip };
            foreach (var tag in tags)
            {
                vm.TagSelections.Add(new TagChoice { Tag = tag });
            }

            return View(vm);
        }

        // POST: Tips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipTagVM vm)
        {
            if (id != vm.Tip.TipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   var result = _context.Update(vm.Tip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipExists(vm.Tip.TipId))
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
            return View(vm);  // If model state not valid
        }

        // POST: Tip/Edit/5
        [HttpPost]
        public async Task<IActionResult> EditTags(TipTagVM vm)
        {
            // Get the tip object with the ID in the VM from the database.
            // We need to do our add/remove tag operations on the object from the DB
            var tip = await _context.Tip.Include(t => t.Tags)
                                        .Where(t => t.TipId == vm.Tip.TipId)
                                        .SingleAsync();

            if (tip == null)
            {
                return NotFound();
            }

            // Note: vm.TagSelections has all the tags from the Tags table of the DB with some possibly selected

            // Remove unselected tags from the tip, add selected ones
            foreach (var selection in vm.TagSelections)
            {
                // Check the selection tag to see if it's already on the tip's list of tags
                if (tip.Tags
                    .Where(t => t.TagId == selection.Tag.TagId)
                    .Any())
                {
                    // If the selection tag IS on the tip's list and is NOT selected remove it
                    if (!selection.Selected)
                    {
                        // In order to romove the tag, we need to get the actual tag object from the tip
                        Tag tagToRemove = null;
                        foreach (Tag tag in tip.Tags)
                        {
                            if (selection.Tag.TagId == tag.TagId)
                            {
                                tagToRemove = tag;
                            }
                        }
                        tip.Tags.Remove(tagToRemove);  // We have to remove this outside the loop
                    }
                }
                // If the selection tag is NOT on the tip's list and IS selected add it
                else if (selection.Selected)
                {
                    tip.Tags.Add(selection.Tag);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipExists(tip.TipId))
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
            return View("Edit", vm); // if ModelState not valid
        }

        // GET: Tips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = await _context.Tip
                .FirstOrDefaultAsync(m => m.TipId == id);
            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        // POST: Tips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tip = await _context.Tip.FindAsync(id);
            _context.Tip.Remove(tip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipExists(int id)
        {
            return _context.Tip.Any(e => e.TipId == id);
        }
    }
}
