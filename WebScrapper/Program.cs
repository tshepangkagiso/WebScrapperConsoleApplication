using CsvHelper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace WebScrapper
{
    // To originze the data
    class Book
    {
        public string Title { get; set; }
        public string Price { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            List<string> booklinks = GetBookLinks(url: "https://books.toscrape.com/catalogue/category/books/food-and-drink_33/index.html");
            Console.WriteLine(format: "Found {0} links\n", arg0: booklinks.Count); // we are retrieving all book links in //h3/a xpath

            List<Book> books = GetBookDetails(urls: booklinks);
            int bookCount = 1;
            foreach (Book book in books)
            {
                Console.WriteLine("{0}) {1} : {2}\n",bookCount ,book.Title, book.Price);
                bookCount++;
                Thread.Sleep(1500);
            }
            /*exportToCSV(books);*/
            Console.ReadKey();

        }

        //Method to extract html file
        static HtmlDocument GetDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }

        //Method to turn the data from the elements in webpage into a list object(links)
        static List<string> GetBookLinks(string url)
        {
            List<string> bookLinks = new List<string>();
            HtmlDocument doc = GetDocument(url);
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes(xpath: "//h3/a");
            Uri baseUri = new Uri(uriString: url); 
            foreach (HtmlNode link in linkNodes)
            {
                string href = link.Attributes[name:"href"].Value;
                bookLinks.Add(item: new Uri(baseUri, relativeUri: href).AbsoluteUri);
            }

            return bookLinks;
        }

        // Method to get book details
        static List<Book> GetBookDetails(List<string> urls)
        {
            List<Book> books = new List<Book>();
            foreach (string url in urls)
            {
                HtmlDocument document = GetDocument(url);
                string titleXPath = "//h1";
                string priceXPath = "//div[contains(@class,\"product_main\")]/p[@class=\"price_color\"]";
                Book book = new Book();
                book.Title = document.DocumentNode.SelectSingleNode(xpath: titleXPath).InnerText;
                book.Price = document.DocumentNode.SelectSingleNode(xpath: priceXPath).InnerText;
                books.Add(item: book);
            }
            return books;
        }
        // create and load books to csv file
       /* static void exportToCSV(List<Book> books)
        {
            using (StreamWriter writer = new StreamWriter(path: "./Books.csv"))
            using (CsvWriter csv = new CsvWriter(writer, culture: CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records: books);
            }
        }*/



















        /*
            Tools used for web scrappering commonly in .Net
            HtmlAgilityPack: Downloads web pages directly or through browser.
            ScrapySharp: supports css selectors
            PuppeteerSharp: .net port for async use and promise based behavior like in js
            CsvHelper: for reading and writing csv files
        */

        public static void DedicatedProxy()
        {
            // Create a HttpClientHandler and configure the proxy
            HttpClientHandler handler = new HttpClientHandler
            {
                Proxy = new WebProxy("proxy.example.com", 8080), // Replace with your proxy server details
                UseProxy = true
            };

            // Create an HttpClient with the configured handler
            HttpClient client = new HttpClient(handler);

            // Make a request using the HttpClient
            HttpResponseMessage response = client.GetAsync("https://www.example.com").Result;

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine("Request failed with status code: " + response.StatusCode);
            }

            /*
             In this example, we create an HttpClientHandler and set the Proxy property to the proxy server's address and port. 
             We then set the UseProxy property to true to ensure the proxy is used for requests made by the HttpClient.

            You should replace "proxy.example.com" with the actual proxy server address and 8080 with the appropriate
            port number. Additionally, you can modify the URL in the GetAsync method to make requests to the desired endpoint.

            Remember to handle any exceptions that might occur during the request, such as 
            HttpRequestException or TaskCanceledException.
             */

        }

        public static void SharedProxy()
        {
            // Create a HttpClientHandler and configure the shared proxy
            HttpClientHandler handler = new HttpClientHandler
            {
                Proxy = new WebProxy("proxy.example.com", 8080), // Replace with your shared proxy server details
                UseProxy = true
            };

            // Create an HttpClient with the configured handler
            HttpClient client = new HttpClient(handler);

            // Make a request using the HttpClient
            HttpResponseMessage response = client.GetAsync("https://www.example.com").Result;

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine("Request failed with status code: " + response.StatusCode);
            }


            /*
                HideMy.name: They offer a limited number of free shared proxies that you can use. You can find more information 
                and get the proxy server addresses from their website: https://hidemy.name/en/proxy-list/

               FreeProxyList.net: This website provides a list of free proxy servers that you can use. You can access the 
               proxy server addresses and ports from their website: https://free-proxy-list.net/

               ProxyScrape: ProxyScrape provides a collection of free proxy lists that you can use. You can find the proxy 
               server addresses and ports on their website: https://proxyscrape.com/free-proxy-list
             */
        }
    }
}
