import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Link } from './link';

@Injectable({
  providedIn: 'root',
})
export class LinkDataService {

  constructor(private http: HttpClient) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  async readAppNames(): Promise<string[] | undefined> {

    const headers = this.getDefaultHeaders();
    return await this.http.get<string[]>('logging/GetLogAppNames', { headers }).toPromise();
  }
  async readLogEntries(logAppName : string): Promise<LogEntry[] | undefined> {
    if (!logAppName) return [];

    const headers = this.getDefaultHeaders();
    const url = 'logging/GetLoggingEntries?appName=' + logAppName;
    return await this.http.get<LogEntry[]>(url, { headers }).toPromise();
  }
  async deleteEntries(logAppName: string): Promise<void> {
    const url = 'logging/DeleteLogs?appName=' + logAppName;
    const headers = this.getDefaultHeaders();
    await this.http.get(url, { headers }).toPromise();
  }
}
