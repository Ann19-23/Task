using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceURL.Models;

namespace ServiceURL.Controllers
{
    public class LinksController : Controller
    {
        private readonly URLContext _context;

        public LinksController(URLContext context)
        {
            _context = context;
        }

        // GET: Links
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public ActionResult Index()
        {
            List<Link> links = _context.Link.ToList();
            foreach (Link item in links)
            {
                item.ShortUrl = "https://" + HttpContext.Request.Host + "/" + item.ShortUrl;
            }

            return (View(links));
        }
        [HttpGet]
        public string GetLastValue()
        {
            Link link = _context.Link.Last();
            StringBuilder res = new StringBuilder($"<tr><td><div>{link.LongUrl}</div></td><td><div>{"https://" + HttpContext.Request.Host + "/" + link.ShortUrl}</div></td><td><div>{link.DateCreate}</div></td><td><div>{link.NumberOfTransitions}</div></td><td><a href='/Links/Edit/{ link.Id }'>Редактировать</a><br><a href='/Links/Delete/{ link.Id }'>Удалить</a></div></td ></tr>");
            return res.ToString();
        }
        

        [HttpPost]
        public string[] Create(string linkUrl)
        {
            string[] url;
            Regex regex;
            string pattern2 = @"(http:\/\/|https:\/\/)";
            regex = new Regex(pattern2); 
            if(!regex.IsMatch(linkUrl))
            {
                linkUrl = "http://" + linkUrl + "/";
            }  
            string pattern = @"(http:\/\/|https:\/\/|www.|\b).{2,}\.\w{2,}(\/|\b)";
            regex = new Regex(pattern);
            if (regex.IsMatch(linkUrl))
            {
                if (FindLongURL(linkUrl) == null)
                {

                    Link link = new Link();
                    link.LongUrl = linkUrl;
                    link.ShortUrl = GetShortURL();
                    link.DateCreate = DateTime.Now;
                    link.NumberOfTransitions = 0;
                    _context.Add(link);
                    _context.SaveChangesAsync();
                    url = new string[2] { "https://" + HttpContext.Request.Host + "/" + link.ShortUrl, "true" };
                    return url;

                }
                else
                {
                    string shortLink;
                    shortLink = _context.Link.Where(o => o.LongUrl == linkUrl).Select(p => p.ShortUrl).First();
                    url = new string[2] { "https://" + HttpContext.Request.Host + "/" + shortLink, "false" };
                    return url;
                }
            }
            else
            {
                url = new string[2] { "Неправильный формат ссылки", "false" };
                return url;
            }
           
           
        }

        public Link FindLongURL(string url)
        {
            Link link = _context.Link.Where(o=>o.LongUrl == url).FirstOrDefault();
            return link;
        }
        public Link FindShorturl (string url)
        {
            Link link = _context.Link.Where(o => o.ShortUrl == url).FirstOrDefault();
            return link;
        }
        public string GetShortURL()
        {
            string symbol = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            StringBuilder result = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < random.Next(3, 7); i++)
            {
                result.Append(symbol[random.Next(0,symbol.Length)]);
            }
            if(FindShorturl(result.ToString()) != null) { 
                GetShortURL();
            }
            return (result.ToString());
        }
        [ResponseCache(NoStore = true,Location = ResponseCacheLocation.None)]
        public void RedirectTo()
        {
            try
            {
                string errorPath;
                Regex regex = new Regex("^.*./");
                string shortUrl;
                errorPath = Request.Query.Keys.First();
                shortUrl = regex.Replace(errorPath, "");
                string longUrl = _context.Link.Where(o => o.ShortUrl == shortUrl).Select(p => p.LongUrl).FirstOrDefault();
                _context.Link.Where(o => o.ShortUrl == shortUrl).FirstOrDefault().NumberOfTransitions++;
                _context.SaveChanges();               
                Response.Redirect(longUrl, true);
            }
            catch
            {
                Response.Redirect("https://" + HttpContext.Request.Host, true);
            }
        }
       
        public  ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Link link = _context.Link.Find(id);
            if (link == null)
            {
                return View();
            }          
            return View(link);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Link link)
        {
            _context.Entry(link).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Link link = _context.Link.Find(id);
            if (link == null)
            {
                return View();
            }
            return View(link);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Link link = _context.Link.Find(id);
            _context.Link.Remove(link);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
