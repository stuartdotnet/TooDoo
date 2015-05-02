using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TooDoo.Model;
using TooDoo.Models;
using TooDoo.Repository;
using TooDoo.Helpers;

namespace TooDoo.Controllers
{
    [CustomHandleError]
    [Authorize(Users = "manager")]
    public class ToDoController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult ReadOnly()
        {
            return View("ReadOnly", db.ToDoRepository.Get().OrderBy(t => t.DueDate).Where(t => !t.IsDone));     
        }

        // GET: /ToDo/
        public ActionResult Index()
        {
            try
            {
                return View("Index", db.ToDoRepository.Get().OrderBy(t => t.DueDate));                
            }
            catch (DataException e)
            {
                Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);
                TempData["ResultText"] = "Could not retrieve ToDo List.";
                TempData["Success"] = false;

                return RedirectToAction("Index");
            }
        }

        // GET: /ToDo/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["ResultText"] = "No ToDo item selected.";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }

            try
            {
                ToDoItem todoitem = db.ToDoRepository.GetById(id);
                if (todoitem == null)
                {
                    TempData["ResultText"] = "No ToDo item found.";
                    TempData["Success"] = false;
                    return RedirectToAction("Index");
                }
                return View(todoitem);
            }
            catch (DataException e)
            {
                Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);
                TempData["ResultText"] = "Could not retrieve ToDo item.";
                TempData["Success"] = false;

                return RedirectToAction("Index");
            }
        }

        // GET: /ToDo/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: /ToDo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TodoItemId,Title,Details,DueDate,IsDone")] ToDoItem todoitem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.ToDoRepository.Insert(todoitem);
                    db.Save();
                    TempData["ResultText"] = "ToDo item saved successfully.";
                    TempData["Success"] = true;
                }
                catch (DataException e)
                {
                    Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);
                    TempData["ResultText"] = "ToDo item was not saved. Please try again";
                    TempData["Success"] = false;
                }
             
                return RedirectToAction("Index");
            }

            return View(todoitem);
        }

        // GET: /ToDo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["ResultText"] = "No ToDo item selected.";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }

            try
            {
                ToDoItem todoitem = db.ToDoRepository.GetById(id);
                if (todoitem == null)
                {
                    TempData["ResultText"] = "No ToDo item found.";
                    TempData["Success"] = false;
                    return RedirectToAction("Index");
                }

                var editModel = new EditToDoViewModel()
                {
                    ToDoItem = todoitem,
                    WasDone = todoitem.IsDone
                };

                return View("Edit", editModel);
            }
            catch (DataException e)
            {
                Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);
                TempData["ResultText"] = "Could not retrieve ToDo item.";
                TempData["Success"] = false;

                return RedirectToAction("Index");
            }          
        }

        // POST: /ToDo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditToDoViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (UpdateToDoItem(model.ToDoItem, model.WasDone))
                {
                    TempData["ResultText"] = "ToDo item saved successfully.";
                    TempData["Success"] = true;
                }
                else
                {
                    TempData["ResultText"] = "ToDo item was not saved. Please try again";
                    TempData["Success"] = false;
                }
                
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /ToDo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["ResultText"] = "No ToDo item selected.";
                TempData["Success"] = false;
                return RedirectToAction("Index");
            }

            try
            {
                ToDoItem todoitem = db.ToDoRepository.GetById(id);

                if (todoitem == null)
                {
                    TempData["ResultText"] = "No ToDo item found.";
                    TempData["Success"] = false;
                    return RedirectToAction("Index");
                }
                
                return View("Delete", todoitem);
            }
            catch (DataException e)
            {
                Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);
                TempData["ResultText"] = "Could not retrieve ToDo item.";
                TempData["Success"] = false;

                return RedirectToAction("Index");
            }
        }

        // POST: /ToDo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ToDoItem todoitem = db.ToDoRepository.GetById(id);
                db.ToDoRepository.Delete(todoitem);
                db.Save();

                TempData["ResultText"] = "ToDo item deleted successfully.";
                TempData["Success"] = true;
            }
            catch (DataException e)
            {
                Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);
                TempData["ResultText"] = "Could not delete ToDo item.";
                TempData["Success"] = false;
            }

            return RedirectToAction("Index");
        }

        public ActionResult QuickComplete(int id)
        {
            try
            {
                ToDoItem todoitem = db.ToDoRepository.GetById(id);

                var wasDone = todoitem.IsDone;

                // Toggle IsDone
                if (todoitem.IsDone) 
                {
                    todoitem.IsDone = false;
                }
                else
                {
                    todoitem.IsDone = true;
                }

                // Update Todo item, return result and updated details
                if (UpdateToDoItem(todoitem, wasDone))
                {
                    return Json(new
                    {
                        success = "true",
                        done = todoitem.IsDone.ToString().ToLower(),
                        completedDateTime = todoitem.Completed.ToShortDateTimeNullable(),
                        overdue = (todoitem.DueDate < DateTime.Now && !todoitem.IsDone).ToString().ToLower() 
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (DataException e)
            {
                Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);
                TempData["ResultText"] = "Something went wrong. Please try again.";
                TempData["Success"] = false;
            }
            
            return Json(new { success = "false" });
        }

        /// <summary>
        /// Updates ToDo item
        /// </summary>
        /// <param name="todo"></param>
        /// <param name="wasDone"></param>
        /// <returns>Success boolean</returns>
        protected bool UpdateToDoItem(ToDoItem todo, bool wasDone)
        {
            try
            {
                // If Edit is marking task as done, update Completed DateTime
                if (todo.IsDone && !wasDone)
                {
                    todo.Completed = DateTime.Now;
                }

                // Or if unchecking Done, clear Completed DateTime
                if (!todo.IsDone && wasDone)
                {
                    todo.Completed = null;
                }

                db.ToDoRepository.Update(todo);
                db.Save();
            }
            catch (DataException e)
            {
                Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
