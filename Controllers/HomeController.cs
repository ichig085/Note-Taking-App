using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NoteItEasyApp.Data;
using NoteItEasyApp.Models;

namespace NoteItEasyApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly DataContext _context;

    public HomeController(ILogger<HomeController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;  // Injected DataContext instance for database operations
    }

    // Get: /Home/Index
    public IActionResult Index()
    {
        // Retrieve all notes from the database

        var notes = _context.NoteModels.ToList();

        // Pass the list of notes to the view
        return View(notes);
    }
}

