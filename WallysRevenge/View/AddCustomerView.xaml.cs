/*
FILE:           AddCustomerView.xaml.cs
PROJECT:	    Assignment #3 - Wally's Revenge
PROGRAMMER:     Shaq Purcell
FIRST VERSION:  2019-12-06
*/

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WallysRevenge.DAL;
using WallysRevenge.Models;

namespace WallysRevenge.View
{
    /// <summary>
    /// Interaction logic for AddCustomerView.xaml
    /// </summary>
    public partial class AddCustomerView : Window
    {
        static SPWallyDB wallyDB = new SPWallyDB();
        private List<Customer> customers = wallyDB.GetCustomers("");

        /*
        FUNCTION:		AddCustomerView()
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        public AddCustomerView()
        {
            InitializeComponent();
            lblCustomerID.Content = customers.Count() + 1;

        }

        /*
        FUNCTION:		btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnCreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            //Check see that empty values are not being submitted
            //Regex validation for phone numbers/name occurs here
            if (tbxFirstName.Text != "" && tbxLastName.Text != "" && tbxPhoneNumber.Text != "")
            {

                Customer tmpCustomer = new Customer
                {
                    CustomerID = customers.Count() + 1,
                    FirstName = tbxFirstName.Text,
                    LastName = tbxLastName.Text,
                    PhoneNumber = tbxPhoneNumber.Text,
                };

                wallyDB.AddCustomer(tmpCustomer);

                MessageBoxResult result = MessageBox.Show("Customer succesfully added!",
                                              "Confirmation",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                }
            } else
            {
                MessageBoxResult result = MessageBox.Show("Invalid data, customer not added!",
                              "Confirmation",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }


        }
    }
}
