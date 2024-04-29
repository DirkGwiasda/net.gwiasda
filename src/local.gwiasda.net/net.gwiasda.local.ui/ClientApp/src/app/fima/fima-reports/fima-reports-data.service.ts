import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MonthlyReport } from './monthlyReport';
import { Observable, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FiMaReportsDataService {

  constructor(private http: HttpClient) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  readMonthlyReportForDate(date: Date): Observable<MonthlyReport> {
    const headers = this.getDefaultHeaders();
    const options: Intl.DateTimeFormatOptions = {
      day: '2-digit', // zweistelliger Tag (dd)
      month: '2-digit', // zweistelliger Monat (MM)
      year: 'numeric' // vierstelliges Jahr (yyyy)
    };
    const formattedDate = date.toLocaleDateString('en-GB', options).replace(/\//g, '');
    return this.http.get<MonthlyReport>('fimareport/GetMonthlyReport?date=' + formattedDate, { headers });
  }
}
