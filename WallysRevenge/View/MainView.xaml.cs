using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WallysRevenge.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        /*
        FUNCTION:		btnAddCustomer_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Open the view to create a new customer
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerView addCustomer = new AddCustomerView();

            addCustomer.ShowDialog();

        }

        /*
        FUNCTION:		btnRawProduct_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Open up raw product view
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnRawProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductManipulationView productManipulation = new ProductManipulationView();

            productManipulation.ShowDialog();
        }

        /*
        FUNCTION:		btnCreateOrder_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Open view to create order
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            CreateOrder createOrder = new CreateOrder();

            createOrder.ShowDialog();

        }

        /*
        FUNCTION:		btnOrderLookUpReturn_Click(object sender, RoutedEventArgs e)
        DESCRIPTION:	Open the menu to return the order
        PARAMETERS:		object sender       - The object that is responsible for firing the event
                        RoutedEventArgs e   - The arguments to be manipulated due to the event
        RETURNS:		NONE
        DEV:            
        */
        private void btnOrderLookUpReturn_Click(object sender, RoutedEventArgs e)
        {
            OrderLookupReturn orderLookupReturn = new OrderLookupReturn();

            orderLookupReturn.ShowDialog();
        }
    }
}
