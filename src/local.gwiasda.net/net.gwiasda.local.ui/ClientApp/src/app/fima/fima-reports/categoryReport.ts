import { FinanceCategory } from '../fima-categories/finance_category';
import { Booking } from '../fima-booking/booking';

export class CategoryReport {
  category: FinanceCategory = new FinanceCategory();
  isCost: boolean = false;
  sum: number = 0;
  childCategories: CategoryReport[] = [];
  bookings: Booking[] = [];
}
