using System.Linq;
using System.Threading.Tasks;
using dasixtytwo.lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp2.Models;

namespace WebApp2.Controllers
{
  public class EmployeeController : Controller
  {
    private Northwind db;

    public EmployeeController(Northwind injectedContext)
    {
      db = injectedContext;
    }

    // GET: Employees
    public IActionResult Index()
    {
      var employees = new EmployeeIndexViewModel
      {
        Employees = db.Employees.ToList()
      };
      return View(employees); //pass model to view
    }

    // GET: Employees/Details/ID
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var employees = await db.Employees
          .Include(e => e.ReportsToNavigation)
          .FirstOrDefaultAsync(m => m.EmployeeID == id);
      if (employees == null)
      {
        return NotFound();
      }

      return View(employees);
    }

    // GET: Employees/Create
    public IActionResult Create()
    {
      ViewData["ReportsTo"] = new SelectList(db.Employees, "EmployeeID", "FirstName");
      return View();
    }

    // POST: Employees/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> Create([Bind("EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Notes,ReportsTo")] Employee employee)
    {
      if (ModelState.IsValid)
      {
        db.Add(employee);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["ReportsTo"] = new SelectList(db.Employees, "EmployeeID", "FirstName", employee.ReportsTo);
      return View(employee);
    }

    // GET: Employees/Edit/ID
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var employees = await db.Employees.FindAsync(id);
      if (employees == null)
      {
        return NotFound();
      }
      ViewData["ReportsTo"] = new SelectList(db.Employees, "EmployeeID", "FirstName", employees.ReportsTo);
      return View(employees);
    }

    // POST: Employees/Edit/ID
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Notes,ReportsTo")] Employee employee)
    {
      if (id != employee.EmployeeID)
      {
        return NotFound("Employee is not found!!");
      }

      if (ModelState.IsValid)
      {
        try
        {
          db.Update(employee);
          await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!EmployeesExists(employee.EmployeeID))
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
      ViewData["ReportsTo"] = new SelectList(db.Employees, "EmployeeID", "FirstName", employee.ReportsTo);
      return View(employee);
    }

    // GET: Employees/Delete/ID
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var employees = await db.Employees
          .Include(e => e.ReportsToNavigation)
          .FirstOrDefaultAsync(m => m.EmployeeID == id);
      if (employees == null)
      {
        return NotFound();
      }

      return View(employees);
    }

    // POST: Employees/Delete/ID
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var employees = await db.Employees.FindAsync(id);
      db.Employees.Remove(employees);
      await db.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool EmployeesExists(int id)
    {
      return db.Employees.Any(e => e.EmployeeID == id);
    }
  }
}