import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { Booking } from '../booking';
import { FinanceCategory } from '../../fima-categories/finance_category';

@Component({
  selector: 'app-fima-recurring-booking-overview',
  templateUrl: './fima-recurring-booking-overview.component.html'
})
export class FiMaRecurringBookingOverviewComponent implements OnChanges {

  constructor() { }

  @Input() recurringBookings: Booking[] = [];
  @Input() costCategories: FinanceCategory[] = [];
  @Input() incomeCategories: FinanceCategory[] = [];
  @Output() selected = new EventEmitter<Booking>();

  ngOnChanges(changes: SimpleChanges) {
    if (changes.recurringBookings?.currentValue) {
      this.recurringBookings = changes.recurringBookings.currentValue;
    }
  }
  getCategoryName(recurringBooking: Booking): string {
     if(recurringBooking.isCost)
      return this.costCategories.find(c => c.id === recurringBooking.categoryId)?.name || '???';
    else
        return this.incomeCategories.find(c => c.id === recurringBooking.categoryId)?.name || '???';
  }
  edit(booking: Booking) {
    if(booking)
      this.selected.emit(booking);
  }
}
