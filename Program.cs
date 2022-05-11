using Dmitriev.Ivan.TestTask.DAL;
using Dmitriev.Ivan.TestTask.UIL;
using System;

namespace Dmitriev.Ivan.TestTask
{
    class MainClass
    {
        /// <summary>
        /// Входная точка. Здесь обрабатываем ошибки, читаем команды и передаем их дальше
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Список команд:");
                Console.WriteLine(@"1/2 : Создать/Удалить товар");
                Console.WriteLine(@"3/4 : Создать/Удалить аптеку");
                Console.WriteLine(@"5/6 : Создать/Удалить склад");
                Console.WriteLine(@"7/8 : Создать/Удалить поставку");
                Console.WriteLine(@"9 : Вывести кол-во товаров в аптеке");
                Console.WriteLine(@"-1 : Выход");

                var command = -1;
                while ((command = ReadCommandCode()) != -1)
                {
                    try
                    {
                        new CommandExecuter(command).Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка:");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            finally
            {
                DBConnectionSingleton.GetInstance().Dispose();
            }
        }

        static int ReadCommandCode()
        {
            Console.Write("Введите команду: ");
            var command = -1;
            if (!int.TryParse(Console.ReadLine(), out command))
            {
                Console.WriteLine("Не удалось распознать код команды");
                return -1;
            }
            return command;
        }
    }
}
