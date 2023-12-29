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
        private readonly Supermarket _supermarket;
        private List<Product> _products = new List<Product>();

        private int _maxMoney = 150;
        private int _money;

        public Client(Supermarket supermarket)
        {
            _supermarket = supermarket;
            _money = RandomGenerator.GetRandomNumber(_maxMoney);
            DialProducts();
        }

        public IReadOnlyList<Product> ProductList => _products;

        public int BuyGoods(int check)
        {
            while (check > _money)
            {
                Product removedProduct = RemoveItemFromCart(RandomGenerator.GetRandomNumber(_products.Count));
                check -= removedProduct.Price;
            }

            if (check > 0)
            {
                Console.WriteLine($"Клиент оплатил: {check} деревянных.");
                _money -= check;
            }
            else
            {
                Console.WriteLine("Клиент ничего не купил.");
            }

            return check;
        }

        private Product RemoveItemFromCart(int index)
        {
            Product product = _products[index];
            _products.RemoveAt(index);
            Console.WriteLine($"Клиент выложил: {product.Type}.");
            return product;
        }

        private void DialProducts()
        {
            IReadOnlyList<string> producstList = _supermarket.GetProductList();
            int productsCount = RandomGenerator.GetRandomNumber(producstList.Count);

            for (int i = 0; i < productsCount; i++)
            {
                int randomNumberProduct = RandomGenerator.GetRandomNumber(producstList.Count);
                _products.Add(_supermarket.GetProduct(randomNumberProduct));
            }
        }
    }

    class Supermarket
    {
        private ProductCreator _creator = new ProductCreator();
        private Queue<Client> _clients = new Queue<Client>();

        private int _money = 0;
        private int _maxQeueLenght = 10;
        private int _delimeterLenght = 35;

        private char _delimeter = '=';

        public Supermarket()
        {
            FillQeue(RandomGenerator.GetRandomNumber(_maxQeueLenght));
        }

        public void ServeClient()
        {
            int clientsCount = _clients.Count;

            for (int i = 0; i < clientsCount; i++)
            {
                Client client = _clients.Dequeue();
                int check = CalculatePrice(client.ProductList);
                Console.WriteLine($"Обслуживаем следующего клиента.\nКлиенту насчитали: {check} деревянных.");
                int paidCheck = client.BuyGoods(check);

                if (paidCheck > 0)
                    _money += paidCheck;

                Console.WriteLine(new string(_delimeter, _delimeterLenght));
            }

            Console.WriteLine($"Все клиенты обслужены. Магазин заработал: {_money} деревянных.");
        }

        public Product GetProduct(int productIndex) => _creator.Create(productIndex);

        public IReadOnlyList<string> GetProductList() => _creator.ProductList;

        private void FillQeue(int qeueLenght)
        {
            for (int i = 0; i < qeueLenght; i++)
                _clients.Enqueue(new Client(this));
        }

        private int CalculatePrice(IReadOnlyList<Product> productList)
        {
            int check = 0;

            foreach (var product in productList)
                check += product.Price;

            return check;
        }
    }

    class Product
    {
        public Product(string type, int price)
        {
            Type = type;
            Price = price;
        }

        public string Type { get; private set; }
        public int Price { get; private set; }
    }

    class ProductCreator
    {
        private List<int> _priceList = new List<int>();
        private List<string> _productList = new List<string>();
        private int _minPrice = 5;
        private int _maxPrice = 70;

        public ProductCreator()
        {
            var tempList = Enum.GetValues(typeof(ProductsType));

            foreach (var product in tempList)
            {
                _productList.Add(product.ToString());
                _priceList.Add(RandomGenerator.GetRandomNumber(_minPrice, _maxPrice + 1));
            }
        }

        private enum ProductsType
        {
            Apple,
            Chips,
            Milk,
            Cheese,
            Sausage,
            Carrot,
            Onion,
            MineralWater,
            Beef,
            Chocolate,
            Candies,
        }

        public IReadOnlyList<string> ProductList => _productList;

        public Product Create(int productIndex) => new Product(ProductList[productIndex], _priceList[productIndex]);
    }

    static class RandomGenerator
    {
        private static Random s_random = new Random();

        public static int GetRandomNumber(int minValue, int maxValue) => s_random.Next(minValue, maxValue);

        public static int GetRandomNumber(int maxValue) => s_random.Next(maxValue);
    }
}
