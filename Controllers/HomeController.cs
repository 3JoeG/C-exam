using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Exam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext _context {get;set;}

          private User GetUserFromDB()
        {
            return _context.Users.FirstOrDefault(u=>u.UserId==HttpContext.Session.GetInt32("UserId"));
        }        

        public HomeController(HomeContext context)
        {
           _context=context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            User userInDb = GetUserFromDB();
            if(userInDb == null)
            {
                 return View();
                
            }
            return RedirectToAction("Dashboard");
            
        }

         [HttpPost("register")]
        public IActionResult NewUser(User reg)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.Email== reg.Email))
                {
                    ModelState.AddModelError("Email","That email is already taken. Please Log in.");
                    return View("Index");
                }               
                
                PasswordHasher<User> hasher=new PasswordHasher<User>();
                reg.Password=hasher.HashPassword(reg,reg.Password);
                
                _context.Users.Add(reg);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId",reg.UserId);              
                return RedirectToAction ("Dashboard");
        
            }
            else
            {
                return View("Index");
            }
        }


         [HttpPost("signin")]
        public IActionResult SignIn(Login check)
        {   
            if(ModelState.IsValid)
            {     
                User userInDb=_context.Users.FirstOrDefault( u=>u.Email==check.Email2);
                if(userInDb==null)
                {    
                    ModelState.AddModelError("Email2","Invalid Email/Password");
                    ModelState.AddModelError("Password2","Invalid Email/Password");
                    return RedirectToAction("Index");
                }    
                
                           
                    PasswordHasher<Login> hash=new PasswordHasher<Login>();
                    var result=hash.VerifyHashedPassword(check,userInDb.Password, check.Password2);
                    
                    if(result == 0)
                    {
                        ModelState.AddModelError("Email2","Invalid Email/Password");
                        ModelState.AddModelError("Password2","Invalid Email/Password");
                        return View("Index");
                    }
                    
                    HttpContext.Session.SetInt32("UserId",userInDb.UserId);
                    return Redirect("Dashboard");
            }
            return View("Index");
            
        }



   [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User userInDb = GetUserFromDB();
            if(userInDb == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.User=userInDb;
            
            List<Event> AllEvents=_context.Events
                                        .OrderBy(e=>e.Start)
                                        .Include(e=>e.Creator)
                                        .Include(e=>e.Attendees)
                                        .ThenInclude(a=>a.Attender)
                                        .ToList();
            return View(AllEvents);
        }

        [HttpGet("new")]
        public IActionResult New()
            {
                User userInDb = GetUserFromDB();
                if(userInDb == null)
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
        
        [HttpPost("create")]
        public IActionResult Create(Event info)
        {   
            GetUserFromDB();
            if(ModelState.IsValid)
            {
                User userInDb=GetUserFromDB();
                if(userInDb == null)
                {
                    return RedirectToAction("Logout");
                }
             
                info.UserId=userInDb.UserId;
                _context.Events.Add(info);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
                return View("New");
        }

        [HttpGet("join/{userId}/{eventId}")]
        public IActionResult Join(int userId, int eventId)
        {
            Attendee going= new Attendee();
            going.UserId=userId;
            going.EventId=eventId;
            _context.Attendees.Add(going);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("remove/{userId}/{eventId}")]
        public IActionResult Remove(int userId, int eventId)
        {
            Attendee cancel=_context.Attendees.FirstOrDefault(a=>a.UserId==userId &&
            a.EventId==eventId);
            _context.Attendees.Remove(cancel);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }


        [HttpGet("delete/{eventId}")]
        public IActionResult Delete(int eventId)
        {
            User userInDb = GetUserFromDB();
            Event canceled=_context.Events.FirstOrDefault(e=>e.EventId==eventId && e.Creator==userInDb);

            if(canceled !=null)
            {            
            _context.Events.Remove(canceled);
            _context.SaveChanges();
            
            }
            return RedirectToAction("Dashboard");
        }


        [HttpGet("/party/{eventId}")]
        public IActionResult Party(int eventId)
        {
            User userInDb=GetUserFromDB();
                if(userInDb == null)
                {
                    return RedirectToAction("Logout");
                }
            ViewBag.User=userInDb;    
            Event see=_context.Events
                                        .Include(e=>e.Attendees)
                                        .ThenInclude(a=>a.Attender)
                                        .Include(e=>e.Creator)
                                        .FirstOrDefault(e=>e.EventId==eventId);
                                            
                                            
            return View(see);
        }


        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
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
