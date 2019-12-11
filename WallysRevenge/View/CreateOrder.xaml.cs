/*
FILE:           CreateOrder.xaml.cs
PROJECT:	    Assignment #3 - Wally's Revenge
PROGRAMMER:     Shaq Purcell
FIRST VERSION:  2019-12-06
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using WallysRevenge.DAL;
using WallysRevenge.Models;

namespace WallysRevenge.View
{
    /// <summary>
    /// Interaction logic for CreateOrder.xaml
    /// </summary>
    public partial class CreateOrder : Window
    {
       
        static SPWallyDB wallyDB = new SPWallyDB();
        List<Customer> customers = new List<Customer>();
        List<Branch> branches = new List<Branch>();
        List<Product> products = new List<Product>();
        List<Order> orders = new List<Order>();

        List<Orderline> allOrderlines = new List<Orderline>();
        List<Orderline> custProdcuts = new List<Orderline>();

        BindingList<string> productBranch = new BindingList<string>();
        BindingList<string> branchName = new BindingList<string>();
        BindingList<string> customerName = new BindingList<string>();
        BindingList<string> shopList = new BindingList<string>();

        private int currentOrder;
        private int currentOrderline;
        
        public CreateOrder()
        {
            InitializeComponent();
            UpdateView();
            cbxCustomers.ItemsSource = customerName;
            cbxBranch.ItemsSource = branchName;
            lbxProductsBranch.ItemsSource = productBranch;
        }



        /*
        FUNCTION:		UpdateView()
        DESCRIPTION:	Update all of the base collection objects, does not clear and update objects
                        UI elements are bound too/
        PARAMETERS:		NONE
        RETURNS:		NONE
        DEV:            
        */
        private void UpdateView()
        {
            customers = wallyDB.GetCustomers("");
            branches = wallyDB.GetBranches();
            orders = wallyDB.GetOrders("");
            allOrderlines = wallyDB.GetOrderlines("");
            
            currentOrder = orders.Count();
            currentOrderline = allOrderlines.Count();


            UpdateBindings();
        }



        /*
        FUNCTION:		UpdateBindings()
        DESCRIPTION:	Update all of the base collection objects, does not clear and update objects
                        UI elements are bound too/
        PARAMETERS:		NONE
        RETURNS:		NONE
        DEV:            
        */
        private void UpdateBindings()
        {
            branchName.Clear();
            customerName.Clear();

            foreach (Customer c in customers)
            {
                customerName.Add(c.FirstName + " " + c.LastName);
            }

            foreach (Branch b in branches)
            {
                branchName.Add(b.Name + " (" + b.Location + ")");
            }

            cbxCustomers.ItemsSource = customerName;
            cbxBranch.ItemsSource = branchName;

        }




        /*
        FUNCTION:		btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerView customerView = new AddCustomerView();
            customerView.Closing += OnChangeUpdate;

            customerView.ShowDialog();

            
        }



        /*
        FUNCTION:		btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void OnChangeUpdate(object sender, CancelEventArgs e)
        {
            customers = wallyDB.GetCustomers("");
            UpdateBindings();
        }




        /*
        FUNCTION:		cbxBranch_SelectionChanged(object sender, EventArgs e)
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void cbxBranch_SelectionChanged(object sender, EventArgs e)
        {
            updateProducts();
            shopList.Clear();
            custProdcuts.Clear();
            currentOrderline = allOrderlines.Count() + 1;

        }



        /*
        FUNCTION:		updateProducts()
        DESCRIPTION:	Update the products within the producut list
        PARAMETERS:		NONE
        RETURNS:		NONE
        DEV:            
        */
        private void updateProducts()
        {
            productBranch.Clear();
            products = wallyDB.GetProducts("branch", cbxBranch.SelectedIndex + 1);

            foreach (Product p in products)
            {
                productBranch.Add(p.Name + " (" + p.Quantity.ToString() + ")");
            }
        }



        /*
        FUNCTION:		btnAdd_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Create the orderlines being added to create the order
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lbxProductsBranch.SelectedIndex != -1)
            {
                if (products[lbxProductsBranch.SelectedIndex].Quantity >= iudAdd.Value)
                {

                    Orderline tmpOL = new Orderline
                    {
                        OrderID = currentOrder,
                        OrderLineID = currentOrderline + 1 ,
                        ProductID = products[lbxProductsBranch.SelectedIndex].ProductID,
                        Amount = (int)iudAdd.Value
                    };

                    custProdcuts.Add(tmpOL);
                    currentOrderline++;
                    products[lbxProductsBranch.SelectedIndex].Quantity = products[lbxProductsBranch.SelectedIndex].Quantity - (int)iudAdd.Value;
                    shopList.Add(products[lbxProductsBranch.SelectedIndex].Name + " (" + tmpOL.Amount + ")");
                    lbxCustomerOrder.ItemsSource = shopList;

                    iudAdd.Value = 0;
                    productBranch.Clear();
                    foreach (Product p in products)
                    {
                        productBranch.Add(p.Name + " (" + p.Quantity.ToString() + ")");
                    }


                }
            }

        }



        /*
        FUNCTION:		btnSubmitOrder_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Validate text fields within the UI, Creates an order object and orderlines to be
                        used in the creation of a new order
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnSubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            if (custProdcuts.Count != 0 && cbxCustomers.SelectedIndex != -1)
            {
                double calcSPrice = 0;
                foreach (Orderline o in custProdcuts)
                {
                    Product tmpProd = products.Find(x => x.ProductID.Equals(o.ProductID));

                    calcSPrice = calcSPrice + (o.Amount * tmpProd.WPrice);
                }

                calcSPrice = calcSPrice * 1.4;

                Order custFinalOrder = new Order
                {
                    OrderID = currentOrder +1,
                    BranchID = cbxBranch.SelectedIndex + 1,
                    CustomerID = cbxCustomers.SelectedIndex + 1,
                    OrderStatus = "PAID",
                    OrderDate = DateTime.Now,
                    ReturnDate = new DateTime(1, 1, 1),
                    SPrice = calcSPrice
                };

                wallyDB.AddOrder(custFinalOrder);

                foreach (Orderline o in custProdcuts)
                {
                    o.OrderID = custFinalOrder.OrderID;
                    wallyDB.AddOrderLines(o);
                    wallyDB.RemoveProductQuantity(custFinalOrder, o);
                }

                orders = wallyDB.GetOrders("");
                currentOrder = orders.Count();
                custProdcuts.Clear();
                shopList.Clear();
            }
        }

        /*
        FUNCTION:		btnSearch_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Search the database given the textbox restrains.
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            customers = wallyDB.GetCustomers(tbxSearch.Text);
            UpdateBindings();
            cbxCustomers.ItemsSource = customerName;
        }
    }
}
