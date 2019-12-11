using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
FILE:           Orderline.cs
PROJECT:	    Assignment #3 - Wally's Revenge
PROGRAMMER:     Shaq Purcell
FIRST VERSION:  2019-12-06
*/
using System.Threading.Tasks;

namespace WallysRevenge.Models
{
    /*
     * NAME:        Orderline
     * DESCIPTION:  Class repsentation of the row within the database, applies buisness rules to objects created.
     *              Validation occurs here.
     */
    class Orderline
    {
        public int OrderLineID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Amount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }

    }
}
