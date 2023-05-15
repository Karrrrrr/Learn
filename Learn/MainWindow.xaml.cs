using Learn.Context;
using Learn.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Learn
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private LearnContext db = new LearnContext();
		private List<Service> services = new List<Service>();
		private bool adminMode = false;
		public MainWindow()
		{
			InitializeComponent();
			ServicesList.ItemsSource = db.Service.ToList();
			services = db.Service.ToList();
			PagesCountTB.Text = services.Count.ToString() + " из " + db.Service.ToList().Count.ToString();
		}

		private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
		{
			LoadServices();
		}

		internal void LoadServices()
		{
			if (IsLoaded)
			{
				services = db.Service.ToList();
				if (FilterCB.SelectedIndex == 1)
				{
					services = (from s in services
								where s.Discount < 5
								select s).ToList();
				}
				else if (FilterCB.SelectedIndex == 2)
				{
					services = (from s in services
								where s.Discount >= 5 && s.Discount < 15
								select s).ToList();
				}
				else if (FilterCB.SelectedIndex == 3)
				{
					services = (from s in services
								where s.Discount >= 15 && s.Discount < 30
								select s).ToList();
				}
				else if (FilterCB.SelectedIndex == 4)
				{
					services = (from s in services
								where s.Discount >= 30 && s.Discount < 70
								select s).ToList();
				}
				else if (FilterCB.SelectedIndex == 5)
				{
					services = (from s in services
								where s.Discount >= 70
								select s).ToList();
				}
				if (SearchTB.Text != "")
				{
					services = services.Where(s => s.Name.ToLower().Contains(SearchTB.Text.ToLower())).ToList();
				}
				if (SortCB.SelectedIndex == 1)
				{
					services = services.OrderBy(s => s.Price).ToList();
				}
				else if (SortCB.SelectedIndex == 2)
				{
					services = services.OrderByDescending(s => s.Price).ToList();
				}
				if (adminMode)
				{
					for (int i = 0; i < services.Count; i++)
					{
						services[i].EditDeleteShow = Visibility.Visible;
					}
				}
				else
				{
					for (int i = 0; i < services.Count; i++)
					{
						services[i].EditDeleteShow = Visibility.Hidden;
					}
				}
				ServicesList.ItemsSource = services;
				PagesCountTB.Text = services.Count.ToString() + " из " + db.Service.ToList().Count.ToString();
			}
		}

		private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			LoadServices();

		}

		private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			LoadServices();
		}

		private void AdminCodeTB_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (AdminCodeTB.Text == "0000")
			{
				AddServiceButton.Visibility = Visibility.Visible;
				AppointmentsButton.Visibility = Visibility.Visible;
				if (!adminMode)
				{
					adminMode = true;
					LoadServices();
				}
			}
			else
			{
				AddServiceButton.Visibility = Visibility.Hidden;
				AppointmentsButton.Visibility = Visibility.Hidden;
				if (adminMode)
				{
					adminMode = false;
					LoadServices();
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			CreateEditService ces = new CreateEditService(sender, db, this);
			ces.ShowDialog();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			db.Service.Remove((sender as Button).DataContext as Service);
			db.SaveChanges();
			LoadServices();
		}

		private void AddServiceButton_Click(object sender, RoutedEventArgs e)
		{
			CreateEditService ces = new CreateEditService(null, db, this);
			ces.ShowDialog();
		}

		private void AppointmentsButton_Click(object sender, RoutedEventArgs e)
		{
			AppointmentsWindow aw = new AppointmentsWindow(db);
			aw.ShowDialog();
		}

		private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (adminMode)
			{
				CreateAppointment ca = new CreateAppointment(sender, db);
				ca.ShowDialog();
			}
        }
    }
}
