using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Shape
    {
        public string ShapeName { get; set; }

        public float Length { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public float Radius { get; set; }

//        public override string ToString()
//        {
//            return "Name : " + ShapeName + " Length : " + Length + " Width : " + Width + " Height : " + Height +
//                   " Radius : " + Radius;
//        }
    }
}