/*
FILE:           SPWallyDB.cs
PROJECT:	    Assignment #3 - Wally's Revenge
PROGRAMMER:     Shaq Purcell
FIRST VERSION:  2019-12-06
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using WallysRevenge.Models;
using MySql.Data.MySqlClient;

namespace WallysRevenge.DAL
{
    /*
     * NAME:        SPWallyDB
     * DESCIPTION:  This class is responsible for handling all connections to the DB that is connected to the frontend of
     *              the Wally's Revenge application.
     *              
     */
    class SPWallyDB
    {


        /*
        FUNCTION:		List<Customer> GetCustomers(string search)
        DESCRIPTION:	Query the database for the amount of customers that are currently within the database.
                        Search is dependant on the last name or the phonenumber of the customer
        PARAMETERS:		string search - The value to search the customer table by
        RETURNS:		customerList  - A list of customers that fill the query
        DEV:            
        */
        public List<Customer> GetCustomers(string search)
        {
            //The command that is sent to the database
            const string sqlStatement = @"  SELECT customerID, firstName, lastName, phoneNumber FROM customers
	                                        WHERE (lastName = @Search
                                            OR phoneNumber = @Search
                                            OR @Search = '')
	                                        ORDER BY customerID ASC; ";

            //Open the connection to the database 
            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter
                {
                    SelectCommand = command
                };

                //Insert custom parameters into the search string
                command.Parameters.AddWithValue("@Search", search);

                DataTable table = new DataTable();

                //Fill data table with query information
                adapter.Fill(table);

                List<Customer> customerList = DataTableToCustomerList(table);
                return customerList;
            }

        }



        /*
        FUNCTION:		List<Orderline> GetOrderlines(string orderID)
        DESCRIPTION:	Query the database for the amount of orderlines that are currently within the database.
                        Search is dependant on the ID of the order given
        PARAMETERS:		string orderID - The value to search the orderLines table by
        RETURNS:		orderlineList  - A list of orderlines that fill the query
        DEV:            
        */
        public List<Orderline> GetOrderlines(string orderID)
        {
            const string sqlStatement = @"  SELECT orderLineID, orderID, productID, amount, orderStatus, orderDate, returnDate
                                            FROM orderline
                                            WHERE (orderID = @OrderID
                                            OR @OrderID = '')
	                                        ORDER BY orderLineID ASC;";

            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter
                {
                    SelectCommand = command
                };

                //Insert custom parameters into the search string
                command.Parameters.AddWithValue("@OrderID", orderID);

                DataTable table = new DataTable();

                //Fill data table with query information
                adapter.Fill(table);

                List<Orderline> orderlineList = DataTableToOrderlineList(table);

                return orderlineList;

            }
        }

        /*
        FUNCTION:		List<Order> GetOrders(string customerID)
        DESCRIPTION:	Query the database for the amount of orders that are currently within the database.
                        Search is dependant on the ID of the customer given, if not given the whole table is returned.
        PARAMETERS:		string customerID - The value to search the orderLines table by
        RETURNS:		orderList  - A list of orderlines that fill the query
        DEV:            An Overloaded function would properly handle the situation where the user enters no input,
                        in this solution the SQL handles this case instead of the backend... which is better?
        */
        public List<Order> GetOrders(string customerID)
        {
            //Command to be exectuted on the database
            const string sqlStatement = @"  SELECT orderID, customerID, branchID
                                            FROM orders
                                            WHERE (customerID = @CustomerID
                                            OR @CustomerID = '')
                                            ORDER BY orderID ASC;";

            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter
                {
                    SelectCommand = command
                };

                //Insert custom parameters into the search string
                command.Parameters.AddWithValue("@CustomerID", customerID);

                DataTable table = new DataTable();

                //Adapter fills the datatable with queried information
                adapter.Fill(table);

                List<Order> orderList = DataTableToOrderList(table);
                return orderList;
            }

        }


        /*
        FUNCTION:		List<Branch> GetBranches()
        DESCRIPTION:	Query the database for rows of branches that are currently within the database.
        PARAMETERS:		NONE
        RETURNS:		branchList  - A list of orderlines that fill the query
        DEV:            
        */
        public List<Branch> GetBranches()
        {
            //Command to be exectuted on the database
            const string sqlStatement = @"  SELECT branchID, location, `name` 
                                            FROM branches
                                            ORDER BY branchID ASC;";

            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter
                {
                    SelectCommand = command
                };

                DataTable table = new DataTable();

                //Adapter fills the datatable with queried information
                adapter.Fill(table);

                List<Branch> branchList = DataTableToBranchList(table);
                return branchList;
            }
        }


        /*
        FUNCTION:		List<Orderline> DataTableToOrderlineList(DataTable table)
        DESCRIPTION:	Converts the information from the database into a list of objects of the same
                        type defined by the database.
        PARAMETERS:		DataTable table - Table containing information for the SQL database
        RETURNS:		orderlines  - A list of orderlines produced by parsing the table given
        DEV:            
        */
        private List<Orderline> DataTableToOrderlineList(DataTable table)
        {
            List<Orderline> orderlines = new List<Orderline>();

            //For every datarow within the table convert that respective column into
            //an attribute for the type of object defined by the database table.
            foreach (DataRow r in table.Rows)
            {
                orderlines.Add(new Orderline
                {
                    OrderLineID = Convert.ToInt32(r["orderLineID"]),
                    OrderID = Convert.ToInt32(r["orderID"]),
                    ProductID = Convert.ToInt32(r["productID"]),
                    Amount = Convert.ToInt32(r["amount"]),
                    OrderStatus = Convert.ToString(r["orderStatus"]),
                    ReturnDate = Convert.ToDateTime(r["returnDate"]),
                    OrderDate = Convert.ToDateTime(r["orderDate"])
                });
            }

            return orderlines;

        }



        /*
        FUNCTION:		List<Branch> DataTableToBranchList(DataTable table)
        DESCRIPTION:	Converts the information from the database into a list of objects of the same
                        type defined by the database.
        PARAMETERS:		DataTable table - Table containing information for the SQL database
        RETURNS:		branches  - A list of branches produced by parsing the table given
        DEV:            
        */
        private List<Branch> DataTableToBranchList(DataTable table) {
            List<Branch> branches = new List<Branch>();


            //For every datarow within the table convert that respective column into
            //an attribute for the type of object defined by the database table.
            foreach (DataRow r in table.Rows)
            {
                branches.Add(new Branch
                {
                    BranchID = Convert.ToInt32(r["branchID"]),
                    Location = Convert.ToString(r["location"]),
                    Name = Convert.ToString(r["name"])
                });
            }
            return branches;

        }



        /*
        FUNCTION:		List<Order> DataTableToOrderList(DataTable table)
        DESCRIPTION:	Converts the information from the database into a list of objects of the same
                        type defined by the database.
        PARAMETERS:		DataTable table - Table containing information for the SQL database
        RETURNS:		orders  - A list of orders produced by parsing the table given
        DEV:            
        */
        private List<Order> DataTableToOrderList(DataTable table)
        {
            List<Order> orders = new List<Order>();


            //For every datarow within the table convert that respective column into
            //an attribute for the type of object defined by the database table.
            foreach (DataRow r in table.Rows)
            {
                orders.Add(new Order
                {
                    OrderID = Convert.ToInt32(r["orderID"]),
                    CustomerID = Convert.ToInt32(r["customerID"]),
                    BranchID = Convert.ToInt32(r["branchID"]),
                });
            }

            return orders;
        }


        /*
        FUNCTION:		List<Product> GetProducts(string modifyer, int branchID)
        DESCRIPTION:	Query the database for the amount of orders that are currently within the database.
                        Search is dependant on the ID of the branch given, if not given the whole table is returned.
        PARAMETERS:		string modifyer - How to search the database
                        int branchID - the branch number to search the database by
        RETURNS:		productList  - A list of orderlines that fill the query
        DEV:            An Overloaded function would properly handle the situation where the user enters no input,
                        in this solution the SQL handles this case instead of the backend... which is better?
                        The search variablity in this situation is handled by the code behind instead of the database
        */
        public List<Product> GetProducts(string modifyer, int branchID)
        {
            List<string> mods = new List<string>() { "branch", "all" };
            string sqlStatement = "";
            List<Product> productList;


            //Searching through all of the all the products is a different task then getting all of the products
            //At a given branch
            if (modifyer == mods[0])
            {
                sqlStatement = @"    SELECT products.productID, products.`name`, products.wPrice, branchproducts.quantity
                                    FROM products
                                    INNER JOIN branchproducts
                                    ON products.productID = branchproducts.productID
                                    WHERE branchID = @BranchID;";
            }

            if (modifyer == mods[1])
            {
                sqlStatement = @"   SELECT productID, `name`, wPrice
                                    FROM products
                                    ORDER BY productID ASC;";
            }

            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter
                {
                    SelectCommand = command
                };

                if (modifyer == mods[0])
                {
                    //Custom parameter when searching by the specific branchID
                    command.Parameters.AddWithValue("@BranchID", branchID);
                }


                DataTable table = new DataTable();

                adapter.Fill(table);

                //How to create the product list dependant on what the program needs
                if (modifyer == mods[1])
                {
                    productList = DataTableToProductList(table, 1);
                } else
                {
                    productList = DataTableToProductList(table, 0);
                }

                return productList;
            }


        }


        /*
        FUNCTION:		List<Product> DataTableToProductList(DataTable table, int type)
        DESCRIPTION:	Create a list of product objects from a database class
        PARAMETERS:		DataTable table - How to search the database
                        int type - the branch number to search the database by
        RETURNS:		products  - A list of product objects produced by the table given
        DEV:           
        */
        private List<Product> DataTableToProductList(DataTable table, int type)
        {
            List<Product> products = new List<Product>();

            //Depending on if we are searching all the products that are available 1,
            //Or if 0 branchproducts
            if (type == 0)
            {
                foreach (DataRow r in table.Rows)
                {
                    products.Add(new Product
                    {
                        ProductID = Convert.ToInt32(r["productID"]),
                        Name = Convert.ToString(r["name"]),
                        WPrice = Convert.ToDouble(r["wPrice"]),
                        Quantity = Convert.ToInt32(r["quantity"])
                    });
                }

            } else
            {
                foreach (DataRow r in table.Rows)
                {
                    products.Add(new Product
                    {
                        ProductID = Convert.ToInt32(r["productID"]),
                        Name = Convert.ToString(r["name"]),
                        WPrice = Convert.ToDouble(r["wPrice"]),
                    });
                }
            }

            return products;
        }





        /*
        FUNCTION:		void AddOrder(Order order)
        DESCRIPTION:	Creates a new row within the order table of the database
        PARAMETERS:		Order order - an object repsenting the row to be updated in the database
        RETURNS:		NONE
        DEV:           
        */
        public void AddOrder(Order order)
        {
            //SQL command to be executed on the database
            const string sqlStatement = @"  INSERT INTO orders (customerID, branchID)
                                            VALUE (@OrderID, @CustomerID, @BranchID);";

            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                //Insert custom parameters into the search string
                command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                command.Parameters.AddWithValue("@BranchID", order.BranchID);

                //Open connection to database, adapter would normally fill this role but since no
                //return data we need open the execute the command...
                connection.Open();
                command.ExecuteNonQuery();

            }
        }




        /*
        FUNCTION:		UpdateProductQuantity(Order order, Orderline orderline)
        DESCRIPTION:	Updates the amount of products within given store with amount specified in the orderline
        PARAMETERS:		Order order - an object repsenting the row to be updated in the database
                        Orderline orderline - The orderline within an order
        RETURNS:		NONE
        DEV:           
        */
        private void UpdateProductQuantity(Order order, Orderline orderline)
        {
            //SQL command to be executed on the database
            const string sqlStatement = @"  UPDATE branchproducts
                                            SET quantity = quantity + @Quantity
                                            WHERE branchID = @BranchID AND productID = @ProductID;";


            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);


                //Insert custom parameters into the search string
                command.Parameters.AddWithValue("@BranchID", order.BranchID);
                command.Parameters.AddWithValue("@ProductID", orderline.ProductID);
                command.Parameters.AddWithValue("@Quantity", orderline.Amount);

                //Open connection to database, adapter would normally fill this role but since no
                //return data we need open the execute the command...
                connection.Open();
                command.ExecuteNonQuery();


            }
        }

        /*
        FUNCTION:		UpdateProductQuantity(Order order, Orderline orderline)
        DESCRIPTION:	Updates the amount of products within given store with amount specified in the orderline
        PARAMETERS:		Order order - an object repsenting the row to be updated in the database
                        Orderline orderline - The orderline within an order
        RETURNS:		NONE
        DEV:           
        */
        public void RemoveProductQuantity(Order order, Orderline orderline)
        {
            //SQL command to be executed on the database
            const string sqlStatement = @"  UPDATE branchproducts
                                            SET quantity = quantity - @Quantity
                                            WHERE branchID = @BranchID AND productID = @ProductID;";


            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);


                //Insert custom parameters into the search string
                command.Parameters.AddWithValue("@BranchID", order.BranchID);
                command.Parameters.AddWithValue("@ProductID", orderline.ProductID);
                command.Parameters.AddWithValue("@Quantity", orderline.Amount);

                //Open connection to database, adapter would normally fill this role but since no
                //return data we need open the execute the command...
                connection.Open();
                command.ExecuteNonQuery();


            }
        }


        /*
        FUNCTION:		UpdateProductQuantity(Order order, Orderline orderline)
        DESCRIPTION:	Updates the amount of products within given store with amount specified in the orderline
        PARAMETERS:	    Orderline orderline - The orderline within an order
        RETURNS:		NONE
        DEV:           
        */
        public void AddOrderLine(Orderline orderline)
        {
            const string sqlStatement = @"  INSERT INTO orderline (orderLineID, orderID, productID, amount, orderStatus, orderDate, returnDate)
                                            VALUE (@OrderLineID, @OrderID, @ProductID, @Amount, @OrderStatus, @OrderDate, @ReturnDate);";

            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                //Insert custom parameters into the search string
                command.Parameters.AddWithValue("@OrderID", orderline.OrderID);
                command.Parameters.AddWithValue("@ProductID", orderline.ProductID);
                command.Parameters.AddWithValue("@Amount", orderline.Amount);
                command.Parameters.AddWithValue("@OrderStatus", orderline.Amount);
                command.Parameters.AddWithValue("@OrderDate", orderline.Amount);
                command.Parameters.AddWithValue("@ReturnDate", orderline.Amount);

                //Open connection to database, adapter would normally fill this role but since no
                //return data we need open the execute the command...
                connection.Open();
                command.ExecuteNonQuery();

            }
        }


        /*
        FUNCTION:		AddCustomer(Customer customer)
        DESCRIPTION:	Adds a new customer to the SQL database
        PARAMETERS:	    Customer customer - The customer object that is going to be converted into a row
        RETURNS:		NONE
        DEV:           
        */
        public void AddCustomer(Customer customer)
        {
            //SQL statement to insert a new customer into the customers table
            const string sqlStatement = @"  INSERT INTO customers (firstName, lastName, phoneNumber)
                                            VALUE (@FirstName, @LastName, @PhoneNumber);";

            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                //Insert custom parameters into the search string

                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@LastName", customer.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);

                //Open connection to database, adapter would normally fill this role but since no
                //return data we need open the execute the command...
                connection.Open();
                command.ExecuteNonQuery();

            }
        }

        /*
        FUNCTION:		BuyProductForBranch(int branchID, Product product)
        DESCRIPTION:	Add product to the branchprodcuts tree, add new products to given branch
        PARAMETERS:	    Product product - The product object that is to be added to the branches products
                        int branchID    - The branch to add the product to
        RETURNS:		NONE
        DEV:           
        */
        public void BuyProductForBranch(int branchID, Product product)
        {
            bool addOrInc = false;
            string sqlStatement = "";

            List<Product> products = GetProducts("branch", branchID);

            //If the product is in the branch then increase the given amount instead of adding
            foreach(Product p in products)
            {
                if (product.ProductID == p.ProductID)
                {
                    addOrInc = true;
                    break;
                }
            }
            
            //Increase
            if (addOrInc == true )
            {
                            sqlStatement = @"  UPDATE branchproducts
                                               SET quantity = quantity + @Quantity
                                               WHERE branchID = @BranchID AND productID = @ProductID;";
            } else  //Add
            {
                            sqlStatement = @"  INSERT INTO branchproducts (branchID, productID, quantity) 
                                               VALUE (@BranchID, @ProductID, @Quantity);";
            }

            using (var connection = new MySqlConnection(Properties.Settings.Default.connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                //Insert custom parameters into the search string
                command.Parameters.AddWithValue("@BranchID", branchID);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                
                //Open connection to database, adapter would normally fill this role but since no
                //return data we need open the execute the command...
                connection.Open();
                command.ExecuteNonQuery();

            }


        }



        /*
        FUNCTION:		List<Customer> DataTableToCustomerList(DataTable table)
        DESCRIPTION:	Produce a list of customer objects resembling the rows that are within the datatable
        PARAMETERS:	    Datatable table - The product object that is to be added to the branches products
        RETURNS:		customers - A list of customer objects
        DEV:           
        */
        private List<Customer> DataTableToCustomerList(DataTable table)
        {
            List<Customer> customers = new List<Customer>();

            foreach (DataRow r in table.Rows)
            {
                customers.Add(new Customer
                {
                    CustomerID = Convert.ToInt32(r["customerID"]),
                    FirstName = Convert.ToString(r["firstName"]),
                    LastName = Convert.ToString(r["lastName"]),
                    PhoneNumber = Convert.ToString(r["phoneNumber"]),
                }); ;
            }

            return customers;
        }
    }
}
