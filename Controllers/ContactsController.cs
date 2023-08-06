
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IES_WebAuth_Project.Data;
using IES_WebAuth_Project.Models;

namespace IES_WebAuth_Project.Controllers
{
    [Authorize(Roles = "User")]
    public class ContactsController : Controller
    {
        private readonly ApplicationDatabaseContext _context;

        public ContactsController(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        // GET: Contacts
        // Displays a list of contacts based on the user's role and approval status.
        public Task<IActionResult> Index()
        {
            // Passes the user's Id to the view to be used as the contact's UserId.
            ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieves contacts associated with the user (created by the user or approved contacts).
            // If the 'Contacts' entity set is null, displays an error message.
            return Task.FromResult<IActionResult>(_context.Contacts != null ?
                View(_context.Contacts.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) || x.Status == "Approved").ToList()) :
                Problem("Entity set 'AppDBContext.Contacts' is null."));
        }

        // GET: Contacts/Details/5
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

        // GET: Contacts/Create
        // Returns the view to create a new contact.
        public IActionResult Create()
        {
            // Passes the user's Id to the view to be used as the contact's UserId.
            ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View();
        }

        // POST: Contacts/Create
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

        // GET: Contacts/Edit/5
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

        // POST: Contacts/Edit/5
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
                    TempData["success"] = "The Edit task was Successful!";
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
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
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

        // POST: Contacts/Delete/5
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
