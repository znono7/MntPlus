using Entities;
using Shared;
using System.Collections.ObjectModel;

namespace MntPlus.WPF
{
    public class EquipmentMeterViewModel : BaseViewModel
    {
        private MeterDto _selectedMember;
        public MeterDto SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                if (_selectedMember.MeterReadings == null)
                {
                    return; 
                }
                Unite = _selectedMember.Unit ?? "";
                MeterReadingDtos = new ObservableCollection<MeterReadingDto>(_selectedMember.MeterReadings);
            }
        }
        public ObservableCollection<MeterDto> Meters { get; set; }
        public ObservableCollection<MeterReadingDto> MeterReadingDtos { get; set; }
        public Guid EquipmentId { get; set; }

        public string Unite { get; set; }
        public EquipmentMeterViewModel(Guid equipmentId)
        {
            Meters = new ObservableCollection<MeterDto>();
            MeterReadingDtos = new ObservableCollection<MeterReadingDto>();
            EquipmentId = equipmentId;
            _ = LoadMeters();
        }

        private async Task LoadMeters()
        {
            var meters = await AppServices.ServiceManager.MeterService.GetAllMetersByEquipmentAsync(EquipmentId,false);

            if (meters.Success && meters is ApiOkResponse<IEnumerable<MeterDto>> result)
            {
                Meters = new ObservableCollection<MeterDto>(result.Result!);
            }
        }
    }
}
