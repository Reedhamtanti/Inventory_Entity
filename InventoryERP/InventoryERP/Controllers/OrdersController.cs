using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryERP.Models;

namespace InventoryERP.Controllers
{
    public class OrdersController : Controller
    {
        private InventoryManagementEntities db = new InventoryManagementEntities();

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            var orders = db.Orders.Include(o => o.Location);
            return View(await orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Address");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OrderId,OrderNumber,OrderTotal,Status,LocationId")] Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await SaveOrder(order);
                    return RedirectToAction("Index");
                }


            }
            catch (SqlException e)
            {
                await SaveOrder(order);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("constrain"))
                {
                    await SaveOrder(order);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Address", order.LocationId);
            return View(order);
        }

        public async Task SaveOrder(Order order)
        {
            order.OrderId = Guid.NewGuid();
            db.Orders.Add(order);
            await db.SaveChangesAsync();
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Address", order.LocationId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OrderId,OrderNumber,OrderTotal,Status,LocationId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Address", order.LocationId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
