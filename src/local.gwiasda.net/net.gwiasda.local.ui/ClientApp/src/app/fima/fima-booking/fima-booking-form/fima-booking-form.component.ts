import { Component, OnInit } from '@angular/core';
import { Booking } from '../booking';
import { FinanceCategory } from '../../fima-categories/finance_category';
import { FiMaCategorySelectionComponent } from '../../fima-categories/fima-category-selection/fima-category-selection.component';
import { FiMaBookingDataService } from '../fima-booking-data.service';
import { FiMaCategoryDataService } from '../../fima-categories/fima-category-data.service';
import { ChangeDetectorRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-fima-booking-form',
  templateUrl: './fima-booking-form.component.html'
})
export class FiMaBookingFormComponent implements OnInit {

  constructor(dataService: FiMaBookingDataService, categoryDataService: FiMaCategoryDataService, private dialog: MatDialog, private cdr: ChangeDetectorRef)
  {
    this.dataService = dataService;
    this.categoryDataService = categoryDataService;
  }

  showRecurring: boolean = false;
  date: Date = new Date();
  dataService: FiMaBookingDataService;
  categoryDataService: FiMaCategoryDataService;
  costCategories: FinanceCategory[] = [];
  incomeCategories: FinanceCategory[] = [];
  recurringBookings: Booking[] = [];
  category: FinanceCategory = new FinanceCategory();
  selectedCategoryName: string = '---';
  booking: Booking = { id: this.generateGUID(), timestamp: new Date(), text: '', categoryId: '', isCost: true, amount: 0, recurringType: 'einmalig', endDate: null };
  formattedAmount: number = 0;
  hackDate: Date = new Date();
  hackEndDate: Date | null = null;

  ngOnInit() {
    this.readCategories();
    this.readRecurringBookings();
  }
  getAmount(amount: number) {
    return amount.toFixed(2);
  }
  updateModel(value: string) {
    if (/^[0-9]+(,[0-9]{0,2})?$/g.test(value)) {
      value = value.replace(".", '').replace(",", '.');
      const amount = parseFloat(value);
      this.booking.amount = amount;
    }
    else {
      this.formattedAmount = 0;
    }
  }
  selectCategory() {
    const dialogRef = this.dialog.open(FiMaCategorySelectionComponent,
      {
      });

    if (this.category == null)
      this.category = new FinanceCategory();

    if (this.booking?.isCost)
      dialogRef.componentInstance.categories = this.costCategories;
    else
      dialogRef.componentInstance.categories = this.incomeCategories;
    dialogRef.componentInstance.allowNoneSelection = false;

    dialogRef.componentInstance.isCostCategory = this.booking == null ? false : this.booking.isCost;
    dialogRef.componentInstance.saved.subscribe((category: FinanceCategory) => {

      if (category.name != null && category.name != '')
        this.selectedCategoryName = category.name;
      else
        this.selectedCategoryName = '---';

      if (this.booking != null)
        this.booking.categoryId = category.id;
    });
  }
  edit(booking: Booking) {
    this.booking = Object.assign({}, booking);
    this.selectedCategoryName = this.booking.categoryId;

    this.formattedAmount = this.booking.amount;
    this.setFormattedAmountInputToGerman(this.booking.amount);
    
    this.hackEndDate = booking.endDate;
    this.hackDate = booking.timestamp;
    let category: FinanceCategory | undefined;
    if (booking.isCost)
      category = this.costCategories.find(c => c.id == booking.categoryId);
    else
      category = this.incomeCategories.find(c => c.id == booking.categoryId);

    if (category != null)
      this.selectedCategoryName = category.name;
    else
      this.selectedCategoryName = '---';
  }
  cancel() {
    this.booking = { id: this.generateGUID(), timestamp: new Date(), text: '', categoryId: '', isCost: true, amount: 0, recurringType: this.booking.recurringType, endDate: null };
    this.formattedAmount = 0;
    this.selectedCategoryName = '---';
  }
  async save() {

    this.booking.timestamp = this.getFuckingHackDate();
    this.booking.endDate = this.getFuckingHackEndDate();

    await this.dataService.write(this.booking);
    this.date = this.booking.timestamp;
    if(this.booking.recurringType != null && this.booking.recurringType != 'einmalig')
      await this.readRecurringBookings();
    this.cancel();
  }

  setFormattedAmountInputToGerman(value: number) {
    var el = document.getElementById("formattedAmountInput");
    if (el instanceof HTMLInputElement) {
      el.value = value.toString().replace(".", ",");
      this.cdr.detectChanges();
      this.cdr.markForCheck();
      console.log("value set to " + el.value);
    }
  }
  getFuckingHackDate(): Date {
    var el = document.getElementById("bookingdate");
    if (el instanceof HTMLInputElement) {
      const dateParts = el.value.split('.');
      if (dateParts.length === 3)
        return new Date(parseInt(dateParts[2]), parseInt(dateParts[1]) - 1, parseInt(dateParts[0]), 8, 0, 0);
    }
    return this.booking.timestamp;
  }
  getFuckingHackEndDate(): Date | null {
    var el = document.getElementById("endDate");
    if (el instanceof HTMLInputElement) {
      const dateParts = el.value.split('.');
      if (dateParts.length === 3)
        return new Date(parseInt(dateParts[2]), parseInt(dateParts[1]) - 1, parseInt(dateParts[0]), 8, 0, 0);
    }
    return null;
  }
  switchOverview(showRecurring: boolean) {
    this.showRecurring = showRecurring;
  }

  async readCategories(): Promise<void> {
    await this.categoryDataService.readCostCategories()
      .then(costCategories => {
        this.costCategories = costCategories ?? [];
      });
    await this.categoryDataService.readIncomeCategories()
      .then(incomeCategories => {
        this.incomeCategories = incomeCategories ?? [];
      });
  }
  async readRecurringBookings(): Promise<void> {
    this.dataService.readRecurringBookings().subscribe(response => {
      this.recurringBookings = response;
      console.log(response);
    });
  }
  
  generateGUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0,
        v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
}
