import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Appointment } from './appointment';

@Injectable({
  providedIn: 'root',
})
export class AppointmentDataService {

  constructor(private http: HttpClient) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  async readAll(): Promise<Appointment[] | undefined> {

    const headers = this.getDefaultHeaders();
    return await this.http.get<Appointment[]>('appointment/GetAll', { headers }).toPromise();
  }
  async write(appointment : Appointment): Promise<void> {
    if (!appointment) return;

    console.log(appointment);

    const headers = this.getDefaultHeaders();
    const url = 'appointment/Save';

    await this.http.post(url, appointment, { headers }).toPromise();
  }
  async delete(id: string): Promise<void> {
    const url = 'appointment/Delete?id=' + id;
    const headers = this.getDefaultHeaders();
    await this.http.get(url, { headers }).toPromise();
  }
}
