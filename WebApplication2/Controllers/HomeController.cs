using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return Redirect("web/index.html");
        }

        /// <summary>
        /// Process query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        //GET : Home/Process
        public string Process([FromUri] string query)
        {
            if (query.IsNullOrWhiteSpace())
            {
                return "No Query Found";
            }

            try
            {
                var error = "";

                var shape = ProcessQuery(query);

                if (!ValidateShape(shape.ShapeName))
                {
                    error = "Shape Not Supported " + shape.ShapeName;
                }

                if (!ValidateDimensions(shape))
                {
                    error += "| Shape Dimenstions Not Supported " + shape.ShapeName;
                }

                return string.IsNullOrEmpty(error)
                    ? JsonConvert.SerializeObject(shape)
                    : JsonConvert.SerializeObject(error);
            }
            catch (ValidationException ex)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }


        /// <summary>
        /// Process query to create Shape object
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static Shape ProcessQuery(string query)
        {
            query = query.ToLower();
            //var query = "Draw an isosceles triangle with a height of 200 and a width of 100 and a length of 300";
            var shape = new Shape();
            var keywordList = new string[] {"draw", "a", "an", "with", "of", "and", "side"}.ToList();
            var shapeWordList = query.Split(' ').ToList();
            var result = shapeWordList.Except(keywordList).ToList();


            shape.ShapeName = result[0];

            for (var i = 0; i < result.Count; i++)
            {
                if (result[i].ToLowerInvariant().Equals("radius"))
                {
                    shape.Radius = float.Parse(result[++i]);
                }

                if (result[i].ToLowerInvariant().Equals("length"))
                {
                    shape.Length = float.Parse(result[++i]);
                }

                if (result[i].ToLowerInvariant().Equals("height"))
                {
                    shape.Height = float.Parse(result[++i]);
                }

                if (result[i].ToLowerInvariant().Equals("width"))
                {
                    shape.Width = float.Parse(result[++i]);
                }
            }

            return shape;
        }

        /// <summary>
        /// Validate shape type
        /// </summary>
        /// <param name="shapeName"></param>
        /// <returns></returns>
        private static bool ValidateShape(string shapeName)
        {
            var keywordList = new[]
            {
                "circle", "isosceles", "square", "scalene", "parallellogram", "equilateral",
                "pentagon", "rectangle", "hexagon", "heptagon", "octagon", "circle", "oval"
            }.ToList();
            return keywordList.Contains(shapeName);
        }

        /// <summary>
        /// Validate dimensions
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        private static bool ValidateDimensions(Shape shape)
        {
            var result = false;
            switch (shape.ShapeName)
            {
                case "circle":
                case "oval":
                    if (shape.Radius > 0)
                    {
                        result = true;
                    }

                    break;
                case "square":
                case "octagon":
                case "heptagon":
                case "pentagon":
                case "hexagon":
                case "equilateral":
                    if (shape.Length > 0)
                    {
                        result = true;
                    }

                    break;
                case "rectangle":
                case "parallellogram":
                
                case "isosceles":
                    if (shape.Width > 0 && shape.Height > 0)
                    {
                        result = true;
                    }

                    break;
                case "scalene":
                    if (shape.Length > 0 && shape.Height > 0 && shape.Width > 0)
                    {
                        result = true;
                    }

                    break;
            }

            return result;
        }
    }
}