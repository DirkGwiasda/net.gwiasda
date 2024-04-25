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

    var data = {
      id: booking.id,
      timestamp: booking.timestamp,
      text: booking.text,
      categoryId: booking.categoryId,
      isCost: booking.isCost,
      amount: parseFloat(booking.amount.toString())
    };

    await this.http.post(url, data, { headers }).toPromise();
  }
  readBookingsFromToday(): Observable<Map<string, Booking>> {
    const headers = this.getDefaultHeaders();
    return this.http.get<Map<string, Booking>>('fimabooking/GetBookingsFromToday', { headers });
  }
}
