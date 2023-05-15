using Learn.Context;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Animation;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Learn.Models
{
    public partial class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }

        public virtual Client Client { get; set; }
        public virtual Service Service { get; set; }

        public string ServiceName
        {
            get
            {
                return Service.Name;
            }
        }

        public string ClientName
        {
            get
            {
                using (LearnContext db = new LearnContext())
                {
                    return db.Client.Find(ClientId).Name;
                }
            }
        }

        public string DateTime
        {
            get
            {
                return Date.ToString("d") + " " + Time.ToString();
            }
        }

        private TimeSpan timeLeft;

        public string TimeLeft
        {
            get
            {
                timeLeft = Date.Add(Time).Subtract(System.DateTime.Now);
				return string.Format("Осталось дней: {0}, часов: {1}, минут: {2}", timeLeft.Days, timeLeft.Hours, timeLeft.Minutes);
            }
        }

        public SolidColorBrush Background
        {
            get
            {
                return Date.Add(Time).AddHours(1) >= System.DateTime.Now ? Brushes.Red : Brushes.White;
            }
        }
    }
}
