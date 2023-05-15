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
using System.Windows.Shapes;

namespace Learn
{
	/// <summary>
	/// Логика взаимодействия для CreateAppointment.xaml
	/// </summary>
	public partial class CreateAppointment : Window
	{
		LearnContext db;
		Service service;
		public CreateAppointment(object sender, LearnContext context)
		{
			InitializeComponent();
			service = (sender as StackPanel).DataContext as Service;
			db = context;

			ServiceNameTB.Text = service.Name;
			ServiceDurationTB.Text = service.Duration.ToString();
			ClientsCB.ItemsSource = (from c in db.Client
									 select c.Name).ToList();
			ClientsCB.SelectedIndex = 0;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (DatePick.SelectedDate != null)
			{
				TimeSpan time;
				if (TimeSpan.TryParse(TimeTB.Text, out time) && (time < TimeSpan.FromDays(1)))
				{
					Appointment appointment = new Appointment() { ClientId = (from c in db.Client where c.Name == (string)ClientsCB.SelectedItem select c.Id).ToList()[0], ServiceId = service.Id, Date = (DateTime)DatePick.SelectedDate, Time = time };
					db.Appointment.Add(appointment);
					db.SaveChanges();
					Close();
				}
				else
				{
					MessageBox.Show("Введите корректное время");
				}
			}
			else
			{
				MessageBox.Show("Выберите дату");
			}
		}
	}
}
