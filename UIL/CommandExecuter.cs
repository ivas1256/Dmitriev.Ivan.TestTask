using System;
using System.Collections.Generic;
using Dmitriev.Ivan.TestTask.DAL.Models;
using System.Linq;

namespace Dmitriev.Ivan.TestTask.UIL
{
    /// <summary>
    /// Реализем работу команд. Для простоты все в одном классе
    /// В реальности логика выносится в класс ModelUI, здесь остаются только вызовы
    /// Там же можно вынести всю логику CRUD один базовый класс и не писать здесь идентичный код (не стал так делать для упрощения)
    /// </summary>
    public class CommandExecuter
    {
        int commandCode;
        Dictionary<int, Action> commands;
        public CommandExecuter(int commandCode)
        {
            this.commandCode = commandCode;
            commands = new Dictionary<int, Action>()
            {
                { 1, createProduct },
                { 2, deleteProduct },
                { 3, createPharmacy },
                { 4, deletePharmacy },
                { 5, createStorage },
                { 6, deleteStorage },
                { 7, createSupply },
                { 8, deleteSupply },
                { 9, getTotalproductAmount }
            };
        }

        public void Execute()
        {
            try
            {
                commands[commandCode]();
            }
            catch (KeyNotFoundException)
            {
                throw new Exception("Такой операции не найдено");
            }
        }
        
        void createProduct()
        {
            Console.WriteLine("Имя товара:");

            var entity = new Product { Name = Console.ReadLine() };
            entity.Create();
        }

        void deleteProduct()
        {
            Console.WriteLine("Выберите ID записи:");           
            foreach (var entity in Product.GetAllEnities())
                Console.WriteLine($"({entity.ProductId}) {entity.Name}");

            Console.WriteLine("ID = ");
            new Product { ProductId = int.Parse(Console.ReadLine()) }.Delete();            
        }

        void createPharmacy()
        {
            var entity = new Pharmacy();
            Console.WriteLine("Имя аптеки:");
            entity.Name = Console.ReadLine();
            Console.WriteLine("Адрес:");
            entity.Address = Console.ReadLine();
            Console.WriteLine("Телефон (только цифры)):");
            entity.Phone = long.Parse(Console.ReadLine());

            entity.Create();
        }

        void deletePharmacy()
        {
            Console.WriteLine("Выберите ID записи:");
            foreach (var entity in Pharmacy.GetAllEnities())
                Console.WriteLine($"({entity.PharmacyId}) {entity.Name}");

            Console.WriteLine("ID = ");
            new Pharmacy { PharmacyId = int.Parse(Console.ReadLine()) }.Delete();            
        }

        void createStorage()
        {
            var entity = new Storage();
            Console.WriteLine("Имя склада:");
            entity.Name = Console.ReadLine();
            Console.WriteLine("Выберите аптеку:");
            foreach (var e in Pharmacy.GetAllEnities())
                Console.WriteLine($"({e.PharmacyId}) {e.Name}");
            Console.WriteLine("ID = ");
            entity.PharmacyId = int.Parse(Console.ReadLine());            

            entity.Create();
        }

        void deleteStorage()
        {
            Console.WriteLine("Выберите ID записи:");
            foreach (var entity in Storage.GetAllEnities())
                Console.WriteLine($"({entity.StorageId}) {entity.Name}");

            Console.WriteLine("ID = ");
            new Storage { StorageId = int.Parse(Console.ReadLine()) }.Delete();
        }

        void createSupply()
        {
            var entity = new Supply();
            Console.WriteLine("Количество в партии:");
            entity.Quantity = int.Parse(Console.ReadLine());

            Console.WriteLine("Выберите товар:");
            foreach (var e in Product.GetAllEnities())
                Console.WriteLine($"({e.ProductId}) {e.Name}");
            Console.WriteLine("ID = ");
            entity.ProductId = int.Parse(Console.ReadLine());

            Console.WriteLine("Выберите склад:");
            foreach (var e in Storage.GetAllEnities())
                Console.WriteLine($"({e.StorageId}) {e.Name}");
            Console.WriteLine("ID = ");
            entity.StorageId = int.Parse(Console.ReadLine());

            entity.Create();
        }

        void deleteSupply()
        {
            Console.WriteLine("Выберите ID записи:");
            foreach (var entity in Supply.GetAllEnities().ToList())
                Console.WriteLine($"({entity.SupplyId}) {entity.ProductName} - {entity.Quantity}шт.");

            Console.WriteLine("ID = ");
            new Supply { SupplyId = int.Parse(Console.ReadLine()) }.Delete();
        }

        void getTotalproductAmount()
        {
            Console.WriteLine("Выберите аптеку:");
            foreach (var e in Pharmacy.GetAllEnities())
                Console.WriteLine($"({e.PharmacyId}) {e.Name}");
            Console.WriteLine("ID = ");
            var pharmacyId = int.Parse(Console.ReadLine());

            var report = new Reports.TotalProductAmount_Report(pharmacyId).GetResult();
            if (report.Count == 0)
            {
                Console.WriteLine("Ничего не найдено.");
                return;
            }
            foreach (var r in report)
                Console.WriteLine($"{r.Key.Name} - {r.Value}шт.");
        }
    }
}
