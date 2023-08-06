using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IES_WebAuth_Project.Areas.Identity.Data;
using IES_WebAuth_Project.Data;
using IES_WebAuth_Project.Models;

namespace IES_WebAuth_Project.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ContactController : Controller
    {
        private readonly SignInManager<WebUser> _signInManager;
        private readonly ApplicationDatabaseContext _context;

        public ContactController(ApplicationDatabaseContext context, SignInManager<WebUser> signInManager)
        {
            _signInManager = signInManager;
            _context = context;
        }

        // GET: Contact
        // Displays a list of contacts.
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contacts.ToListAsync());
        }

        // GET: Contact/Details/5
        // Displays details of a specific contact based on its 'id'.
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contact/Create
        // Returns the view to create a new contact.
        public IActionResult Create()
        {
            // Passes the user's Id to the view to be used as the contact's UserId.
            ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View();
        }

        // POST: Contact/Create
        // Handles the form submission to create a new contact.
        // Redirects to the Index view after successful creation.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Address,City,State,Zip,Status,UserId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Id = Guid.NewGuid();
                _context.Add(contact);
                await _context.SaveChangesAsync();
                TempData["success"] = "The Creation task was Successful!";
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contact/Edit/5
        // Returns the view to edit an existing contact based on its 'id'.
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contact/Edit/5
        // Handles the form submission to edit an existing contact.
        // Redirects to the Index view after successful update.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,Email,Address,City,State,Zip,Status,UserId")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["success"] = "The Update task was Successful!";
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: MangerContact/Delete/5
        // Returns the view to delete a specific contact based on its 'id'.
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contact/Delete/5
        // Handles the form submission to delete a contact based on its 'id'.
        // Redirects to the Index view after successful deletion.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'AppDBContext.Contacts' is null.");
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }
            await _context.SaveChangesAsync();
            TempData["success"] = "The delete task was Successful!";
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a contact exists in the database based on its 'id'.
        private bool ContactExists(Guid id)
        {
            return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
