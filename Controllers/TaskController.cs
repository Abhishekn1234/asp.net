using Employee.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Net;
using System.Threading.Tasks;

namespace Employee.Controllers
{
    public class TasksController : Controller
    {
        private readonly IMongoCollection<Employee.Models.Task> _tasksCollection;

        public TasksController(IMongoDatabase database)
        {
            _tasksCollection = database.GetCollection<Employee.Models.Task>("Tasks");
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _tasksCollection.Find(_ => true).ToListAsync();
            return View(tasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var task = await _tasksCollection.Find(t => t.ID == id).FirstOrDefaultAsync();
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,DueDate,IsComplete")] Employee.Models.Task employeeTask)
        {
            if (ModelState.IsValid)
            {
                await _tasksCollection.InsertOneAsync(employeeTask);
                return RedirectToAction("Index");
            }

            return View(employeeTask);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var task = await _tasksCollection.Find(t => t.ID == id).FirstOrDefaultAsync();
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ID,Name,Description,DueDate,IsComplete")] Employee.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                var filter = Builders<Employee.Models.Task>.Filter.Eq(t => t.ID, task.ID);
                await _tasksCollection.ReplaceOneAsync(filter, task);
                return RedirectToAction("Index");
            }

            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var task = await _tasksCollection.Find(t => t.ID == id).FirstOrDefaultAsync();
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var filter = Builders<Employee.Models.Task>.Filter.Eq(t => t.ID, id);
            await _tasksCollection.DeleteOneAsync(filter);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
