using Shared;


namespace MntPlus.WPF
{
    public class CheckListStore
    {
        public event Action<CheckListDto?>? CheckListCreated;
        public event Action<CheckListDto?>? CheckListUpdated;
        public event Action<CheckListDto?>? CheckListSelected;

        public void CreateCheckList(CheckListDto? checkList)
        {
            CheckListCreated?.Invoke(checkList);
        }

        public void UpdateCheckList(CheckListDto? checkList)
        {
            CheckListUpdated?.Invoke(checkList);
        }

        public void SelectCheckList(CheckListDto? checkList)
        {
            CheckListSelected?.Invoke(checkList);
        }
    }
}
