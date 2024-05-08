import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Booking } from './booking';
import { Observable, catchError } from 'rxjs';
import { map } from 'rxjs/operators';
import { DateService } from '../../date-service';

@Injectable({
  providedIn: 'root',
})
export class FiMaBookingDataService {

  constructor(private http: HttpClient, private dateService: DateService) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  async write(booking: Booking) : Promise<void> {
    const headers = this.getDefaultHeaders();
    headers.set('Content-Type', 'application/json');
    const url = 'fimabooking/Save';

    var timestamp = this.dateService.renderDateTimeAsApiParameter(booking.timestamp);

    var data = {
      id: booking.id,
      timestamp: timestamp,
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
    return this.http.get<Map<string, Booking>>('fimabooking/GetBookingsFromToday?date=' + formattedDate, { headers })
      .pipe(map(dataArray => {
        const bookings = new Map<string, Booking>();
        for (let key in dataArray) {
          const booking = dataArray.get(key);
          if (booking) {
            var t = booking.timestamp;
            booking.timestamp = this.dateService.getUTCDate(booking.timestamp);
            bookings.set(key, booking);
          }
        }
        return bookings;
      }));
  }

  readRecurringBookings(): Observable<Booking[]> {
    const headers = this.getDefaultHeaders();
    return this.http.get<Booking[]>('fimabooking/GetRecurringBookings', { headers })
      .pipe(map(dataArray => {
        for (let booking of dataArray) {
          booking.timestamp = this.dateService.getUTCDate(booking.timestamp);
        }
        return dataArray;
      }
    ));
  }
}
