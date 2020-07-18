using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    public class Orders
    {

        [DisplayName("Category Name")]
        public String CategoryName  {get; set;}

        [DataType(DataType.Currency)]
        [DisplayName("Total Price")]
        public decimal TotalPrice { get; set; }


        [DisplayName("Product Count")]
        public int ProductCount { get; set; }
    }
}