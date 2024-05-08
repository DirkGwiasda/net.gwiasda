import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Appointment } from './appointment';
import { Observable } from 'rxjs';
import { DateService } from '../date-service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AppointmentDataService {

  constructor(private http: HttpClient, private dateService: DateService) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  getAppointmentsForTimespan(from: Date, to: Date): Observable<Appointment[] | undefined> {

    const headers = this.getDefaultHeaders();

    const options: Intl.DateTimeFormatOptions = {
      day: '2-digit', // zweistelliger Tag (dd)
      month: '2-digit', // zweistelliger Monat (MM)
      year: 'numeric' // vierstelliges Jahr (yyyy)
    };
    var url = 'appointments/GetAppointmentsForTimespan?from=' + this.dateService.renderDateAsApiParameter(from) + '&to=' + this.dateService.renderDateAsApiParameter(to);
    console.log(url);
    return this.http.get<Appointment[]>(url, { headers });
  }
  getAppointment(id: string): Observable<Appointment | undefined> {

    const headers = this.getDefaultHeaders();

    var url = 'appointments/GetAppointment?id=' + id;
    return this.http.get<Appointment>(url, { headers });
  }

  async delete(id: string): Promise<void> {

    const headers = this.getDefaultHeaders();

    var url = 'appointments/Delete?id=' + id;
    await this.http.get(url, { headers }).toPromise();
  }

  async write(appointment: Appointment): Promise<void> {
    const headers = this.getDefaultHeaders();
    headers.set('Content-Type', 'application/json');
    const url = 'appointments/Save';
    var data = {
      id: appointment.id,
      date: this.dateService.getUTCDate(appointment.date),
      endDate: appointment.endDate ? this.dateService.getUTCDate(appointment.endDate) : null,
      title: appointment.title,
      text: appointment.text,
      who: appointment.who,
      recurringType: appointment.recurringType,
      googleMapsLink: appointment.googleMapsLink,
      notInSchoolHolidays: appointment.notInSchoolHolidays,
      keepAppointmentAfterItsEnd: appointment.keepAppointmentAfterItsEnd,
      isSchoolHoliday: appointment.isSchoolHoliday
    };
    await this.http.post(url, data, { headers }).toPromise();
  }
}
