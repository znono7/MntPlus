using Service.Contracts;

namespace Presentation.WPF
{
    public class EquipmentPresentation
    {
        private readonly IServiceManager _service;
        public EquipmentPresentation(IServiceManager service) => _service = service;


    }
}
