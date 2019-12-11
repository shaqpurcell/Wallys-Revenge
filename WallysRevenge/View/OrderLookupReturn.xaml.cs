/*
FILE:           OrderLookupReturn.xaml.cs
PROJECT:	    Assignment #3 - Wally's Revenge
PROGRAMMER:     Shaq Purcell
FIRST VERSION:  2019-12-06
*/
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WallysRevenge.DAL;
using WallysRevenge.Models;

namespace WallysRevenge.View
{
    /// <summary>
    /// Interaction logic for OrderLookupReturn.xaml
    /// </summary>
    public partial class OrderLookupReturn : Window
    {
        static SPWallyDB wallyDB = new SPWallyDB();
        List<Customer> customers = new List<Customer>();
        List<Order> orders = new List<Order>();
        List<Product> products = new List<Product>();
        List<Branch> branches = new List<Branch>();
        BindingList<string> custOrders = new BindingList<string>();
        BindingList<string> customerName = new BindingList<string>();

        public OrderLookupReturn()
        {
            InitializeComponent();
            customers = wallyDB.GetCustomers("");
            //orders = wallyDB.GetOrders("");
            products = wallyDB.GetProducts("all", 0);
            branches = wallyDB.GetBranches();
            UpdateView();
            cbxCustomer.ItemsSource = customerName;
            lbxOrders.ItemsSource = custOrders;
        }

        private void UpdateView()
        {

            customerName.Clear();
            custOrders.Clear();

            foreach (Customer c in customers)
            {
                customerName.Add(c.FirstName + " " + c.LastName);
            }

            foreach (Order o in orders)
            {
                custOrders.Add(o.OrderID.ToString() + " <<" + o.OrderDate + ">>");
            }

        }


        /*
        FUNCTION:		btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnLookUp_Click(object sender, RoutedEventArgs e)
        {
            customers = wallyDB.GetCustomers(tbxLookup.Text);
            UpdateView();
        }

        private void GenerateInvoice(Order order, Customer customer)
        {
            List<Orderline> orderlines = wallyDB.GetOrderlines(order.OrderID.ToString());
            double subtotal = 0;
            tblOrderSummary.Text = "";

            tblOrderSummary.Inlines.Add("Thank you for shopping at\n");
            tblOrderSummary.Inlines.Add("Wally's World " + branches[order.BranchID - 1].Name + "\n");
            tblOrderSummary.Inlines.Add("On " + order.OrderDate.ToString() + "," + customer.FirstName + " " + customer.LastName + "!\n\n");
            tblOrderSummary.Inlines.Add("Order ID: " + order.OrderID + "\n\n");

            foreach (Orderline o in orderlines)
            {
                Product tmpProduct = products.Find(x => x.ProductID.Equals(o.ProductID));

                tblOrderSummary.Inlines.Add(tmpProduct.Name + " (" + o.Amount.ToString() + "*" + (tmpProduct.WPrice * 1.4).ToString() + ") = $" + ((tmpProduct.WPrice * 1.4) * o.Amount).ToString() + "\n");
                subtotal = subtotal + (o.Amount * tmpProduct.WPrice);
            }

            tblOrderSummary.Inlines.Add("\n");
            tblOrderSummary.Inlines.Add("Subtotal = $" + subtotal + "\n");
            tblOrderSummary.Inlines.Add("HST (13%) = $" + (subtotal * 0.13) + "\n");
            tblOrderSummary.Inlines.Add("Sale Total = $" + (subtotal + (subtotal * 0.13)) + "\n\n");

            if (order.OrderStatus == "PAID")
            {
                tblOrderSummary.Inlines.Add("Paid - Thank you!");
            } else
            {
                tblOrderSummary.Inlines.Add("Returned - Shop Again!");
            }
        }

        /*
        FUNCTION:		btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */

        private void cbxCustomer_Selected(object sender, RoutedEventArgs e)
        {
            if (cbxCustomer.SelectedIndex != -1)
            {
                orders = wallyDB.GetOrders(customers[cbxCustomer.SelectedIndex].CustomerID.ToString());
                UpdateView();
            }
            
        }

        /*
        FUNCTION:		btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */

        private void lbxOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Order order = orders[lbxOrders.SelectedIndex];
            Customer customer = customers.Find(x => x.CustomerID.Equals(order.CustomerID));

            GenerateInvoice(order, customer);
        }

        /*
        FUNCTION:		btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnReturnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (lbxOrders.SelectedIndex != -1)
            {
                Order order = orders[lbxOrders.SelectedIndex];
                Customer customer = customers.Find(x => x.CustomerID.Equals(order.CustomerID));

                wallyDB.ReturnOrder(order);
                GenerateInvoice(order, customer);
            }
        }
    }
}
