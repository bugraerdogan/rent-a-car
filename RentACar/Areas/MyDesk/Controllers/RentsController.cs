﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentACar.Models;

namespace RentACar.Areas.MyDesk.Controllers
{
    [Authorize(Roles = "Moderator")]
    public class RentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MyDesk/Rents
        public async Task<ActionResult> Index()
        {
            var rents = db.Rents.Include(r => r.Bill).Include(r => r.Car).Include(r => r.User);
            return View(await rents.ToListAsync());
        }

        // GET: MyDesk/Rents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = await db.Rents.FindAsync(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            return View(rent);
        }

        // GET: MyDesk/Rents/Create
        public ActionResult Create()
        {
            IEnumerable<Car> cars = db.Cars
                .ToList()
                .Select(m => new Car
                {
                    CarId = m.CarId,
                    Model = m.Brand.Name + " " + m.Model
                });

            IEnumerable<MyUser> users = db.Users
                .ToList()
                .Select(m => new MyUser
                {
                    Id = m.Id,
                    UserName = m.UserDetails.FirstName + " " + m.UserDetails.LastName
                });

            ViewBag.CarId = new SelectList(cars, "CarId", "Model");
            ViewBag.UserId = new SelectList(users, "Id", "UserName");
            ViewBag.RentId = new SelectList(db.Bills, "RentId", "RentId");

            return View();
        }

        // POST: MyDesk/Rents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RentId,StartDate,EndDate,Paid,UserId,CarId")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                db.Rents.Add(rent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            IEnumerable<Car> cars = db.Cars
                .ToList()
                .Select(m => new Car
                {
                    CarId = m.CarId,
                    Model = m.Brand.Name + " " + m.Model
                });

            IEnumerable<MyUser> users = db.Users
                .ToList()
                .Select(m => new MyUser
                {
                    Id = m.Id,
                    UserName = m.UserDetails.FirstName + " " + m.UserDetails.LastName
                });

            ViewBag.CarId = new SelectList(cars, "CarId", "Model", rent.RentId);
            ViewBag.UserId = new SelectList(users, "Id", "UserName", rent.RentId);
            ViewBag.RentId = new SelectList(db.Bills, "RentId", "RentId", rent.RentId);
            return View(rent);
        }

        // GET: MyDesk/Rents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = await db.Rents.FindAsync(id);
            if (rent == null)
            {
                return HttpNotFound();
            }

            IEnumerable<Car> cars = db.Cars
                .ToList()
                .Select(m => new Car
                {
                    CarId = m.CarId,
                    Model = m.Brand.Name + " " + m.Model
                });

            IEnumerable<MyUser> users = db.Users
                .ToList()
                .Select(m => new MyUser
                {
                    Id = m.Id,
                    UserName = m.UserDetails.FirstName + " " + m.UserDetails.LastName
                });

            ViewBag.CarId = new SelectList(cars, "CarId", "Model", rent.RentId);
            ViewBag.UserId = new SelectList(users, "Id", "UserName", rent.RentId);
            ViewBag.RentId = new SelectList(db.Bills, "RentId", "RentId", rent.RentId);
            return View(rent);
        }

        // POST: MyDesk/Rents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RentId,StartDate,EndDate,Paid,UserId,CarId")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            IEnumerable<Car> cars = db.Cars
                .ToList()
                .Select(m => new Car
                {
                    CarId = m.CarId,
                    Model = m.Brand.Name + " " + m.Model
                });

            IEnumerable<MyUser> users = db.Users
                .ToList()
                .Select(m => new MyUser
                {
                    Id = m.Id,
                    UserName = m.UserDetails.FirstName + " " + m.UserDetails.LastName
                });

            ViewBag.CarId = new SelectList(cars, "CarId", "Model", rent.RentId);
            ViewBag.UserId = new SelectList(users, "Id", "UserName", rent.RentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", rent.UserId);

            return View(rent);
        }

        // GET: MyDesk/Rents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = await db.Rents.FindAsync(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            return View(rent);
        }

        // POST: MyDesk/Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Rent rent = await db.Rents.FindAsync(id);
            Bill bill = await db.Bills.FindAsync(id);
            db.Bills.Remove(bill);
            db.Rents.Remove(rent);
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