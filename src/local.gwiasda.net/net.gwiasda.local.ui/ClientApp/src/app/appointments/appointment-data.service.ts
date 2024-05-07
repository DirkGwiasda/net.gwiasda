import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Appointment } from './appointment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AppointmentDataService {

  constructor(private http: HttpClient) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  getAppointmentsForTimespan(from: Date, to: Date): Observable<Appointment[] | undefined> {

    const headers = this.getDefaultHeaders();

    const options: Intl.DateTimeFormatOptions = {
      day: '2-digit', // zweistelliger Tag (dd)
      month: '2-digit', // zweistelliger Monat (MM)
      year: 'numeric' // vierstelliges Jahr (yyyy)
    };
    const formattedFromDate = from.toLocaleDateString('en-GB', options).replace(/\//g, '');
    const formattedToDate = to.toLocaleDateString('en-GB', options).replace(/\//g, '');
    var url = 'appointments/GetAppointmentsForTimespan?from=' + formattedFromDate + '&to=' + formattedToDate;
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

    await this.http.post(url, appointment, { headers }).toPromise();
  }
}
