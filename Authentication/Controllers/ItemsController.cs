﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Authentication.Controllers
{
    public class ItemsController : Controller
    {
        private ItemSevice _itemService;
        private JournalService _journalService;
        private BookService _bookService;
        private BrochureService _brochureService;
        public ItemsController()
        {
            _itemService = new ItemSevice();
            _bookService = new BookService();
            _journalService = new JournalService();
            _brochureService = new BrochureService();
            _itemService.DeleteAll();
        }

       

        public void CreateBase()
        {
            int id_of_items = 0;
            _itemService.DeleteAll();


            foreach (var item in _brochureService.GetBrochureList())
            {
                _itemService.Add(new Item { Id = item.Id, Name = item.Name, Author = "-", Price = item.Price, Number = "-", Type = "Brochure" });
            }
            foreach (var item in _journalService.GetJournalsList())
            {
                _itemService.Add(new Item { Id = item.Id, Name = item.Name, Author = item.Author, Price = item.Price, Number = item.Number, Type = "Jurnal" });
            }
            foreach (var item in _bookService.GetBookList())
            {
                _itemService.Add(new Item { Id = item.Id, Name = item.Name, Author = item.Author, Price = item.Price, Number = "-", Type = "Book" });
            }
            _itemService.Save();
        }

        public ActionResult Index()
        {
            CreateBase();
            ViewBag.DataTable = _itemService.GetItemList();
            return View();
        }

        public ActionResult Details(int id)
        {
            CreateBase();
            int objId = id + _itemService.GetItemList().Count();
            Item item = _itemService.GetItem(objId);
            if (item != null)
            {
                return PartialView("Details", item);
            }
            return View("Index");
        }


        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            
            Item comp = _itemService.GetItem(id);
            if (comp != null)
            {
                return PartialView("Delete", comp);
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteRecord(int id)
        {
           
            Item it = _itemService.GetItem(id);

            if (it != null)
            {
                _itemService.Delete(id);
                _itemService.Save();
            }
            else
            {
                return Content("<h2>Такого объекта не существует!</h2>");
            }
            return RedirectToAction("Index");
        }

    }
}