using System;
using System.Collections.Generic;
using System.Linq;
/*
FILE:           Product.cs
PROJECT:	    Assignment #3 - Wally's Revenge
PROGRAMMER:     Shaq Purcell
FIRST VERSION:  2019-12-06
*/

using System.Text;
using System.Threading.Tasks;

namespace WallysRevenge.Models
{
    /*
     * NAME:        Product
     * DESCIPTION:  Class repsentation of the row within the database, applies buisness rules to objects created.
     *              Validation occurs here.
     */
    class Product
    {
        public int ProductID {get; set;}
        public string Name { get; set; }
        public double WPrice { get; set; }

        public int Quantity { get; set; }
    }
}
