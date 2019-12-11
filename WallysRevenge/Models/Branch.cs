/*
FILE:           Branch.cs
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
     * NAME:        Branch
     * DESCIPTION:  Class repsentation of the row within the database, applies buisness rules to objects created.
     *              Validation occurs here.
     */
    class Branch
    {
        public int BranchID { get; set; }
        public string Name { get; set; }

        public string Location { get; set; }
    }
}
