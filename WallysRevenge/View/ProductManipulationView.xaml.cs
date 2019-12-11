/*
FILE:           ProductManipulationView.xaml.cs
PROJECT:	    Assignment #3 - Wally's Revenge
PROGRAMMER:     Shaq Purcell
FIRST VERSION:  2019-12-06
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WallysRevenge.DAL;
using WallysRevenge.Models;

namespace WallysRevenge.View
{
    /// <summary>
    /// Interaction logic for ProductManipulationView.xaml
    /// </summary>
    public partial class ProductManipulationView : Window
    {
        static SPWallyDB wallyDB = new SPWallyDB();
        private List<Product> products = new List<Product>();
        private List<Branch> branches = wallyDB.GetBranches();
        private BindingList<Product> sysproducts = new BindingList<Product>(wallyDB.GetProducts("all", 0));
        public ProductManipulationView()
        {
            InitializeComponent();
            UpdateBranches();
            UpdateSystemProduct();

        }

        /*
        FUNCTION:		cbxBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void cbxBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            products = wallyDB.GetProducts("branch", cbxBranch.SelectedIndex + 1);
            var prodBind = new BindingList<Product>(products);
            lbxProducts.ItemsSource = prodBind;

        }

        private void UpdateBranches()
        {
            foreach (Branch b in branches)
            {
                cbxBranch.Items.Add(b.Name + " (" + b.Location + ")");
            }

        }

        private void UpdateSystemProduct()
        {
            foreach (Product p in sysproducts)
            {
                cbxProdSystem.Items.Add(p.Name);
            }
        }

        /*
        FUNCTION:		btnPurchaseProduct_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Validate text fields within the UI, create a customer object to be added to database
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnPurchaseProduct_Click(object sender, RoutedEventArgs e)
        {
            Product tmpProduct = new Product
            {
                ProductID = cbxProdSystem.SelectedIndex + 1,
                Quantity = Convert.ToInt32(iudAmount.Value)
            };

            wallyDB.BuyProductForBranch(cbxBranch.SelectedIndex + 1, tmpProduct);

            
            
        }
    }
}
