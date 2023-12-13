using System;
using System.Collections.Generic;

namespace L46_supermarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            Supermarket supermarket = new Supermarket(random);

            supermarket.ServeClient();

            Console.WriteLine($"Все клиенты обслужены. Магазин заработал: {supermarket.Money} деревянных.");
        }
    }

    class Client
    {
        private Random _random;
        private List<Product> _products = new List<Product>();

        private int _maxMoney = 100;
        private int _money;

        public Client(Random random)
        {
            _random = random;
            _money = _random.Next(_maxMoney);
            DialProducts();
        }

        public List<Product> ProductList
        {
            get
            {
                List<Product> list = new List<Product>();

                foreach (Product product in _products)
                    list.Add(product);

                return list;
            }
        }

        public int BuyGoods(int check)
        {
            while (TryPayPucrhases(check) == false)
            {
                Product product = RemoveItemFromCart(_random.Next(_products.Count));
                check -= (int)product;
                Console.WriteLine($"Клиент выложил: {(Product)product}.");
            }

            Console.WriteLine($"Клиент оплатил: {check} деревянных.");
            _money -= check;
            return check;
        }

        private bool TryPayPucrhases(int check)
        {
            if (check <= _money)
                return true;
            else
                return false;
        }

        private Product RemoveItemFromCart(int index)
        {
            Product product = _products[index];
            _products.RemoveAt(index);
            return product;
        }

        private void DialProducts()
        {
            Array listProduct = Enum.GetValues(typeof(Product));
            int countProduct = _random.Next(listProduct.Length);

            for (int i = 0; i < countProduct; i++)
            {
                int randomNumberProduct = _random.Next(listProduct.Length);
                _products.Add((Product)listProduct.GetValue(randomNumberProduct));
            }
        }
    }

    class Supermarket
    {
        private Random _random;
        private Queue<Client> _clients = new Queue<Client>();

        private int _maxQeueLenght = 10;
        private int _delimeterLenght = 35;

        private char _delimeter = '=';

        public Supermarket(Random random)
        {
            _random = random;
            FillQeue(_random.Next(_maxQeueLenght));
        }

        public int Money { get; private set; }

        public void ServeClient()
        {
            int clientCount = _clients.Count;

            for (int i = 0; i < clientCount; i++)
            {
                Client client = _clients.Dequeue();
                int check = CalculatePrice(client.ProductList);
                Console.WriteLine($"Обслуживаем следующего клиента.\nКлиенту насчитали: {check} деревянных.");

                Money += client.BuyGoods(check);
                Console.WriteLine(new string(_delimeter, _delimeterLenght));
            }
        }

        private void FillQeue(int qeueLenght)
        {
            for (int i = 0; i < qeueLenght; i++)
                _clients.Enqueue(new Client(_random));
        }

        private int CalculatePrice(List<Product> productList)
        {
            int check = 0;

            foreach (var product in productList)
                check += (int)product;

            return check;
        }
    }

    enum Product
    {
        Apple = 30,
        Chips = 15,
        Milk = 33,
        Cheese = 27,
        Sausage = 55,
        Carrot = 22,
        Onion = 20,
        MineralWater = 10,
        Beef = 48,
        Chocolate = 17,
        Candies = 25,
    }
}
