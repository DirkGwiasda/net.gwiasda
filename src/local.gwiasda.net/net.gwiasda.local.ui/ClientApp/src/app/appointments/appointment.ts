export class Appointment {
  id: string = '';
  date: Date = new Date();
  endDate: Date | null = null;
  title: string = '';
  text: string | null = null;
  who: string = '-';
  recurringType: string = '-';
  googleMapsLink: string | null = null;
  notInSchoolHolidays: boolean = false;
  keepAppointmentAfterItsEnd: boolean = false;
  isSchoolHoliday: boolean = false;
}
