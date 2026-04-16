using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class RestaurantController : Controller
    {
        AssignmentEntities db = new AssignmentEntities();
        public ActionResult CreateOrder()
        {
            var foodItemGroups = db.FoodItems
           .GroupBy(fi => fi.ItemType)
           .Select(group => new FoodItemGroup { ItemType = group.Key, Items = group.ToList() })
           .ToList();

            var model = new OrderModel
            {
                FoodItemGroups = foodItemGroups
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult CreateOrder(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var newCustomer = new Customer
                {
                    Name = model.CustomerName,
                    Phone = model.CustomerPhone
                };

                db.Customers.Add(newCustomer);
                db.SaveChanges();
                long customerId = newCustomer.CustomerID;

                foreach (var foodItemId in model.SelectedFoodItemIds)
                {
                    var orderItem = new Order
                    {
                        Customer_ID = customerId,
                        Item_ID = foodItemId
                    };

                    db.Orders.Add(orderItem);
                }

                db.SaveChanges();
                var selectedFoodItems = db.FoodItems
                    .Where(fi => model.SelectedFoodItemIds.Contains(fi.Id))
                    .ToList();

                return View("OrderSuccess");
            }

            var foodItemGroups = db.FoodItems
                .GroupBy(fi => fi.ItemType)
                .Select(group => new FoodItemGroup { ItemType = group.Key, Items = group.ToList() })
                .ToList();

            model.FoodItemGroups = foodItemGroups;

            return View(model);
        }

        public ActionResult OrderSuccess()
        {
            return View();
        }
    }
}