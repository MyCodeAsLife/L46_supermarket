using System;
using System.Collections.Generic;

namespace L46_supermarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Supermarket supermarket = new Supermarket();

            supermarket.ServeClient();
        }
    }

    class Client
    {
        private Random _random;
        private List<Product> _products = new List<Product>();

        private int _maxMoney = 150;
        private int _money;

        public Client(Random random)
        {
            _random = random;
            _money = _random.Next(_maxMoney);
            DialProducts();
        }

        public IReadOnlyList<Product> ProductList => _products;

        public void BuyGoods(ref int check)
        {
            while (check > _money)
            {
                Product productPrice = RemoveItemFromCart(_random.Next(_products.Count));
                check -= (int)productPrice;
            }

            if (check > 0)
            {
                Console.WriteLine($"Клиент оплатил: {check} деревянных.");
                _money -= check;
            }
            else
            {
                Console.WriteLine("У клиента недостаточно средств для покупок.");
            }
        }

        private Product RemoveItemFromCart(int index)
        {
            Product product = _products[index];
            _products.RemoveAt(index);
            Console.WriteLine($"Клиент выложил: {product}.");
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

        private int _money;
        private int _maxQeueLenght = 10;
        private int _delimeterLenght = 35;

        private char _delimeter = '=';

        public Supermarket()
        {
            _random = new Random();
            FillQeue(_random.Next(_maxQeueLenght));
        }

        public void ServeClient()
        {
            int clientCount = _clients.Count;

            for (int i = 0; i < clientCount; i++)
            {
                Client client = _clients.Dequeue();
                int check = CalculatePrice(client.ProductList);
                Console.WriteLine($"Обслуживаем следующего клиента.\nКлиенту насчитали: {check} деревянных.");
                client.BuyGoods(ref check);
                _money += check;
                Console.WriteLine(new string(_delimeter, _delimeterLenght));
            }

            Console.WriteLine($"Все клиенты обслужены. Магазин заработал: {_money} деревянных.");
        }

        private void FillQeue(int qeueLenght)
        {
            for (int i = 0; i < qeueLenght; i++)
                _clients.Enqueue(new Client(_random));
        }

        private int CalculatePrice(IReadOnlyList<Product> productList)
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
