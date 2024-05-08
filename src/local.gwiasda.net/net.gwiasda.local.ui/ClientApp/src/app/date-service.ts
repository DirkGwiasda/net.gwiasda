import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DateService {

  constructor() { }

  parseDate(dateString: string): Date {
    const year = parseInt(dateString.substr(0, 4), 10);
    const month = parseInt(dateString.substr(4, 2), 10) - 1;
    const day = parseInt(dateString.substr(6, 2), 10);
    const hours = parseInt(dateString.substr(8, 2), 10);
    const minutes = parseInt(dateString.substr(10, 2), 10);

    return new Date(year, month, day, hours, minutes);
  }

  getUTCDate(date: Date): Date {
    var d = new Date(date);
    return new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes()));
  }

  renderDate(date: Date): string {
    if (!date) return '';
    var d = new Date(date);
    var result = this.to2DigitString(d.getUTCDate()) + '.' + this.to2DigitString(d.getUTCMonth() + 1) + '.' + d.getUTCFullYear();
    return result;
  }

  renderSimpleDate(date: Date): string {
    if (!date) return '';
    var d = new Date(date);
    return d.getUTCDate() + '.' + (d.getUTCMonth() + 1) + '.' + d.getUTCFullYear();
  }

  renderDateAsApiParameter(date: Date): string {
    return date.getFullYear() + this.to2DigitString(date.getMonth() + 1) + this.to2DigitString(date.getDate());
  }

  renderDateTimeAsApiParameter(date: Date): string {
    var d = new Date(date);
    return d.getFullYear() + this.to2DigitString(d.getMonth() + 1) + this.to2DigitString(d.getDate())
      + this.to2DigitString(d.getHours()) + this.to2DigitString(d.getMinutes());
  }

  renderTime(date: Date): string {
    if (!date) return '';
    var d = new Date(date);
    return this.to2DigitString(d.getUTCHours()) + ':' + this.to2DigitString(d.getUTCMinutes());
  }

  renderDateTime(date: Date): string {
    return this.renderDate(date) + ' ' + this.renderTime(date);
  }

  to2DigitString(value: number): string {
    return ('0' + value).slice(-2);
  }
}
