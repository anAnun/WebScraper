using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                var html = client.GetStringAsync("https://en.wikipedia.org/wiki/List_of_chords").Result;

                var parser = new HtmlParser();
                var document = parser.Parse(html);
                var tablerows = document.QuerySelectorAll("table.sortable tr").Skip(1);

                List<Pitches> results = new List<Pitches>();

                foreach (var tr in tablerows)
                {

                    var pitches = new Pitches();
                    
                    var aElement = tr.QuerySelector("td a");
                    pitches.Name = aElement.TextContent;

                    var fifthElement = tr.QuerySelector("td:nth-child(5)");
                    pitches.Pitch = fifthElement.TextContent;

                    var six = tr.QuerySelector("td:nth-child(6)");
                    pitches.Quality = six.TextContent;

                    results.Add(pitches);
                }
                Console.WriteLine(JsonConvert.SerializeObject(results, Formatting.Indented));

            }

        }
    }
}
