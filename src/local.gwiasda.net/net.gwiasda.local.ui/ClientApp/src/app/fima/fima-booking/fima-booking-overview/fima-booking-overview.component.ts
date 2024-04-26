import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { Booking } from '../booking';
import { FiMaBookingDataService } from '../fima-booking-data.service';


@Component({
  selector: 'app-fima-booking-overview',
  templateUrl: './fima-booking-overview.component.html'
})
export class FiMaBookingOverviewComponent implements OnInit, OnChanges {

  constructor(private dataService: FiMaBookingDataService) { }

  @Input() injectedDate: Date = new Date();
  @Output() selected = new EventEmitter<Booking>();

  date: Date = new Date();

  categoryKeys: string[] = [];
  formattedAmount: number = 0;
  bookingsPerTimeUnit: Map<string, Booking[]> = new Map<string, Booking[]>();

  ngOnInit() {
    this.date = this.injectedDate;
    this.readBookingsFromDay();
  }

  refresh(date: Date) {
    this.date = date;
    this.readBookingsFromDay();
  }

  async readBookingsFromDay() {
    this.dataService.readBookingsFromDay(this.date).subscribe(response => {
      this.bookingsPerTimeUnit = new Map<string, Booking[]>(Object.entries(response));
      this.categoryKeys = Object.keys(response);
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.injectedDate?.currentValue) {
      this.date = changes.injectedDate.currentValue;
      this.readBookingsFromDay();
    }
  }

  edit(booking: Booking) {
    if(booking)
      this.selected.emit(booking);
  }
}