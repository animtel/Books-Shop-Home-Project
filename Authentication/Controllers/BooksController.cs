﻿using Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Services;

namespace Authentication.Controllers
{
    public class BooksController : Controller
    {
        private BookService book_service;

        public BooksController()
        {
            book_service = new BookService();
        }

        [Authorize(Roles = "user")]
        public ActionResult Index()
        {
            ViewBag.DataTable = book_service.GetBookList().ToList();

            return View("Index");
        }

        [Authorize(Roles = "user")]
        public ActionResult Details(int id)
        {
            foreach (var item in book_service.GetBookList())
            {
                book_service.Add(item);
            }
            Book it = book_service.GetBook(id);
            if (it != null)
            {
                return PartialView("Details", it);
            }
            return View("Index");
        }

        [Authorize(Roles = "user")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                book_service.Add(book);
                book_service.Save();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        [Authorize(Roles = "user")]
        public ActionResult Edit(int id)
        {
            Book book = book_service.GetBook(id);
            if (book != null)
            {
                return PartialView("Edit", book);
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult Edit(Book book)
        {
            book_service.Update(book);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "user")]
        public ActionResult Delete(int id)
        {
            Book comp = book_service.GetBook(id);
            if (comp != null)
            {
                return PartialView("Delete", comp);
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "user")]
        public ActionResult DeleteRecord(int id)
        {
            Book comp = book_service.GetBook(id);

            if (comp != null)
            {
                book_service.Delete(id);
                book_service.Save();
            }
            return RedirectToAction("Index");
        }
    }
}