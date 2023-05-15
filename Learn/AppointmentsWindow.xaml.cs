using Learn.Context;
using Learn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
	/// Логика взаимодействия для AppointmentsWindow.xaml
	/// </summary>
	public partial class AppointmentsWindow : Window
	{
		private LearnContext db;
		private List<Appointment> appointments = new List<Appointment>();
		public AppointmentsWindow(LearnContext context)
		{
			InitializeComponent();
			db = context;
			SelectAppointments();
			AppointmentsList.ItemsSource = appointments;
			Timer timer = new Timer(TimerTick, null, 30000, 30000);
		}

		private void SelectAppointments()
		{
			foreach (Appointment appointment in db.Appointment)
			{
				if ((appointment.Date == DateTime.Now.Subtract(DateTime.Now.TimeOfDay)) && (appointment.Date.Add(appointment.Time) > DateTime.Now) || (appointment.Date == DateTime.Now.AddDays(1).Subtract(DateTime.Now.TimeOfDay)))
				{
					appointments.Add(appointment);
				}
			}
			appointments = appointments.OrderBy(a => a.Date).ThenBy(a => a.Time).ToList();
		}

		private void TimerTick(object state)
		{
			SelectAppointments();
			AppointmentsList.ItemsSource = appointments;
		}
	}
}
