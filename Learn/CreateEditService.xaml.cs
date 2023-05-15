using Learn.Context;
using Learn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace Learn
{
	/// <summary>
	/// Логика взаимодействия для CreateEditService.xaml
	/// </summary>
	public partial class CreateEditService : Window
	{
		private LearnContext db;
		private Service service;
		private MainWindow Window;
		public CreateEditService(object sender, LearnContext context, MainWindow window)
		{
			InitializeComponent();
			db = context;
			Window = window;
			if (sender == null)
			{
				Title = "Создание услуги";
				IDLabel.Visibility = Visibility.Hidden;
				IDTB.Visibility = Visibility.Hidden;
			}
			else
			{
				Title = "Изменение услуги";
				IDLabel.Visibility = Visibility.Visible;
				IDTB.Visibility = Visibility.Visible;

				service = (sender as Button).DataContext as Service;
				IDTB.Text = service.Id.ToString();
				NameTB.Text = service.Name;
				PriceTB.Text = service.Price.ToString();
				DurationTB.Text = service.Duration.ToString();
				DescriptionTB.Text = service.Description;
				DiscountTB.Text = service.Discount.ToString();
				ImageTB.Text = service.Image;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			decimal price;
			int duration;
			int discount = 0;
			if (NameTB.Text != "")
			{
				if (decimal.TryParse(PriceTB.Text, out price) && (price > 0))
				{
					if (int.TryParse(DurationTB.Text, out duration) && (duration > 0) && (duration <= 240))
					{
						if ((DiscountTB.Text == "") || int.TryParse(DiscountTB.Text, out discount))
						{
							if ((from s in db.Service where s.Name == NameTB.Text select s).ToList().Count == 0)
							{
								if (service == null)
								{
									Service service = new Service() { Name = NameTB.Text, Description = DescriptionTB.Text, Price = price, Duration = duration, Image = ImageTB.Text };
									if (DiscountTB.Text != "")
									{
										service.Discount = discount;
									}
									db.Service.Add(service);
									db.SaveChanges();
								}
								else
								{
									service.Name = NameTB.Text;
									service.Description = DescriptionTB.Text;
									service.Price = price;
									if (DiscountTB.Text != "")
									{
										service.Discount = discount;
									}
									else
									{
										service.Discount = null;
									}
									service.Duration = duration;
									service.Image = ImageTB.Text;
									db.SaveChanges();
								}
								Window.LoadServices();
								Close();
							}
							else
							{
								MessageBox.Show("Такая услуга уже существует");
							}
						}
						else
						{
							MessageBox.Show("Скидка должна быть положительным числом или равна 0");
						}
					}
					else
					{
						MessageBox.Show("Длительность должна быть числом и не превышать 4 часа");
					}
				}
				else
				{
					MessageBox.Show("Цена должна быть положительным числом");
				}
			}
			else
			{
				MessageBox.Show("Заполните все поля");
			}
		}
	}
}
