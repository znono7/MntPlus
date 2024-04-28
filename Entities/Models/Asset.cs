using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Asset
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Location { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Manufacturer { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public string? AssetTag { get; set; }
        public string? PurchaseOrder { get; set; }
        public string? PurchaseDate { get; set; }
        public string? PurchaseCost { get; set; }
        public string? Warranty { get; set; }
        public string? WarrantyExpiration { get; set; }
        public string? MaintenanceSchedule { get; set; }
        public string? MaintenanceLast { get; set; }
        public string? MaintenanceNext { get; set; }
        public string? MaintenanceCost { get; set; }
        public string? MaintenanceVendor { get; set; }
        public string? MaintenanceNotes { get; set; }
        public string? DepreciationSchedule { get; set; }
        public string? DepreciationStart { get; set; }
        public string? DepreciationEnd { get; set; }
        public string? DepreciationValue { get; set; }
        public string? DepreciationNotes { get; set; }
        public string? DisposalDate { get; set; }
        public string? DisposalMethod { get; set; }
        public string? DisposalValue { get; set; }
        public string? DisposalNotes { get; set; }
        public string? Image { get; set; }
    }
}
