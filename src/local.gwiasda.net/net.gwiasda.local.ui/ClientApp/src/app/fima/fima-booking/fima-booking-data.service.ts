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
      amount: parseFloat(booking.amount.toString()),
      recurringType: booking.recurringType,
      endDate: booking.endDate
    };

    //console.log("writing booking:::::::::::::::::");
    //console.log(JSON.stringify(data));

    await this.http.post(url, data, { headers }).toPromise();
  }
  readBookingsFromDay(date: Date): Observable<Map<string, Booking>> {
    const headers = this.getDefaultHeaders();
    const options: Intl.DateTimeFormatOptions = {
      day: '2-digit', // zweistelliger Tag (dd)
      month: '2-digit', // zweistelliger Monat (MM)
      year: 'numeric' // vierstelliges Jahr (yyyy)
    };
    const formattedDate = date.toLocaleDateString('en-GB', options).replace(/\//g, '');
    return this.http.get<Map<string, Booking>>('fimabooking/GetBookingsFromToday?date=' + formattedDate, { headers });
  }
  readRecurringBookings(): Observable<Booking[]> {
    const headers = this.getDefaultHeaders();
    return this.http.get<Booking[]>('fimabooking/GetRecurringBookings', { headers });
  }
}
