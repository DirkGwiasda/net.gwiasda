import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Booking } from './booking';
import { Observable, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FiMaBookingDataService {

  constructor(private http: HttpClient) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  async write(booking: Booking) : Promise<void> {
    const headers = this.getDefaultHeaders();
    headers.set('Content-Type', 'application/json');
    const url = 'fimabooking/Save';

    await this.http.post(url, booking, { headers }).toPromise();
  }
}
