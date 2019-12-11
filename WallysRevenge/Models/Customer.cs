/*
FILE:           Customer.cs
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
     * NAME:        Customer
     * DESCIPTION:  Class repsentation of the row within the database, applies buisness rules to objects created.
     *              Validation occurs here.
     */
    class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
