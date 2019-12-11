/*
FILE:           Order.cs
PROJECT:	    Assignment #3 - Wally's Revenge
PROGRAMMER:     Shaq Purcell
FIRST VERSION:  2019-12-06
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallysRevenge.Models
{
    /*
     * NAME:        Order
     * DESCIPTION:  Class repsentation of the row within the database, applies buisness rules to objects created.
     *              Validation occurs here.
     */
    class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int BranchID { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public double SPrice { get; set; }
    }
}
