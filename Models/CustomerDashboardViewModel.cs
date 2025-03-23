using System;
using System.Collections.Generic;

namespace IT15_Trojan_B.Models
{
    public class CustomerDashboardViewModel
    {
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int UpcomingAppointments { get; set; }
        public decimal TotalPayments { get; set; }
        public List<MaterialViewModel> RequestedMaterials { get; set; }
        public List<SecurityLogViewModel> SecurityLogs { get; set; }
    }

    public class MaterialViewModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public class SecurityLogViewModel
    {
        public string UserName { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}