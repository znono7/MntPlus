using Shared;
using System.Text;

namespace MntPlus.WPF
{
    public class PreventiveRecordViewModel : BaseViewModel
    {
        public PreventiveMaintenanceDto? PreventiveMaintenanceDto { get; set; }
        public bool IsSelected { get; set; }
        public string? Number { get; set; }
        public string? Name { get; set; }
        public string? Asset { get; set; }
       public string? AssignedTo { get; set; }
        public string? PmAssigned { get; set; }
        public DateTime? NextDueDate { get; set; } 
        public string? ScheduleText { get; set; } 
         
        public PreventiveRecordViewModel(PreventiveMaintenanceDto? preventiveMaintenanceDto)
        {
            PreventiveMaintenanceDto = preventiveMaintenanceDto;
            Number = AddDynamicLeadingZeros(preventiveMaintenanceDto?.Number ?? 0);
            Name = preventiveMaintenanceDto?.Name;
            Asset = preventiveMaintenanceDto?.Asset?.Name;
            AssignedTo = SetAssigned();
            if(PreventiveMaintenanceDto?.Schedule != null && PreventiveMaintenanceDto?.MeterSchedule == null)
            {
                SetScheduleText(PreventiveMaintenanceDto?.Schedule);
                NextDueDate = ScheduleCalculator.GetNextDueDate(PreventiveMaintenanceDto?.Schedule, PreventiveMaintenanceDto?.Schedule.StartDate ?? DateTime.Now);
                ScheduleText = $"{ScheduleText} | prochaine échéance le {NextDueDate.Value:dd/MM/yyyy}";

            }
            else if(PreventiveMaintenanceDto?.MeterSchedule != null && PreventiveMaintenanceDto?.Schedule == null)
            {
                ScheduleText = SetScheduleText(PreventiveMaintenanceDto?.MeterSchedule);
            }else if (PreventiveMaintenanceDto?.MeterSchedule != null && PreventiveMaintenanceDto?.Schedule != null)
            {
                SetScheduleText(PreventiveMaintenanceDto?.Schedule);
                NextDueDate = ScheduleCalculator.GetNextDueDate(PreventiveMaintenanceDto?.Schedule, PreventiveMaintenanceDto?.Schedule.StartDate ?? DateTime.Now);
                ScheduleText = $"{ScheduleText} | prochaine échéance le {NextDueDate.Value:dd/MM/yyyy}";

                ScheduleText = $"{ScheduleText} OU {SetScheduleText(PreventiveMaintenanceDto?.MeterSchedule)}";
               
            }
        }

        private string SetScheduleText(MeterScheduleDto? meterSchedule)
        {
            return $"Utilisation : Créé quand {meterSchedule?.Meter?.Name} , {meterSchedule?.Interval} {meterSchedule?.Meter?.Unit}";
        }

        private string? SetAssigned()
        {
            if(PreventiveMaintenanceDto?.UserAssignedTo != null)
            {
                PmAssigned = "user";
                return PreventiveMaintenanceDto.UserAssignedTo.FullName ;
            }else if(PreventiveMaintenanceDto?.TeamAssignedTo != null)
            {
                PmAssigned = "team";
                return PreventiveMaintenanceDto.TeamAssignedTo.Name;
            }else
            {
                return "Aucun";
            }
        }

        public string AddDynamicLeadingZeros(int number)
        {
            // Get the number of digits in the number
            int numberOfDigits = number.ToString().Length;

            // Calculate the total length after adding zeros
            int totalLength = numberOfDigits * 2 + 1;

            // Pad the number with leading zeros
            return number.ToString().PadLeft(totalLength, '0');
        }

        private void SetScheduleText(ScheduleDto? ScheduleModel)
        {
            if (ScheduleModel == null) return;
            switch (ScheduleModel)
            {
                case DailyScheduleDto dailySchedule:
                    ScheduleText = $"Se répète tous les {dailySchedule.Interval} jour(s)";
                    break;
                case WeeklyScheduleDto weeklySchedule:
                    ScheduleText = $"Se répète tous les {weeklySchedule.Interval} semaine(s) le {ConvertDaysOfWeekToFrench(weeklySchedule.DaysOfWeek!)}";
                    break;
                case MonthlyWeekdayScheduleDto monthlyWeekdaySchedule:
                    ScheduleText = $"Se répète tous les {monthlyWeekdaySchedule.Interval} mois le {ConvertWeekOfMonth(monthlyWeekdaySchedule.WeekOfMonth)} {ConvertDaysOfWeekToFrench(monthlyWeekdaySchedule.DayOfWeek)}";
                    break;
                case MonthlyNumericScheduleDto monthlyNumericSchedule:
                    ScheduleText = $"Se répète tous les {monthlyNumericSchedule.Interval} mois le {ConvertDayOfMonth(monthlyNumericSchedule.DayOfMonth)}";
                    break;
                case YearlyOrdinalScheduleDto yearlyOrdinalSchedule:
                    ScheduleText = $"Se répète tous les {yearlyOrdinalSchedule.Interval} ans le  {ConvertWeekOfMonth(yearlyOrdinalSchedule.WeekOfMonth)} {ConvertDaysOfWeekToFrench(yearlyOrdinalSchedule.DayOfWeek)} de {ConvertIndexToMonth(yearlyOrdinalSchedule.Month)}";
                    break;
                case YearlyNumericScheduleDto yearlyNumericSchedule:
                    ScheduleText = $"Se répète tous les {yearlyNumericSchedule.Interval} ans en {ConvertIndexToMonth(yearlyNumericSchedule.Month)} le {ConvertDayOfMonth(yearlyNumericSchedule.DayOfMonth)}";
                    break;
                default:
                    throw new InvalidOperationException("Unknown schedule type");
            }
        }
        private string ConvertDaysOfWeekToFrench(DayOfWeek daysOfWeek)
        {
            switch (daysOfWeek)
            {
                case DayOfWeek.Monday:
                    return "Lundi";
                case DayOfWeek.Tuesday:
                    return "Mardi";
                case DayOfWeek.Wednesday:
                    return "Mercredi";
                case DayOfWeek.Thursday:
                    return "Jeudi";
                case DayOfWeek.Friday:
                    return "Vendredi";
                case DayOfWeek.Saturday:
                    return "Samedi";
                case DayOfWeek.Sunday:
                    return "Dimanche";
                default:
                    throw new InvalidOperationException("Invalid day of week");
            }

        }

        private string ConvertDaysOfWeekToFrench(List<DayOfWeek> daysOfWeek)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var dayOfWeek in daysOfWeek)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        sb.Append("Lundi, ");
                        break;
                    case DayOfWeek.Tuesday:
                        sb.Append("Mardi, ");
                        break;
                    case DayOfWeek.Wednesday:
                        sb.Append("Mercredi, ");
                        break;
                    case DayOfWeek.Thursday:
                        sb.Append("Jeudi, ");
                        break;
                    case DayOfWeek.Friday:
                        sb.Append("Vendredi, ");
                        break;
                    case DayOfWeek.Saturday:
                        sb.Append("Samedi, ");
                        break;
                    case DayOfWeek.Sunday:
                        sb.Append("Dimanche, ");
                        break;
                    default:
                        throw new InvalidOperationException("Invalid day of week");
                }
            }
            return sb.ToString().TrimEnd(',', ' ');


        }
        private string ConvertIndexToMonth(int monthIndex)
        {
            switch (monthIndex)
            {
                case 1:
                    return "Janvier";
                case 2:
                    return "Février";
                case 3:
                    return "Mars";
                case 4:
                    return "Avril";
                case 5:
                    return "Mai";
                case 6:
                    return "Juin";
                case 7:
                    return "Juillet";
                case 8:
                    return "Août";
                case 9:
                    return "Septembre";
                case 10:
                    return "Octobre";
                case 11:
                    return "Novembre";
                case 12:
                    return "Décembre";
                default:
                    throw new InvalidOperationException("Invalid month index");
            }
        }
        private string ConvertWeekOfMonth(int weekOfMonth)
        {
            switch (weekOfMonth)
            {
                case 1:
                    return "1er";
                case 2:
                    return "2ème";
                case 3:
                    return "3ème";
                case 4:
                    return "4ème";
                case 5:
                    return "5ème";
                default:
                    throw new InvalidOperationException("Invalid week of month");
            }
        }
        private string ConvertDayOfMonth(int dayOfMonth)
        {
            if (dayOfMonth == 1) return "1er";
            return $"{dayOfMonth}ème";
        }
    }
}
