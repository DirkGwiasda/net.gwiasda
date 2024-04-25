import { Component, OnInit, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { Booking } from '../booking';
import { FiMaBookingDataService } from '../fima-booking-data.service';


@Component({
  selector: 'app-fima-booking-overview',
  templateUrl: './fima-booking-overview.component.html'
})
export class FiMaBookingOverviewComponent implements OnInit, OnChanges {

  constructor(private dataService: FiMaBookingDataService) { }

  @Input() injectedDate: Date = new Date();
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
    //console.log("edit");
    //console.log(booking);
    //this.booking = booking;
    //this.selectedCategoryName = this.booking.categoryId;
    //this.formattedAmount = this.booking.amount;
    //let category: FinanceCategory | undefined;
    //if (booking.isCost)
    //  category = this.costCategories.find(c => c.id == booking.categoryId);
    //else
    //  category = this.incomeCategories.find(c => c.id == booking.categoryId);

    //if (category != null)
    //  this.selectedCategoryName = category.name;
    //else
    //  this.selectedCategoryName = '---';
  }
}
