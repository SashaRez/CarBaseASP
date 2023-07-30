using CarBaseFinal.Data;
using CarBaseFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CarBaseFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _applicationContext;
        private readonly CarModel carModel;
        private readonly KorzinaModel korzinaModel;
        private readonly OrderModel orderModel;
        private readonly AllOrdersModel allOrdersModel;
        private readonly ClientModel clientModel;
        private readonly SaleModel saleModel;


        public HomeController(ILogger<HomeController> logger, ApplicationContext applicationContext)
        {
            _logger = logger;
            _applicationContext = applicationContext;

            carModel = new CarModel();
            carModel.CarDetailList = new List<Car>();

            korzinaModel = new KorzinaModel();
            korzinaModel.KorzinaDetailList = new List<Korzina>();

            orderModel = new OrderModel();
            orderModel.OrderList = new List<Order>();

            allOrdersModel = new AllOrdersModel();
            allOrdersModel.AllOrdersDetailList = new List<AllOrders>();

            clientModel = new ClientModel();
            clientModel.ClientDetailList = new List<Client>();

            saleModel = new SaleModel();
            saleModel.SaleDetailList = new List<Sale>();
        }


        public IActionResult Index(string selectedMark, string selectedType)
        {
            var data = _applicationContext.Cars;

            foreach (var item in data)
            {

                item.Mark = _applicationContext.Marks.FirstOrDefault(t => t.MarkId == item.MarkID)?.Marka;
                item.Type = _applicationContext.Types.FirstOrDefault(t => t.TypeID == item.TypeID)?.Type;


                carModel.CarDetailList?.Add(new Car
                {
                    CarID = item.CarID,
                    TypeID = item.TypeID,
                    Type = item.Type,
                    MarkID = item.MarkID,
                    Mark = item.Mark,
                    Model = item.Model,
                    Color = item.Color,
                    Price = item.Price,
                });
            }

            if (selectedMark != "Все марки" && !string.IsNullOrEmpty(selectedMark))
            {
                carModel.CarDetailList = carModel.CarDetailList?.Where(c => c.Mark?.Trim() == selectedMark?.Trim()).ToList();
            }

            if (selectedType != "Все типы" && !string.IsNullOrEmpty(selectedMark))
            {
                carModel.CarDetailList = carModel.CarDetailList?.Where(c => c.Type?.Trim() == selectedType?.Trim()).ToList();
            }

            return View(carModel);

        }

        public IActionResult Korzina(string selectedMark, string selectedType)
        {
            var korzina = _applicationContext.Korzina;

            foreach (var item in korzina)
            {

                korzinaModel.KorzinaDetailList?.Add(new Korzina
                {
                    CarID = item.CarID,
                    Mark = item.Mark,
                    Model = item.Model,
                    Type = item.Type,
                    Color = item.Color,
                    Kol = item.Kol,
                    Price = item.Kol * item.Price,
                });
            }

            if (selectedMark != "Все марки" && !string.IsNullOrEmpty(selectedMark))
            {
                korzinaModel.KorzinaDetailList = korzinaModel.KorzinaDetailList?.Where(c => c.Mark?.Trim() == selectedMark?.Trim()).ToList();
            }

            if (selectedType != "Все типы" && !string.IsNullOrEmpty(selectedMark))
            {
                korzinaModel.KorzinaDetailList = korzinaModel.KorzinaDetailList?.Where(c => c.Type?.Trim() == selectedType?.Trim()).ToList();
            }

            return View(korzinaModel);

        }


        [HttpGet]
        public IActionResult MakeOrder(int? id)
        {
            var korzina = _applicationContext.Korzina.Find(id);

            if (korzina != null)
            {
                orderModel.OrderList?.Add(new Order
                {
                    CarID = korzina.CarID,
                    Mark = korzina.Mark?.Trim(),
                    Model = korzina.Model?.Trim(),
                    Type = korzina.Type?.Trim(),
                    Color = korzina.Color?.Trim(),
                    Kol = korzina.Kol,
                    Price = korzina.Price,
                });

            }

            return View(orderModel);

        }


        [HttpPost]
        public IActionResult MakeOrder([FromForm] string ClientName, string ClientPhone, int CarID)
        {
            _applicationContext.Clients.Add(new Client
            {
                ClientName = ClientName.Trim(),
                ClientPhone = long.Parse(ClientPhone),
            });

            _applicationContext.SaveChanges();

            var client = _applicationContext.Clients.FirstOrDefault(c => c.ClientName == ClientName.Trim() && c.ClientPhone == Convert.ToInt64(ClientPhone));

            if (client != null)
            {
                _applicationContext.Orders.Add(new AllOrders
                {
                    CarID = CarID,
                    ClientID = client.ClientID,
                    ClientName = ClientName,
                    ClientPhone = long.Parse(ClientPhone),
                });
            }

            _applicationContext.SaveChanges();

            return RedirectToAction("Order");

        }


        public IActionResult Client()
        {
            var clients = _applicationContext.Clients;

            foreach (var item in clients)
            {
                clientModel.ClientDetailList?.Add(new Client
                {
                    ClientID = item.ClientID,
                    ClientName = item.ClientName,
                    ClientPhone = item.ClientPhone,
                });
            }

            return View(clientModel);

        }


        public IActionResult Order()
        {
            var orders = _applicationContext.Orders;

            foreach (var item in orders)
            {

                allOrdersModel.AllOrdersDetailList?.Add(new AllOrders
                {
                    OrderID = item.OrderID,
                    CarID = item.CarID,
                    ClientID = item.ClientID,
                    ClientName = item.ClientName,
                    ClientPhone = item.ClientPhone,
                });
            }

            return View(allOrdersModel);
        }


        public IActionResult DeleteFromKorzina(int? id)
        {

            var data = _applicationContext.Korzina.Find(id);

            if (data != null)
            {
                _applicationContext.Korzina.Remove(data);
                _applicationContext.SaveChanges();
            }

            return RedirectToAction("Korzina");
        }


        [HttpPost]
        public IActionResult DeleteFromOrders(int id)
        {

            _applicationContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Orders', RESEED, {id - 1})");

            var data = _applicationContext.Orders.Find(id);

            if (data != null)
            {
                _applicationContext.Orders.Remove(data);
                _applicationContext.SaveChanges();
            }

            return RedirectToAction("Order");

        }


        public IActionResult PayOrder(int id)
        {
            var carID = _applicationContext.Orders.FirstOrDefault(o => o.OrderID == id)?.CarID;

            var order = _applicationContext.Orders.Find(id);
            var car = _applicationContext.Korzina.Find(carID);

            if (order != null && car != null)
            {
                _applicationContext.Sales.Add(new Sale
                {
                    OrderID = id,
                    ClientID = order.ClientID,
                    Mark = car.Mark,
                    Model = car.Model,
                    Type = car.Type,
                    Color = car.Color,
                    Price = car.Price,
                    ClientName = order.ClientName,
                    ClientPhone = order.ClientPhone,
                });

            }

            DeleteFromKorzina(carID);

            return RedirectToAction("Sale");
        }


        public IActionResult Sale()
        {
            var sales = _applicationContext.Sales;

            foreach (var item in sales)
            {

                saleModel.SaleDetailList?.Add(new Sale
                {
                    OrderID = item.OrderID,
                    ClientID = item.ClientID,
                    Mark = item.Mark,
                    Model = item.Model,
                    Type = item.Type,
                    Color = item.Color,
                    Price = item.Price,
                    ClientName = item.ClientName,
                    ClientPhone = item.ClientPhone,
                });

            }

            return View(saleModel);
        }

        [HttpGet]
        public ActionResult AddToKorzina(int id)
        {
            var dataCars = _applicationContext.Cars.Find(id);

            if (dataCars != null)
            {
                dataCars.Mark = _applicationContext.Marks.FirstOrDefault(t => t.MarkId == dataCars.MarkID)?.Marka;
                dataCars.Type = _applicationContext.Types.FirstOrDefault(t => t.TypeID == dataCars.TypeID)?.Type;

                korzinaModel.KorzinaDetailList?.Add(new Korzina
                {
                    CarID = dataCars.CarID,
                    Mark = dataCars.Mark?.Trim(),
                    Model = dataCars.Model?.Trim(),
                    Type = dataCars.Type?.Trim(),
                    Color = dataCars.Color?.Trim(),
                    Price = dataCars.Price,
                });

            }

            return View(korzinaModel);

        }



        [HttpPost]
        public IActionResult AddToKorzina([FromForm] int CarID, string Mark, string Model, string Type, string Color, int Kol, decimal Price)
        {

            var existingItem = _applicationContext.Korzina.FirstOrDefault(k => k.CarID == CarID);

            if (existingItem != null)
            {
                existingItem.Kol += Kol;
            }

            else
            {
                _applicationContext.Korzina.Add(new Korzina
                {
                    CarID = CarID,
                    Mark = Mark,
                    Model = Model,
                    Type = Type,
                    Color = Color,
                    Kol = Kol,
                    Price = Price,
                });
            }


            _applicationContext.SaveChanges();


            return RedirectToAction("Korzina");

        }




        [HttpGet]
        public ActionResult Edit(int id)
        {

            var data = _applicationContext.Cars.Find(id);

            if (data != null)
            {
                carModel.CarDetailList?.Add(new Car
                {
                    CarID = data.CarID,
                    TypeID = data.TypeID,
                    MarkID = data.MarkID,
                    Model = data.Model?.Trim(),
                    Color = data.Color?.Trim(),
                    Price = data.Price,
                });


            }

            return View(carModel);
        }


        [HttpPost]
        public IActionResult Edit([FromForm] int TypeID, int MarkID, string Model, string Color, decimal Price, int id)
        {

            var data = _applicationContext.Cars.Find(id);


            if (data != null)
            {

                data.TypeID = TypeID;
                data.MarkID = MarkID;
                data.Model = Model;
                data.Color = Color;
                data.Price = Price;

                _applicationContext.Cars.Update(data);
                _applicationContext.SaveChanges();

            }

            return RedirectToAction("Index");

        }


        public IActionResult CarInfo(int id)
        {
            var car = _applicationContext.Cars.FirstOrDefault(c => c.CarID == id);

            if (car != null)
            {
                car.Mark = _applicationContext.Marks.FirstOrDefault(t => t.MarkId == car.MarkID)?.Marka;
                car.Type = _applicationContext.Types.FirstOrDefault(t => t.TypeID == car.TypeID)?.Type;
            }

            return View(car);

        }


        [HttpPost]
        public ActionResult Delete(int id)
        {

            _applicationContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Cars', RESEED, {id - 1})");

            var data = _applicationContext.Cars.Find(id);

            if (data != null)
            {
                _applicationContext.Cars.Remove(data);
                _applicationContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public ActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCar([FromForm] int TypeID, int MarkID, string Model, string Color, decimal Price)
        {

            _applicationContext.Cars.Add(new Car
            {
                TypeID = TypeID,
                MarkID = MarkID,
                Model = Model,
                Color = Color,
                Price = Price,
            });

            _applicationContext.SaveChanges();


            return RedirectToAction("Index");
        }


        public ActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}